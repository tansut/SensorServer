using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Kalitte.Sensors.Processing.Metadata;
using System.Threading;
using System.Diagnostics;
using Kalitte.Sensors.Core;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core
{
    abstract class SingleManager<E> : IDisposable, IRunnable where E : PersistEntityBase
    {
        WaitCallback changeStateCallback;
        protected ReaderWriterLockSlim itemlock = new ReaderWriterLockSlim(LockRecursionPolicy.SupportsRecursion);
        protected internal LastEventList LastEvents { get; private set; }
        protected internal LastEventFilter LastEventsFilter { get; private set; }
        private int lastEventsSize;
        protected ServerItemMonitor runningKeeper;

        public SingleManager(E entity)
        {
            this.Entity = entity;
            IEntityPropertyProvider entityWithProperties = Entity as IEntityPropertyProvider;
            if (entityWithProperties != null)
            {
                entityWithProperties.GetProperties().ResetState();
            }
            runningKeeper = null;
            changeStateCallback = new WaitCallback(changeStateCb);
            lastEventsSize = ServerConfiguration.Current.LastEventsSize;
            LastEvents = lastEventsSize <= 0 ? null: new LastEventList(lastEventsSize);
            LastEventsFilter = LastEventFilter.Empty;
        }

        public ProcessingItem ItemType
        {
            get
            {
                return Manager.ItemType;
            }
        }

        protected void AddEventToLastEvents(string source, SensorEventBase sensorEvent)
        {
            if (LastEvents != null)
                LastEvents.Add(source, sensorEvent, LastEventsFilter);
        }

        internal void SetLastEventFilter(LastEventFilter filter)
        {
            this.LastEventsFilter = filter;
            if (LastEvents != null)
                LastEvents.Clear();
        }

        internal Events.LastEventFilter GetLastEventFilter()
        {
            return LastEventsFilter;
        }

        internal IList<LastEvent> GetCurrentEvents()
        {
            //return new List<LastEvent>() { new LastEvent(DateTime.Now, "dd", null)};
            return LastEvents == null ? new List<LastEvent>() : LastEvents.GetCurrentEvents();
        }

        protected ILogger Logger
        {
            get
            {
                return Manager.Logger;
            }
        }

        protected void SyncProfile(PropertyList propertyProfile, Dictionary<PropertyKey, EntityMetadata> metaData)
        {
            var tempProfile = new PropertyList();
            foreach (var item in propertyProfile)
            {
                tempProfile.Add(item.Key, item.Value);
            }

            foreach (var profileItem in propertyProfile)
            {
                if (!metaData.ContainsKey(profileItem.Key))
                    tempProfile.Remove(profileItem.Key);
            }

            foreach (var metaDataItem in metaData)
            {
                if (!tempProfile.ContainsKey(metaDataItem.Key))
                    tempProfile.Add(metaDataItem.Key, metaDataItem.Value.DefaultValue);
            }

            propertyProfile.Clear();

            foreach (var item in tempProfile)
            {
                propertyProfile.Add(item.Key, item.Value);
            }

        }

        protected Dictionary<PropertyKey, EntityMetadata> GetEntityMetadata<T>(Dictionary<PropertyKey, T> source)
        {
            return SensorCommon.GetEntityMetadata<T>(source);
        }


        protected internal T GetMetadata<T>(string type)
        {
            try
            {
                return Manager.itemMetadataManager.GetTypeMetadata<T>(type);
            }
            catch (System.Exception exc)
            {
                Logger.LogException("Unable to get metadata of {0}", exc, Entity.Name);
                return default(T);
            }
        }


        protected internal virtual E CheckAndSendItem()
        {
            if (Entity is IEntityPropertyProvider)
            {
                IEntityPropertyProvider entityWithProperties = (IEntityPropertyProvider)Entity;
                var extendedMetadataOfItem = ServerManager.GetItemExtendedMetadata(this.ItemType);
                if (extendedMetadataOfItem != null)
                {
                    var extendedMetadata = GetEntityMetadata<ExtendedPropertyMetadata>(extendedMetadataOfItem.PropertyMetadata);
                    SyncProfile(entityWithProperties.GetProperties().ExtendedProfile, extendedMetadata);
                }

                var defaultMetadata = Manager.GetDefaultMetadata(Entity.Name);
                if (defaultMetadata != null)
                {
                    SyncProfile(entityWithProperties.GetProperties().Profile, defaultMetadata);
                }
            }
            return this.Entity;
        }

        public abstract OperationManagerBase Manager { get; }

        public E Entity { get; set; }

        public virtual void DelayedStartup()
        {
            if (Entity is IEntityPropertyProvider)
            {
                IEntityPropertyProvider entityWithProperties = (IEntityPropertyProvider)Entity;
                if (entityWithProperties.GetProperties().Startup == ItemStartupType.Automatic)
                {
                    ChangeState(ItemState.Running, false);
                }
            }
        }

        public virtual void Startup()
        {

        }

        protected virtual void CleanupBeforeDeleting()
        {
        }

        internal protected void Delete()
        {
            itemlock.EnterWriteLock();
            try
            {
                CleanupBeforeDeleting();
                Dispose();
            }
            finally
            {
                itemlock.ExitWriteLock();
            }


        }

        protected ServerManager ServerManager
        {
            get
            {
                return this.Manager.ServerManager;
            }
        }

        public ItemState GetState()
        {
            return GetState(true);
        }

        protected virtual internal ItemState GetState(bool useLock = true)
        {
            return GetStateInfo(useLock).State;
        }
        

        protected virtual internal ItemStateInfo GetStateInfo(bool useLock = true)
        {
            var currentState = ItemStateInfo.Stopped;
            IEntityPropertyProvider entityWithProperties = Entity as IEntityPropertyProvider;
            if (entityWithProperties != null)
            {
                if (useLock) itemlock.EnterReadLock();
                try
                {
                    currentState = entityWithProperties.GetProperties().StateInfo;
                }
                finally
                {
                    if (useLock) itemlock.ExitReadLock();
                }
            }
            return currentState;
        }


        public virtual void Shutdown()
        {
            StopMonitoring();
            if (GetState() == ItemState.Running)
            {
                ChangeState(ItemState.Stopped, false);
            }
        }

        protected abstract ItemStateInfo Run();
        protected abstract ItemStateInfo Stop();

        protected virtual ItemStateInfo GetEntityStateUsingException(ItemState newState, System.Exception exc)
        {
            var state = new ItemStateInfo(exc);
            return state;
        }

        public void RunItem()
        {
            Logger.Warning("Monitoring: Trying to change state for {0} {1}", this.GetType().Name, Entity.Name);
            var newState = ChangeState(ItemState.Running, true);
            if (newState != null && newState.State == ItemState.Running)
                newState.StateText = string.Format("Auto ({0}) {1}", DateTime.Now.ToString(), newState.StateText);
        }

        internal void StartMonitoring()
        {
            if (runningKeeper != null)
                runningKeeper.Stop();
            runningKeeper = new ServerItemMonitor(this, ((IEntityPropertyProvider)this.Entity).GetProperties().MonitoringData, this.Logger, Entity.Name);
            runningKeeper.Start();
        }

        internal void StopMonitoring()
        {
            if (runningKeeper != null)
            {
                runningKeeper.Stop();
                runningKeeper = null;
            }
        }

        public ItemStateInfo ChangeState(ItemState newState, bool throwException = true, System.Exception startingException = null)
        {
            itemlock.EnterWriteLock();
            try
            {
                bool doChangeState = true;

                IEntityPropertyProvider entityWithProperties = Entity as IEntityPropertyProvider;
                ItemStateInfo stateInfo = null;
                if (entityWithProperties != null)
                {
                    stateInfo = GetStateInfo(false); // ((IEntityPropertyProvider)Entity).GetProperties().StateInfo;
                    if (stateInfo.State == newState)
                    {
                        doChangeState = false;
                    }
                }


                if (doChangeState)
                {
                    ItemStateInfo newEntityState;

                    try
                    {
                        Logger.Info("Changing state to {0} for {1} {2}", newState, this.GetType().Name, Entity.Name);
                        if (newState == ItemState.Running)
                        {
                            newEntityState = Run();
                            LastEvents.Clear();
                        }
                        else newEntityState = Stop();
                        Logger.Info("Changed state to {0} for {1} {2}", newEntityState.State, this.GetType().Name, Entity.Name);
                        if (stateInfo != null)
                        {
                            entityWithProperties.GetProperties().StateInfo = newEntityState;

                            if (startingException != null)
                            {
                                newEntityState = GetEntityStateUsingException(newEntityState.State, startingException);
                            }

                            return newEntityState;
                        }
                        else return null;
                    }

                    catch (System.Exception exc)
                    {
                        System.Exception extShouldTransfer = startingException == null ? exc : startingException;
                        newEntityState = GetEntityStateUsingException(stateInfo.State, extShouldTransfer);
                        if (exc is ThreadAbortException)
                        {
                            if (extShouldTransfer == exc)
                                newEntityState = new ItemStateInfo(newState);
                            else newEntityState = new ItemStateInfo(extShouldTransfer);
                        }
                        else
                        {
                            Manager.Logger.LogException("Unable to change state to {0} for {1}-{2}", exc, newState, this.GetType().Name, this.Entity.Name);
                        }

                        if (stateInfo != null)
                        {
                            entityWithProperties.GetProperties().StateInfo = newEntityState;
                            if (throwException && !(exc is ThreadAbortException))
                                throw;
                            else return newEntityState;
                        }
                        else return null;
                    }
                }
                else return stateInfo;
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }

        public void ChangeStateDelayed(ItemState newState)
        {
            ThreadPool.QueueUserWorkItem(changeStateCallback, newState);
        }

        private void changeStateCb(object itemState)
        {

            ChangeState((ItemState)itemState, false);


        }



        protected void ValidateState(ItemState state)
        {
            if (GetState() != state)
                throw new InvalidOperationException(string.Format("Item state should be {0} to do this operation", state));
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
        }

        ~SingleManager()
        {
            this.Dispose(false);
        }
        #endregion
    }
}

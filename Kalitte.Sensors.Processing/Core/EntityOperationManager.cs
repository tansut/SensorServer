using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Processing.Metadata;

using System.Threading;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Events;

namespace Kalitte.Sensors.Processing.Core
{
    internal abstract class EntityOperationManager<S, E> : OperationManagerBase
        where E : PersistEntityBase
        where S : SingleManager<E>
    {
        protected SafeDictionary<S> CurrentItems;

        protected abstract S NewSingleManagerInstanceFromEntity(E entity);
        protected abstract void CreateUsingProvider(S singleManager);
        public abstract IEnumerable<E> RetreiveEntitiesFromProvider();
        public abstract void DeleteEntityFromProvider(S singleManager);

        protected EntityOperationManager(ServerManager serverManager)
            : base(serverManager)
        {
            CurrentItems = new SafeDictionary<S>();
        }

        internal ItemState GetItemState(string itemName)
        {
            var item = ValidateAndGetItem(itemName);
            return item.GetState();
        }

        public override IList<Events.LastEvent> GetLastEvents(string itemName)
        {
            var singleManager = ValidateAndGetItem(itemName, false);
            return singleManager.GetCurrentEvents();
        }

        internal override void SetLastEventFilter(string itemName, LastEventFilter filter)
        {
            var singleManager = ValidateAndGetItem(itemName);
            singleManager.SetLastEventFilter(filter);
        }

        internal override Events.LastEventFilter GetLastEventFilter(string itemName)
        {
            var singleManager = ValidateAndGetItem(itemName, false);
            return singleManager.GetLastEventFilter();
        }

        protected S ValidateAndGetItem(string itemName, bool useLock = true)
        {
            return CurrentItems.ValidateAndGetItem(itemName, useLock);
        }

        protected S TryGetItem(string itemName, bool useLock = true)
        {
            return CurrentItems.TryGetItem(itemName, useLock);
        }

        protected void ValidateItemDoesNotExist(string itemName)
        {
            CurrentItems.ValidateKeyDoesNotExist(itemName);
        }



        protected virtual S CreateSingleManager(E newEntity, bool persistToProvider = false)
        {

            ValidateItemDoesNotExist(newEntity.Name);
            var singleManagerInstance = NewSingleManagerInstanceFromEntity(newEntity);

            if (persistToProvider)
                CreateUsingProvider(singleManagerInstance);

            CurrentItems.Add(newEntity.Name, singleManagerInstance);

            var entityWithProperties = newEntity as IEntityPropertyProvider;
            if (entityWithProperties != null && entityWithProperties.GetProperties().Startup == ItemStartupType.Automatic)
                singleManagerInstance.StartMonitoring();
            return singleManagerInstance;

        }

        internal void ChangeState(string itemName, ItemState newState)
        {
            var singleManager = ValidateAndGetItem(itemName);
            singleManager.ChangeState(newState);
        }

        public override void Shutdown()
        {
            base.Shutdown();
            var singleManagers = CurrentItems.GetCopiedList();
            foreach (var singleManager in singleManagers)
            {
                singleManager.Shutdown();
                singleManager.Dispose();
            }
        }



        public virtual void DeleteItem(string itemName)
        {
            S singleManager = ValidateAndGetItem(itemName);
            CurrentItems.RemoveItem(itemName);
            singleManager.Shutdown();
            singleManager.Delete();
            DeleteEntityFromProvider(singleManager);
        }

        public override void DelayedStartup()
        {
            try
            {
                IEnumerable<E> initialItems = null;
                try
                {
                    initialItems = RetreiveEntitiesFromProvider();
                }
                catch (Exception exc)
                {
                    throw new StartupException("Unable to get data from metadata provider. Quiting", exc);
                }
                foreach (var item in initialItems)
                {
                    var singleManager = CreateSingleManager(item);
                    singleManager.DelayedStartup();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                base.DelayedStartup();
            }
        }


        internal virtual List<E> GetCurrentEntityList()
        {
            return GetCurrentEntityListWithChecking(false);
        }        

        internal virtual List<E> GetCurrentEntityListWithChecking(bool doItemCheck)
        {
            var currentItems = CurrentItems.GetCopiedList();
            if (doItemCheck)
                return currentItems.Select(p => p.CheckAndSendItem()).ToList();
            else return currentItems.Select(p => p.Entity).ToList();
        }

        internal virtual E GetEntity(string itemName)
        {
            var item = CurrentItems.ValidateAndGetItem(itemName);
            return item.CheckAndSendItem();// item.Entity;
        }


    }
}

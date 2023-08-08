using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;

using Kalitte.Sensors.Interfaces;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections;
using Kalitte.Sensors.Processing.ServerAnalyse.Events;

namespace Kalitte.Sensors.Processing.Core.Sensor
{
    class SingleLogicalSensor : SingleManager<LogicalSensorEntity>
    {
        internal LogicalSensorManager manager { get; private set; }

        public SingleLogicalSensor(LogicalSensorManager manager, LogicalSensorEntity entity)
            : base(entity)
        {
            this.manager = manager;
        }

        public override void Shutdown()
        {
            MetadataManager.UpdateLogicalSensor(Entity);
            base.Shutdown();
        }

        protected internal override LogicalSensorEntity CheckAndSendItem()
        {

            return base.CheckAndSendItem();
        }

        protected override ItemStateInfo Run()
        {
            return ItemStateInfo.Running;
        }

        protected override ItemStateInfo Stop()
        {
            return ItemStateInfo.Stopped;
        }

        public override OperationManagerBase Manager
        {
            get { return this.manager; }
        }

        internal void NotifyProcessManager(object notificationObj)
        {
            //Logical2ProcessorBindingEntity[] bindings = null;
            //itemlock.EnterReadLock();
            //try
            //{
            //    bindings = Entity.ProcessorBindings.ToArray();
            //}
            //finally
            //{
            //    itemlock.ExitReadLock();
            //}
            //KeyValuePair<string, Events.SensorEventBase> notification = (KeyValuePair<string, Events.SensorEventBase>)notificationObj;
            //ServerManager.ProcessingManager.Notify(notification.Key, notification.Value, bindings);

            try
            {
                Logical2ProcessorBindingEntity[] bindings = Entity.ProcessorBindings.ToArray();
                KeyValuePair<string, Events.SensorEventBase> notification = (KeyValuePair<string, Events.SensorEventBase>)notificationObj;
                ServerManager.ProcessingManager.Notify(notification.Key, notification.Value, bindings);
            }
            catch (Exception exc)
            {
                Logger.Error("Error in SingleLogical.NotifyProcessManager. {0}", exc);

            }


        }

        internal void GetProcessorBinding(List<Logical2ProcessorBindingEntity> list, string processorName)
        {
            
            itemlock.EnterReadLock();
            try
            {
                var binding = Entity.ProcessorBindings.SingleOrDefault(p => p.ProcessorName == processorName);
                if (binding != null)
                    list.Add(binding);
            }
            finally
            {
                itemlock.ExitReadLock();
            }
        }

        public bool ProcessEvent(string sensorDeviceName, Events.SensorEventBase evt, Logical2SensorBindingEntity binding)
        {
            ArrayList list = new ArrayList();
            //itemlock.EnterReadLock();
            try
            {
                var foundBinding = true;
                ISensorObservation observation = evt as ISensorObservation;
                if (observation != null)
                    foundBinding = string.IsNullOrEmpty(binding.SensorSource) || observation.Source == binding.SensorSource;
                if (foundBinding)
                {
                    Manager.WatchManager.LogicalSensorEvent(Entity.Name, new LogicalSensorEventArgs(evt));
                    AddEventToLastEvents(Entity.Name, evt);
                    Thread t = new Thread(NotifyProcessManager);
                    list.Add(t);
                    t.Start(new KeyValuePair<string, Events.SensorEventBase>(Entity.Name, evt));
                }
                foreach (var thread in list)
                {
                    ((Thread)thread).Join();
                }
                return foundBinding;
            }
            finally
            {
                //itemlock.ExitReadLock();
            }
        }



        internal void Update(string description, LogicalSensorProperty properties)
        {
            StopMonitoring();
            itemlock.EnterWriteLock();
            try
            {

                Entity.Description = description;
                Entity.Properties.Startup = properties.Startup;
                Entity.Properties.Profile = properties.Profile;
                Entity.Properties.ExtendedProfile = properties.ExtendedProfile;

            }
            finally
            {
                itemlock.ExitWriteLock();
                StartMonitoring();
            }
        }

        internal void UpdateProcessorBinding(string processorName, IEnumerable<Logical2ProcessorBindingEntity> bindings)
        {
            itemlock.EnterWriteLock();
            try
            {
                if (bindings == null)
                    bindings = new List<Logical2ProcessorBindingEntity>();
                var ownedBindings = bindings.Where(p => p.LogicalSensorName == Entity.Name);
                var list = new List<Logical2ProcessorBindingEntity>(Entity.ProcessorBindings);
                list.RemoveAll(p=>p.ProcessorName == processorName);
                Entity.ProcessorBindings.Clear();
                foreach (var item in list)
                    Entity.ProcessorBindings.Add(item);
                foreach (var binding in ownedBindings)
                {
                    Entity.ProcessorBindings.Add(binding);
                }
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }
    }
}

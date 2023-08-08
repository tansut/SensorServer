using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.SensorDevices;
using Kalitte.Sensors.Commands;
using Kalitte.Sensors.Events;
using Kalitte.Sensors.Events.Management;
using System.Reflection;
using System.Threading;
using Kalitte.Sensors.Interfaces;
using Kalitte.Sensors.Exceptions;
using Kalitte.Sensors.Configuration;
using System.Collections.ObjectModel;


namespace Kalitte.Sensors.Processing.Core.Sensor
{
    internal class SingleSensor : SingleManager<SensorDeviceEntity>
    {
        public VirtualSensor Device { get; private set; }
        internal SensorManager SensorManager { get; private set; }

        protected internal override SensorDeviceEntity CheckAndSendItem()
        {
            SensorDeviceEntity entity = base.CheckAndSendItem();
            entity.GetProperties().StateInfo = GetStateInfo();
            return entity;

        }

        protected internal override ItemStateInfo GetStateInfo(bool useLock = true)
        {
            ItemStateInfo currentState;

            if (useLock) itemlock.EnterReadLock();
            try
            {
                currentState = base.GetStateInfo(false);
                bool lostDeviceConnection = Device == null || !Device.IsConnectionAlive();
                if (currentState.State == ItemState.Running && lostDeviceConnection)
                {
                    Stop();
                    currentState = new ItemStateInfo(ItemState.Stopped, "Device seems closed connection");
                    Entity.GetProperties().StateInfo = currentState;
                    Logger.Warning("Device {0} seems closed connection", Entity.Name);
                }
            }
            finally
            {
                if (useLock) itemlock.ExitReadLock();
            }

            return currentState;
        }

        public IEnumerable<Logical2SensorBindingEntity> GetLogicalSensorBindings()
        {
            itemlock.EnterReadLock();
            try
            {
                return Entity.LogicalSensorBindings.Select(p => p).AsEnumerable();
            }
            finally
            {
                itemlock.ExitReadLock();
            }
        }

        public override void Shutdown()
        {
            MetadataManager.UpdateSensorDevice(Entity);
            MetadataManager.UpdateLogical2SensorBindings(Entity.Name, Entity.LogicalSensorBindings.ToArray());
            base.Shutdown();
        }

        internal void UpdateLogicalSensorBindings(Logical2SensorBindingEntity[] bindings)
        {
            itemlock.EnterWriteLock();
            try
            {
                Entity.LogicalSensorBindings.Clear();
                foreach (var binding in bindings)
                {
                    Entity.LogicalSensorBindings.Add(binding);
                }
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }



        private void SyncActiveProfileWithExistingProfile()
        {
            PropertyList profile = GetCurrentProfileFromDevice();
            var metadata = ServerManager.SensorProviderManager.GetSensorMetadata(Entity.ProviderName);

            foreach (var item in profile)
            {
                DevicePropertyMetadata metadataOfItem;
                var metaDataFound = metadata.TryGetValue(item.Key, out metadataOfItem);
                if (metaDataFound)
                {
                    if (metadataOfItem.PropertyTargets == SensorPropertyRelation.Device ||
                        metadataOfItem.PropertyTargets == SensorPropertyRelation.DeviceAndSource)
                    {
                        if (!Entity.Properties.Profile.ContainsKey(item.Key))
                            Entity.Properties.Profile.Add(item.Key, item.Value);
                        //else Entity.Properties.Profile[item.Key] = item.Value;
                    }
                }
            }

            var sources = Device.GetSources();
            var nonExistentSources = Entity.Properties.SourceProfiles.Where(p => !sources.ContainsKey(p.Key)).Select(m=>m.Key);
            foreach (var item in nonExistentSources)
            {
                Entity.Properties.SourceProfiles.Remove(item);
            }
            foreach (var item in sources)
            {
                foreach (var property in item.Value)
                {
                    DevicePropertyMetadata metadataOfItem;
                    var metaDataFound = metadata.TryGetValue(property.Key, out metadataOfItem);
                    if (metaDataFound)
                    {

                        if (metadataOfItem.PropertyTargets == SensorPropertyRelation.Source ||
                            metadataOfItem.PropertyTargets == SensorPropertyRelation.DeviceAndSource)
                        {
                            PropertyList existingSourcePropertyList;
                            bool foundSourceProperty = Entity.Properties.SourceProfiles.TryGetValue(item.Key, out existingSourcePropertyList);
                            if (!foundSourceProperty)
                            {
                                existingSourcePropertyList = new PropertyList();
                                Entity.Properties.SourceProfiles.Add(item.Key, existingSourcePropertyList);
                            }
                            if (existingSourcePropertyList.ContainsKey(property.Key))
                                ;//existingSourcePropertyList[property.Key] = property.Value;
                            else existingSourcePropertyList.Add(property.Key, property.Value);
                        }
                    }
                }

            }
        }


        public PropertyList GetCurrentProfileFromDevice(string source = null)
        {
            ResponseEventArgs args;
            if (source == null)
                args = Device.ExecuteCommand(new GetActivePropertyListCommand());
            else args = Device.ExecuteCommand(source, new GetActivePropertyListCommand());
            if (args.CommandError == null)
            {
                GetActivePropertyListResponse response = (args.Command as GetActivePropertyListCommand).Response;
                return response.CurrentProfile;
            }
            return null;
        }

        public void ApplyCurrentProfileToSensor(SensorDeviceProperty properties)
        {
            ValidateState(ItemState.Running);
            var metaData = Manager.ProviderManager.GetSensorMetadata(Entity.ProviderName);
            Dictionary<PropertyKey, EntityMetadata> m = new Dictionary<PropertyKey, EntityMetadata>();
            foreach (var item in metaData)
                if (item.Value.PropertyTargets == SensorPropertyRelation.Device || item.Value.PropertyTargets == SensorPropertyRelation.DeviceAndSource)
                m.Add(item.Key, item.Value);
            var settableProps = properties.GetSettableProfile(properties.Profile, m);
            ApplyPropertyListCommand cmd = new ApplyPropertyListCommand(settableProps);
            ResponseEventArgs args = Device.ExecuteCommand(cmd);
            if (args.CommandError != null)
                throw new SensorException("Applying sensor profile failed", args.CommandError.ProviderException, args.CommandError.ErrorCode.ToString());
            m.Clear();
            foreach (var item in metaData)
                if (item.Value.PropertyTargets == SensorPropertyRelation.Source || item.Value.PropertyTargets == SensorPropertyRelation.DeviceAndSource)
                    m.Add(item.Key, item.Value);
            foreach (var source in properties.SourceProfiles)
            {
                settableProps = properties.GetSettableProfile(source.Value, m);
                cmd = new ApplyPropertyListCommand(settableProps);
                args = Device.ExecuteCommand(source.Key, cmd);
                if (args.CommandError != null)
                    throw new SensorException("Applying sensor profile failed", args.CommandError.ProviderException, args.CommandError.ErrorCode.ToString());
            }
        }


        public void AddDeviceName(SensorEventBase evt, string deviceName)
        {
            ISensorObservation observation = evt as ISensorObservation;
            if (observation != null)
            {
                observation.DeviceName = deviceName;
                AddEventToLastEvents(observation.Source, evt);
            }
            else
            {
                AddEventToLastEvents(null, evt);
                DeviceManagementEvent event2 = evt as DeviceManagementEvent;
                if (event2 != null)
                {
                    event2.DeviceName = deviceName;
                }
                else
                {
                    PropertyInfo prop = evt.GetType().GetProperty("DeviceName");
                    if (prop != null && prop.PropertyType == typeof(string))
                    {
                        try
                        {
                            prop.SetValue(evt, deviceName, null);
                        }
                        catch (System.Exception exc)
                        {
                            SensorManager.Logger.LogException("Cannot set devicename property for event {0} on {1}", exc, evt.ToString(), Entity.Name);
                        }
                    }
                }
            }
        }

        protected override ItemStateInfo Run()
        {
            ValidateState(ItemState.Stopped);

            if (this.SensorManager.ServerManager.SensorProviderManager.GetItemState(this.Entity.ProviderName) == ItemState.Running)
            {
                Device = SensorManager.ProviderManager.GetPhysicalSensor(Entity.ProviderName, Entity.Properties.Connection);
                Entity.Properties.StateInfo = new ItemStateInfo(ItemState.Stopped, "Tryring to connect");
                Device.SetupConnection(Entity.Properties.Authentication);
                Entity.Properties.StateInfo = new ItemStateInfo(ItemState.Running, "Connected. Applying profile.");
                return Run(Device);

            }
            else throw new SensorException(string.Format("{0} is not running", Entity.ProviderName));
        }

        private ItemStateInfo Run(VirtualSensor sensor)
        {
            try
            {
                this.Device = sensor;
                SyncActiveProfileWithExistingProfile();
                ApplyCurrentProfileToSensor(Entity.Properties);
                MetadataManager.UpdateSensorDevice(Entity);
                Device.CmdResponseEvent += device_CmdResponseEvent;
                Device.DeviceNotificationEvent += device_DeviceNotificationEvent;
                if (string.IsNullOrWhiteSpace(Entity.SensorId))
                    Entity.SensorId = Device.DeviceInformation.DeviceId;
                return new ItemStateInfo(ItemState.Running, "Connection Successful.");
            }
            catch (System.Exception)
            {
                Stop();
                throw;
            }
        }



        protected override ItemStateInfo Stop()
        {
            if (Device == null) // Connection'da oluşturulmamış olabilir.
                return ItemStateInfo.Stopped;

            Device.CmdResponseEvent -= device_CmdResponseEvent;
            Device.DeviceNotificationEvent -= device_DeviceNotificationEvent;
            try
            {
                try
                {
                    if (Device.IsConnectionAlive())
                        Device.Close();
                }
                finally
                {
                    Device = null;
                }
            }
            catch (System.Exception exc)
            {
                SensorManager.Logger.LogException("Error disconnecting from {0}", exc, Entity.Name);
            }

            return ItemStateInfo.Stopped;
        }

        public SingleSensor(SensorManager manager, SensorDeviceEntity entity)
            : base(entity)
        {
            this.SensorManager = manager;
            this.Device = null;
        }

        void device_DeviceNotificationEvent(object sender, Events.NotificationEventArgs e)
        {
            SensorEventBase evt = e.Notification.Event;
            AddDeviceName(evt, Entity.Name);
            SensorManager.Notify(this, evt);
            if (e.Notification.Event is DeviceConnectionDownEvent)
            {
                itemlock.EnterWriteLock();
                try
                {
                    Stop();
                    Entity.GetProperties().StateInfo = new ItemStateInfo(ItemState.Stopped, "Device connection down");
                }
                finally
                {
                    itemlock.ExitWriteLock();
                }
            }
        }

        void device_CmdResponseEvent(object sender, Commands.ResponseEventArgs e)
        {
            SensorManager.CmdResponse(this, e);
        }


        internal void Update(string sensorId, string description, SensorDeviceProperty properties)
        {
            bool reconnectNeeded = false;
            StopMonitoring();
            itemlock.EnterWriteLock();

            try
            {
                var oldItemState = Entity.State;
                if (Entity.Properties.Authentication == null || (!Entity.Properties.Authentication.Equals(properties.Authentication)))
                    reconnectNeeded = true;
                else if (!Entity.Properties.Connection.Equals(properties.Connection))
                    reconnectNeeded = true;

                if (oldItemState == ItemState.Running)
                {
                    ApplyCurrentProfileToSensor(properties);
                    
                }

                if (reconnectNeeded && oldItemState == ItemState.Running)
                    ChangeState(ItemState.Stopped);

                Entity.Properties.Profile = properties.Profile;
                Entity.Properties.Authentication = properties.Authentication;
                Entity.Properties.Connection = properties.Connection;
                Entity.Properties.Startup = properties.Startup;
                Entity.Properties.ExtendedProfile = properties.ExtendedProfile;
                Entity.Properties.MonitoringData = properties.MonitoringData;
                Entity.Properties.SourceProfiles = properties.SourceProfiles;
                Entity.Description = description;
                Entity.SensorId = sensorId;
                if (reconnectNeeded && oldItemState == ItemState.Running)
                {
                    ChangeStateDelayed(ItemState.Running);
                }
            }
            finally
            {
                itemlock.ExitWriteLock();
                StartMonitoring();
            }


        }

        internal Dictionary<string, Sensors.Configuration.PropertyList> GetSources()
        {
            ValidateState(ItemState.Running);
            return Device.GetSources();
        }

        internal ResponseEventArgs ExecuteCommand(string source, SensorCommand command)
        {
            ValidateState(ItemState.Running);
            return Device.ExecuteCommand(source, command);
        }

        public override OperationManagerBase Manager
        {
            get { return this.SensorManager; }
        }



        internal void HandleDiscoveredSensor(DiscoveryEventArgs e, VirtualSensor sensor)
        {
            itemlock.EnterWriteLock();
            try
            {
                this.Entity.Properties.StateInfo = new ItemStateInfo(ItemState.Running);
                this.Entity.Properties.StateInfo = Run(sensor);
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }

        internal void SetProfile(string source, PropertyList properties)
        {
            ValidateState(ItemState.Running);
        }
    }
}

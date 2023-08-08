using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Core.Process
{
    class SingleEventModule : SingleModule<EventModuleEntity>
    {
        public EventModuleManager manager { get; set; }

        public SingleEventModule(EventModuleManager manager, EventModuleEntity entity)
            : base(entity)
        {
            this.manager = manager;
        }

        internal EventModuleMetadata GetMetadata()
        {
            return base.GetMetadata<EventModuleMetadata>(Entity.TypeQ);
        }

        protected internal override EventModuleEntity CheckAndSendItem()
        {
            EventModuleMetadata metaDataOfItem = base.GetMetadata<EventModuleMetadata>(Entity.TypeQ);
            if (metaDataOfItem != null)
            {
                Dictionary<PropertyKey, EntityMetadata> metaData = GetEntityMetadata<EventModulePropertyMetadata>(metaDataOfItem.ModulePropertyMetadata);
                SyncProfile(Entity.Properties.Profile, metaData);
            }
            return base.CheckAndSendItem();
        }

        

        public override OperationManagerBase Manager
        {
            get { return manager; }
        }

        protected override ItemStateInfo Run()
        {
            return ItemStateInfo.Running;
        }

        protected override ItemStateInfo Stop()
        {
            return ItemStateInfo.Stopped;

        }

        internal void Update(string description, string type, EventModuleProperty properties)
        {
            itemlock.EnterWriteLock();
            try
            {

                Entity.Description = description;
                Entity.Properties.Startup = properties.Startup;
                Entity.TypeQ = type;
                Entity.Properties.Profile = properties.Profile;
                Entity.Properties.ExtendedProfile = properties.ExtendedProfile;
            }
            finally
            {
                itemlock.ExitWriteLock();
            }
        }
    }
}

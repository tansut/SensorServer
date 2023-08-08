using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Web.Business
{
    public class SensorProviderBusiness: EntityBusiness<SensorProviderEntity>
    {
        public override SensorProviderEntity GetItem(string id)
        {
            return SensorProxy.GetSensorProvider(id);
        }

        public SensorProviderEntity Create(string name, string description, string type, ItemStartupType startup)
        {
            return SensorProxy.CreateSensorProvider(name, description, type, startup);
        }

        public override void UpdateItem(SensorProviderEntity entity)
        {
            SensorProxy.UpdateSensorProvider(entity.Name, entity.Description, entity.TypeQ, entity.Properties);
        }

        public override void DeleteItem(string id)
        {
            SensorProxy.DeleteSensorProvider(id);
        }

        public override System.Collections.IList GetItems()
        {
            return SensorProxy.GetSensorProviders();
        }

        public override void ChangeState(string id, Processing.ItemState newState)
        {
            SensorProxy.ChangeProviderState(id, newState);
        }

        public ProviderMetadata GetProviderMetadata(string providerName)
        {
            return SensorProxy.GetSensorProviderMetadata(providerName);
        }
    }
}

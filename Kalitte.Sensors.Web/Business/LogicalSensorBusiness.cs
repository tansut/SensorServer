using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Web.Business
{
    public class LogicalSensorBusiness: EntityBusiness<LogicalSensorEntity>
    {
        public override LogicalSensorEntity GetItem(string id)
        {
            return SensorProxy.GetLogicalSensor(id);
        }

        public override void UpdateItem(LogicalSensorEntity entity)
        {
            SensorProxy.UpdateLogicalSensor(entity.Name, entity.Description, entity.Properties);
        }

        public override void DeleteItem(string id)
        {
            SensorProxy.DeleteLogicalSensor(id);
        }

        public override System.Collections.IList GetItems()
        {
            return SensorProxy.GetLogicalSensors();
        }

        public override void ChangeState(string id, Processing.ItemState newState)
        {
            SensorProxy.ChangeLogicalSensorState(id, newState);
        }

        public LogicalSensorEntity CreateItem(string sensorName, string description, ItemStartupType startup)
        {
            return SensorProxy.CreateLogicalSensor(sensorName, description, startup);
        }

        public Logical2SensorBindingEntity[] GetSensorBindings(string logicalSensorName)
        {
            return SensorProxy.GetSensor2LogicalBindings(logicalSensorName);
        }

         
    }
}

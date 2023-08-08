using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing;

namespace Kalitte.Sensors.Web.Business
{
    public abstract class EntityBusiness<E> : BusinessBase where E : class
    {
        public abstract E GetItem(string id);
        //public abstract void CreateItem(E entity);
        public abstract void UpdateItem(E entity);
        public abstract void DeleteItem(string id);
        public abstract void ChangeState(string id, ItemState newState);
    }
}

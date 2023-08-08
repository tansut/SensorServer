using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Processing.Utilities;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Processing.Core
{
    abstract class SingleModule<E> : SingleManager<E> where E : PersistEntityBase, IEntityPropertyProvider
    {
        public SingleModule(E entity)
            : base(entity)
        {
        }




       
    }
}

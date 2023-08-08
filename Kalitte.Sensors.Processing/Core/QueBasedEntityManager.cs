using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;


namespace Kalitte.Sensors.Processing.Core
{
    abstract class QueBasedEntityManager<S, E, VR> : EntityOperationManager<S, E>
        where S : QueBasedSingleManager<E, VR>
        where E : PersistEntityBase, IEntityPropertyProvider
        where VR: VirtualRunnableBase
    {
        internal QueBasedEntityManager(ServerManager serverManager)
            : base(serverManager)
        {

        }



    }
}

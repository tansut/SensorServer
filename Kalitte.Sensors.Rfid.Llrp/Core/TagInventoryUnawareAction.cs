namespace Kalitte.Sensors.Rfid.Llrp.Core
{
    using System;

    public enum TagInventoryUnawareAction
    {
        SelectMatchUnselectNonMatch,
        SelectMatchDoNothingNonMatch,
        DoNothingMatchUnselectNonMatch,
        UnselectMatchDoNothingNonMatch,
        UnselectMatchSelectNonMatch,
        DoNothingMatchSelectNonMatch
    }
}

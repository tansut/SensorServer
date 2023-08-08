﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Processing.Metadata
{
    public interface IEntityPropertyProvider
    {
        string Name { get; }
        EntityPropertyBase GetProperties();
    }
}

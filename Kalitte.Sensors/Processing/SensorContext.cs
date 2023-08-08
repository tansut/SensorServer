using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Core;

namespace Kalitte.Sensors.Processing
{
    public abstract class SensorContext: ISensorContext
    {
        private Dictionary<string, object> contextObjects;
        protected static SensorContext current;
        protected static object currentLock = new object();
        private ILogger logger;

        protected SensorContext(Dictionary<string, object> contextObjects, ILogger logger)
        {
            this.contextObjects = contextObjects;
            this.logger = logger;
            Current = this;
        }

        public static ILogger GetLogger(string name)
        {
            return AppContext.GetLogger(name);
        }

        public Dictionary<string, object> ContextObjects
        {
            get
            {
                return this.contextObjects;
            }
        }

        public static SensorContext Current
        {
            get
            {
                lock (currentLock)
                {
                    return current;
                }
            }
            internal protected set
            {
                lock (currentLock)
                {
                    current = value;
                }
            }
        }

        public ILogger Logger
        {
            get
            {
                return this.logger;
            }
            internal protected set
            {
                this.logger = value;
            }
        }
    }
}

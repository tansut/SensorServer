using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing.Metadata;
using Kalitte.Sensors.Core;
using System.Threading;

namespace Kalitte.Sensors.Processing.Core
{
    internal class ServerItemMonitor
    {
        private ItemMonitoringData data;
        private IRunnable runnable;
        private ILogger logger;
        private long retryCount = 0;
        private Thread stateCheckThread;
        private volatile bool isRunning;
        ManualResetEvent runWait;
        private object stateLock = new object();
        private string name;

        public long RetryCount
        {
            get
            {
                return Interlocked.Read(ref retryCount);
            }
        }

        public void Stop()
        {
            lock (stateLock)
            {
                if (isRunning)
                {
                    isRunning = false;
                    runWait.Set();
                    stateCheckThread.Join(30000);
                }
            }
        }

        private void tryRunItem()
        {
            while (isRunning)
            {
                try
                {
                    if (data.MaxRetryCount != 0 && RetryCount > data.MaxRetryCount)
                    {
                        logger.Warning("Stopping monitoring item {0} due to retry count.", name);
                        Stop();
                        break;
                    }
                    runWait.WaitOne(data.CheckInterval);
                    if (isRunning == false)
                        break;
                    ItemState currentState = runnable.GetState();
                    if (currentState == ItemState.Stopped)
                        runnable.RunItem();
                }
                catch
                {
                    Interlocked.Increment(ref retryCount);
                }
            }
        }

        public void Start()
        {
            lock (stateLock)
            {
                if (!isRunning)
                {
                    this.retryCount = 0;
                    if (data.Enabled && data.CheckInterval > 0)
                    {
                        stateCheckThread = new Thread(tryRunItem);
                        isRunning = true;
                        runWait.Reset();
                        stateCheckThread.Start();
                    }
                }
            }
        }

        public ServerItemMonitor(IRunnable runnable, ItemMonitoringData data, ILogger logger, string name)
        {
            this.data = (ItemMonitoringData)data.Clone();
            this.runnable = runnable;
            this.logger = logger;
            isRunning = false;
            this.name = name;
            runWait = new ManualResetEvent(false);
        }
    }
}

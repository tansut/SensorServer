namespace Kalitte.Sensors.Rfid.Llrp.Communication
{
    using System;
    using System.Threading;
    using Kalitte.Sensors.Rfid.Llrp.Properties;

    internal abstract class AsyncResult : IAsyncResult
    {
        private AsyncCallback m_callback;
        private bool m_completedSynchronously;
        private bool m_endCalled;
        private Exception m_exception;
        private bool m_isCompleted;
        private ManualResetEvent m_manualResetEvent;
        private object m_state;
        private object m_thisLock;

        protected AsyncResult(AsyncCallback callback, object state)
        {
            
            this.m_callback = callback;
            this.m_state = state;
            this.m_thisLock = new object();
        }

        protected void Complete(bool completedSynchronously)
        {
            if (this.m_isCompleted)
            {
                throw new InvalidOperationException(LlrpResources.AsynchronousResultCompleteCalledTwice);
            }
            this.m_completedSynchronously = completedSynchronously;
            if (completedSynchronously)
            {
                this.m_isCompleted = true;
            }
            else
            {
                lock (this.m_thisLock)
                {
                    this.m_isCompleted = true;
                    if (this.m_manualResetEvent != null)
                    {
                        this.m_manualResetEvent.Set();
                    }
                }
            }
            if (this.m_callback != null)
            {
                this.m_callback(this);
            }
        }

        internal void Complete(bool completedSynchronously, Exception exception)
        {
            this.m_exception = exception;
            this.Complete(completedSynchronously);
        }

        protected static TAsyncResult End<TAsyncResult>(IAsyncResult result) where TAsyncResult: AsyncResult
        {
            if (result == null)
            {
                throw new ArgumentNullException("result");
            }
            TAsyncResult local = result as TAsyncResult;
            if (local == null)
            {
                throw new ArgumentException("Invalid async result.", "result");
            }
            if (local.m_endCalled)
            {
                throw new InvalidOperationException("Async object already ended.");
            }
            local.m_endCalled = true;
            if (!local.m_isCompleted)
            {
                local.AsyncWaitHandle.WaitOne();
            }
            if (local.m_manualResetEvent != null)
            {
                local.m_manualResetEvent.Close();
            }
            if (local.m_exception != null)
            {
                throw local.m_exception;
            }
            return local;
        }

        public object AsyncState
        {
            get
            {
                return this.m_state;
            }
        }

        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (this.m_manualResetEvent == null)
                {
                    lock (this.m_thisLock)
                    {
                        if (this.m_manualResetEvent == null)
                        {
                            this.m_manualResetEvent = new ManualResetEvent(this.m_isCompleted);
                        }
                    }
                }
                return this.m_manualResetEvent;
            }
        }

        public bool CompletedSynchronously
        {
            get
            {
                return this.m_completedSynchronously;
            }
        }

        public bool IsCompleted
        {
            get
            {
                return this.m_isCompleted;
            }
        }
    }
}

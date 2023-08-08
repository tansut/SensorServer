namespace Kalitte.Sensors.Rfid.Llrp.Communication
{
    using System;

    internal class TypedAsyncResult<T> : AsyncResult
    {
        private T m_data;

        internal TypedAsyncResult(AsyncCallback callback, object state) : base(callback, state)
        {
        }

        internal void Complete(T data, bool completedSynchronously)
        {
            this.m_data = data;
            base.Complete(completedSynchronously);
        }

        public static T End(IAsyncResult result)
        {
            return AsyncResult.End<TypedAsyncResult<T>>(result).Data;
        }

        public T Data
        {
            get
            {
                return this.m_data;
            }
        }
    }
}

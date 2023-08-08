using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Diagnostics;
using System.Threading;

namespace Kalitte.Sensors.Processing.Utilities
{
    public class SafeDictionary<T> 
    {
        private System.Threading.ReaderWriterLockSlim listLock = new System.Threading.ReaderWriterLockSlim();
        private Dictionary<string, T> itemsHolder = new Dictionary<string, T>();

        //public T this [int index] {
        //    get
        //    {
        //        return itemsHolder.ind
        //    }
        //}

        public T First
        {
            get
            {
                return itemsHolder.First().Value;
            }
        }

        public void EnterReadLock()
        {
            listLock.EnterReadLock();
        }

        public void EnterWriteLock()
        {
            listLock.EnterWriteLock();
        }

        public void ExitReadLock()
        {
            listLock.ExitReadLock();
        }

        public void ExitWriteLock()
        {
            listLock.ExitWriteLock();
        }

        internal IEnumerable<KeyValuePair<string, T>> GetCopiedItems()
        {
            listLock.EnterReadLock();
            try
            {
                List<KeyValuePair<string, T>> items = new List<KeyValuePair<string, T>>(itemsHolder.Count);
                foreach (KeyValuePair<string, T> item in itemsHolder)
                    items.Add(new KeyValuePair<string, T>(item.Key, item.Value));
                return items.AsEnumerable();
            }
            finally
            {
                listLock.ExitReadLock();
            }
        }

        internal IEnumerable<T> GetCopiedList()
        {
            listLock.EnterReadLock();
            try
            {
                List<T> items = new List<T>(itemsHolder.Count);
                foreach (var item in itemsHolder)
                    items.Add(item.Value);
                return items.AsEnumerable();
            }
            finally
            {
                listLock.ExitReadLock();
            }
        }

        internal void ValidateKeyDoesNotExist(string itemName)
        {
            listLock.EnterReadLock();
            try
            {
                if (itemsHolder.ContainsKey(itemName))
                    throw new InvalidOperationException(string.Format("{0} already exist in list", itemName));
            }
            finally
            {
                listLock.ExitReadLock();
            }
        }


        internal void RemoveItem(string itemName)
        {
            listLock.EnterWriteLock();
            try
            {
                if (!itemsHolder.ContainsKey(itemName))
                    throw new InvalidOperationException(string.Format("{0} doesnot exist in list", itemName));
                else itemsHolder.Remove(itemName);
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        internal T ValidateAndGetItem(string itemName, bool useLock = true)
        {

            T item = TryGetItem(itemName, useLock);
            if (item == null)
                throw new InvalidOperationException(string.Format("{0} doesnot exist in list", itemName));
            return item;

        }

        internal void Add(string itemName, T item)
        {
            listLock.EnterWriteLock();
            try
            {
                Trace.WriteLine(Thread.CurrentThread.Name + " " + itemName + ":" + item.ToString());
                itemsHolder.Add(itemName, item);
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }





        #region IEnumerable Members

        public IEnumerator GetEnumerator()
        {
            return itemsHolder.GetEnumerator();
        }

        #endregion

        internal T TryGetItem(string itemName, bool useLock = true)
        {
            if (useLock)
            {
                listLock.EnterReadLock();
                try
                {
                    if (!itemsHolder.ContainsKey(itemName))
                        return default(T);
                    return itemsHolder[itemName];
                }
                finally
                {
                    listLock.ExitReadLock();
                }
            }
            else
            {
                if (!itemsHolder.ContainsKey(itemName))
                    return default(T);
                return itemsHolder[itemName];
            }
        }



        internal void Clear()
        {
            listLock.EnterWriteLock();
            try
            {
                itemsHolder.Clear();
            }
            finally
            {
                listLock.ExitWriteLock();
            }
        }

        internal void AddIfNotExits(string itemName, T item)
        {
            T value = TryGetItem(itemName);
            if (value == null)
            {
                listLock.EnterWriteLock();
                try
                {
                    if (!itemsHolder.ContainsKey(itemName))
                        itemsHolder.Add(itemName, item);
                }
                finally
                {
                    listLock.ExitWriteLock();
                }
            }
          
        }

        internal void RemoveItemIfExists(string itemName)
        {
            T value = TryGetItem(itemName);
            if (value != null)
            {
                listLock.EnterWriteLock();
                try
                {
                    if (!itemsHolder.ContainsKey(itemName))
                        itemsHolder.Remove(itemName);
                }
                finally
                {
                    listLock.ExitWriteLock();
                }
            }
        }

        public int Count
        {
            get
            {
                listLock.EnterReadLock();
                try
                {
                    return itemsHolder.Count;
                }
                finally
                {
                    listLock.ExitReadLock();
                }
            }
        }
    }
}

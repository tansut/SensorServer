using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Utilities;
using Kalitte.Sensors.Processing.Utilities;

namespace Kalitte.Sensors.Processing.Core
{
    internal class ItemMetadataManager
    {
        private static Dictionary<string, object> cache = new Dictionary<string, object>();

        //private static SafeDictionary<object> extendedMetadataCache = new SafeDictionary<object>();
        private static SafeDictionary<object> metadataCache = new SafeDictionary<object>();
        private static SafeDictionary<ExtendedMetadata> metadataOfItemCache = new SafeDictionary<ExtendedMetadata>();

        private ServerManager serverManager;

        public ItemMetadataManager(ServerManager serverManager)
        {
            this.serverManager = serverManager;
        }

        public ExtendedMetadata GetExtendedMetadata(ProcessingItem itemType)
        {
            lock (cache)
            {
                if (cache.ContainsKey(itemType.ToString())) {
                    return (ExtendedMetadata)cache[itemType.ToString()];
                }
                try
                {
                    var eventModules = serverManager.EventModuleManager.GetCurrentEntityListWithChecking(false).Where(p=>p.State == ItemState.Running);
                    ExtendedMetadata extendedMetadata = new ExtendedMetadata(new Dictionary<PropertyKey, ExtendedPropertyMetadata>());
                    foreach (var item in eventModules)
                    {
                        ExtendedMetadata metaData = null;
                        try
                        {
                            metaData = GetTypeMetadataOfItem(item.TypeQ, itemType);
                        }
                        catch (System.Exception exc)
                        {
                            serverManager.Logger.LogException("Error in getting metadata of {0}", exc, item.Name);
                        }
                        
                        if (metaData != null)
                        {
                            foreach (var property in metaData.PropertyMetadata)
                            {
                                if (!extendedMetadata.PropertyMetadata.ContainsKey(property.Key))
                                    extendedMetadata.PropertyMetadata.Add(property.Key, property.Value);
                            }
                        }
                    }
                    cache[itemType.ToString()] = extendedMetadata;
                    return extendedMetadata;
                }
                catch (System.Exception exc)
                {
                    serverManager.Logger.LogException("Error in getting metadata", exc);
                    return null;
                }
            }
        }

        public void ClearCache()
        {
            lock (cache)
            {
                cache.Clear();
            }
        }

        public T GetTypeMetadata<T>(string type)
        {
            object result = metadataCache.TryGetItem(type);
            if (result != null)
                return (T)result;
            result = MarshalHelper.GetMetadata<T>(type);
            metadataCache.AddIfNotExits(type, result);
            return (T)result;
        }

        public ExtendedMetadata GetTypeMetadataOfItem(string type, ProcessingItem itemType)
        {
            ExtendedMetadata result = metadataOfItemCache.TryGetItem(type);
            if (result != null)
                return result;
            result = MarshalHelper.GetMetadataOfItem<ExtendedMetadata>(type, itemType);
            metadataOfItemCache.AddIfNotExits(type, result);
            return result;
        }
    }
}

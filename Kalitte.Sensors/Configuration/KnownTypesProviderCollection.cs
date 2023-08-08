using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;

namespace Kalitte.Sensors.Configuration
{
    public sealed class KnownTypesProviderCollection : ProviderCollection
    {

        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (!(provider is KnownTypesProvider))
            {
                throw new ArgumentException("Provider must be a KnownTypesProvider");
            }
            base.Add(provider);
        }

        public void CopyTo(KnownTypesProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }

        public KnownTypesProvider this[string name]
        {
            get
            {
                return (KnownTypesProvider)base[name];
            }
        }
    }

}

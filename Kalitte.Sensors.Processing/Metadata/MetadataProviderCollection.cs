using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Provider;
using Kalitte.Sensors.Processing.Metadata;

namespace Kalitte.Sensors.Processing.Metadata
{
    internal sealed class MetadataProviderCollection : ProviderCollection
    {

        public override void Add(ProviderBase provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException("provider");
            }
            if (!(provider is MetadadataProvider))
            {
                throw new ArgumentException("Provider must be a WidgetFrameworkProvider");
            }
            base.Add(provider);
        }

        public void CopyTo(MetadadataProvider[] array, int index)
        {
            base.CopyTo(array, index);
        }

        public MetadadataProvider this[string name]
        {
            get
            {
                return (MetadadataProvider)base[name];
            }
        }
    }


}

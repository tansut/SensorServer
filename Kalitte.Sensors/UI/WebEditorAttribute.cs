using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.UI
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public abstract class WebEditorAttribute : Attribute
    {
        readonly string userControlPath;

        protected WebEditorAttribute(string userControlPath)
        {
            this.userControlPath = userControlPath;
        }

        public string UserControlPath
        {
            get { return userControlPath; }
        }

    }
}

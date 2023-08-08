using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace Kalitte.Sensors.Web.UI
{
    public abstract class BaseEditor<T> : UserControl where T : class
    {
        public T Current
        {
            get
            {
                if (ViewState["Current"] == null) ViewState["Current"] = Activator.CreateInstance<T>();
                return (T)ViewState["Current"];
            }
            set
            {
                ViewState["Current"] = value;
            }
        }

        public abstract void Bind(T entity);
        public abstract void Retrieve(T entity);
        public abstract void Clear();        
    }
}

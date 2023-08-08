using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.SetupConfiguration.Core.Data;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Core.Exception;

namespace Kalitte.Sensors.SetupConfiguration.Core
{
    public class BaseEditor<T> : UserControl, IEditor<T> where T : class
    {
        public T Current { get; set; }

        #region IEditor<T> Members

        public virtual bool IsValid()
        {
            throw new TechnicalException("Method must be override");
        }

        public virtual void Bind(T curent)
        {
            throw new TechnicalException("Method must be override");
        }

        public virtual void Retrieve(T curent)
        {
            throw new TechnicalException("Method must be override");
        }

        public virtual string WizardMessage
        {
            get { throw new TechnicalException("Method must be override"); }
        }


        public virtual bool IsProcessControl
        {
            get { return false; }
        }

        public virtual event ProcessingCompletedHandler ProcessingCompleted = null;

        public virtual event ProcessingStartedHandler ProcessingStarted = null;

        #endregion
    }
}

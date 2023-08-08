using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net.Utilities;
using System.Web.UI;
using Kalitte.Sensors.Web.Core;
using System.Reflection;
using Kalitte.Sensors.Web.Business;

namespace Kalitte.Sensors.Web.UI
{
    public class ViewPageBase : BasePage, ICommandHandler
    {
        ControlCommandHandler handler = new ControlCommandHandler();
        class MvcData
        {
            public BaseViewUserControl ListerControl { get; set; }
            public BaseViewUserControl EditorControl { get; set; }
            public BusinessBase BusinessObject { get; set; }
        }



        public virtual bool ProcessCommand(object sender, CommandInfo command)
        {

            return handler.ProcessCommand(sender, command, this);
        }

        private Dictionary<Type, MvcData> businessTable;
        
        protected override void OnInit(EventArgs e)
        {
            List<BaseViewUserControl> userControls = ControlUtils.FindControls<BaseViewUserControl>(this);
            businessTable = new Dictionary<Type, MvcData>(2);

            foreach (BaseViewUserControl control in userControls)
            {
                PropertyInfo propBo = control.GetType().GetProperty("BusinessObject");
                if (propBo != null)
                {
                    BusinessBase businessToSet = null;
                    if (businessTable.ContainsKey(propBo.PropertyType))
                    {
                        MvcData data = businessTable[propBo.PropertyType];
                        businessToSet = data.BusinessObject;
                        if (control.ControlType == ViewControlType.Editor)
                            data.EditorControl = control;
                        else if (control.ControlType == ViewControlType.Lister)
                            data.ListerControl = control;
                    }
                    else
                    {
                        businessToSet = (BusinessBase)Activator.CreateInstance(propBo.PropertyType);
                        MvcData data = new MvcData() { BusinessObject = businessToSet };
                        if (control.ControlType == ViewControlType.Editor)
                            data.EditorControl = control;
                        else if (control.ControlType == ViewControlType.Lister)
                            data.ListerControl = control;
                        businessTable[propBo.PropertyType] = data;
                    }
                    propBo.SetValue(control, businessToSet, new object[] { });
                }
            }

            base.OnInit(e);
        }

        public T GetBusinessObject<T>() where T: BusinessBase
        {

            if (businessTable.ContainsKey(typeof(T)))
                return (T)businessTable[typeof(T)].BusinessObject;
            else return null;
        }

        public ListerViewControl<T> GetLister<T>() where T : BusinessBase
        {
            if (businessTable.ContainsKey(typeof(T)))
                return (ListerViewControl<T>)businessTable[typeof(T)].ListerControl;
            else return null;
        }

        public EditorViewControl<T> GetEditor<T>() where T : BusinessBase
        {
            if (businessTable.ContainsKey(typeof(T)))
                return (EditorViewControl<T>)businessTable[typeof(T)].EditorControl;
            else return null;
        }
    }
}

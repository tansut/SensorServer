using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using Ext.Net.Utilities;

namespace Kalitte.Sensors.Web.Controls
{
    public enum LabelFieldSource
    {
        UseNote,
        UseFieldLabel,
    }

    public class TTCompositeField : CompositeField
    {

        public LabelFieldSource LabelSource { get; set; }

        public override void ClearInvalid()
        {
            base.ClearInvalid();
            ControlUtils.FindControls<Field>(this).Each(p => p.ClearInvalid());
        }

        public override void Clear()
        {
            base.Clear();
            ControlUtils.FindControls<Field>(this).Each(p => p.Clear());
            ClearInvalid();
        }


        public void SetLabelSource(Component obj, ItemsCollection<Component> objItems)
        {
            int baseCtr = 0;
            foreach (object o in objItems)
            {
                if (o is Field)
                {
                    Field f = (Field)o;
                    f.HideLabel = true;
                    f.NoteAlign = Ext.Net.NoteAlign.Top;
                    if (!string.IsNullOrWhiteSpace(f.FieldLabel))
                        f.Note = f.FieldLabel + f.LabelSeparator;
                    else f.Note = "&nbsp;";
                    f.NoteCls = "noteCls";
                    baseCtr++;
                }
                else if (o is ContainerBase)
                {
                    ContainerBase c = (ContainerBase)o;
                    SetLabelSource(c, c.Items);
                }
                else if (o is Component)
                {
                    Component m = (Component)o;
                    if (!m.StyleSpec.Contains("margin-top"))
                    {                        
                            m.StyleSpec += "margin-top:14px;";                            
                    }
                }
            }
            if (baseCtr == 0)
            {
                obj.HideLabel = false;
                obj.HideLabels = false;
            }
            else
            {
                obj.HideLabel = true;
                obj.HideLabels = true;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (LabelSource == LabelFieldSource.UseNote && !Ext.Net.X.IsAjaxRequest)
            {
                SetLabelSource(this, this.Items);
            }
        }
    }
}

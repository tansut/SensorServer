using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.UI;
using Kalitte.Sensors.Utilities;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTPropertyGridParameter : PropertyGridParameter
    {
        public EntityMetadata Metadata { get; set; }
        public PropertyKey PropertyKey { get; set; }
        public PropertyProfileGrid Owner { get; set; }
        private bool hasCustomEditor;

        public Field GetEditor(EntityMetadata metaData)
        {
            Type type = metaData.Type;
            Field fieldCreated = null;

            if (type == typeof(int) || type == typeof(int?))
            {
                TTNumberField field = new TTNumberField();
                field.AllowDecimals = false;
                fieldCreated = field;

            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                TTComboBox field = new TTComboBox();
                field.Items.Add(new ListItem("true", "true"));
                field.Items.Add(new ListItem("false", "false"));
                fieldCreated = field;
            }

            else if (metaData.ValueSet != null && metaData.ValueSet.Count > 0)
            {
                TTComboBox field = new TTComboBox();
                object[] list = metaData.ValueSet.ToArray();
                foreach (var item in list)
                    field.Items.Add(new ListItem(item.ToString(), item.ToString()));
                fieldCreated = field;
            }
            else if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                TTDateField field = new TTDateField();
                fieldCreated = field;
            }
            else if (type != null)
            {
                if (Metadata.HasCustomPropertyEditor())
                {
                    TriggerField field = new TriggerField();
                    field.ID = Owner.ID + SensorCommon.RemoveNonCharsAndDigits(PropertyKey.GroupName + PropertyKey.PropertyName);
                    FieldTrigger trigger = new FieldTrigger();
                    trigger.Icon = TriggerIcon.SimpleEllipsis;
                    trigger.Tag = metaData.Type.FullName;
                    field.Triggers.Add(trigger);
                    fieldCreated = field;
                    hasCustomEditor = true;
                    field.Listeners.TriggerClick.Handler = string.Format("propertyEditorWindow('{0}', '{1}', '{2}', '{3}', '{4}', '{5}');",
                        type.FullName, metaData.GetType().FullName, PropertyKey.GroupName, PropertyKey.PropertyName, field.ID, Owner.ResolveClientUrl("~/Pages/Shared/LoadCustomPropertyEditor.aspx"));

                }
            }
            else fieldCreated = new TTTextField();

            if (fieldCreated != null)
            {
                fieldCreated.ReadOnly = !metaData.IsWritable;
            }

            return fieldCreated;
        }



        public TTPropertyGridParameter()
            : base()
        {

        }

        public TTPropertyGridParameter(Config config)
            : base(config)
        {
        }


        public override EditorCollection Editor
        {
            get
            {
                EditorCollection coll = base.Editor;
                if (Metadata != null && coll.Count == 0)
                {
                    Field f = GetEditor(Metadata);
                    if (f != null)
                        coll.Add(f);
                }
                return coll;
            }
        }
        public override Renderer Renderer
        {
            get
            {
                var renderer = base.Renderer;
                if (this.Metadata != null && this.Metadata.Type == typeof(DateTime))
                {
                    renderer = new Renderer("return new Ext.util.Format.dateRenderer('Y/m/d');");
                    return renderer;
                }
                if ((Metadata != null && !Metadata.IsWritable) || hasCustomEditor)
                {
                    renderer.Handler = "metadata.attr = \"style='color: gray;'\"; return value;";
                    //renderer.Fn = "RenderColor";


                }
                return renderer;
            }
        }

    }
}

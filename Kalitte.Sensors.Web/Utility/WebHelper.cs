using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Globalization;
using System.Threading;
using Ext.Net;
using System.Reflection;
using System.ComponentModel;
using System.Security.Cryptography;
using Kalitte.Sensors.Configuration;
using Kalitte.Sensors.Web.Controls;

namespace Kalitte.Sensors.Web.Utility
{
    public enum MessageType
    {
        Info,
        Warning,
        Error,
        InfoAsFloating
    }

    public static class WebHelper
    {
        public static void SetUserLocale()
        {
            HttpRequest Request = HttpContext.Current.Request;
            if (Request.UserLanguages == null)
                return;

            string Lang = Request.UserLanguages[0];
            if (Lang != null)
            {
                if (Lang.Length < 3)
                    Lang = Lang + "-" + Lang.ToUpper();
                try
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    Thread.CurrentThread.CurrentUICulture = culture;
                }
                catch
                { ;}
            }
        }

        public static void ShowMessage(string message)
        {
            ShowMessage(message, MessageType.Info);
        }

        public static void ShowMessage(string message, MessageType type)
        {
            string messageHtml = HttpUtility.HtmlEncode(message);
            messageHtml = messageHtml.Replace("\n", "<br />");
            if (type == MessageType.InfoAsFloating)
            {

                Ext.Net.Notification.Show(new NotificationConfig()
                {
                    Icon = Ext.Net.Icon.Information,
                    Title = "Operation Successfully",
                    Html = "<b>" + messageHtml + "</b>",
                    CloseVisible = true,
                    AlignCfg = new NotificationAlignConfig
                    {
                        ElementAnchor = AnchorPoint.TopRight,
                        TargetAnchor = AnchorPoint.TopRight,
                        OffsetX = -10,
                        OffsetY = 10
                    },
                    ShowFx = new Frame { Color = "C3DAF9", Count = 1, Options = new FxConfig { Duration = 2 } },
                    HideFx = new SwitchOff(),
                    Pinned = false,
                    ShowPin = true,
                    AutoScroll = true
                });
            }
            else

                X.MessageBox.Show(new MessageBoxConfig()
                {
                    Message = messageHtml,
                    Title = "Message",
                    Icon = (type == MessageType.Info ? MessageBox.Icon.INFO :
                           (type == MessageType.Warning ? MessageBox.Icon.WARNING : MessageBox.Icon.ERROR)),
                    Buttons = MessageBox.Button.OK
                });

        }


        public static Dictionary<string, string> GetDescriptionalEnumInfo(Type enumType, Func<KeyValuePair<string, string>, bool> filter = null)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            string[] names = Enum.GetNames(enumType);
            foreach (string item in names)
            {
                string desc;
                desc = GetEnumDescription(enumType, item);
                if (desc != string.Empty)
                {
                    result.Add(item, desc);
                }
                else
                {
                    result.Add(item, item);
                }
            }
            if (filter != null)
                result = result.Where(filter).ToDictionary(p => p.Key, p => p.Value);
            return result;
        }

        public static string GetEnumDescription(Type enumType, string enumItem)
        {
            string result = string.Empty;
            MemberInfo[] member = enumType.GetMember(enumItem);
            if (member != null && member.Length > 0)
            {
                object[] att = member[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (att != null)
                {
                    DescriptionAttribute desc = att.Where(p => p.GetType() == (typeof(DescriptionAttribute))).Select(u => (DescriptionAttribute)u).SingleOrDefault();
                    if (desc != null) result = desc.Description;
                }
            }
            return result;

        }

        public static void BindMetadataToControls(Dictionary<PropertyKey, EntityProperty> defaultProfile,
            Dictionary<PropertyKey, EntityMetadata> metaData, PropertyProfileEditor editor)
        {

        }
    }
}

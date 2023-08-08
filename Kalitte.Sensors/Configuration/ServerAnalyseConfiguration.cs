using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Processing;
using System.Xml.Linq;
using Kalitte.Sensors.Extensions;
namespace Kalitte.Sensors.Configuration
{
    [Serializable]
    public class ServerAnalyseConfiguration
    {
        public Dictionary<ServerAnalyseItem, ServerAnalyseLevel> Levels { get; private set; }

        public ServerAnalyseConfiguration(Dictionary<ServerAnalyseItem, ServerAnalyseLevel> levels)
        {
            Levels = new Dictionary<ServerAnalyseItem, ServerAnalyseLevel>(levels);
        }

        public ServerAnalyseConfiguration(XElement element)
        {
            Levels = new Dictionary<ServerAnalyseItem, ServerAnalyseLevel>();
            if (element == null)
            {
                foreach (var item in Enum.GetValues(typeof(ServerAnalyseItem)))
                {
                    Levels.Add((ServerAnalyseItem)item, ServerAnalyseLevel.Detailed);
                }
            }
            else
            {
                var att = element.Attributes("defaultLevel").FirstOrDefault();
                ServerAnalyseLevel defVal = ServerAnalyseLevel.Detailed;
                if (att != null) defVal = att.Value.ToEnum<ServerAnalyseLevel>();
                foreach (var item in Enum.GetValues(typeof(ServerAnalyseItem)))
                {
                    var cElement = element.Descendants(item.ToString()).FirstOrDefault();
                    if (cElement != null)
                        Levels.Add((ServerAnalyseItem)item, cElement.Value.ToEnum<ServerAnalyseLevel>());
                    else Levels.Add((ServerAnalyseItem)item, defVal);
                }
            }
        }

        public ServerAnalyseLevel GetLevel(ServerAnalyseItem item)
        {
            if (Levels.ContainsKey(item))
                return Levels[item];
            else return ServerAnalyseLevel.None;
        }
        
    }
}

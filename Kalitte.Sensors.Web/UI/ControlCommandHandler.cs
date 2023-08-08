using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kalitte.Sensors.Web.Core;
using Ext.Net.Utilities;
using System.Web.UI;
using System.Reflection;

namespace Kalitte.Sensors.Web.UI
{
    public class ControlCommandHandler
    {

        #region ICommandHandler Members

        public bool ProcessCommand(object sender, CommandInfo command, Control ownercontrol)
        {
            List<BaseViewUserControl> userControls = ControlUtils.FindControls<BaseViewUserControl>(ownercontrol);
            List<Control> controlsToCheck = new List<Control>(userControls);



            controlsToCheck.Add(ownercontrol);

            object controlToCall = null;
            MethodInfo methodToCall = null;

            foreach (var control in controlsToCheck)
            {
                object objectInstance = control;

                var methods = objectInstance.GetType().BaseType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.NonPublic);

                foreach (var method in methods)
                {
                    object[] attributes = method.GetCustomAttributes(typeof(CommandHandlerAttribute), false);
                    if (attributes.Length > 0)
                    {
                        CommandHandlerAttribute attribute = attributes[0] as CommandHandlerAttribute;

                        if (command.Source != null && attribute.ControllerType != null)
                        {
                            if (!attribute.ControllerType.IsAssignableFrom(command.Source.ControllerObject.GetType()))
                                break;
                        }

                        if (command.KnownCommand != Security.KnownCommand.None)
                        {
                            if (attribute.KnownCommand == command.KnownCommand)
                            {
                                methodToCall = method;
                                break;
                            }
                        }
                        else if (command.CommandName.Equals(attribute.CommandName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            methodToCall = method;
                            break;
                        }
                    }
                }

                if (methodToCall != null)
                {
                    controlToCall = objectInstance;
                    break;
                }
            }

            if (controlToCall != null)
            {
                methodToCall.Invoke(controlToCall, new object[] { sender, command });
                return true;
            }
            else throw new InvalidOperationException("Cannot find command handler for: " + command.CommandName);
        }

        #endregion
    }
}

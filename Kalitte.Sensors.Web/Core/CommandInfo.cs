using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ext.Net;
using Kalitte.Sensors.Web.Security;
using Kalitte.Sensors.Web.Core;
using Kalitte.Sensors.Web.Controls;


namespace Kalitte.Sensors.Web.Core
{
    public class CommandResult
    {
        public bool Success { get; set; }

        private Dictionary<string, object> parameters;

        public Dictionary<string, object> Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new Dictionary<string, object>();
                return parameters;
            }
            private set
            {
                Parameters.Clear();
                foreach (var item in value)
                    Parameters.Add(item.Key, item.Value);
            }
        }

        public string Message { get; set; }

        public CommandResult()
        {
            Success = true;
        }
    }


    public sealed class CommandInfo
    {
        public string CommandName { get; private set; }
        public KnownCommand KnownCommand { get; private set; }

        private Dictionary<string, object> parameters;
        private CommandResult result = null;
        public DirectEventArgs EventArgs { get; set; }
        public ICommandSource Source { get; internal set; }

        public CommandResult Result
        {
            get
            {
                if (result == null)
                    result = new CommandResult();
                return result;
            }
        }

        public Dictionary<string, object> Parameters
        {
            get
            {
                if (parameters == null)
                    parameters = new Dictionary<string, object>();
                return parameters;
            }
            private set
            {
                Parameters.Clear();
                foreach (var item in value)
                    Parameters.Add(item.Key, item.Value);
            }
        }
        public CommandInfo(string commandName)
        {
            this.CommandName = commandName;
            var knownCmd = Security.KnownCommand.None;
            string knownCommandString = commandName;
            if (!string.IsNullOrEmpty(knownCommandString))
                Enum.TryParse<KnownCommand>(knownCommandString, true, out knownCmd);
            this.KnownCommand = knownCmd;
        }

        public CommandInfo(string commandName, Dictionary<string, object> parameters)
            : this(commandName)
        {
            if (parameters != null)
                Parameters = parameters;
        }


        public CommandInfo(string commandName, ParameterCollection coll)
            : this(commandName)
        {
            foreach (var item in coll)
                Parameters.Add(item.Name, item.Value);
        }

        public CommandInfo(string commandName, DirectEventArgs e) :
            this(commandName, e.ExtraParams)
        {
            this.EventArgs = e;
        }

        public CommandInfo(GridRowCommandEventArgs e)
            : this(e.Command)
        {
            foreach (var item in e.E.ExtraParams)
                Parameters.Add(item.Name, item.Value);
        }

        public string RecordID
        {
            get
            {
                if (Parameters.ContainsKey("id"))
                    return Parameters["id"].ToString();
                else return string.Empty;
            }
            set
            {
                Parameters["id"] = value;
            }
        }
        public int RowIndex
        {
            get
            {
                return int.Parse(Parameters["rowIndex"].ToString());
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Kalitte.Sensors.Commands
{
    [Serializable]
    public sealed class ResponseEventArgs : EventArgs
    {
        // Fields
        private readonly SensorCommand command;
        private readonly CommandError error;

        // Methods
        public ResponseEventArgs(SensorCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            this.command = command;
        }

        public ResponseEventArgs(SensorCommand command, CommandError error)
            : this(command)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            this.error = error;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<responseEventArgs>");
            builder.Append("<command>");
            builder.Append(this.command);
            builder.Append("</command>");
            builder.Append("<error>");
            builder.Append(this.error);
            builder.Append("</error>");
            builder.Append("</responseEventArgs>");
            return builder.ToString();
        }

        // Properties
        public SensorCommand Command
        {
            get
            {
                return this.command;
            }
        }

        public CommandError CommandError
        {
            get
            {
                return this.error;
            }
        }
    }




}

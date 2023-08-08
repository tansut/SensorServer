using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.SetupConfiguration.Core
{
    public delegate void ProcessingCompletedHandler(object sender,EventArgs e);
    public delegate void ProcessingStartedHandler(object sender, EventArgs e);
    public interface IEditor<T>
    {
         bool IsValid();
         void Bind(T curent);
         void Retrieve(T curent);
         string WizardMessage { get; }
         bool IsProcessControl { get; }
         event ProcessingCompletedHandler ProcessingCompleted;
         event ProcessingStartedHandler ProcessingStarted;
    }
}


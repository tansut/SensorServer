using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Kalitte.Sensors.SetupConfiguration.Forms;

namespace Kalitte.Sensors.SetupConfiguration.Core.Exception
{
    public static class ExceptionManager
    {
        public delegate void UserExceptionHandler(System.Exception e);
        public static event UserExceptionHandler OnUserException = null;

        public static void Manage(System.Exception e)
        {
            if (e.GetType() == typeof(UserException))
            {
                if (OnUserException != null) OnUserException(e);
            }
            else
            {
                ShowException(e);
            }
        }

        public static void ShowException(System.Exception e)
        {
            ExceptionForm eform = new ExceptionForm();
            eform.Execute(e);
        }


    }
}

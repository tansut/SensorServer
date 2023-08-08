using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalitte.Sensors.Web.Controls
{
    public class TTCommandColumn : Ext.Net.CommandColumn
    {
        private int mergeColumnIndex = -1;

        public int MergeColumnIndex 
        {
            get
            {
               return mergeColumnIndex;
            }
            set
            {
                mergeColumnIndex = value;
            }
        }
    }
}

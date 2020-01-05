using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VoltAnalyzer.ValueObjects.Types.Common
{
    public static class MessageConstants
    {
        #region general messages

        /// <summary>
        /// Set the message shown on the busy overlay
        /// </summary>
        public static String SetBusyMessage { get { return "SetBusyMessage"; } }

        /// <summary>
        /// Show an message page with content of object in input
        /// </summary>
        public static String ShowMessageView { get { return "ShowMessageView"; } }

        public static String ChangeGraph { get { return "ChangeGraph"; } }
        #endregion
    }
}

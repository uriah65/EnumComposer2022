using EnumComposer;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer_2022
{
    internal class EnumLog : IEnumLog
    {
        private IVsOutputWindow _outputWindow;

        public EnumLog(IVsOutputWindow outputWindow)
        {
            _outputWindow = outputWindow;
        }

        public void WriteLine(string format, params object[] arguments)
        {
            Guid generalPaneGuid = VSConstants.GUID_OutWindowDebugPane;//.GUID_OutWindowGeneralPane;
            IVsOutputWindowPane outputPane;
            _outputWindow.GetPane(ref generalPaneGuid, out outputPane);
            if (outputPane != null)
            {
                string message = string.Format($"{Environment.NewLine}{DateTime.Now.ToString("HH:mm:ss")} EC: ");
                if (arguments.Length != 0)
                {
                    message += string.Format(format, arguments);
                }
                else
                {
                    message += format;
                }
                outputPane.OutputString(message);
                outputPane.Activate();
            }
        }
    }
}
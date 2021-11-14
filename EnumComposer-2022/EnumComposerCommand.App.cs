using EnumComposer;
using EnumComposerVSIX;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer_2022
{
    internal sealed partial class EnumComposerCommand
    {
        public void ApplyComposer(TextDocument document, ComposerStrings composer)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            /* get document bounds */
            EditPoint startEdit = document.CreateEditPoint(document.StartPoint);
            EditPoint endEdit = document.EndPoint.CreateEditPoint();

            /* run composer */
            string text = startEdit.GetText(document.EndPoint);
            composer.Compose(text);
            if (composer.EnumModels != null && composer.EnumModels.Count > 0)
            {
                /* get new file*/
                text = composer.GetResultFile();

                /* delete and re-insert full document */
                startEdit.Delete(endEdit);
                startEdit.Insert(text);
            }
        }

        public string Reverse(string text)
        {
            /* test method, not used */
            char[] cArray = text.ToCharArray();
            string reverse = "";
            for (int i = cArray.Length - 1; i > -1; i--)
            {
                reverse += cArray[i];
            }
            return reverse;
        }

        private TextDocument ObtainActiveDocument(DTE2 applicationObject)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                /* query ActiveDocument can cause exception if active document f.e. is project properties */
                if (applicationObject.ActiveDocument == null)
                {
                    return null;
                }

                TextDocument document = (TextDocument)applicationObject.ActiveDocument.Object("TextDocument");
                return document;
            }
            catch
            {
                /* see notes in try{} */
                return null;
            }
        }

        private void RunComposerScan(IEnumLog log)
        {
            try
            {
                ThreadHelper.ThrowIfNotOnUIThread();

                RunComposerScan_Inner(log);
            }
            catch (Exception ex)
            {
                string message = "Sorry, and exception has occurred." + Environment.NewLine + Environment.NewLine + ex.Message + Environment.NewLine + Environment.NewLine + "See the Output\\Debug window for details.";
                if (log != null)
                {
                    string logMessage = DedbugLog.ExceptionMessage(ex);
                    log.WriteLine(logMessage);
                }

                string title = "EnumComposerCommand";

                VsShellUtilities.ShowMessageBox(
                    this.package,
                    message,
                    title,
                    OLEMSGICON.OLEMSGICON_INFO,
                    OLEMSGBUTTON.OLEMSGBUTTON_OK,
                    OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
            }
        }

        private void RunComposerScan_Inner(IEnumLog log)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            DTE2 applicationObject = (DTE2)Package.GetGlobalService(typeof(EnvDTE.DTE));

            TextDocument document = ObtainActiveDocument(applicationObject);
            if (document == null)
            {
                log.WriteLine("not a C# file.");
                return;
            }

            DbReader dbReader = new DbReader(null, null, log);
            IEnumConfigReader configReaderVsp = new ConfigReaderVsp(applicationObject.ActiveDocument.ProjectItem.ContainingProject);
            dbReader._configReader = configReaderVsp;

            ComposerStrings composer = new ComposerStrings(dbReader, log);
            ApplyComposer(document, composer);
        }
    }
}
using PoshWebDriver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace PoshWorkflowTestUI
{
    public class PSRunner : IDisposable
    {
        private static PSRunner _instance;
        private PowerShell _pw;
        private WebDriverSession _session;

        public static PSRunner Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new PSRunner();

                return _instance;
            }
        }

        private PSRunner()
        {
            _pw = PowerShell.Create();

            startDriver();
            importPSModules();
        }

        public void StartSession()
        {
            if (_session == null)
            {
                _pw.AddCommand("New-WebDriverSession").AddParameter("Url", "http://localhost:9515/");
                Collection<PSObject> result = _pw.Invoke();
                _session = result.Where(x => x.BaseObject.GetType() == typeof(WebDriverSession)).FirstOrDefault().BaseObject as WebDriverSession;
            }
        }

        public void Invoke(Command[] commands)
        {
            _pw.Commands.Clear();

            foreach (var command in commands)
                _pw.Commands.AddCommand(command);

            _pw.Invoke();
        }

        public void Invoke(Command command)
        {
            _pw.Commands.Clear();
            _pw.Commands.AddCommand(command);
            _pw.Invoke();
        }

        private void importPSModules()
        {
            _pw.AddScript(string.Format("Import-Module {0}", Path.GetFullPath(@"../../../packages\PoshWebDriver.0.0.1\lib\net45\PoshWebDriver.dll")));
            _pw.Invoke();
            _pw.Commands.Clear();
        }

        private void startDriver()
        {
            _pw.AddCommand("Get-Process").AddArgument("chromedriver");
            Collection<PSObject> result = _pw.Invoke();
            _pw.Commands.Clear();


            if (result.Count == 0)
            {
                _pw.AddCommand("Start-Process").AddArgument(Path.GetFullPath(@"../../../../tools\chromedriver\chromedriver.exe"));
            }
        }

        public void Dispose()
        {
            _pw.Dispose();
            _instance = null;
            _session = null;
        }
    }
}

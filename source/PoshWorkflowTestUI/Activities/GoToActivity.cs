using System;
using System.Activities;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PoshWorkflowTestUI.Activities
{
    public class GoToActivity : NativeActivity, IActivityTemplateFactory
    {
        [RequiredArgument]
        public InArgument<string> Url { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            var uriBuilder = new UriBuilder(Url.Get(context).ToString());
            MainWindow.pw.Commands.Clear();
            MainWindow.pw.AddCommand("Set-WebDriverSessionUrl").AddParameter("Url", uriBuilder.Uri.ToString());
            MainWindow.pw.Invoke();
        }

        public Activity Create(DependencyObject target)
        {
            return new GoToActivity() { DisplayName = "GoTo" };
        }
    }
}

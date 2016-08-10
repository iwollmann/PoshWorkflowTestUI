using System;
using System.Activities;
using System.Activities.Presentation;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
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
            var command = new Command("Set-WebDriverSessionUrl");
            command.Parameters.Add("Url", uriBuilder.Uri.ToString());

            PSRunner.Instance.Invoke(command);
        }

        public Activity Create(DependencyObject target)
        {
            return new GoToActivity() { DisplayName = "GoTo" };
        }
    }
}

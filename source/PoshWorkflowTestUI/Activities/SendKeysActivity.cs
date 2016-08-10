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
    public class SendKeysActivity : NativeActivity, IActivityTemplateFactory
    {
        [RequiredArgument]
        public InArgument<string> Value { get; set; }

        [RequiredArgument]
        public InArgument<string> ElementCssSelector { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            var command = new Command("Set-WebDriverSessionSendKeys");
            command.Parameters.Add("Element", ElementCssSelector.Get(context));
            command.Parameters.Add("Keys", new string[] { Value.Get(context) });

            PSRunner.Instance.Invoke(command);
        }

        public Activity Create(DependencyObject target)
        {
            return new SendKeysActivity() { DisplayName = "SendKeys" };
        }
    }
}

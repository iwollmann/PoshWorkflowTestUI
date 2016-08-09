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
    public class SendKeysActivity : NativeActivity, IActivityTemplateFactory
    {
        [RequiredArgument]
        public InArgument<string> Value { get; set; }

        [RequiredArgument]
        public InArgument<string> ElementCssSelector { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            MainWindow.pw.Commands.Clear();
            MainWindow.pw.AddCommand("Set-WebDriverSessionSendKeys")
                .AddParameters(new Dictionary<string, object>() {
                    { "Element", ElementCssSelector.Get(context) },
                    { "Keys",new string[] { Value.Get(context) } } });
            MainWindow.pw.Invoke();
        }

        public Activity Create(DependencyObject target)
        {
            return new SendKeysActivity() { DisplayName = "SendKeys" };
        }
    }
}

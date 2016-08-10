using PoshWorkflowTestUI.Activities;
using System;
using System.Activities;
using System.Activities.Core.Presentation;
using System.Activities.Presentation;
using System.Activities.Presentation.Toolbox;
using System.Activities.Presentation.View;
using System.Activities.Statements;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace PoshWorkflowTestUI
{

    public partial class MainWindow : Window
    {
        private WorkflowDesigner wd;
        private PSRunner _runner;

        public MainWindow()
        {
            InitializeComponent();
            RegisterMetadata();
            AddDesigner();

            this.AddToolBox();
            this.AddPropertyInspector();
        }

        private void AddDesigner()
        {
            //Create an instance of WorkflowDesigner class.
            this.wd = new WorkflowDesigner();

            //Place the designer canvas in the middle column of the grid.
            Grid.SetColumn(this.wd.View, 1);
            Grid.SetRow(this.wd.View, 1);
            Grid.SetRowSpan(this.wd.View, 2);

            //Load a new Flow as default.
            this.wd.Load(new Flowchart());

            //Hide variables and other items
            var designerView = wd.Context.Services.GetService<DesignerView>();
            designerView.WorkflowShellBarItemVisibility = ShellBarItemVisibility.MiniMap | ShellBarItemVisibility.Zoom;

            //Add the designer canvas to the grid.
            mainGrid.Children.Add(this.wd.View);
        }


        private void RegisterMetadata()
        {
            DesignerMetadata dm = new DesignerMetadata();
            dm.Register();
        }

        private ToolboxControl GetToolboxControl()
        {
            // Create the ToolBoxControl.
            ToolboxControl ctrl = new ToolboxControl();

            // Create a category.
            ToolboxCategory commandsCategory = new ToolboxCategory("Commands");

            this.GetType().Assembly.GetTypes().Where(x => x.BaseType == typeof(NativeActivity))
                .ToList().ForEach(x => commandsCategory.Add(new ToolboxItemWrapper(x)));

            ctrl.Categories.Add(commandsCategory);
            return ctrl;
        }

        private void AddToolBox()
        {
            ToolboxControl tc = GetToolboxControl();
            Grid.SetColumn(tc, 0);
            Grid.SetRow(tc, 1);
            Grid.SetRowSpan(tc, 2);
            mainGrid.Children.Add(tc);
        }

        private void AddPropertyInspector()
        {
            Grid.SetColumn(wd.PropertyInspectorView, 2);
            Grid.SetRow(wd.PropertyInspectorView, 1);
            Grid.SetRowSpan(wd.PropertyInspectorView, 2);
            mainGrid.Children.Add(wd.PropertyInspectorView);
        }

        private void OnRunClick(object sender, System.Windows.RoutedEventArgs e)
        {
            wd.Flush();
            var workflowStream = new MemoryStream(ASCIIEncoding.Default.GetBytes(wd.Text));
            var wf = System.Activities.XamlIntegration.ActivityXamlServices.Load(workflowStream);
            var app = new WorkflowApplication(wf);

            _runner = PSRunner.Instance;
            _runner.StartSession();

            app.Run();

            app.Completed = (x) =>
            {
                //pw.Dispose();
            };
        }
    }
}
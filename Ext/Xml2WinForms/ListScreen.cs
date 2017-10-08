using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public partial class ListScreen : UserControl
    {
        private string _templateName;
        private string _templateBaseDirectory;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CurrentEntity => mTunableListView.CurrentEntity;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuItemResolverDelegate MenuItemResolver
        {
            get => mTunableListView.MenuItemResolver;
            set => mTunableListView.MenuItemResolver = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<IEnumerable<ListItemContent>> RefreshCallback { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<Exception> ErrorAction { get; set; }

        public ListScreen()
        {
            InitializeComponent();

            mTunableListView.ServiceCommand += (sender, args) =>
            {
                ServiceCommand?.Invoke(this, args);
            };
        }

        public void ApplyTemplate(ListScreenTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            _templateName = template.TemplateName;
            _templateBaseDirectory = template.BaseDirectory;

            if (null == template.TunableList)
                throw new BadTemplateException("null == template.TunableList");

            mTunableListView.ApplyTemplate(template.TunableList);

            // Command buttons
            if (0 != template.CommandButtons.Count)
            {
                commandFlowLayoutPanel.SuspendLayout(); // SuspendLayout

                foreach (var buttonTemplate in template.CommandButtons)
                {
                    var tunableButton = new TunableButton
                    {
                        Margin = refreshButton.Margin,
                        Size = new Size(refreshButton.Width, refreshButton.Height)
                    };

                    tunableButton.ApplyTemplate(buttonTemplate);

                    tunableButton.ServiceCommand += (sender, args) => ServiceCommand?.Invoke(sender, args);
                    commandFlowLayoutPanel.Controls.Add(tunableButton);
                }

                commandFlowLayoutPanel.ResumeLayout(); // ResumeLayout
            }
        }

        public void DisplayContent(List<ListItemContent> contentItems)
        {
            if (null == contentItems)
                throw new ArgumentNullException(nameof(contentItems));

            mTunableListView.DisplayContent(contentItems);
        }

        public void RefreshContent()
        {
            refreshButton_Click(this, null);
        }

        public void Reset()
        {
            mTunableListView.Reset();

            // CommandBar.
            var outdatedControls = (from c in commandFlowLayoutPanel.Controls.Cast<Control>()
                where !c.Name.Equals(refreshButton.Name, StringComparison.Ordinal)
                select c).ToList();

            commandFlowLayoutPanel.SuspendLayout(); // SuspendLayout
            foreach (Control control in outdatedControls)
            {
                commandFlowLayoutPanel.Controls.Remove(control);
                control.Dispose();
            }
            commandFlowLayoutPanel.ResumeLayout(); // ResumeLayout
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            ServiceCommand?.Invoke(this, new CommandEventArgs {Command = "BeginRefresh"});
            refreshButton.Enabled = false;

            mBackgroundWorker.RunWorkerAsync();
        }

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = RefreshCallback?.Invoke();
        }

        private void mBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ServiceCommand?.Invoke(this, new CommandEventArgs { Command = "EndRefresh" });

            if (IsDisposed)
                return;

            refreshButton.Enabled = true;

            var error = e.Error;

            if (null != error)
            {
                if (null != ErrorAction)
                    ErrorAction(error);
                else
                {
                    var errorFormTemplate = new ErrorFormTemplate
                    {
                        Caption = error.GetType().Name,
                        Message = error.Message,
                        Details = error.ToString()
                    };

                    errorFormTemplate.SetTemplateInternals(_templateName, _templateBaseDirectory);

                    ErrorForm.ShowDialog(this, errorFormTemplate);
                }

                return;
            }

            if (null != e.Result)
                mTunableListView.DisplayContent((List<ListItemContent>) e.Result);
        }
    }
}
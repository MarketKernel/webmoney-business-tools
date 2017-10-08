using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public delegate bool MenuItemResolverDelegate(object currentEntity, string command);

    public sealed class TunableMenu : ContextMenuStrip
    {
        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuItemResolverDelegate MenuItemResolver { get; set; }

        public TunableMenu()
        {
        }

        public TunableMenu(IContainer container)
            : base(container)
        {
        }

        public void ApplyTemplate(TunableMenuTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            var toolStripItems = new List<ToolStripItem>();

            foreach (var itemTemplate in template.Items)
            {
                if (null == itemTemplate.Text)
                    throw new BadTemplateException("null == itemTemplate.Text");

                if ("-".Equals(itemTemplate.Text, StringComparison.OrdinalIgnoreCase))
                {
                    toolStripItems.Add(new ToolStripSeparator());
                    continue;
                }

                var image = itemTemplate.BuildIconHolder()?.TryBuildImage();

                var toolStripLabel = new ToolStripMenuItem(itemTemplate.Text, image, OnMenuItemClick);

                toolStripItems.Add(toolStripLabel);

                if (null == itemTemplate.Command)
                    continue;

                toolStripLabel.Tag = itemTemplate.Command;
            }

            SuspendLayout();
            Items.AddRange(toolStripItems.ToArray());
            ResumeLayout();
        }

        public void Reset()
        {
            var items = new List<ToolStripItem>(Items.Cast<ToolStripItem>()).ToList();

            SuspendLayout(); // SuspendLayout
            foreach (var item in items)
            {
                item.Dispose();
            }
            Items.Clear();
            ResumeLayout(); // ResumeLayout
        }

        private void OnMenuItemClick(object sender, EventArgs e)
        {
            var toolStripItem = sender as ToolStripItem;

            var commandText = toolStripItem?.Tag as string;

            if (commandText == null)
                return;

            ServiceCommand?.Invoke(this, new CommandEventArgs {Command = commandText});
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            base.OnOpening(e);

            bool hide = true;

            foreach (ToolStripItem item in Items)
            {
                var command = item.Tag as string;

                if (null == command)
                    continue;

                if (null != MenuItemResolver && !MenuItemResolver(null, command))
                {
                    item.Enabled = false;
                    continue;
                }

                item.Enabled = true;
                hide = false;
            }

            if (hide)
                e.Cancel = true;
        }
    }
}
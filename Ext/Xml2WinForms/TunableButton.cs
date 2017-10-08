using System;
using System.ComponentModel;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed class TunableButton : Button, IServiceControl
    {
        private string _command;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        public TunableButton()
        {
            Click += (sender, args) =>
            {
                ServiceCommand?.Invoke(this, new CommandEventArgs { Command = _command });
            };
        }

        public void ApplyTemplate(TunableButtonTemplate template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Text = template.Text;
            Enabled = template.Enabled;
            _command = template.Command;
        }

        public void Reset()
        {
            Text = string.Empty;
            Enabled = true;
            _command = null;
        }
    }
}

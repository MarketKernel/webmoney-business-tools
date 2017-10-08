using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed class TunableGroupBox : GroupBox, IServiceControl
    {
        private readonly List<IControlHolder> _controlHolders = new List<IControlHolder>();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal IEnumerable<IControlHolder> ControlHolders => _controlHolders;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        public void ApplyTemplate<TColumnTemplate>(GroupBoxTemplate<TColumnTemplate> template)
            where TColumnTemplate : class, IShapeColumnTemplate
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            Text = template.Desc;
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            if (null != template.Name)
                Name = template.Name;

            var innerControlWidth = template.ControlWidth -
                                    (Padding.Left + Padding.Right + ControlTemplate.ControlMargin +
                                     ControlTemplate.ControlMargin);

            // Корректировка шаблонов вложенных элементов (устанавливаем ширину).
            foreach (var controlTemplate in template.Column.Controls)
            {
                controlTemplate.ControlWidth = innerControlWidth;
            }

            var controlHolders = ControlHolderHelper.BuildControlHolders(template.Column.Controls);

            var tunableShape = new TunableShape
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Location = new Point(DisplayRectangle.Location.X, DisplayRectangle.Location.Y)
            };

            tunableShape.BuildSingleColumn(controlHolders);

            SuspendLayout();
            Controls.Add(tunableShape);
            ResumeLayout();

            _controlHolders.AddRange(controlHolders);

            foreach (var controlHolder in ControlHolders)
            {
                var serviceControl = controlHolder.Control as IServiceControl;

                if (null == serviceControl)
                    continue;

                serviceControl.ServiceCommand += (sender, args) =>
                {
                    ServiceCommand?.Invoke(serviceControl, args);
                };
            }
        }

        public void Reset()
        {
            var outdatedControls = Controls.Cast<Control>().ToList();

            SuspendLayout(); // SuspendLayout
            foreach (Control control in outdatedControls)
            {
                control.Dispose();
            }

            Controls.Clear();
            ResumeLayout(); // ResumeLayout

            _controlHolders.Clear();
        }
    }
}

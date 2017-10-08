using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.StronglyTypedWrappers;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions.Forms
{
    internal sealed partial class RequestNumberSettingsForm : Form
    {
        private readonly SessionContext _context;

        public RequestNumberSettingsForm(SessionContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            InitializeComponent();
        }

        private void RequestNumberSettingsForm_Load(object sender, EventArgs e)
        {
            var owner = Owner;

            StartPosition = FormStartPosition.Manual;
            Location = new Point(owner.Location.X + (owner.Width - Width) / 2,
                owner.Location.Y + (owner.Height - Height) / 2);
        }

        public void ApplyTemplate(TunableShapeTemplate<ShapeColumnTemplate> template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            var requestNumberSettings = _context.Session.AuthenticationService.GetRequestNumberSettings();

            mTunableShape.ApplyTemplate(template);

            var valuesWrapper =
                new RequestNumberSettingsFormValuesWrapper
                {
                    Control1GenerationMethod = requestNumberSettings.Method,
                    Control2Increment = requestNumberSettings.Increment
                };

            mTunableShape.ApplyValues(valuesWrapper.CollectIncomeValues());
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            var values = mTunableShape.SelectValues();

            var valuesWrapper = new RequestNumberSettingsFormValuesWrapper(values);

            var generationMethod = valuesWrapper.Control1GenerationMethod;
            var increment = valuesWrapper.Control2Increment;

            var requestNumber = _context.Session.AuthenticationService.GetRequestNumber(generationMethod, increment);

            liveViewTextBox.Text = requestNumber.ToString(CultureInfo.CurrentCulture.NumberFormat);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var values = mTunableShape.SelectValues();

            var valuesWrapper = new RequestNumberSettingsFormValuesWrapper(values);

            var generationMethod = valuesWrapper.Control1GenerationMethod;
            var increment = valuesWrapper.Control2Increment;

            var authenticationService = _context.Session.AuthenticationService;

            var requestNumberSettings = authenticationService.GetRequestNumberSettings();
            requestNumberSettings.Method = generationMethod;
            requestNumberSettings.Increment = increment;
            authenticationService.SetRequestNumberSettings(requestNumberSettings);

            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

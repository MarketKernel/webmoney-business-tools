using System;
using System.Globalization;
using System.Windows.Forms;
using Xml2WinForms.Templates;

namespace WMBusinessTools.Extensions
{
    public partial class RequestNumberSettingsForm : Form
    {
        public RequestNumberSettingsForm()
        {
            InitializeComponent();
        }

        public void ApplyTemplate(TunableShapeTemplate<ShapeColumnTemplate> template)
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            mTunableShape.ApplyTemplate(template);
        }

        private void mTimer_Tick(object sender, EventArgs e)
        {
            if (IsDisposed)
                return;

            var values = mTunableShape.SelectValues();

            if (values.Count != 2)
                throw new InvalidOperationException("values.Count != 2");

            if (null == values[0] || null == values[1])
                return;

            ulong requestNumber;

            var generationMethod = (string)values[0];

            if (generationMethod.Equals("UnixTimeStamp"))
                requestNumber = (uint) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            else if (generationMethod.Equals("yyMMddHHmmssfff"))
            {
                string timestamp = DateTime.UtcNow.ToString("yyMMddHHmmssfff", CultureInfo.InvariantCulture.DateTimeFormat);
                requestNumber = ulong.Parse(timestamp, NumberStyles.Integer, CultureInfo.InvariantCulture.NumberFormat);
            }
            else
                throw new InvalidOperationException("generationMethod == " + generationMethod);

            requestNumber += (ulong) (decimal) values[1];

            liveViewTextBox.Text = requestNumber.ToString(CultureInfo.CurrentCulture.NumberFormat);
        }

        private void okButton_Click(object sender, EventArgs e)
        {

        }
    }
}

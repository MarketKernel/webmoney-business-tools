using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public partial class ChartForm : Form
    {
        private readonly Chart _mChart;

        public ChartForm()
        {
            InitializeComponent();

            _mChart = new Chart
            {
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point(3, 3)
            };

            var chartArea = new ChartArea();
            var series = new Series();

            _mChart.BeginInit();

            chartArea.Area3DStyle.Enable3D = true;
            chartArea.Area3DStyle.IsRightAngleAxes = false;
            chartArea.Area3DStyle.LightStyle = LightStyle.Realistic;
            chartArea.BackColor = System.Drawing.SystemColors.Control;
            chartArea.BackHatchStyle = ChartHatchStyle.Cross;
            chartArea.Name = "MainChartArea";

            series.ChartArea = "MainChartArea";
            series.ChartType = SeriesChartType.Column;
            series.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
            series.Name = "MainSeries";

            _mChart.ChartAreas.Add(chartArea);
            _mChart.Name = "mChart";
            _mChart.Series.Add(series);
            _mChart.Visible = false;

            _mChart.EndInit();

            mPanel.SuspendLayout();
            mPanel.Controls.Add(_mChart);
            mPanel.ResumeLayout();
        }

        public void DisplayChart(List<ChartPoint> chartPoints)
        {
            ResetChart();

            if (0 == chartPoints.Count)
                return;

            var dataPoints = new List<DataPoint>();

            foreach (var chartPoint in chartPoints)
            {
                var dataPoint = new DataPoint(0d, chartPoint.Value)
                {
                    Label = $"{chartPoint.Name}",
                };

                if (null != chartPoint.Color)
                    dataPoint.Color = chartPoint.Color.Value;

                if (null != chartPoint.FontColor)
                    dataPoint.LabelForeColor = chartPoint.FontColor.Value;

                dataPoints.Add(dataPoint);
            }

            var mSeries = _mChart.Series[0];

            _mChart.BeginInit();
            foreach (var dataPoint in dataPoints)
            {
                mSeries.Points.Add(dataPoint);
            }
            _mChart.EndInit();

            _mChart.Visible = true;

        }

        private void ResetChart()
        {
            if (null == _mChart)
                return;

            var mSeries = _mChart.Series[0];

            _mChart.BeginInit();
            mSeries.Points.Clear();
            _mChart.EndInit();

            _mChart.Visible = false;
        }

        private void ChartForm_Load(object sender, EventArgs e)
        {
            FormUtils.MoveToCenterParent(this);
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}

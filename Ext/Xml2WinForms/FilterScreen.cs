using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public partial class FilterScreen : UserControl
    {
        public const string BeginUpdateServiceCommand = "BeginUpdate";
        public const string EndUpdateServiceCommand = "EndUpdate";
        public const string DisplayContentServiceCommand = "DisplayContent";

        private readonly Chart _mChart;

        private string _templateName;
        private string _templateBaseDirectory;

        private readonly List<ChartPoint> _chartPoints = new List<ChartPoint>();

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CurrentEntity => mTunableGrid.CurrentEntity;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuItemResolverDelegate MenuItemResolver
        {
            get => mTunableGrid.MenuItemResolver;
            set => mTunableGrid.MenuItemResolver = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<List<object>, FilterScreenContent> WorkCallback { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<Exception> ErrorAction { get; set; }

        public FilterScreen()
        {
            InitializeComponent();

            if (!ApplicationUtility.IsRunningOnMono)
            {
                _mChart = new Chart {Cursor = Cursors.Hand};

                _mChart.Click += (sender, args) =>
                {
                    var chartForm = new ChartForm();
                    chartForm.DisplayChart(_chartPoints);

                    chartForm.Show(this);
                };

                var chartArea = new ChartArea();
                var series = new Series();

                _mChart.BeginInit();

                _mChart.Anchor = AnchorStyles.Top;
                chartArea.Area3DStyle.Enable3D = true;
                chartArea.Area3DStyle.IsRightAngleAxes = false;
                chartArea.Area3DStyle.LightStyle = LightStyle.Realistic;
                chartArea.BackColor = System.Drawing.SystemColors.Control;
                chartArea.BackHatchStyle = ChartHatchStyle.Cross;
                chartArea.Name = "MainChartArea";
                _mChart.ChartAreas.Add(chartArea);
                _mChart.Location = new System.Drawing.Point(3, 198);
                _mChart.Name = "mChart";
                series.ChartArea = "MainChartArea";
                series.ChartType = SeriesChartType.Doughnut;
                series.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
                series.Name = "MainSeries";
                _mChart.Series.Add(series);
                _mChart.Size = new System.Drawing.Size(150, 150);
                _mChart.Margin = new Padding(3, 32, 3, 3);
                _mChart.TabIndex = 2;
                _mChart.Visible = false;

                _mChart.EndInit();

                rightFlowLayoutPanel.SuspendLayout();
                rightFlowLayoutPanel.Controls.Add(_mChart);
                rightFlowLayoutPanel.ResumeLayout();
            }

            mTunableGrid.ServiceCommand += (sender, args) =>
            {
                ServiceCommand?.Invoke(sender, args);
            };

            mTunableShape.ServiceCommand += (sender, args) =>
            {
                ServiceCommand?.Invoke(sender, args);
            };
        }

        private void FilterScreen_Load(object sender, EventArgs e)
        {
        }

        public void ApplyTemplate<TColumnTemplate>(FilterScreenTemplate<TColumnTemplate> template)
            where TColumnTemplate : class, IShapeColumnTemplate
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            _templateName = template.TemplateName;
            _templateBaseDirectory = template.BaseDirectory;

            if (null == template.Grid)
                throw new BadTemplateException("null == template.Grid");

            mTunableGrid.ApplyTemplate(template.Grid);

            bool hasRightContent = false;

            if (null != template.Column && 0 != template.Column.Controls.Count)
            {
                if (null == template.FilterButtonText)
                    throw new BadTemplateException("null == template.FilterButtonText");

                filterButton.Text = template.FilterButtonText;

                mTunableShape.ApplyTemplate(template.Column);
                mTunableShape.Visible = true;
                filterFlowLayoutPanel.Visible = true;

                hasRightContent = true;
            }
            else
            {
                if (null == template.FilterButtonText)
                {
                    mTunableShape.Visible = false;
                    filterFlowLayoutPanel.Visible = false;
                }
                else
                {
                    filterButton.Text = template.FilterButtonText;

                    mTunableShape.Visible = false;
                    filterFlowLayoutPanel.Visible = true;
                    hasRightContent = true;
                }
            }

            if (template.CommandButtons.Count > 0)
            {
                commandBarFlowLayoutPanel.SuspendLayout(); // SuspendLayout

                foreach (var buttonTemplate in template.CommandButtons)
                {
                    var button = new TunableButton
                    {
                        Width = 139
                    };
                    button.ApplyTemplate(buttonTemplate);

                    button.ServiceCommand += (sender, args) => ServiceCommand?.Invoke(sender, args);

                    commandBarFlowLayoutPanel.Controls.Add(button);
                }

                commandBarFlowLayoutPanel.ResumeLayout(); // ResumeLayout

                commandBarGroupBox.Visible = true;
                hasRightContent = true;
            }
            else
                commandBarGroupBox.Visible = false;

            rightFlowLayoutPanel.Visible = hasRightContent;
        }

        public void ApplyShapeValues(Dictionary<string, object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            mTunableShape.ApplyValues(values);
        }

        public void ShowData()
        {
            filterButton_Click(this, null);
        }

        public void UpdateRow(GridRowContent rowContent)
        {
            if (null == rowContent)
                throw new ArgumentNullException(nameof(rowContent));

            mTunableGrid.UpdateRow(rowContent);
        }

        public List<GridColumnSettings> SelectGridSettings()
        {
            return mTunableGrid.SelectGridSettings();
        }

        public List<object> SelectContentItems()
        {
            return mTunableGrid.SelectContentItems();
        }

        public void Reset()
        {
            mTunableGrid.Reset();
            mTunableShape.Reset();

            ResetChart();

            // CommandBar
            var outdatedControls = commandBarFlowLayoutPanel.Controls.Cast<Control>().ToList();

            commandBarFlowLayoutPanel.SuspendLayout(); // SuspendLayout
            foreach (Control control in outdatedControls)
            {
                control.Dispose();
            }
            commandBarFlowLayoutPanel.Controls.Clear();
            commandBarFlowLayoutPanel.ResumeLayout(); // ResumeLayout
        }

        private void filterButton_Click(object sender, EventArgs e)
        {
            filterButton.Enabled = false;
            ServiceCommand?.Invoke(this, new CommandEventArgs {Command = BeginUpdateServiceCommand});

            var values = mTunableShape.SelectValues();
            mBackgroundWorker.RunWorkerAsync(values);
        }

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = WorkCallback?.Invoke((List<object>) e.Argument);
        }

        private void mBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;

            ServiceCommand?.Invoke(this, new CommandEventArgs {Command = EndUpdateServiceCommand});

            filterButton.Enabled = true;

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

            mTunableGrid.ClearContent();
            ResetChart();

            var result = e.Result as FilterScreenContent;

            if (null != result?.RowContentList && 0 != result.RowContentList.Count)
            {
                mTunableGrid.DisplayContent(result.RowContentList);

                _chartPoints.Clear();

                if (null != result.ChartPoints && 0 != result.ChartPoints.Count)
                {
                    DisplayChart(result.ChartPoints);
                    _chartPoints.AddRange(result.ChartPoints);
                }
            }

            ServiceCommand?.Invoke(this, new CommandEventArgs {Command = DisplayContentServiceCommand});
        }

        private void DisplayChart(List<ChartPoint> chartPoints)
        {
            if (null == _mChart)
                return;

            if (null == chartPoints || 0 == chartPoints.Count)
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
    }
}

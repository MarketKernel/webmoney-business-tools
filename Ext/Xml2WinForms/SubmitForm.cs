using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public sealed partial class SubmitForm : Form
    {
        #region Step

        private sealed class Step
        {
            public TunableShape Shape { get; }
            public string ActionText { get; }

            public Step(TunableShape shape, string actionText)
            {
                Shape = shape;
                ActionText = actionText;
            }
        }

        #endregion

        private readonly int _borderHeight;
        private readonly int _borderWidth;

        private readonly List<Step> _steps = new List<Step>();

        private string _templateName;
        private string _templateBaseDirectory;
        private int _currentShapeIndex;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<int, List<object>, bool> VerificationCallback { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<int, List<object>, Dictionary<string, object>> WorkCallback { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<Exception> ErrorAction { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<Dictionary<string, object>, bool> FinalAction { get; set; }

        public SubmitForm()
        {
            if (ApplicationUtility.IsRunningOnMono)
                Font = new Font("Arial", 8);

            InitializeComponent();

            if (ApplicationUtility.IsRunningOnMono)
            {
                AutoSize = false;
                AutoSizeMode = AutoSizeMode.GrowOnly;

                mFlowLayoutPanel.Dock = DockStyle.None;
            }

            _borderHeight = Height - ClientSize.Height;
            _borderWidth = Width - ClientSize.Width;
        }

        private void SubmitForm_Load(object sender, EventArgs e)
        {
            FormUtils.MoveToCenterParent(this);
        }

        public void ApplyTemplate<TColumnTemplate>(SubmitFormTemplate<TColumnTemplate> template,
            Dictionary<string, object> values = null)
            where TColumnTemplate : class, IShapeColumnTemplate
        {
            if (null == template)
                throw new ArgumentNullException(nameof(template));

            Reset();

            _templateName = template.TemplateName;
            _templateBaseDirectory = template.BaseDirectory;

            Text = template.Text ?? throw new BadTemplateException("null == template.Text");

            foreach (var stepTemplate in template.Steps)
            {
                if (null == stepTemplate.TunableShape)
                    throw new BadTemplateException("null == stepTemplate.TunableShape");

                var tunableShape = new TunableShape
                {
                    Top = 0,
                    Left = 0,
                    Margin = new Padding(0, 0, 0, 0)
                };

                tunableShape.ApplyTemplate(stepTemplate.TunableShape);

                tunableShape.ServiceCommand += (sender, args) =>
                {
                    ServiceCommand?.Invoke(sender, args);
                };

                _steps.Add(new Step(tunableShape, stepTemplate.ActionText));
            }

            ApplyShape(values);

            previousButton.Visible = false;
        }

        public void ApplyValues(Dictionary<string, object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            var step = _steps[_currentShapeIndex];
            var shape = step.Shape;

            shape.ApplyValues(values);
        }

        public void Submit()
        {
            okButton_Click(null, null);
        }

        public void Reset()
        {
            Text = string.Empty;

            // mFlowLayoutPanel
            var outdatedControls = mFlowLayoutPanel.Controls.Cast<Control>().ToList();

            mFlowLayoutPanel.SuspendLayout();
            foreach (Control control in outdatedControls)
            {
                control.Dispose();
            }

            mFlowLayoutPanel.Controls.Clear();
            mFlowLayoutPanel.ResumeLayout();

            // Steps
            foreach (var step in _steps)
            {
                step.Shape.Dispose();
            }

            _steps.Clear();
            _currentShapeIndex = 0;

            previousButton.Visible = false;
        }

        private void previousButton_Click(object sender, EventArgs e)
        {
            if (_currentShapeIndex < 1)
                return;

            _currentShapeIndex--;

            ApplyShape(null);

            if (_currentShapeIndex < 1)
                previousButton.Visible = false;

            if (!AutoSize)
                UpdateSize();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            var currentShape = _steps[_currentShapeIndex].Shape;

            if (!currentShape.Inspect())
                return;

            var values = currentShape.SelectValues();

            if (null != VerificationCallback)
                if (!VerificationCallback(_currentShapeIndex, values))
                    return;

            mFlowLayoutPanel.Enabled = false;
            previousButton.Enabled = false;
            nextButton.Enabled = false;

            mToolStripProgressBar.Visible = true;

            mBackgroundWorker.RunWorkerAsync(new object[]
            {
                _currentShapeIndex, values, Thread.CurrentThread.CurrentCulture, Thread.CurrentThread.CurrentUICulture
            });
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void mBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] parameters = (object[]) e.Argument;

            int currentStepNumber = (int) parameters[0];
            var shapeValues = (List<object>) parameters[1];
            Thread.CurrentThread.CurrentCulture = (CultureInfo)parameters[2];
            Thread.CurrentThread.CurrentUICulture = (CultureInfo) parameters[3];

            e.Result = WorkCallback?.Invoke(currentStepNumber, shapeValues);
        }

        private void mBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (IsDisposed)
                return;

            mFlowLayoutPanel.Enabled = true;
            previousButton.Enabled = true;
            nextButton.Enabled = true;

            mToolStripProgressBar.Visible = false;

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

            var values = e.Result as Dictionary<string, object>;

            if (_currentShapeIndex + 1 < _steps.Count)
            {
                _currentShapeIndex++;
                ApplyShape(values);
                previousButton.Visible = true;

                return;
            }

            if (null != FinalAction)
            {
                if (!FinalAction(values))
                    return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        internal void UpdateSize()
        {
            if (AutoSize)
                throw new InvalidOperationException("AutoSize == true");

            var step = _steps[_currentShapeIndex];
            var shape = step.Shape;

            Height = mPanel.Padding.Top +
                     shape.CalculateHeight() +
                     mPanel.Padding.Bottom +
                     bottomFlowLayoutPanel.Height +
                     mStatusStrip.Height +
                     _borderHeight;
        }

        private void ApplyShape(Dictionary<string, object> values)
        {
            var step = _steps[_currentShapeIndex];
            var shape = step.Shape;

            mFlowLayoutPanel.SuspendLayout(); // SuspendLayout
            mFlowLayoutPanel.Controls.Clear();
            mFlowLayoutPanel.Controls.Add(shape);
            mFlowLayoutPanel.ResumeLayout(); // ResumeLayout

            if (null != values && values.Count > 0)
                shape.ApplyValues(values);

            if (!AutoSize)
            {
                // В Mono не корректно работает AutoSize формы (для Mono AutoSize отключаем вручную в конструкторе формы).
                // Можно рассмотреть вариант ручной установки размеров GroupBox в комбинации с установкой размеров формы
                // по событию Layout (актуально для случая сокрытия элементов в GroupBox по событию).

                Width = mPanel.Padding.Left +
                        shape.PreferredSize.Width +
                        mPanel.Padding.Right +
                        _borderWidth;

                Height = mPanel.Padding.Top +
                         shape.PreferredSize.Height +
                         mPanel.Padding.Bottom +
                         bottomFlowLayoutPanel.Height +
                         mStatusStrip.Height +
                         _borderHeight;
            }

            if (null != step.ActionText)
                nextButton.Text = step.ActionText;
        }
    }
}
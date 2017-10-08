using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms
{
    public partial class FilterForm : Form
    {
        private string _title;

        private string _fileFilter;
        private string _lastFileName;
        private List<string> _labelValues;

        private Func<List<object>, FilterFormContent> _workCallback;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object CurrentEntity => mFilterScreen.CurrentEntity;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MenuItemResolverDelegate MenuItemResolver
        {
            get => mFilterScreen.MenuItemResolver;
            set => mFilterScreen.MenuItemResolver = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Func<List<object>, FilterFormContent> WorkCallback
        {
            get => _workCallback;
            set
            {
                _workCallback = value;

                if (null != value)
                {
                    mFilterScreen.WorkCallback = list =>
                    {
                        var result = _workCallback(list);
                        _labelValues = result.LabelValues;
                        return result.ScreenContent;
                    };
                }
                else
                    mFilterScreen.WorkCallback = null;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<Exception> ErrorAction
        {

            get => mFilterScreen.ErrorAction;
            set => mFilterScreen.ErrorAction = value;
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Action<string, List<object>> SaveItemsCallback { get; set; }

        public FilterForm()
        {
            InitializeComponent();

            mFilterScreen.ServiceCommand += (sender, args) =>
            {
                if (FilterScreen.BeginUpdateServiceCommand.Equals(args.Command))
                {
                    saveToolStripMenuItem.Enabled = false;
                    saveAsToolStripMenuItem.Enabled = false;
                    mToolStripProgressBar.Visible = true;
                    return;
                }

                if (FilterScreen.EndUpdateServiceCommand.Equals(args.Command))
                {
                    mToolStripProgressBar.Visible = false;
                    return;
                }

                if (FilterScreen.DisplayContentServiceCommand.Equals(args.Command))
                {
                    if (null != SaveItemsCallback && mFilterScreen.SelectContentItems().Any())
                    {
                        saveToolStripMenuItem.Enabled = true;
                        saveAsToolStripMenuItem.Enabled = true;
                    }

                    if (null != _labelValues)
                    {
                        int index = 0;
                        foreach (ToolStripItem item in mStatusStrip.Items)
                        {
                            var toolStripStatusLabel = item as ToolStripStatusLabel;

                            if (null == toolStripStatusLabel)
                                continue;

                            // Пропускаем разделитель
                            if (toolStripStatusLabel.Spring)
                                continue;

                            if (_labelValues.Count > index)
                            {
                                toolStripStatusLabel.Text = _labelValues[index];
                                toolStripStatusLabel.Visible = true;
                            }
                            else
                            {
                                toolStripStatusLabel.Text = string.Empty;
                                toolStripStatusLabel.Visible = false;

                                break;
                            }

                            index++;
                        }
                    }

                    return;
                }

                ServiceCommand?.Invoke(sender, args);
            };
        }

        private void FilterForm_Load(object sender, EventArgs e)
        {
            FormUtils.MoveToCenterParent(this);
        }

        public void ApplyTemplate<TColumnTemplate>(FilterFormTemplate<TColumnTemplate> template)
            where TColumnTemplate : class, IShapeColumnTemplate
        {
            if (null == template)
                throw new ArgumentException(nameof(template));

            if (null == _title)
                _title = Text;

            Reset();

            Text = template.Title;
            var filterScreen = template.FilterScreen;

            if (null == filterScreen)
                throw new BadTemplateException("null == filterControl");

            _fileFilter = template.FileFilter;

            mFilterScreen.ApplyTemplate(filterScreen);

            // Status labels
            var labelTemplates = template.StatusLabels;

            if (labelTemplates.Count > 0)
            {
                mStatusStrip.SuspendLayout(); // SuspendLayout
                mStatusStrip.Items.Add(new ToolStripStatusLabel { Spring = true });

                foreach (var labelTemplate in labelTemplates)
                {
                    var toolStripStatusLabel = new ToolStripStatusLabel
                    {
                        Width = 150,
                        AutoSize = false,
                        TextAlign = ContentAlignment.TopLeft,
                        BorderSides = ToolStripStatusLabelBorderSides.Left |
                                      ToolStripStatusLabelBorderSides.Top |
                                      ToolStripStatusLabelBorderSides.Right,
                        Visible = false
                    };

                    toolStripStatusLabel.Click += (sender, args) =>
                    {
                        if (string.IsNullOrEmpty(toolStripStatusLabel.Text))
                            return;

                        Clipboard.SetText(toolStripStatusLabel.Text);
                    };

                    if (null != labelTemplate.TextColor)
                        toolStripStatusLabel.ForeColor = DisplayContentHelper.SelectColor(labelTemplate.TextColor);

                    mStatusStrip.Items.Add(toolStripStatusLabel);
                }

                mStatusStrip.ResumeLayout(); // ResumeLayout
            }
        }

        public void ApplyShapeValues(Dictionary<string, object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            mFilterScreen.ApplyShapeValues(values);
        }

        public void ShowData()
        {
            mFilterScreen.ShowData();
        }

        public void UpdateRow(GridRowContent rowContent)
        {
            if (null == rowContent)
                throw new ArgumentNullException(nameof(rowContent));

            mFilterScreen.UpdateRow(rowContent);
        }

        public List<GridColumnSettings> SelectGridSettings()
        {
            return mFilterScreen.SelectGridSettings();
        }

        public void Reset()
        {
            Text = _title;

            saveToolStripMenuItem.Enabled = false;
            saveAsToolStripMenuItem.Enabled = false;

            _fileFilter = null;
            _lastFileName = null;

            mFilterScreen.Reset();

            // Labels
            var outdatedStatusLabels = mStatusStrip.Items
                .Cast<ToolStripItem>()
                .OfType<ToolStripStatusLabel>()
                .ToList();

            mStatusStrip.SuspendLayout(); // SuspendLayout
            foreach (var item in outdatedStatusLabels)
            {
                mStatusStrip.Items.Remove(item);
                item.Dispose();
            }
            mStatusStrip.ResumeLayout(); // ResumeLayout

            _labelValues?.Clear();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(_lastFileName);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Save(null);
        }

        private void Save(string fileName)
        {
            if (null == SaveItemsCallback)
                throw new InvalidOperationException("null == SaveItemsCallback");

            if (null == fileName)
            {
                var saveFileDialog = new SaveFileDialog();

                if (null != _fileFilter)
                    saveFileDialog.Filter = _fileFilter;

                if (DialogResult.OK != saveFileDialog.ShowDialog(this))
                    return;

                fileName = saveFileDialog.FileName;
                _lastFileName = fileName;
            }

            SaveItemsCallback(fileName, mFilterScreen.SelectContentItems());
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

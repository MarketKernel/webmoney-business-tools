using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LocalizationAssistant;
using Xml2WinForms.Templates;

namespace Xml2WinForms
{
    public sealed partial class TunableShape : UserControl
    {
        /// <summary>
        /// Содержит только контролы верхнего уровня (не включает те что вложены в GroupBox).
        /// Используется для:
        /// 1) установки поведения (требуетя ссылка на _namedControlHolders верхнего уровня);
        /// 2) установки errorProvider (ссылка на mErrorProvider верхнего уровня);
        /// 3) подписки на сервисное событие;
        /// 4) первичной инициализации (проводится в после всех установок);
        /// 5) получения значения контролов (отдаются в виде списка).
        /// Контролы второго уровня инициализируются опосредованно (через родителя).
        /// </summary>
        private readonly List<IControlHolder> _controlHolders;

        /// <summary>
        /// Содержит все контролы с именем, включая контролы второго уровня (те, что вложены
        /// в GroupBox). Используется для реализации поведения (сокрытие по условию и пр.)
        /// и для установки значения контрола по имени.
        /// </summary>
        private readonly Dictionary<string, IControlHolder> _namedControlHolders;

        [Category("Action"), Description("Service command.")]
        public event EventHandler<CommandEventArgs> ServiceCommand;

        public TunableShape()
        {
            InitializeComponent();

            _controlHolders = new List<IControlHolder>();
            _namedControlHolders = new Dictionary<string, IControlHolder>();
        }

        protected override void OnLoad(EventArgs e)
        {
            foreach (var controlHolder in _controlHolders)
            {
                controlHolder.InitControl();
            }

            base.OnLoad(e);
        }

        public void ApplyTemplate<TColumnTemplate>(TunableShapeTemplate<TColumnTemplate> template)
            where TColumnTemplate : class, IShapeColumnTemplate
        {
            Reset();

            if (null == template)
                return;

            ((ISupportInitialize)mErrorProvider).BeginInit();
            bodyFlowLayoutPanel.SuspendLayout();

            foreach (var columnTemplate in template.Columns)
            {
                if (null == columnTemplate.Controls)
                    throw new BadTemplateException("null == columnTemplate.Controls");

                var flowLayoutPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

                var controlHolders = ControlHolderHelper.BuildControlHolders(columnTemplate.Controls);

                AddControlHoldersTo(flowLayoutPanel, controlHolders);
                bodyFlowLayoutPanel.Controls.Add(flowLayoutPanel);

                ApplyControlHolders(controlHolders);
            }

            ((ISupportInitialize)mErrorProvider).EndInit();
            bodyFlowLayoutPanel.ResumeLayout();
        }

        /// <summary>
        /// Используется если нужна только одна колонка (для упрощения).
        /// К примеру, метод вызывает FilterScreen.
        /// </summary>
        /// <typeparam name="TColumnTemplate">Тип колонки.</typeparam>
        /// <param name="template">Шаблон колонки.</param>
        public void ApplyTemplate<TColumnTemplate>(TColumnTemplate template)
            where TColumnTemplate : class, IShapeColumnTemplate
        {
            Reset();

            if (null == template)
                return;

            var controlHolders = ControlHolderHelper.BuildControlHolders(template.Controls);

            BuildSingleColumn(controlHolders);

            ((ISupportInitialize)mErrorProvider).BeginInit();
            ApplyControlHolders(controlHolders);
            ((ISupportInitialize)mErrorProvider).EndInit();
        }

        public void ApplyValues(Dictionary<string, object> values)
        {
            if (null == values)
                throw new ArgumentNullException(nameof(values));

            foreach (var keyValuePair in values)
            {
                var namedControlHolder = _namedControlHolders[keyValuePair.Key];
                namedControlHolder.ApplyValue(keyValuePair.Value);
            }
        }

        public bool Inspect()
        {
            var inspectionReports = new List<InspectionReport>();

            foreach (var controlHolder in _controlHolders)
            {
                inspectionReports.AddRange(controlHolder.Inspect(_namedControlHolders));
            }

            if (0 == inspectionReports.Count)
                return true;

            bool first = true;
            var stringBuilder = new StringBuilder();

            foreach (var inspectionReport in inspectionReports)
            {
                if (inspectionReport.Handled)
                    continue;

                if (first)
                    first = false;
                else
                    stringBuilder.AppendLine();

                stringBuilder.AppendLine(inspectionReport.Message);
            }

            var message = stringBuilder.ToString();

            if (!string.IsNullOrEmpty(message))
            {
                message = $"{message}{Environment.NewLine}{Environment.NewLine}";

                MessageBox.Show(this, message, Translator.Instance.Translate("xml2winforms", "Error!"),
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return false;
        }

        public List<object> SelectValues()
        {
            var values = new List<object>();

            foreach (var controlHolder in _controlHolders)
            {
                values.AddRange(controlHolder.SelectValues());
            }

            return values;
        }

        public void Reset()
        {
            var outdatedControls = bodyFlowLayoutPanel.Controls.Cast<Control>().ToList();

            bodyFlowLayoutPanel.SuspendLayout(); // SuspendLayout
            foreach (Control control in outdatedControls)
            {
                control.Dispose();
            }
            bodyFlowLayoutPanel.Controls.Clear();
            bodyFlowLayoutPanel.ResumeLayout(); // ResumeLayout

            _controlHolders.Clear();
            _namedControlHolders.Clear();

            ((ISupportInitialize)mErrorProvider).BeginInit();
            mErrorProvider.Clear();
            ((ISupportInitialize)mErrorProvider).EndInit();
        }

        /// <summary>
        /// Используется для вычисления высоты контрола в Mono (стандартный AutoSize работает не корректно).
        /// </summary>
        /// <returns>Высота контрола.</returns>
        internal int CalculateHeight()
        {
            int height = 0;

            foreach (Control control in bodyFlowLayoutPanel.Controls)
            {
                var column = control as FlowLayoutPanel;

                if (column != null)
                {
                    var columnHeight = CalculateHeight(column);

                    if (columnHeight > height)
                        height = columnHeight;
                }
                else
                {
                    height = CalculateHeight(bodyFlowLayoutPanel);
                    break;
                }
            }

            return height;
        }

        /// <summary>
        /// Используется для GroupBox. Можно инициализировать только один раз (повторная инициализация приведет к IOE).
        /// Только добавляет контролы в форму, но не применяет поведение, не включает в список и не подписывает на
        /// сервисное событие.
        /// </summary>
        /// <param name="controlHolders">Список контролов.</param>
        internal void BuildSingleColumn(List<IControlHolder> controlHolders)
        {
            if (null == controlHolders)
                throw new ArgumentNullException(nameof(controlHolders));

            if (_controlHolders.Count > 0)
                throw new InvalidOperationException("_controlHolders.Count > 0");

            bodyFlowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            AddControlHoldersTo(bodyFlowLayoutPanel, controlHolders);
        }

        private static void AddControlHoldersTo(FlowLayoutPanel flowLayoutPanel, List<IControlHolder> controlHolders)
        {
            flowLayoutPanel.SuspendLayout();

            foreach (var controlHolder in controlHolders)
            {
                if (null != controlHolder.Label)
                    flowLayoutPanel.Controls.Add(controlHolder.Label);

                flowLayoutPanel.Controls.Add(controlHolder.Control);
            }

            flowLayoutPanel.ResumeLayout();
        }

        /// <summary>
        /// 1. Добавляет в список _controlHolders (только верхний уровень).
        /// 2. Добавляет в _namedControlHolders контролы с именем, включая дочерние из GroupBox.
        /// 3. Устанавливает поведение.
        /// 4. Устанавливает mErrorProvider.
        /// 5. Подписывает на сервисное событие.
        /// </summary>
        /// <param name="controlHolders">Список для обработки.</param>
        private void ApplyControlHolders(List<IControlHolder> controlHolders)
        {
            foreach (var controlHolder in controlHolders)
            {
                _controlHolders.Add(controlHolder);

                foreach (var namedControlHolder in controlHolder.CollectNamedControlHolders())
                {
                    _namedControlHolders.Add(namedControlHolder.Name, namedControlHolder);
                }

                // Установка поведения.
                controlHolder.ApplyBehavior(_namedControlHolders);

                // Установка errorProvider.
                controlHolder.SetErrorProvider(mErrorProvider);

                // Подписка на сервисное событие.
                var serviceControl = controlHolder.Control as IServiceControl;

                if (null != serviceControl)
                    serviceControl.ServiceCommand += (sender, args) =>
                    {
                        ServiceCommand?.Invoke(serviceControl, args);
                    };
            }
        }

        private int CalculateHeight(FlowLayoutPanel column)
        {
            int height = 0;

            foreach (Control innerControl in column.Controls)
            {
                if (!innerControl.Visible)
                    continue;

                height += innerControl.Margin.Top + innerControl.Height + innerControl.Margin.Bottom;
            }

            return height;
        }
    }
}
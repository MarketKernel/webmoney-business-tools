using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using WMBusinessTools.Extensions;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms.Sandbox
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/Form1.json");

            var sf = new SubmitForm();
            sf.ApplyTemplate(sft);

            sf.ShowDialog(this);


            //var filterForm = new FilterForm();

            //filterForm.DisplayChart(new List<FilterControl.ChartPoint>()
            //    {
            //        new FilterControl.ChartPoint()
            //        {
            //            Name = "Приход",
            //            Value = 6,
            //            Color = Color.FromArgb(66, 179, 113),
            //            FontColor = Color.FromArgb(4, 100, 150)
            //        },
            //        new FilterControl.ChartPoint()
            //        {
            //            Name = "Расход",
            //            Value = 4,
            //            Color = Color.FromArgb(255, 67, 67),
            //            FontColor = Color.FromArgb(4, 100, 150)
            //        },
            //    }
            //);


            //filterForm.Show(this);
        }

        public class CertItem1
        {
            public string Name { get; set; }
            public string Thumbprint { get; set; }
            public DateTime ExpirationDate { get; set; }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            

            try
            {
                var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/Reg2.json");

                var sf = new SubmitForm();

                var templates = new List<ListItemContent>
                {
                    new ListItemContent(
                        new CertItem1 {Name = "name1", Thumbprint = "1324234", ExpirationDate = DateTime.Now}),
                    new ListItemContent(
                        new CertItem1 {Name = "name2", Thumbprint = "2324234", ExpirationDate = DateTime.Now})
                };

                sf.ServiceCommand += (o, args) =>
                {
                    if (args.Command.Equals("SelectFile"))
                        MessageBox.Show("test");
                };

                sf.ApplyTemplate(sft, new Dictionary<string, object>()
                {
                    {"Certificate", templates}
                });


                sf.ShowDialog(this);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString());
                throw;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DbSettingsForm bcsF = new DbSettingsForm();
            bcsF.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/ClassicKeySettingsForm.json");

            var sf = new SubmitForm();

            sf.ApplyTemplate(sft);

            sf.ShowDialog(this);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/LightKeySettings.json");

            var sf = new SubmitForm();

            sf.ApplyTemplate(sft);

            sf.ShowDialog(this);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/PasswordSettingsForm.json");

            sft.Steps[0].TunableShape.Columns[0].Controls[0].Visible = false;

            var sf = new SubmitForm();

            sf.ApplyTemplate(sft);

            sf.ShowDialog(this);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<TunableShapeTemplate<ShapeColumnTemplate>>("./Templates/RequestNumberSettingsForm.json");


            var requestNumberSettingsForm = new RequestNumberSettingsForm();
            requestNumberSettingsForm.ApplyTemplate(sft);

            requestNumberSettingsForm.ShowDialog(this);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            var abt= TemplateLoader.LoadTemplateFromJsonFile<AboutBoxTemplate>("./Templates/AboutForm.json");

            AboutBox ab = new AboutBox();

            try
            {
                ab.ApplyTemplate(abt);

            }
            catch (Exception exception)
            {

                MessageBox.Show(exception.ToString());
            }

            ab.ShowDialog(this);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var ef = new ErrorForm(
                new ErrorFormTemplate("Заголовок ошибки", "Сообщение об ошибке!") {Details = "Детали ошибки"});
            ef.ShowDialog(this);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }

        private SubmitForm sf;

        private void button1_Click_2(object sender, EventArgs e)
        {

            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/SubmitFormTest1.json");

            bool show = false;

            if (null == sf)
            {
                sf = new SubmitForm();
                show = true;
            }

            sf.ApplyTemplate(sft);

            if (show)
                sf.Show(this);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/SubmitFormTest2.json");

            bool show = false;

            if (null == sf)
            {
                sf = new SubmitForm();
                show = true;
            }

            sf.ApplyTemplate(sft);

            if (show)
                sf.Show(this);
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }

        public class Row
        {
            public string Column1 { get; set; }
            public string Column2 { get; set; }
        }

        private FilterForm ff;

        private void button10_Click(object sender, EventArgs e)
        {
            

            try
            {
                FilterFormTemplate<ShapeColumnTemplate> fft = TemplateLoader.LoadTemplateFromJsonFile<FilterFormTemplate<ShapeColumnTemplate>>("./Templates/FilterFormTest1.json");

                bool show = false;

                if (null == ff)
                {
                    ff = new FilterForm();
                    show = true;

                    ff.SaveItemsCallback = (s, list) =>
                    {

                    };

                    ff.MenuItemResolver = (entity, command) =>
                    {
                        var item = entity as Row;

                        if (null == item)
                            return false;

                        if (command.Equals("Menu1"))
                            return false;

                        return true;
                    };

                    ff.WorkCallback += objects =>
                    {
                        Thread.Sleep(1000);

                        List<GridRowContent> list = new List<GridRowContent>();
                        list.Add(new GridRowContent("tset1", new Row { Column1 = "tset1", Column2 = "test2" }));
                        list.Add(new GridRowContent("tset2", new Row { Column1 = "tset2", Column2 = "test2" }));
                        list.Add(new GridRowContent("tset3", new Row { Column1 = "tset3", Column2 = "test2" }));

                        return new FilterFormContent()
                        {
                            ScreenContent = new FilterScreenContent()
                            {
                                RowContentList = list,
                                ChartPoints = new List<ChartPoint>()
                                {
                                    new ChartPoint("test1", 10),
                                    new ChartPoint("test2", 20),
                                }
                            },
                            LabelValues = new List<string>()
                            {
                                "test1",
                                "test2"
                            }
                        };
                    };
                }

                ff.ApplyTemplate(fft);

                if (show)
                    ff.Show(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private ListViewForm lvf;
        private void button11_Click(object sender, EventArgs e)
        {
            bool show = false;

            if (null == lvf)
            {
                lvf = new ListViewForm();
                show = true;
            }

            lvf.ApplyTemlate();

            if (show)
                lvf.Show();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            SettingsForm sf = new SettingsForm("Настройки");
            sf.ShowDialog(this);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/SubmitFormTest2.json");

            bool show = false;

            if (null == sf)
            {
                sf = new SubmitForm();
                show = true;
            }

            sf.ApplyTemplate(sft);

            if (show)
                sf.Show(this);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            ff.UpdateRow(new GridRowContent("tset3", new Row { Column1 = "newtset3", Column2 = "NEWtest2" }));
        }

        private void button16_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/SubmitFormTest1.json");

            bool show = false;

            if (null == sf)
            {
                sf = new SubmitForm();
                show = true;
            }

            sf.ApplyTemplate(sft, new Dictionary<string, object>() {{"name1", "v1"}});

            if (show)
                sf.Show(this);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            var sft = TemplateLoader.LoadTemplateFromJsonFile<SubmitFormTemplate<ShapeColumnTemplate>>("./Templates/SubmitFormTest3.json");

            bool show = false;

            if (null == sf)
            {
                sf = new SubmitForm();
                show = true;
            }

            sf.ApplyTemplate(sft);

            if (show)
                sf.Show(this);
        }
    }
}

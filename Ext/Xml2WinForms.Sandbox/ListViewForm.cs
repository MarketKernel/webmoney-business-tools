using System.Collections.Generic;
using System.Windows.Forms;
using Xml2WinForms.Templates;
using Xml2WinForms.Utils;

namespace Xml2WinForms.Sandbox
{
    public partial class ListViewForm : Form
    {
        public class Row
        {
            public string Column1 { get; set; }
            public string Column2 { get; set; }
        }

        public ListViewForm()
        {
            InitializeComponent();



            listViewWithCommandBar1.MenuItemResolver = (entity, command) =>
            {
                var item = entity as Row;

                if (null == item)
                    return false;

                if (command.Equals("Menu1"))
                    return false;

                return true;
            };

            listViewWithCommandBar1.RefreshCallback += () =>
            {
                var list = new List<ListItemContent>();

                list.Add(new ListItemContent(new Row {Column1 = "cl1", Column2 = "cl2"}));
                list.Add(new ListItemContent(new Row { Column1 = "cl3", Column2 = "cl4" }));


                return list;
            };



            //listViewWithCommandBar1.DisplayContent();
        }

        public void ApplyTemlate()
        {
            ListScreenTemplate lvcbt =
                TemplateLoader.LoadTemplateFromJsonFile<ListScreenTemplate>(
                    "./Templates/ListViewWithCommandBarTest1.json");

            listViewWithCommandBar1.ApplyTemplate(lvcbt);
        }

        private void ListViewForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
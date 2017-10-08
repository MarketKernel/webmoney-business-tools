using System;
using System.Drawing;
using System.Windows.Forms;

namespace Xml2WinForms.Utils
{
    public static class FormUtils
    {
        public static void MoveToCenterParent(Form form)
        {
            if (null == form)
                throw new ArgumentNullException(nameof(form));

            if (!form.Modal)
            {
                var owner = form.Owner;

                if (null != owner)
                {
                    form.StartPosition = FormStartPosition.Manual;
                    form.Location = new Point(owner.Location.X + (owner.Width - form.Width) / 2,
                        owner.Location.Y + (owner.Height - form.Height) / 2);
                }
            }
        }
    }
}

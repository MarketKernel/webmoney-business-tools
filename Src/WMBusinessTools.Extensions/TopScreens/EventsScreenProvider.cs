using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;
using WMBusinessTools.Extensions.Contracts;
using WMBusinessTools.Extensions.Contracts.Contexts;
using WMBusinessTools.Extensions.DisplayHelpers;
using WMBusinessTools.Extensions.Properties;
using Xml2WinForms;

namespace WMBusinessTools.Extensions
{
    public sealed class EventsScreenProvider : ITopScreenProvider
    {
        private ScreenContainerContext _context;
        private ListScreen _screen;

        public bool CheckCompatibility(ScreenContainerContext context)
        {
            if (null == context)
                throw new ArgumentNullException(nameof(context));

            return false;
        }

        public Control GetScreen(ScreenContainerContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));

            _screen = new ListScreen
            {
                Dock = DockStyle.Fill
            };

            _screen.RefreshCallback += () =>
            {
                SoundPlayer soundPlayer = new SoundPlayer(Resources.payment);
                soundPlayer.Play();

                return new List<ListItemContent>();
            };

            ErrorFormDisplayHelper.ApplyErrorAction(context.ExtensionManager, _screen);

            //_screen.ApplyTemplate(template);

            return _screen;
        }
    }
}

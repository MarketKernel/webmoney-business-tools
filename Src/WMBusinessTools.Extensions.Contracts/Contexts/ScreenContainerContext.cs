using System;

namespace WMBusinessTools.Extensions.Contracts.Contexts
{
    public class ScreenContainerContext : SessionContext
    {
        public IScreenContainer ScreenContainer { get; }

        public ScreenContainerContext(ScreenContainerContext origin)
            : base(origin)
        {
            ScreenContainer = origin.ScreenContainer;
        }

        public ScreenContainerContext(SessionContext baseContext, IScreenContainer screenContainer)
            : base(baseContext)
        {
            ScreenContainer = screenContainer ?? throw new ArgumentNullException(nameof(screenContainer));
        }
    }
}
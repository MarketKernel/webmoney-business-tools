using System;

namespace WMBusinessTools.Extensions.Utils
{
    internal static class ApplicationUtility
    {
        public static readonly bool IsRunningOnMono;

        static ApplicationUtility()
        {
            IsRunningOnMono = null != Type.GetType("Mono.Runtime");
        }
    }
}
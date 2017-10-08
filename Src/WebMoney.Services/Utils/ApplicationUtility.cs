using System;

namespace WebMoney.Services.Utils
{
    internal static class ApplicationUtility
    {
        public static readonly bool IsRunningOnMono;
        public static readonly bool IsUnix;

        static ApplicationUtility()
        {
            IsRunningOnMono = null != Type.GetType("Mono.Runtime");
            IsUnix = Environment.OSVersion.Platform == PlatformID.Unix;
        }
    }
}
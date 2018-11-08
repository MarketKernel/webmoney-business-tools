using System.Reflection;

[assembly: AssemblyDescription("See: www.webmoney-business-tools.com")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif PRODUCTION
[assembly: AssemblyConfiguration("Production")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("MarketKernel Team")]
[assembly: AssemblyProduct("WebMoney Business Tools")]
[assembly: AssemblyCopyright("© MarketKernel Team <global@marketkernel.com> 2009-2018")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

#if PRODUCTION
[assembly: AssemblyKeyFile(@"..\..\..\Solution\Keys\ProductionPublicKey-sha1.snk")]
[assembly: AssemblyDelaySign(true)]
#else
[assembly: AssemblyKeyFile(@"..\..\..\Solution\Keys\DevelopmentKey.snk")]
#endif
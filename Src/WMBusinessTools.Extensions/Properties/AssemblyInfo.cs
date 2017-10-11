using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using log4net.Config;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
// associated with an assembly.

[assembly: XmlConfigurator(Watch = true, ConfigFile = "Log4Net.config")]

[assembly: InternalsVisibleTo("Xml2WinForms.Utility, PublicKey=0024000004800000940000000602000000240000525341310004000001000100f5b1dfa496f2b6f98741d92cf19d468e3f4b948b8b0e571468b18a73ef6371d0fa53622a1188c03cc6b6715ca3c6b8f3ba0ff5816da2a4c9520831d4000920d8265a573e8c0aee453547e149c060419e88c2ef4829e84973e58a0601989b07e0fab346784ebb4df9620c2665d233c30b07240a2af8611c90289bebdf80e780cc")]

[assembly: AssemblyTitle("WMBusinessTools.Extensions")]
[assembly: AssemblyDescription("See: www.webmoney-business-tools.com")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("MarketKernel Team")]
[assembly: AssemblyProduct("WebMoney Business Tools")]
[assembly: AssemblyCopyright("© MarketKernel Team <global@marketkernel.com> 2009-2017")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("4ac68622-dd96-4821-b3cf-b1bc44fec77c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

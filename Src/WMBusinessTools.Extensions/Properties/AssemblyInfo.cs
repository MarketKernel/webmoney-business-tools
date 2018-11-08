using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using log4net.Config;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
// associated with an assembly.

[assembly: XmlConfigurator(Watch = true, ConfigFile = "Log4Net.config")]

[assembly: InternalsVisibleTo("Xml2WinForms.WrappersBuilder, PublicKey=00240000048000009400000006020000002400005253413100040000010001003d2a8cfbb1562e120500872972a8044ca1e811ae6c6a6f9ad486f95a7ff102c1a268b709b4530632a43f33ed4c06ab592c74bb8ac19444e027a547f6684b8ff9e57b5d24e5e48d6b0877c8b7810ae70ede4c1fb80fc38ada768d2c4d389f00afcf2b7a34eec08ae1d276cbf0ed41f41b78f792cc47738716810d24d3c8f4a89e")]

[assembly: AssemblyTitle("WMBusinessTools.Extensions")]

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
[assembly: AssemblyVersion("2.0.0.0")]
[assembly: AssemblyFileVersion("2.0.0.0")]

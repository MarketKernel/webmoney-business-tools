using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using log4net.Config;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: XmlConfigurator(Watch = true, ConfigFile = "Log4Net.config")]
[assembly: InternalsVisibleTo("WebMoney.Services.Tests, PublicKey=00240000048000009400000006020000002400005253413100040000010001003d2a8cfbb1562e120500872972a8044ca1e811ae6c6a6f9ad486f95a7ff102c1a268b709b4530632a43f33ed4c06ab592c74bb8ac19444e027a547f6684b8ff9e57b5d24e5e48d6b0877c8b7810ae70ede4c1fb80fc38ada768d2c4d389f00afcf2b7a34eec08ae1d276cbf0ed41f41b78f792cc47738716810d24d3c8f4a89e")]
[assembly: InternalsVisibleTo("WebMoney.Services.Tests, PublicKey=0024000004800000140100000602000000240000525341310008000001000100394C2AD91919D81173A5CF9DACFAA501DE530500A483555996E7FBC41568899ECC36362FF77A795FB83D206E309DCB0072D3C57690B02A69DF9BAAA25F46D4841230BF8CD8ECD74FC8B74F904982CA909E5CEEADCF2532E47847159AAB51FA11B3484D663D793EB21AEBA3E1A3727F98C86BD1310136C4353FF41CD85E2DF856F8BFD539E7798726B8B58D1F391736D085076B56B789C41256F02CB343DC2EFE0B24666A46F969541C02BFFF9DD9C21A52D3B9CE860C1CEE31BA4F80DF01248D5C3A523ED25937D34A1250FA0F3F4351C1485CCBA4662EA2F0E03698E9C565F80C612E31A3E10357148CBFC1D0532B4F598B5E5A16D305B5AF9525C4A85B8BB7")]

[assembly: AssemblyTitle("WebMoney.Services")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0f597d1c-d6f8-4fa6-a582-48b36a0f8491")]

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

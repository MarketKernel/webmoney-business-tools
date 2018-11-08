// <auto-generated />

using System;
using System.Text;

namespace WebMoney.Services.Migrations.OracleDB
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    using WebMoney.Services.Utils;


    [GeneratedCode("EntityFramework.Migrations", "6.2.0-61023")]
    public sealed partial class V2 : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(V2));
        
        string IMigrationMetadata.Id
        {
            get { return "201811010120444_V2"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get
            {
                var xml = MigrationUtility.Decompress(Resources.GetString("Target"));
                xml = xml.Replace("Schema=\"WMBTUSER\"", $"Schema=\"{Schema}\"");
                return MigrationUtility.Compress(xml);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebMoney.Services.Contracts;

namespace WebMoney.Services.Tests
{
    [TestClass]
    public class ImportExportServiceTest
    {
        public class Entity
        {
            public string Name { get; set; }
            public DateTime Date { get; set; }
        }

        private readonly IImportExportService _importExportService;

        public ImportExportServiceTest()
        {
            var unityContainer = new UnityContainer();
            new ConfigurationService().RegisterServices(unityContainer);

            _importExportService = unityContainer.Resolve<IImportExportService>();
        }

        [TestMethod]
        public void DateTimeTest()
        {
            var entities = new List<Entity>();

            var startDate = new DateTime(2001, 1, 1, 0, 0, 0);

            for (int i = 0; i < 365; i++)
            {
                entities.Add(new Entity
                {
                    Name = "Item_" + i,
                    Date = startDate.AddDays(i).AddHours(i % 24).AddMinutes(i % 60).AddSeconds(i % 60)
                });
            }
            
            string tempFile = Path.Combine(Path.GetTempPath(), Path.ChangeExtension(Path.GetRandomFileName(), "xlsx"));

            Debug.WriteLine(tempFile);

            try
            {
                _importExportService.Save(entities, tempFile);

                var loadedEnitites = _importExportService.Load<Entity>(tempFile).ToList();

                for (int i = 0; i < 356; i++)
                {
                    var date = startDate.AddDays(i).AddHours(i % 24).AddMinutes(i % 60).AddSeconds(i % 60);
                    Assert.AreEqual(date, loadedEnitites[i].Date);
                }
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }
    }
}
using System.Collections.Generic;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;

namespace WebMoney.Services.Contracts
{
    public interface IImportExportService
    {
        IEnumerable<IOriginalTransfer> LoadExportableTransfers(string fileName,
            FileFormat fileFormat = FileFormat.Xml);

        IEnumerable<TObject> Load<TObject>(string fileName, FileFormat fileFormat = FileFormat.Xlsx)
            where TObject : class;

        void SaveExportableTransfers(IEnumerable<IOriginalTransfer> transfers, string fileName,
            FileFormat fileFormat = FileFormat.Xml);

        void Save<TObject>(IEnumerable<TObject> items, string fileName, FileFormat fileFormat = FileFormat.Xlsx)
            where TObject : class;
    }
}

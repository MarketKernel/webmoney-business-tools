using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using ClosedXML.Excel;
using WebMoney.Services.BusinessObjects;
using WebMoney.Services.Contracts;
using WebMoney.Services.Contracts.BasicTypes;
using WebMoney.Services.Contracts.BusinessObjects;
using WebMoney.Services.Contracts.Exceptions;

namespace WebMoney.Services
{
    public sealed class ImportExportService : IImportExportService
    {
        public IEnumerable<IOriginalTransfer> LoadExportableTransfers(string fileName,
            FileFormat fileFormat = FileFormat.Xml)
        {
            if (null == fileName)
                throw new ArgumentNullException(nameof(fileName));

            if (FileFormat.Xml != fileFormat)
                throw new ArgumentOutOfRangeException(nameof(fileFormat));

            ExportableTransferBundle exportableTransferBundle;

            var xmlSerializer = new XmlSerializer(typeof(ExportableTransferBundle));

            using (var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
            {
                exportableTransferBundle = (ExportableTransferBundle) xmlSerializer.Deserialize(streamReader);
            }

            return exportableTransferBundle.Transfers
                .Select(et => (IOriginalTransfer) new OriginalTransfer(et.TransferId, string.Empty, et.TargetPurse,
                    et.Amount, et.Description));
        }

        public IEnumerable<TObject> Load<TObject>(string fileName, FileFormat fileFormat = FileFormat.Xlsx)
            where TObject : class
        {
            if (null == fileName)
                throw new ArgumentNullException(nameof(fileName));

            if (FileFormat.Xlsx != fileFormat)
                throw new InvalidOperationException("fileFormat == " + fileFormat);

            var workbook = new XLWorkbook(fileName);

            if (workbook.Worksheets.Count < 0)
                throw new WrongFileFormatException();

            var worksheet = workbook.Worksheet(1);

            var rows = new List<List<object>>();

            foreach (var xlRow in worksheet.Rows())
            {
                var row = new List<object>();

                foreach (var xlCell in xlRow.Cells())
                {
                    row.Add(xlCell.Value);
                }

                rows.Add(row);
            }

            if (rows.Count < 2)
                return new List<TObject>();

            var properties = GetProperties(typeof(TObject));

            var headerRow = rows.First();
            var actualProperties = new List<PropertyInfo>();

            foreach (string columnName in headerRow)
            {
                var actualProperty =
                    properties.FirstOrDefault(pi => pi.Name.Equals(columnName.Replace(" ", string.Empty),
                        StringComparison.OrdinalIgnoreCase));

                actualProperties.Add(actualProperty);
            }

            var items = new List<TObject>();

            foreach (var row in rows.Skip(1))
            {
                var item = (TObject) Activator.CreateInstance(typeof(TObject), true);

                for (int columnIndex = 0; columnIndex < actualProperties.Count; columnIndex++)
                {
                    if (row.Count <= columnIndex)
                        break;

                    var actualProperty = actualProperties[columnIndex];

                    if (null == actualProperty)
                        continue;

                    var value = row[columnIndex];

                    if (value is double)
                    {
                        if (typeof(int) == actualProperty.PropertyType)
                            value = ((IConvertible) value).ToInt32(CultureInfo.InvariantCulture.NumberFormat);
                        if (typeof(long) == actualProperty.PropertyType)
                            value = ((IConvertible) value).ToInt64(CultureInfo.InvariantCulture.NumberFormat);
                        if (typeof(decimal) == actualProperty.PropertyType)
                            value = ((IConvertible) value).ToDecimal(CultureInfo.InvariantCulture.NumberFormat);
                        if (typeof(string) == actualProperty.PropertyType)
                            value = ((IConvertible) value).ToString(CultureInfo.InvariantCulture.NumberFormat);
                    }
                    else if (value is string s && typeof(DateTime) == actualProperty.PropertyType)
                    {
                        if (!DateTime.TryParseExact(s, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture.DateTimeFormat,
                            DateTimeStyles.AssumeLocal, out var dateTime))
                            continue;

                        value = dateTime.ToUniversalTime();
                    }
                    else if (value is string && typeof(decimal) == actualProperty.PropertyType)
                    {
                        if (!decimal.TryParse((string) value, out var d))
                            continue;

                        value = d;
                    }
                    else if (value is DateTime)
                    {
                        value = ((DateTime) value).ToUniversalTime();
                    }

                    actualProperty.SetValue(item, value, null);
                }

                items.Add(item);
            }

            return items;
        }

        public void SaveExportableTransfers(IEnumerable<IOriginalTransfer> transfers, string fileName,
            FileFormat fileFormat = FileFormat.Xml)
        {
            if (null == transfers)
                throw new ArgumentNullException(nameof(transfers));

            if (null == fileName)
                throw new ArgumentNullException(nameof(fileName));

            if (FileFormat.Xml != fileFormat)
                throw new ArgumentOutOfRangeException(nameof(fileFormat));

            var transferBundle = new ExportableTransferBundle();
            transferBundle.Transfers.AddRange(
                transfers.Select(t => new ExportableTransfer(t.PaymentId, t.TargetPurse, t.Amount, t.Description)));

            var xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add("", "http://tempuri.org/ds.xsd");

            var xmlSerializer = new XmlSerializer(typeof(ExportableTransferBundle));

            using (var fileStream = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            using (var streamWriter = new StreamWriter(fileStream, Encoding.UTF8))
            {
                xmlSerializer.Serialize(streamWriter, transferBundle, xmlSerializerNamespaces);

                streamWriter.Flush();
                fileStream.Flush(true);
            }
        }

        public void Save<TObject>(IEnumerable<TObject> items, string fileName, FileFormat fileFormat = FileFormat.Xlsx)
            where TObject : class
        {
            if (null == items)
                throw new ArgumentNullException(nameof(items));

            if (null == fileName)
                throw new ArgumentNullException(nameof(fileName));

            if (FileFormat.Xlsx != fileFormat)
                throw new InvalidOperationException("fileFormat == " + fileFormat);

            var itemList = items.ToList();

            if (0 == itemList.Count)
                return;

            var item = itemList.First();

            var properties = GetProperties(item.GetType());

            var rows = new List<List<object>>();

            var headerRow = new List<object>();

            foreach (var propertyInfo in properties)
            {
                headerRow.Add(propertyInfo.Name);
            }

            rows.Add(headerRow);

            foreach (var o in itemList)
            {
                var row = new List<object>();

                foreach (var propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(o, null);
                    row.Add(value);
                }

                rows.Add(row);
            }

            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            int rowNumber = 1;

            foreach (var row in rows)
            {
                int cellNumber = 1;

                foreach (var cellValue in row)
                {
                    var cell = worksheet.Row(rowNumber).Cell(cellNumber);

                    if (!(cellValue is null))
                    {
                        var type = cellValue.GetType();

                        if (type == typeof(string))
                        {
                            cell.DataType = XLCellValues.Text;
                            cell.Value = cellValue;
                        }
                        else if (type == typeof(bool))
                        {
                            cell.DataType = XLCellValues.Boolean;
                            cell.Value = (bool) cellValue ? "TRUE" : "FALSE";
                        }
                        else if (type == typeof(int))
                        {
                            cell.DataType = XLCellValues.Number;
                            cell.Value = ((int) cellValue).ToString(CultureInfo.InvariantCulture);
                        }
                        else if (type == typeof(long))
                        {
                            cell.DataType = XLCellValues.Number;
                            cell.Value = ((long) cellValue).ToString(CultureInfo.InvariantCulture);
                        }
                        else if (type == typeof(decimal))
                        {
                            cell.DataType = XLCellValues.Number;
                            cell.Value = ((decimal) cellValue).ToString(CultureInfo.InvariantCulture);
                        }
                        else if (type == typeof(DateTime))
                        {
                            cell.DataType = XLCellValues.Text;
                            cell.Value =
                                ((DateTime) cellValue).ToLocalTime()
                                .ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture.DateTimeFormat);
                        }
                    }

                    cellNumber++;
                }

                rowNumber++;
            }

            workbook.SaveAs(fileName);
        }

        private static List<PropertyInfo> GetProperties(Type itemType)
        {
            var properties = itemType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(pi =>
                {
                    if (pi.GetCustomAttributes(typeof(NotMappedAttribute), true).Length > 0)
                        return false;

                    var type = pi.PropertyType;

                    if (typeof(string) == type ||
                        typeof(bool) == type ||
                        typeof(int) == type ||
                        typeof(long) == type ||
                        typeof(decimal) == type ||
                        typeof(DateTime) == type)
                        return true;

                    return false;
                }).ToList();

            return properties;
        }
    }
}

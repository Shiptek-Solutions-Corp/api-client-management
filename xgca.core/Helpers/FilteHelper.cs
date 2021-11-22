using ClosedXML.Excel;
using CsvHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xgca.core.Helpers
{
    public static class FileHelper
    {
        public static async Task<byte[]> GetCsvBytes(IEnumerable<object> items)
        {
            await using var memoryStream = new MemoryStream();
            await using var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
            await csvWriter.WriteRecordsAsync(items);
            await streamWriter.FlushAsync();

            return memoryStream.ToArray();
        }

        public static async Task<byte[]> GetXlsxBytes<T>(List<T> items, string sheetName = null)
        {
            if (items == null || !items.Any()) return null;

            var wb = new XLWorkbook();
            var table = new DataTable { TableName = sheetName ?? "Sheet1" };
            var properties = typeof(T).GetProperties();

            // add columns
            foreach (var propertyInfo in properties)
                table.Columns.Add(propertyInfo.Name, propertyInfo.PropertyType);

            // add rows
            foreach (var item in items)
            {
                var row = table.NewRow();
                foreach (var property in properties)
                    row[property.Name] = property.GetValue(item, null);
                table.Rows.Add(row);
            }

            wb.Worksheets.Add(table);
            await using var memoryStream = new MemoryStream();
            wb.SaveAs(memoryStream);

            return memoryStream.ToArray();
        }
    }
}

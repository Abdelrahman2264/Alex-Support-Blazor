namespace AlexSupport.Services.Extensions
{
    // Services/ExcelService.cs
    using ClosedXML.Excel;
    using System.Text;

    public class ExcelService
    {
        public byte[] GenerateExcel<T>(IEnumerable<T> data, string sheetName = "Sheet1")
        {
            // Required for proper encoding in Excel
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(sheetName);

            // Add header row
            var properties = typeof(T).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                worksheet.Cell(1, i + 1).Value = properties[i].Name;
            }

            // Add data rows
            int row = 2;
            foreach (var item in data)
            {
                for (int i = 0; i < properties.Length; i++)
                {
                    worksheet.Cell(row, i + 1).Value = properties[i].GetValue(item)?.ToString();
                }
                row++;
            }

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);
            return stream.ToArray();
        }
    }
}

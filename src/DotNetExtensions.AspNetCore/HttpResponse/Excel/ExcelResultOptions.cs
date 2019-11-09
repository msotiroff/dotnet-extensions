using DotNetExtensions.ExcelWorksheets.Options;

namespace DotNetExtensions.AspNetCore.HttpResponse.Excel
{
    public class ExcelResultOptions : ExcelWorksheetOptions
    {
        public string FileName { get; set; } = "Worksheet1";

        public ExcelFileExtension FileExtension { get; set; } = ExcelFileExtension.xlsx;
    }
}

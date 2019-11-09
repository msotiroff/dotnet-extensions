using System.Drawing;

namespace DotNetExtensions.ExcelWorksheets.Options
{
    public class ExcelWorksheetOptions
    {
        private const string DefaultSheetName = "Sheet";
        private const string DefaultSheetFontFamily = "Times New Roman";
        private const int DefaultSheetHeaderFontSize = 14;
        private const int DefaultSheetBodyFontSize = 12;

        public string SheetName { get; set; } = DefaultSheetName;

        public int SheetBodyFontSize { get; set; } = DefaultSheetBodyFontSize;

        public int SheetHeaderFontSize { get; set; } = DefaultSheetHeaderFontSize;

        public string SheetBodyFontFamily { get; set; } = DefaultSheetFontFamily;

        public string SheetHeaderFontFamily { get; set; } = DefaultSheetFontFamily;

        public Color SheetHeaderColor { get; set; } = Color.SkyBlue;
    }
}

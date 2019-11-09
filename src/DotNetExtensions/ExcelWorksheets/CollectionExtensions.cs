using DotNetExtensions.Common;
using DotNetExtensions.ExcelWorksheets.Attributes;
using DotNetExtensions.ExcelWorksheets.Options;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace DotNetExtensions.ExcelWorksheets
{
    public static class CollectionExtensions
    {
        private static int currentRow = 1;

        /// <summary>
        /// Builds an excel worksheet and returns its bytes.
        /// </summary>
        /// <typeparam name="TSource">The type, that should be represented in the worksheet.</typeparam>
        /// <param name="source">The collection of elements, to be inserted in the worksheet.</param>
        /// <param name="optionsExpression">(optional) Style options expression.</param>
        /// <returns>The built worksheet's bytes.</returns>
        public static IEnumerable<byte> ToExcelWorksheet<TSource>(
            this IEnumerable<TSource> source,
            Action<ExcelWorksheetOptions> optionsExpression = default)
            where TSource : class
        {
            var options = new ExcelWorksheetOptions();

            if (optionsExpression != default)
            {
                optionsExpression(options);
            }

            return ToExcelWorksheet(source, options);
        }

        /// <summary>
        /// Builds an excel worksheet and returns its bytes.
        /// </summary>
        /// <typeparam name="TSource">The type, that should be represented in the worksheet.</typeparam>
        /// <param name="source">The collection of elements, to be inserted in the worksheet.</param>
        /// <param name="optionsExpression">(optional) Style options expression.</param>
        /// <returns>The built worksheet's bytes.</returns>
        public static IEnumerable<byte> ToExcelWorksheet<TSource>(
            this IEnumerable<TSource> source, 
            ExcelWorksheetOptions options = default)
            where TSource : class
        {
            options = options ?? new ExcelWorksheetOptions();

            if (typeof(IEnumerable).IsAssignableFrom(typeof(TSource)))
            {
                var multiCollection = new List<List<object>>();
                var matrix = source as IEnumerable<IEnumerable>;

                foreach (var row in matrix)
                {
                    var currentRow = new List<object>();
                    
                    multiCollection.Add(currentRow);

                    foreach (var column in row)
                    {
                        currentRow.Add(column);
                    }
                }

                return ToComplexExcelWorksheet(multiCollection, options);
            }

            using (var application = new ExcelPackage())
            {
                application.Workbook.Properties.Title = options.SheetName;
                var sheet = application.Workbook.Worksheets.Add(options.SheetName);

                var allowedProperties = typeof(TSource)
                    .GetProperties()
                    .Select(property => new
                    {
                        Property = property,
                        Attributes = property.GetCustomAttributes(false)
                    })
                    .Where(o => o.Attributes
                        .All(attribute => !(attribute is ExcelIgnoreAttribute)))
                    .ToDictionary(a => a.Property, a => a.Attributes.Cast<Attribute>());

                var headerFont = new Font(
                    options.SheetHeaderFontFamily, 
                    options.SheetHeaderFontSize, 
                    FontStyle.Bold);
                GenerateExcelHeader(
                    sheet, 
                    allowedProperties, 
                    headerFont, 
                    options.SheetHeaderColor);

                var bodyFont = new Font(options.SheetBodyFontFamily, options.SheetBodyFontSize);
                GenerateExcelBody(sheet, source, allowedProperties, bodyFont);

                FormatSheet(sheet);

                currentRow = 1;

                return application.GetAsByteArray();
            }
        }

        /// <summary>
        /// Builds an excel worksheet and returns its bytes.
        /// </summary>
        /// <param name="multiCollections">
        /// The collections of different elements, to be inserted in the worksheet.
        /// </param>
        /// <returns>The built worksheet's bytes.</returns>
        public static IEnumerable<byte> ToComplexExcelWorksheet(
            IEnumerable<IEnumerable<object>> multiCollections,
            Action<ExcelWorksheetOptions> optionsExpression = default)
        {
            var options = new ExcelWorksheetOptions();

            if (optionsExpression != default)
            {
                optionsExpression(options);
            }

            return ToComplexExcelWorksheet(multiCollections, options);
        }

        /// <summary>
        /// Builds an excel worksheet and returns its bytes.
        /// </summary>
        /// <param name="multiCollections">
        /// The collections of different elements, to be inserted in the worksheet.
        /// </param>
        /// <returns>The built worksheet's bytes.</returns>
        public static IEnumerable<byte> ToComplexExcelWorksheet(
            IEnumerable<IEnumerable<object>> multiCollections, 
            ExcelWorksheetOptions options = default)
        {
            if (multiCollections.IsNullOrEmpty())
            {
                return default;
            }

            options = options ?? new ExcelWorksheetOptions();

            using (var application = new ExcelPackage())
            {
                application.Workbook.Properties.Title = options.SheetName;
                var sheet = application.Workbook.Worksheets.Add(options.SheetName);
                var headerFont = new Font(
                    options.SheetHeaderFontFamily, 
                    options.SheetHeaderFontSize, 
                    FontStyle.Bold);
                var bodyFont = new Font(options.SheetBodyFontFamily, options.SheetBodyFontSize);

                foreach (var array in multiCollections)
                {
                    if (array.IsNullOrEmpty())
                    {
                        continue;
                    }

                    var allowedProperties = array
                        .First()
                        .GetType()
                    .GetProperties()
                    .Select(property => new
                    {
                        Property = property,
                        Attributes = property.GetCustomAttributes(true)
                    })
                    .Where(o => o.Attributes
                        .All(attribute => !(attribute is ExcelIgnoreAttribute)))
                    .ToDictionary(a => a.Property, a => a.Attributes.Cast<Attribute>());

                    GenerateExcelHeader(sheet, allowedProperties, headerFont, Color.SkyBlue);

                    GenerateExcelBody(sheet, array, allowedProperties, bodyFont);
                }

                FormatSheet(sheet);

                currentRow = 1;

                return application.GetAsByteArray();
            }
        }

        private static void FormatSheet(ExcelWorksheet sheet)
        {
            sheet.Cells.AutoFitColumns();
        }

        private static void GenerateExcelBody<TSource>(
            ExcelWorksheet sheet,
            IEnumerable<TSource> source,
            Dictionary<PropertyInfo, IEnumerable<Attribute>> properties,
            Font font)
        {
            foreach (var item in source)
            {
                var currentColumn = 1;
                foreach (var property in properties)
                {
                    var currentCell = sheet.Cells[currentRow, currentColumn];
                    var propertyValue = property.Key.GetValue(item);

                    var formatAttribute = property.Value
                        .FirstOrDefault(attr => typeof(ExcelValueFormatAttribute)
                            .IsAssignableFrom(attr.GetType())) as ExcelValueFormatAttribute;

                    var isPropertyFormattable = typeof(IFormattable).IsAssignableFrom(property.Key.PropertyType);

                    if (formatAttribute != null && isPropertyFormattable)
                    {
                        propertyValue = ((IFormattable)propertyValue)
                            .ToString(formatAttribute.Format, CultureInfo.InvariantCulture);
                    }

                    currentCell.Value = propertyValue;
                    currentCell.Style.Font.SetFromFont(font);
                    currentCell.Style.Indent = 1;
                    currentCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    currentCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    currentCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);
                    currentCell.Style.WrapText = false;

                    currentColumn++;
                }

                currentRow++;
            }

            currentRow++;
        }

        private static void GenerateExcelHeader(
            ExcelWorksheet sheet,
            Dictionary<PropertyInfo, IEnumerable<Attribute>> properties,
            Font font,
            Color color)
        {
            var currentColumn = 1;

            foreach (var property in properties)
            {
                var currentCell = sheet.Cells[currentRow, currentColumn];

                var displayName = property
                    .Value
                    .Any(attr => attr.GetType() == typeof(ExcelDisplayNameAttribute))
                    ? ((ExcelDisplayNameAttribute)property
                        .Value
                        .First(attr => attr.GetType() == typeof(ExcelDisplayNameAttribute)))
                        .Name
                    : property.Key.Name;

                currentCell.Value = displayName;

                currentCell.Style.Font.SetFromFont(font);
                currentCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                currentCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                currentCell.Style.Fill.BackgroundColor.SetColor(color);
                currentCell.Style.Border.BorderAround(ExcelBorderStyle.Thin);

                currentColumn++;
            }

            currentRow++;
        }
    }
}

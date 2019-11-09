using DotNetExtensions.ExcelWorksheets;
using DotNetExtensions.ExcelWorksheets.Options;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetExtensions.AspNetCore.HttpResponse.Excel
{
    public static class ControllerBaseExtensions
    {
        public static ExcelResult Excel<T>(
            this ControllerBase controller,
            IEnumerable<T> data,
            Action<ExcelResultOptions> optionsExpression = default)
            where T : class
        {
            var options = new ExcelResultOptions();

            if (optionsExpression != default)
            {
                optionsExpression(options);
            }

            var worksheetOptions = options as ExcelWorksheetOptions;

            return new ExcelResult(data.ToExcelWorksheet(worksheetOptions), optionsExpression);
        }

        public static ExcelResult Excel<T>(
            this ControllerBase controller,
            IEnumerable<IEnumerable<object>> data,
            Action<ExcelResultOptions> optionsExpression = default)
            where T : class
        {
            var options = new ExcelResultOptions();

            if (optionsExpression != default)
            {
                optionsExpression(options);
            }

            var worksheetOptions = options as ExcelWorksheetOptions;

            return new ExcelResult(data.ToExcelWorksheet(worksheetOptions), optionsExpression);
        }
    }
}

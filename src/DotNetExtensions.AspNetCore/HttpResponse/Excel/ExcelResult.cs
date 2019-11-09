﻿using DotNetExtensions.Common;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetExtensions.AspNetCore.HttpResponse.Excel
{
    public class ExcelResult : IActionResult
    {
        private const string ResponseContentType = 
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        private const string ContentDispositionHeaderKey = "content-disposition";
        private const string ContentDispositionHeaderValue = "attachment; filename={0}.{1}";

        private readonly IEnumerable<byte> data;
        private readonly ExcelResultOptions options;

        /// <summary>
        /// Initialize a new instance of MSToolKit.Mvc.ExcelResult.
        /// </summary>
        /// <param name="data">The file's bytes to be attached to the current http response.</param>
        /// <param name="fileName">(optional)The name, that should be used for the attached file.</param>
        /// <param name="fileExtension">(optional)The extension, that should be used for the attached file.</param>
        public ExcelResult(
            IEnumerable<byte> data,
            Action<ExcelResultOptions> optionsExpression)
        {
            this.data = data;
            this.options = new ExcelResultOptions();

            this.ApplyOptions(optionsExpression);
        }

        /// <summary>
        /// Executes the result operation of the action method asynchronously. 
        /// This method is called by MVC to process the result of an action method.
        /// </summary>
        /// <param name="context">
        /// The context in which the result is executed. The context information includes
        /// information about the action that was executed and request information.
        /// </param>
        /// <returns>
        /// The System.Threading.Tasks.Task that represents the asynchronous execute operation.
        /// </returns>
        public async Task ExecuteResultAsync(ActionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var response = context.HttpContext.Response;
            response.ContentType = ResponseContentType;
            response.Headers.Add(
                ContentDispositionHeaderKey,
                string.Format(ContentDispositionHeaderValue,
                    this.options.FileName,
                    this.options.FileExtension.ToString()));

            await response.Body.WriteAsync(this.data.ToArray(), 0, this.data.Count());
        }

        private void ApplyOptions(Action<ExcelResultOptions> optionsExpression)
        {
            if (optionsExpression.IsNull())
            {
                return;
            }

            optionsExpression(this.options);
        }
    }
}

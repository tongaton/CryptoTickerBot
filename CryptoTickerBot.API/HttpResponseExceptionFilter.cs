using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTickerBot.API
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter, IFilterMetadata
    {
        public int Order { get; } = int.MaxValue - 10;

        public void OnActionExecuting(ActionExecutingContext context) { }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception is HttpResponseException exception)
            {
                context.Result =
                    new ObjectResult(new { message = exception.Message, code = exception.Status })
                    {
                        StatusCode = exception.Status,
                    };
                context.ExceptionHandled = true;
            }
        }
    }
}

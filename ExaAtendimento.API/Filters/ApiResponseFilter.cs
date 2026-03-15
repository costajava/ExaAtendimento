using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ExaAtendimento.API.Helpers;

namespace ExaAtendimento.API.Filters
{
    public class ApiResponseFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // nada a fazer por enquanto
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Result is ObjectResult objectResult)
            {
                if (objectResult.StatusCode.HasValue)
                {
                    var statusCode = objectResult.StatusCode.Value;

                    // Se já é 2xx não faz nada
                    if (statusCode >= 200 && statusCode < 300) return;

                    context.Result = new JsonResult(new
                    {
                        statusCode,
                        message = ApiErrorHelper.GetMessageForStatusCode(statusCode)
                    })
                    {
                        StatusCode = statusCode
                    };
                }
            }
            else if (context.Result is StatusCodeResult statusCodeResult)
            {
                var statusCode = statusCodeResult.StatusCode;

                // Se já é 2xx não faz nada
                if (statusCode >= 200 && statusCode < 300) return;

                context.Result = new JsonResult(new
                {
                    statusCode,
                    message = ApiErrorHelper.GetMessageForStatusCode(statusCode)
                })
                {
                    StatusCode = statusCode
                };
            }
        }


    }
}

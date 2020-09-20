using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TechTalkIntegrationTests.Web.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            var result = new ObjectResult(new
            {
                code = 500,
                message = "A server error occurred.",
                detailedMessage = context.Exception.Message
            })
            {
                StatusCode = 500
            };
            context.Result = result;
            context.HttpContext.Response.StatusCode = result.StatusCode.Value;
        }
    }
}

﻿using Microsoft.AspNetCore.Mvc;
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
            });
            context.Result = result;
            context.HttpContext.Response.StatusCode = 500;
        }
    }
}

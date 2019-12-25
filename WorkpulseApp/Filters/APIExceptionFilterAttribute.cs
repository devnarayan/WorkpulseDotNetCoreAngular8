using CORTNE.Exception;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CORTNE.Filters
{
    public class APIExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            HandleException(context.Exception, context);

            base.OnException(context);
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            HandleException(context.Exception, context);

            return base.OnExceptionAsync(context);
        }

        private static void HandleException(System.Exception theException, ExceptionContext actionExecutedContext)
        {
            // 1. cast to type to handle
            // 2. log exception and relevant data
            // 3. throw new HttpResponseException with desired HTTP status code and content for the response

            // RepositoryException is an internal exception that needs to be logged
            var aRepositoryException = theException as RepositoryException;
            if (aRepositoryException != null)
            {
                // Handle web api exception
                actionExecutedContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                actionExecutedContext.HttpContext.Response.ContentType = "application/json";
                actionExecutedContext.Result = new JsonResult(actionExecutedContext.Exception.Message);
            }

        }
    }
}

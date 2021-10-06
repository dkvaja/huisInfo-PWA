using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Portal.JPDS.AppCore.Helpers;

namespace Portal.JPDS.Web.Helpers
{
    public class HttpResponseExceptionFilter : IActionFilter, IOrderedFilter
    {
        public int Order { get; set; } = int.MaxValue - 10;
        public void OnActionExecuting(ActionExecutingContext context) { }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (context != null && context.Exception is HttpResponseException exception)
            {
                context.Result = new ObjectResult($"{exception.ExMessage}")
                {
                    StatusCode = exception.StatusCode,
                };
                context.ExceptionHandled = true;
            }
            //else if (context != null && context.Exception != null)
            //{
            //    context.Result = new ObjectResult($"Error Code : {500} - {context.Exception.Message}")
            //    {
            //        StatusCode = 500,
            //    };
            //    context.ExceptionHandled = true;
            //}
        }
    }
}
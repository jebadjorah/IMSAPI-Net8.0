using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMSAPI.Filters
{
    public class CustomAuthoizationFilter : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            //throw new NotImplementedException();
            Console.WriteLine("OnAction Executed");
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("OnAction Executing");
        }
    }
    public class CustomPrivilageFilter : Attribute, IAuthorizeData
    {
        public string Policy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Roles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string AuthenticationSchemes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            throw new NotImplementedException("");
           // Console.WriteLine("OnAction Executing");
        }
    }

    //public class CustomAuthoizationFilter : Attribute, IAsyncActionFilter
    //{
    //    public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}

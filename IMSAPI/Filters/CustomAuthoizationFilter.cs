using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IMSAPI.Filters
{

    public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Check if the user is in the "Admin" role
            if (!context.HttpContext.User.IsInRole("Admin"))
            {
                // If not, deny access and return a forbidden status
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

    }
    //public class CustomAuthoizationFilter : Attribute, IActionFilter
    //{
    //    public void OnActionExecuted(ActionExecutedContext context)
    //    {
    //        //throw new NotImplementedException();
    //        Console.WriteLine("OnAction Executed");
    //    }

    //    public void OnActionExecuting(ActionExecutingContext context)
    //    {
    //        Console.WriteLine("OnAction Executing");
    //    }
    //}
    //public class CustomPrivilageFilter : Attribute, IAuthorizeData
    //{
    //    public string Policy { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //    public string Roles { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    //    public string AuthenticationSchemes { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    //    {
    //        throw new NotImplementedException("");
    //       // Console.WriteLine("OnAction Executing");
    //    }
    //}



}

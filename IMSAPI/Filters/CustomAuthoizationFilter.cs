using IMSAPI.DB;
using IMSAPI.Services.Administration;
using IMSAPI.Services.Administration.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;

namespace IMSAPI.Filters
{

    public class CustomAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        private IRolePrivilageService _rolePrivilageService;

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _rolePrivilageService = context.HttpContext.RequestServices.GetRequiredService<IRolePrivilageService>();
            
            var UserId = int.Parse(context.HttpContext.User.FindFirst("UserId").Value);
            var RoleId = int.Parse(context.HttpContext.User.FindFirst("RoleId").Value);
            var CompanyId=int.Parse( context.HttpContext.User.FindFirst("CompanyId").Value);
            var actionName = "";
            var ctrlName = "";

            var descriptor = context?.ActionDescriptor as ControllerActionDescriptor;
            if (descriptor != null)
            {
                actionName = descriptor.ActionName;
                ctrlName = descriptor.ControllerName;
            }
            var hasAccess = _rolePrivilageService.GetAccess(CompanyId, RoleId, UserId, actionName, ctrlName);
            // Check if the user is in the "Admin" role
            //if (!context.HttpContext.User.IsInRole("Admin"))
            if(! Convert.ToBoolean(hasAccess))
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

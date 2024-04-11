using IMSAPI.DB;
using IMSAPI.Models.Administration;
using IMSAPI.Services.Administration.Interface;
using IMSAPI.ViewModels.Administration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;
using System.Formats.Asn1;
using System.Reflection;

namespace IMSAPI.Services.Administration
{
    public class RolePrivilageService : IRolePrivilageService
    {
        private readonly AppDbContext _config;
        //public RolePrivilageService()
        //{

        //}
        public RolePrivilageService(AppDbContext config)
        {
            _config = config;
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var item = await _config.rolePrivilageModels.FindAsync(id);
                if (item != null)
                {
                    //await _config.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<IEnumerable<RolePrivilageEntity>> Get(int companayId, int roleId = 0, int id = 0)
        {
            var objList = new List<RolePrivilageEntity>();
            try
            {
                var obj = await _config.rolePrivilageModels.Where(x => (x.Id == id || id == 0) && (x.RoleId == roleId || roleId == 0)).ToListAsync();
                if (obj != null)
                {
                    objList = obj
                        .Select(x => new RolePrivilageEntity
                        {
                            ControllerName = x.ControllerName,
                            ActionName = x.ActionName,
                            RoleId = x.RoleId,
                            IsAllowed = x.IsAllowed,
                            Id = x.Id
                        }).ToList();
                }

            }
            catch (Exception ex)
            {
                return null;
            }
            return objList;
        }
        public bool GetAccess(int companayId, int roleId,int userId, string actionName, string controllerName)
        {
            try
            {
                var obj = _config.rolePrivilageModels.Where(x => x.RoleId == roleId && x.ActionName== actionName && x.ControllerName== controllerName).First();
                if (obj != null)
                {
                   // return true;
                    return obj.IsAllowed ?? false;
                }//
            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public async Task<bool> SaveUpdate(RolePrivilageEntity obj)
        {
            try
            {
                if (obj.Id > 0)
                {
                    var result = await _config.rolePrivilageModels.FirstOrDefaultAsync(x => x.Id == obj.Id);
                    if (result != null)
                    {
                        result.ControllerName = obj.ControllerName;
                        result.ActionName = obj.ActionName;
                        result.RoleId = obj.RoleId;
                        result.IsAllowed = obj.IsAllowed;
                        result.UpdatedOn = DateTime.Now;
                        result.UpdatedBy = 0;
                        await _config.SaveChangesAsync();
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    var result = new RolePrivilageModel();
                    result.ControllerName = obj.ControllerName;
                    result.ActionName = obj.ActionName;
                    result.RoleId = obj.RoleId;
                    result.IsAllowed = obj.IsAllowed;
                    result.CreatedOn = DateTime.Now;
                    result.CreatedBy = 0;
                    await _config.rolePrivilageModels.AddAsync(result);
                    await _config.SaveChangesAsync();
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task GetAllControllerANDActionName()
        {

            
                Assembly asm = Assembly.GetAssembly(typeof(Program));

                var controlleractionlist = asm.GetTypes()
                        .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                        .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
                        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            foreach (var item in controlleractionlist)
            {
                if (!item.Controller.Contains("Login"))
                {
                    // add into your list
                    Console.WriteLine($"Controller: {item.Controller}, Action: {item.Action} ");
                }
            }

            //var controllerTypes = Assembly.GetExecutingAssembly().GetTypes()
            //.Where(t => typeof(ControllerBase).IsAssignableFrom(t));

            //foreach (var controllerType in controllerTypes)
            //{
            //    // Get all public methods of the controller
            //    var actionMethods = controllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
            //        .Where(m => typeof(ActionResult).IsAssignableFrom(m.ReturnType));

            //    foreach (var method in actionMethods)
            //    {
            //        // Print the controller and action names
            //        Console.WriteLine($"Controller: {controllerType.Name}, Action: {method.Name}");
            //    }
            //}
        }
    }
}

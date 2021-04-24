using AdrianEShop.Core.Services.Category;
using AdrianEShop.Core.Services.User;
using AdrianEShop.DataAccess.Data;
using AdrianEShop.Models.ViewModels.Category;
using AdrianEShop.Models.ViewModels.User;
using AdrianEShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class UserController : Controller
    {

        private readonly IUserManagementService _userManagementService;

        public UserController(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }
        public IActionResult Index()
        {
            UserManagementIndexVM userManagementVM = new UserManagementIndexVM();
            userManagementVM.PageTitle = "Users List";
        
            return View(userManagementVM);
        }

       

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allUsers = _userManagementService.GetAll();
            return Json(new { data = allUsers });
        }

        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            Guid guid = Guid.Parse(id);
            var item = _userManagementService.Get(guid);
            if(item == null)
            {
                return Json(new { success = false, message = "Couldn't perform operation" });
            }
            if(item.LockoutEnd != null && item.LockoutEnd > DateTime.Now)
            {
                item.LockoutEnd = DateTime.Now;
            }
            else
            {
                item.LockoutEnd = DateTime.Now.AddYears(100);
            }
            _userManagementService.Save();
            return Json(new { success = true, message = "Operation successfull" });
        }

        #endregion
    }
}

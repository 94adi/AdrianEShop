using AdrianEShop.Core.Services.Category;
using AdrianEShop.Core.Services.User;
using AdrianEShop.DataAccess.Data;
using AdrianEShop.Models.ViewModels.Category;
using AdrianEShop.Models.ViewModels.User;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
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

        #endregion
    }
}

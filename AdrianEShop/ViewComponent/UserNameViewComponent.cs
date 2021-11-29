using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AdrianEShop.Core.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace AdrianEShop.ViewComponents
{
    public class UserNameViewComponent : ViewComponent
    {

        private readonly IUserManagementService _userManagementService;

        public UserNameViewComponent(IUserManagementService userManagementService)
        {
            _userManagementService = userManagementService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var guid = new Guid(claims.Value.ToString());
            var userFromDb = _userManagementService.Get(guid);

            return View(userFromDb);
        }

    }
}

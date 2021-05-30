using AdrianEShop.Core.Services.ShoppingCart;
using AdrianEShop.Core.Services.User;
using AdrianEShop.Models;
using AdrianEShop.Models.ViewModels.ShoppingCart;
using AdrianEShop.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly int MAX_DESCRIPTION_CHARS = 100;

        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IUserManagementService _userManagementService;

        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IEmailSender emailSender,
                              UserManager<IdentityUser> userManager,
                              IShoppingCartService shoppingCartService,
                              IUserManagementService userManagementService)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _shoppingCartService = shoppingCartService;
            _userManagementService = userManagementService;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = claim.Value;

            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _shoppingCartService.GetAll(userId, includeProperties: "Product")
            };

            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _userManagementService.Get(new Guid(userId));

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
                cart.Product.ShortDescription = StaticDetails.ConvertToRawHtml(cart.Product.ShortDescription);
                if(cart.Product.ShortDescription.Length > MAX_DESCRIPTION_CHARS)
                {
                    cart.Product.ShortDescription = cart.Product.ShortDescription.Substring(0, 99) + "...";
                }
            }

            return View(ShoppingCartVM);
        }
    }
}

using AdrianEShop.Core.Services.ShoppingCart;
using AdrianEShop.Core.Services.User;
using AdrianEShop.Models;
using AdrianEShop.Models.ViewModels.ShoppingCart;
using AdrianEShop.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
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

        [HttpGet]
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

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var user = _userManagementService.Get(claim.Value);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Verification email is empty!");
                return RedirectToAction("Index");
            }

            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(user.Email, "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");

            return RedirectToAction("Index");
        }

        public IActionResult Plus(int cartId)
        {
            var cart = _shoppingCartService.GetCart(cartId, includeProperties: "Product");
            cart.Count += 1;
            cart.Price = cart.Product.Price * cart.Count;
            _shoppingCartService.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Minus(int cartId)
        {
            var cart = _shoppingCartService.GetCart(cartId, includeProperties: "Product");
            if(cart.Count > 1)
            {
                cart.Count -= 1;
                cart.Price = cart.Product.Price * cart.Count;
            }
            else
            {
                int productsCount = _shoppingCartService.GetProductsCount(cart.ApplicationUserId) - 1;
                HttpContext.Session.SetInt32(StaticDetails.Shopping_Cart_Session, productsCount);
                _shoppingCartService.Remove(cartId);
            }
            _shoppingCartService.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Remove(int cartId)
        {
            var cart = _shoppingCartService.GetCart(cartId);

            int productsCount = _shoppingCartService.GetProductsCount(cart.ApplicationUserId) - 1;
            HttpContext.Session.SetInt32(StaticDetails.Shopping_Cart_Session, productsCount);

            _shoppingCartService.Remove(cartId);
            _shoppingCartService.Save();

            return RedirectToAction(nameof(Index));
        }
    }
}

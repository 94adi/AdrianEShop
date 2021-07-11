using AdrianEShop.Core.DAInterfaces;
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
using Stripe;
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
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IEmailSender emailSender,
                              UserManager<IdentityUser> userManager,
                              IShoppingCartService shoppingCartService,
                              IUserManagementService userManagementService,
                              IUnitOfWork unitOfWork)
        {
            _emailSender = emailSender;
            _userManager = userManager;
            _shoppingCartService = shoppingCartService;
            _userManagementService = userManagementService;
            _unitOfWork = unitOfWork;
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

        [HttpGet]
        public IActionResult Summary()
        {

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Guid userId = new Guid(claim.Value);

            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _shoppingCartService.GetAll(userId.ToString(), includeProperties: "Product")
            };



            ShoppingCartVM.OrderHeader.ApplicationUser = _userManagementService.Get(userId);

            foreach (var cart in ShoppingCartVM.ListCart)
            {
                cart.Price = cart.Product.Price;
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            ShoppingCartVM.OrderHeader.Name = ShoppingCartVM.OrderHeader.ApplicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = ShoppingCartVM.OrderHeader.ApplicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = ShoppingCartVM.OrderHeader.ApplicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = ShoppingCartVM.OrderHeader.ApplicationUser.City;
            ShoppingCartVM.OrderHeader.State = ShoppingCartVM.OrderHeader.ApplicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = ShoppingCartVM.OrderHeader.ApplicationUser.PostalCode;

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            Guid userId = new Guid(claim.Value);

            ShoppingCartVM.OrderHeader.ApplicationUser = _userManagementService.Get(userId);

            ShoppingCartVM.ListCart = _shoppingCartService.GetAll(claim.Value, includeProperties: "Product");

            ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
            ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;
            ShoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach(var item in ShoppingCartVM.ListCart)
            {
                item.Price = item.Count * item.Product.Price;
                OrderDetails orderDetails = new OrderDetails()
                {
                    ProductId = item.ProductId,
                    OrderId = ShoppingCartVM.OrderHeader.Id,
                    Price = item.Price,
                    Count = item.Count
                };

                ShoppingCartVM.OrderHeader.OrderTotal += orderDetails.Count * orderDetails.Price;
                _unitOfWork.OrderDetails.Add(orderDetails);
                _unitOfWork.Save();

            }

            _unitOfWork.ShoppingCart.Remove(ShoppingCartVM.ListCart);
            
            _unitOfWork.Save();

            HttpContext.Session.SetInt32(StaticDetails.Shopping_Cart_Session, 0);

            if(stripeToken == null)
            {

            }
            else
            {
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(ShoppingCartVM.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = "Order ID : " + ShoppingCartVM.OrderHeader.Id,
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);

                if(charge.BalanceTransactionId == null)
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusRejected;
                }
                else
                {
                    ShoppingCartVM.OrderHeader.TransactionId = charge.BalanceTransactionId;
                }
                if(charge.Status.ToLower() == "succeeded")
                {
                    ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusApproved;
                    ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusApproved;
                    ShoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
                }
            }

            _unitOfWork.Save();

            return RedirectToAction("OrderConfirmation","Cart",new { id = ShoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}

using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.Core.Services.Product;
using AdrianEShop.Core.Services.ShoppingCart;
using AdrianEShop.Models;
using AdrianEShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _shoppingCartService;

        public HomeController(IUnitOfWork unitOfWork,
                               IProductService productService,
                               IShoppingCartService shoppingCartService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _shoppingCartService = shoppingCartService;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productsList = _unitOfWork.Product.GetAll(includeProperties:"Category,Manufacturer");

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                var count = _shoppingCartService.GetProductsCount(claim.Value);
                HttpContext.Session.SetInt32(StaticDetails.Shopping_Cart_Session, count);
            }

            return View(productsList);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var productFromDb = _productService.GetProduct(id, includeProperties: "Manufacturer,Category");
            ShoppingCart cartObj = new ShoppingCart()
            {
                Product = productFromDb,
                ProductId = productFromDb.Id
            };

            return View(cartObj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            shoppingCart.Id = 0;
            var productFromDb = _productService.GetProduct(shoppingCart.ProductId, includeProperties: "Manufacturer,Category");

            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = _shoppingCartService.GetCurrentCart(claim.Value, shoppingCart.ProductId);

                if(cartFromDb == null)
                {
                    shoppingCart.Product = productFromDb;

                    _shoppingCartService.Upsert(shoppingCart);
                }
                else
                {
                    cartFromDb.Count += shoppingCart.Count;
                    //optional
                    _shoppingCartService.Upsert(cartFromDb);
                }

                _shoppingCartService.Save();

                var count = _shoppingCartService.GetProductsCount(shoppingCart.ApplicationUserId);
                HttpContext.Session.SetInt32(StaticDetails.Shopping_Cart_Session, count);
                return RedirectToAction(nameof(Index));
            }


                ShoppingCart cartObj = new ShoppingCart()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.Id
                };

                return View(cartObj);
        }
    }
}

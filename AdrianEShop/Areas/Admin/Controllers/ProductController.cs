using AdrianEShop.Core.Services.Manufacturer;
using AdrianEShop.Core.Services.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdrianEShop.Core.Services.Category;
using AdrianEShop.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdrianEShop.Models;

namespace AdrianEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;

        public ProductController(IProductService productService,
                                 IManufacturerService manufacturerService,
                                 ICategoryService categoryService)
        {
            _productService = productService;
            _manufacturerService = manufacturerService;
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            var manufacturers = _manufacturerService.GetAllManufacturers();
            var categories = _categoryService.GetAllCategories();

            ProductUpsertVM productUpsertVM = new ProductUpsertVM
            {
                CategoryList = categories.Select(i => new SelectListItem {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                ManufacturerList = manufacturers.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Product = new Product()
            };

            if(id == null)
            {
                return View();
            }

            productUpsertVM.Product = _productService.GetProduct(id.Value);
            if(productUpsertVM.Product == null)
            {
                return NotFound();
            }
            return View(productUpsertVM);
        }
    }
}

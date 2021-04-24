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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using AdrianEShop.Utility;

namespace AdrianEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
    public class ProductController : Controller
    {

        private readonly IProductService _productService;
        private readonly IManufacturerService _manufacturerService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IProductService productService,
                                 IManufacturerService manufacturerService,
                                 ICategoryService categoryService,
                                 IWebHostEnvironment hostEnvironment)
        {
            _productService = productService;
            _manufacturerService = manufacturerService;
            _categoryService = categoryService;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            var products = _productService.GetAllProducts();
            return View();
        }

        [HttpGet]
        public IActionResult Upsert(Guid? id)
        {
            var manufacturers = _manufacturerService.GetAll();
            var categories = _categoryService.GetAll();

            ProductUpsertVM productUpsertVM = new ProductUpsertVM
            {
                CategoryList = categories.Select(i => new SelectListItem
                {
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

            if (id.HasValue)
            {
                productUpsertVM.Product = _productService.GetProduct(id.Value);
                productUpsertVM.PageTitle = "Update Product";
            }
            else
            {
                productUpsertVM.Product = new Product();
                productUpsertVM.PageTitle = "Create Product";
            }

            if (productUpsertVM.Product == null)
            {
                return NotFound();
            }
            return View(productUpsertVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductUpsertVM productVM)
        {
            if (ModelState.IsValid)
            {
                string rootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if(files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadFolder = Path.Combine(rootPath, @"images\products");
                    var extension = Path.GetExtension(files[0].FileName);

                    if(productVM.Product.ImageURL != null)
                    {
                        var existingFilePath = Path.Combine(rootPath, productVM.Product.ImageURL.TrimStart('\\'));
                        if (System.IO.File.Exists(existingFilePath))
                        {
                            System.IO.File.Delete(existingFilePath);
                        }
                    }
                    using var fileStream = new FileStream(Path.Combine(uploadFolder, fileName + extension), FileMode.Create);
                    files[0].CopyTo(fileStream);
                    productVM.Product.ImageURL = @"\images\products\" + fileName + extension;
                }
                else
                {
                    if(productVM.Product.Id != Guid.Empty)
                    {
                        string filePathFromDb = _productService.GetProduct(productVM.Product.Id).ImageURL;
                        productVM.Product.ImageURL = filePathFromDb;
                    }
                }
                _productService.Upsert(productVM.Product);
                _productService.Save();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                //rebuild VM
                productVM.CategoryList = _categoryService.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                productVM.ManufacturerList = _manufacturerService.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

                if(productVM.Product.Id != Guid.Empty)
                {
                    productVM.Product = _productService.GetProduct(productVM.Product.Id);
                }
            }
            return View(productVM);
        }



# region API_CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _productService.GetAllProducts(includeProperties: "Manufacturer,Category");
            return Json(new { data = products});
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var productFromDb = _productService.GetProduct(id);
            if (productFromDb == null)
            {
                TempData["Error"] = "Could not delete product";
                return Json(new { success = false, message = "Could not delete product" });
            }

            _productService.Remove(productFromDb);
            _productService.Save();
            TempData["Success"] = "The product has been successfully deleted";
            return Json(new { success = true, message = "The product has been successfully deleted" });
        }

#endregion
    }
}

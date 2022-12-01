using AdrianEShop.Core.Services.Category;
using AdrianEShop.Models.ViewModels.Category;
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
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(int productPage = 1)
        {
            CategoryIndexVM categoryVM = new CategoryIndexVM();

            var categories = await _categoryService.GetAllAsync();

            categoryVM.PageTitle = "Categories List";

            categoryVM.Categories = categories.OrderBy(c => c.Name).Skip((productPage - 1) * 2).Take(2).ToList();

            var count = categories.Count();

            categoryVM.PagingInfo = new Models.PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = 2,
                TotalItem = count,
                urlParam = "/Admin/Category/Index?productPage=:"
            };

            return View(categoryVM);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            CategoryUpsertVM categoryVM = new CategoryUpsertVM();
           
            if (id.HasValue)
            {
                Models.Category category = await _categoryService.GetAsync(id.Value);

                if(category == null)
                {
                    return NotFound();
                }
                categoryVM.PageTitle = "Update Category";
                categoryVM.Category = category;
                return View(categoryVM);
            }
            else
            {
                categoryVM.PageTitle = "Create a new category";
                categoryVM.Category = new Models.Category();
            }

            return View(categoryVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(CategoryUpsertVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpsertAsync(categoryVM.Category);
                _categoryService.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(categoryVM);
        }

        #region API CALLS

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var allCategories = await _categoryService.GetAllAsync();
            return Json(new { data = allCategories });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var categoryFromDb = await _categoryService.GetAsync(id);
            if(categoryFromDb != null)
            {
                await _categoryService.RemoveAsync(categoryFromDb.Id);
                _categoryService.Save();
                TempData["Success"] = "Category successfully deleted";
                return Json(new { success = true, message = "Category successfully deleted" });
            }

            TempData["Error"] = "Error deleting category";
            return Json(new { success = false, message = "Error deleting category" });
        }

        #endregion
    }
}

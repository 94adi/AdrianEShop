using AdrianEShop.Core.Services.Category;
using AdrianEShop.Models.ViewModels.Category;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public IActionResult Index()
        {
            CategoryIndexVM categoryVM = new CategoryIndexVM();
            categoryVM.PageTitle = "Categories List";

            categoryVM.Categories = _categoryService.GetAll();

            return View(categoryVM);
        }

        [HttpGet]
        public IActionResult Upsert(Guid? id)
        {
            CategoryUpsertVM categoryVM = new CategoryUpsertVM();
           
            if (id.HasValue)
            {
                Models.Category category = _categoryService.Get(id.Value);

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
        public IActionResult Upsert(CategoryUpsertVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                _categoryService.Upsert(categoryVM.Category);
                _categoryService.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(categoryVM);
        }

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allCategories = _categoryService.GetAll();
            return Json(new { data = allCategories });
        }

        [HttpDelete]
        public IActionResult Delete(Guid id)
        {
            var categoryFromDb = _categoryService.Get(id);
            if(categoryFromDb != null)
            {
                _categoryService.Remove(categoryFromDb.Id);
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

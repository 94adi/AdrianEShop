using AdrianEShop.Core.Services.Manufacturer;
using AdrianEShop.Models;
using AdrianEShop.Models.ViewModels.Manufacturer;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        public IActionResult Index()
        {
            ManufacturerIndexVM manufacturerVM = new ManufacturerIndexVM();
            manufacturerVM.Manufacturers = _manufacturerService.GetAll();
            manufacturerVM.PageTitle = "Manufacturers List";
            return View(manufacturerVM);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ManufacturerUpsertVM manufacturerVM = new ManufacturerUpsertVM();

            if(id == null)
            {
                manufacturerVM.Manufacturer = new Manufacturer();
                manufacturerVM.PageTitle = "Create manufacturer";
                return View(manufacturerVM);
            }

            manufacturerVM.Manufacturer = _manufacturerService.Get(id.Value);
            manufacturerVM.PageTitle = "Update manufacturer";

            if (manufacturerVM.Manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ManufacturerUpsertVM manufacturerVM)
        {
            if (ModelState.IsValid)
            {
                _manufacturerService.Upsert(manufacturerVM.Manufacturer);
                _manufacturerService.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(manufacturerVM);
        }

        #region API

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var manufacturerFromDb = _manufacturerService.Get(id);
            if(manufacturerFromDb == null)
            {
                TempData["Error"] = "Could not delete manufacturer";
                return Json(new { success = false, message = "Could not delete manufacturer" });
            }

            _manufacturerService.Remove(manufacturerFromDb.Id);
            _manufacturerService.Save();
            TempData["Success"] = "The manufacturer has been successfully deleted";
            return Json(new { success = true, message = "The manufacturer has been successfully deleted" });
            
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Manufacturer> manufacturers = _manufacturerService.GetAll();
            return Json(new { data = manufacturers });
        }


        #endregion
    }
}

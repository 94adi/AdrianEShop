using AdrianEShop.Core.Services.Manufacturer;
using AdrianEShop.Models;
using AdrianEShop.Models.ViewModels.Manufacturer;
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
    public class ManufacturerController : Controller
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        public async Task<IActionResult> Index()
        {
            ManufacturerIndexVM manufacturerVM = new ManufacturerIndexVM();
            manufacturerVM.Manufacturers = await _manufacturerService.GetAllAsync();
            manufacturerVM.PageTitle = "Manufacturers List";
            return View(manufacturerVM);
        }

        [HttpGet]
        public async Task<IActionResult> Upsert(Guid? id)
        {
            ManufacturerUpsertVM manufacturerVM = new ManufacturerUpsertVM();

            if(id == null)
            {
                manufacturerVM.Manufacturer = new Manufacturer();
                manufacturerVM.PageTitle = "Create manufacturer";
                return View(manufacturerVM);
            }

            manufacturerVM.Manufacturer = await _manufacturerService.GetAsync(id.Value);
            manufacturerVM.PageTitle = "Update manufacturer";

            if (manufacturerVM.Manufacturer == null)
            {
                return NotFound();
            }

            return View(manufacturerVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Upsert(ManufacturerUpsertVM manufacturerVM)
        {
            if (ModelState.IsValid)
            {
                await _manufacturerService.UpsertAsync(manufacturerVM.Manufacturer);
                _manufacturerService.Save();
                return RedirectToAction(nameof(Index));
            }

            return View(manufacturerVM);
        }

        #region API

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            var manufacturerFromDb = await _manufacturerService.GetAsync(id);
            if(manufacturerFromDb == null)
            {
                TempData["Error"] = "Could not delete manufacturer";
                return Json(new { success = false, message = "Could not delete manufacturer" });
            }

            await _manufacturerService.RemoveAsync(manufacturerFromDb.Id);
            _manufacturerService.Save();
            TempData["Success"] = "The manufacturer has been successfully deleted";
            return Json(new { success = true, message = "The manufacturer has been successfully deleted" });
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            IEnumerable<Manufacturer> manufacturers = await _manufacturerService.GetAllAsync();
            return Json(new { data = manufacturers });
        }


        #endregion
    }
}

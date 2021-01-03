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
            manufacturerVM.Manufacturers = _manufacturerService.GetAllManufacturers();
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

            manufacturerVM.Manufacturer = _manufacturerService.GetManufacturer(id.Value);
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
                return RedirectToAction(nameof(Index));
            }

            return View(manufacturerVM);
        }
    }
}

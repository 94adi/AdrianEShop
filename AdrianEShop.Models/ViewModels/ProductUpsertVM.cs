using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Models.ViewModels
{
    public class ProductUpsertVM
    {
        public Product Product { get; set; }

        public IEnumerable<SelectListItem> ManufacturerList { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}

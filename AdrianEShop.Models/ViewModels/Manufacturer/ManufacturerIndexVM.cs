using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Models.ViewModels.Manufacturer
{
    public class ManufacturerIndexVM
    {
        public IEnumerable<Models.Manufacturer> Manufacturers {get;set;}
        public string PageTitle { get; set; }
    }
}

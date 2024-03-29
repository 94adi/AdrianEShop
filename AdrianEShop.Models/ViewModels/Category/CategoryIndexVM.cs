﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Models.ViewModels.Category
{
    public class CategoryIndexVM
    {
        public IEnumerable<Models.Category> Categories { get; set; }

        public string PageTitle { get; set; }

        public PagingInfo PagingInfo { get; set; }
    }
}

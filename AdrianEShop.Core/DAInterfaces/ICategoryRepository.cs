﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;
namespace AdrianEShop.Core.DAInterfaces
{
    public interface ICategoryRepository : IRepositoryAsync<Category>
    {
        Task UpdateAsync(Category category);
    }
}

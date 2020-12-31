﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AdrianEShop.Core.DAInterfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Product { get; }

        IManufacturerRepository Manufacturer { get; }

        ICategoryRepository Category { get; }

        void Save();
    }
}
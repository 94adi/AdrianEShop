﻿using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;

            Product = new ProductRepository(db);
            Category = new CategoryRepository(db);
            Manufacturer = new ManufacturerRepository(db);
            ApplicationUser = new ApplicationUserRepository(db);
        }

        public IProductRepository Product { get; private set; }

        public IManufacturerRepository Manufacturer { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }
        public void Dispose()
        {
            _db.Dispose();
        }

        public void Save()
        {
            _db.SaveChanges();
        }

    }
}

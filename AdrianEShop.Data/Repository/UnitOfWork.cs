using AdrianEShop.Core.DAInterfaces;
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

            Product = new ProductRepository(_db);
            Category = new CategoryRepository(_db);
            Manufacturer = new ManufacturerRepository(_db);
            ApplicationUser = new ApplicationUserRepository(_db);
            ShoppingCart = new ShoppingCartRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);
            OrderHeader = new OrderHeaderRepository(_db);
        }

        public IProductRepository Product { get; private set; }

        public IManufacturerRepository Manufacturer { get; private set; }

        public ICategoryRepository Category { get; private set; }

        public IApplicationUserRepository ApplicationUser { get; private set; }

        public IOrderHeaderRepository OrderHeader { get; private set; }

        public IOrderDetailsRepository OrderDetails { get; private set; }

        public IShoppingCartRepository ShoppingCart { get; private set;
        }
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

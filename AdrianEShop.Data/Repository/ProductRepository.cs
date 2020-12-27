using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.DataAccess.Data;
using AdrianEShop.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var productFromDb = _db.Products.FirstOrDefault(p => p.Id == product.Id);

            if(productFromDb != null)
            {
                productFromDb.ImageURL = product.ImageURL != null ? product.ImageURL : productFromDb.ImageURL;
                productFromDb.LongDescription = product.LongDescription;
                productFromDb.ShortDescription = product.ShortDescription;
                productFromDb.ManufacturerId = product.ManufacturerId;
                productFromDb.CategoryId = product.CategoryId;
                productFromDb.Price = product.Price;
                productFromDb.Title = product.Title;
                productFromDb.YearOfManufacture = product.YearOfManufacture;
                productFromDb.DiscountCode = product.DiscountCode;
                productFromDb.LastEdited = DateTime.Now;
            }
        }
    }
}

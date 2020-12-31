using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.DataAccess.Data;
using AdrianEShop.Models;
namespace AdrianEShop.DataAccess.Repository
{
    public class ManufacturerRepository : Repository<Manufacturer>, IManufacturerRepository
    {
        private readonly ApplicationDbContext _db;

        public ManufacturerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public void Update(Manufacturer manufacturer)
        {
            var manufacturerFromDb = _db.Manufacturers.FirstOrDefault(m => m.Id == manufacturer.Id);
            if(manufacturerFromDb != null)
            {
                manufacturerFromDb.Name = manufacturer.Name;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;
using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.DataAccess.Data;
using Microsoft.AspNetCore.Identity;

namespace AdrianEShop.DataAccess.Repository
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;

        public ApplicationUserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<IdentityUserRole<string>> GetUserRoles()
        {
            return _db.UserRoles.ToList();
        }

        public IEnumerable<IdentityRole> GetAllRoles()
        {
            return _db.Roles.ToList();
        }
    }
}

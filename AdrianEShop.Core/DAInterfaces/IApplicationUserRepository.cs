using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;
using Microsoft.AspNetCore.Identity;

namespace AdrianEShop.Core.DAInterfaces
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser>
    {
        public IEnumerable<IdentityUserRole<string>> GetUserRoles();
        public IEnumerable<IdentityRole> GetAllRoles();
    }
}

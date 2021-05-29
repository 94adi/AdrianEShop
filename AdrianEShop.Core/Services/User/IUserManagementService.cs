using AdrianEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.User
{
    public interface IUserManagementService
    {
         IEnumerable<ApplicationUser> GetAll();

         ApplicationUser Get(Guid id);

         ApplicationUser Get(string email);

         void Save();
    }
}

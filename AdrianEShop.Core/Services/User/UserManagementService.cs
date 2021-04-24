using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.User
{
    public class UserManagementService : IUserManagementService
    {

        private readonly IUnitOfWork _unitOfWork;

        public UserManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ApplicationUser Get(Guid id)
        {
            var objFromDb = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id.ToString());
            return objFromDb;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            var users = _unitOfWork.ApplicationUser.GetAll();
            var userRole = _unitOfWork.ApplicationUser.GetUserRoles();
            var roles = _unitOfWork.ApplicationUser.GetAllRoles();

            foreach(var user in users)
            {              
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;

            }

            return users;
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}

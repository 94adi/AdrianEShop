using AdrianEShop.Core.DAInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdrianEShop.Models;
using System.Security.Claims;
using AdrianEShop.Utility;

namespace AdrianEShop.Core.Services.OrderHeader
{
    public class OrderHeaderService : IOrderHeaderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderHeaderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<Models.OrderHeader> GetAllOrders(string includeProperties = null)
        {
            IEnumerable<Models.OrderHeader> orderHeaderList = _unitOfWork.OrderHeader.GetAll(includeProperties: includeProperties);

            return orderHeaderList;
        }

        public IEnumerable<Models.OrderHeader> GetAllOrders(string id, string includeProperties = null)
        {
            IEnumerable<Models.OrderHeader> orderHeaderList = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId == id,
                                                                                            includeProperties: includeProperties);

            return orderHeaderList;
        }

        public IEnumerable<Models.OrderHeader> GetAllOrders(ClaimsPrincipal User, string includeProperties = null)
        {
            IEnumerable<Models.OrderHeader> orderHeaderList;
            string userId = StaticDetails.GetUserId(User);

            if (User.IsInRole(StaticDetails.Role_Admin) || User.IsInRole(StaticDetails.Role_Employee))
            {
                orderHeaderList = GetAllOrders(includeProperties: "ApplicationUser");
            }
            else
            {
                orderHeaderList = GetAllOrders(userId, includeProperties: "ApplicationUser");
            }

            return orderHeaderList;
        }

        public Models.OrderHeader GetOrder(string id)
        {
            var uid = new Guid(id);
            return _unitOfWork.OrderHeader.Get(uid);
        }

        public Models.OrderHeader GetOrder(string id, string includeProperties = null)
        {
            var uid = new Guid(id);
            return _unitOfWork.OrderHeader.Get(uid, null,includeProperties);
        }

        public void Remove(Models.OrderHeader orderHeader)
        {
            _unitOfWork.OrderHeader.Remove(orderHeader);
        }

        public void Save()
        {
            _unitOfWork.Save();
        }

        public void Upsert(Models.OrderHeader orderHeader)
        {
            if (orderHeader.Id == Guid.Empty)
            {
                _unitOfWork.OrderHeader.Add(orderHeader);
            }
            else
            {
                _unitOfWork.OrderHeader.Update(orderHeader);
            }
            
        }
    }
}

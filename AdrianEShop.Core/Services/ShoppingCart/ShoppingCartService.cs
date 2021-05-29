using AdrianEShop.Core.DAInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AdrianEShop.Core.Services.ShoppingCart
{
    public class ShoppingCartService : IShoppingCartService
    {

        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Models.ShoppingCart GetCurrentCart(string userId, Guid productId)
        {
            Expression<Func<Models.ShoppingCart, bool>> filter =  (u) => u.ApplicationUserId == userId && u.ProductId == productId;
            var objFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(filter, includeProperties: "Product");
            return objFromDb;
        }

        public void Upsert(Models.ShoppingCart cart)
        {
            if(cart == null)
            {
                _unitOfWork.ShoppingCart.Add(cart);
            }
            else
            {
                _unitOfWork.ShoppingCart.Update(cart);
            }
        }

        public int GetProductsCount(string userId)
        {
            int count = 0;
            var result = _unitOfWork.ShoppingCart.GetAll(c => c.ApplicationUserId == userId);
            if(result != null)
            {
                count = result.ToList().Count();
            }
            return count;
        }

        public void Save()
        {
            _unitOfWork.Save();
        }
    }
}

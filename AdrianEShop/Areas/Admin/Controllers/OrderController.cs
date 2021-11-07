using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.Core.Services.OrderHeader;
using AdrianEShop.Models.ViewModels.Order;
using AdrianEShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AdrianEShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IOrderHeaderService _orderHeaderService;
        private readonly IUnitOfWork _unitOfWork;

        [BindProperty]
        public OrderDetailsVM OrderDetailsVM { get; set; }

        public OrderController(IOrderHeaderService orderHeaderService,IUnitOfWork unitOfWork)
        {
            _orderHeaderService = orderHeaderService;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(Guid id)
        {
            //rderDetailsVM.OrderHeader = _orderHeaderService.GetOrder(id, "ApplicationUser");
            //OrderDetailsVM.OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId);
            return View();
        }


        #region API_CALLS
        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            string userId = StaticDetails.GetUserId(User);
            Func<Models.OrderHeader, bool> filterOrders = null;
            IEnumerable<Models.OrderHeader> orderHeaderList;

            if(User.IsInRole(StaticDetails.Role_Admin) || User.IsInRole(StaticDetails.Role_Employee))
            {
                orderHeaderList = _orderHeaderService.GetAllOrders(includeProperties: "ApplicationUser");
            }
            else
            {
                orderHeaderList = _orderHeaderService.GetAllOrders(userId, includeProperties: "ApplicationUser");
            }

            switch (status)
            {
                case "pending":
                    filterOrders = (o => o.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
                    orderHeaderList = orderHeaderList.Where(filterOrders);
                    break;

                case "inprocess":
                    filterOrders = (o => o.OrderStatus == StaticDetails.StatusApproved ||
                                         o.OrderStatus == StaticDetails.StatusInProcess ||
                                         o.OrderStatus == StaticDetails.StatusPending);
                    orderHeaderList = orderHeaderList.Where(filterOrders);
                    break;

                case "completed":
                    filterOrders = (o => o.OrderStatus == StaticDetails.StatusShipped);
                    orderHeaderList = orderHeaderList.Where(filterOrders);
                    break;

                case "rejected":
                    filterOrders = (o => o.OrderStatus == StaticDetails.StatusCancelled ||
                                         o.OrderStatus == StaticDetails.StatusRefunded ||
                                         o.OrderStatus == StaticDetails.PaymentStatusRejected);
                    orderHeaderList = orderHeaderList.Where(filterOrders);
                    break;

                default:
                    break;
            }

            

            return Json(new { data = orderHeaderList });
        }
        #endregion
    }

}

using AdrianEShop.Core.DAInterfaces;
using AdrianEShop.Core.Services.OrderHeader;
using AdrianEShop.Models;
using AdrianEShop.Models.ViewModels.Order;
using AdrianEShop.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
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
        public OrderDetailsVM OrderVM { get; set; }

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
            OrderVM = new OrderDetailsVM();
            OrderVM.OrderHeader = _orderHeaderService.GetOrder(id.ToString(), "ApplicationUser");
            //To refactor as service (orderDetailsService)
            OrderVM.OrderDetails = _unitOfWork.OrderDetails.GetAll(o => o.OrderId == id, includeProperties:"Product");
            return View(OrderVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult Details(string stripeToken)
        {
            var id = OrderVM.OrderHeader.Id.ToString();
            OrderHeader orderHeader = _orderHeaderService.GetOrder(id, includeProperties:"ApplicationUser");

            if(stripeToken != null)
            {
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal*100),
                    Currency = "usd",
                    Description = "Order ID : " + orderHeader.Id,
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);

                if (charge.Id == null)
                {
                    orderHeader.PaymentStatus = StaticDetails.PaymentStatusRejected;
                }
                else
                {
                    orderHeader.TransactionId = charge.Id;
                }
                if (charge.Status.ToLower() == "succeeded")
                {
                    orderHeader.PaymentStatus = StaticDetails.PaymentStatusApproved;
                    orderHeader.PaymentDate = DateTime.Now;
                }

                _orderHeaderService.Save();
            }
            return RedirectToAction("Details", "Order", new { id = orderHeader.Id });
        }

        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        public IActionResult StartProcessing(Guid id)
        {
            OrderHeader orderHeader = _orderHeaderService.GetOrder(id.ToString());
            orderHeader.OrderStatus = StaticDetails.StatusInProcess;
            _orderHeaderService.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        public IActionResult ShipOrder()
        {
            var id = OrderVM.OrderHeader.Id.ToString();
            OrderHeader orderHeader = _orderHeaderService.GetOrder(id);
            orderHeader.TrackingNumber = OrderVM.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderVM.OrderHeader.Carrier;
            orderHeader.ShippingDate = DateTime.Now;
            orderHeader.OrderStatus = StaticDetails.StatusShipped;

            _orderHeaderService.Save();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = StaticDetails.Role_Admin + "," + StaticDetails.Role_Employee)]
        public IActionResult CancelOrder(Guid id)
        {
            OrderHeader orderHeader = _orderHeaderService.GetOrder(id.ToString());
            if(orderHeader.PaymentStatus == StaticDetails.StatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
                    Reason = RefundReasons.RequestedByCustomer,
                    Charge = orderHeader.TransactionId
                };

                var service = new RefundService();
                Refund refund = service.Create(options);

                orderHeader.OrderStatus = StaticDetails.StatusRefunded;
                orderHeader.PaymentStatus = StaticDetails.StatusRefunded;
            }
            else
            {
                orderHeader.OrderStatus = StaticDetails.StatusCancelled;
                orderHeader.PaymentStatus = StaticDetails.StatusCancelled;
            }

            _orderHeaderService.Save();
            return RedirectToAction(nameof(Index));
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

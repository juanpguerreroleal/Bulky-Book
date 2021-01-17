using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Claims;
using System.Threading.Tasks;
using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using Bulky_Book.Models.ViewModels;
using Bulky_Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace Bulky_Book.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderDetailsViewModel OrderDetailsViewModel { get; set; }

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        #region API Calls
        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeaders;

            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
            }
            else
            {
                orderHeaders = _unitOfWork.OrderHeader.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "ApplicationUser");
            }
            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == SD.PaymentStatusDelayedPayment);
                    break;
                case "rejected":
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == SD.StatusCancelled || x.PaymentStatus == SD.StatusRefunded || x.PaymentStatus == SD.PaymentStatusRejected);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == SD.StatusApproved || x.PaymentStatus == SD.StatusInProcess || x.PaymentStatus == SD.StatusPending);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(x => x.PaymentStatus == SD.StatusShipped);
                    break;
                default:
                    break;
            }
            return Json(new { data = orderHeaders });
        }
        public IActionResult Details(int id)
        {
            OrderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id, includeProperties:"ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll(x => x.OrderId == id, includeProperties: "Product")
            };
            return View(OrderDetailsViewModel);
        }
        [Authorize(Roles = SD.Role_Admin+","+SD.Role_Employee)]
        public IActionResult StartProcessing(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id);
            orderHeader.OrderStatus = SD.StatusInProcess;
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult ShipOrder()
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == OrderDetailsViewModel.OrderHeader.Id);
            orderHeader.TrackingNumber = OrderDetailsViewModel.OrderHeader.TrackingNumber;
            orderHeader.Carrier = OrderDetailsViewModel.OrderHeader.Carrier;
            orderHeader.OrderStatus = SD.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
        public IActionResult CancelOrder(int id)
        {
            OrderHeader orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(x => x.Id == id);
            if (orderHeader.PaymentStatus == SD.StatusApproved)
            {
                var options = new RefundCreateOptions
                {
                    Amount = Convert.ToInt32(orderHeader.OrderTotal*100),
                    Reason = RefundReasons.RequestedByCustomer,
                    Charge = orderHeader.TransactionId
                };
                var service = new RefundService();
                Refund refund = service.Create(options);

                orderHeader.OrderStatus = SD.StatusRefunded;
                orderHeader.PaymentStatus = SD.StatusRefunded;
            }
            else
            {
                orderHeader.OrderStatus = SD.StatusCancelled;
                orderHeader.PaymentStatus = SD.StatusCancelled;
            }
            _unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
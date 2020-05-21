using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bulky_Book.Models.ViewModels;
using Bulky_Book.Models;
using System.Security.Claims;
using System.Linq;
using Bulky_Book.Utility;
using Microsoft.AspNetCore.Http;

namespace Bulky_Book.Areas.Customer.Controllers
{
    [Area(nameof(Customer))]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private DataAccess.Repository.IRepository.IUnitOfWork _unitOfWork { get; set; }
        public HomeController(ILogger<HomeController> logger, DataAccess.Repository.IRepository.IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var products = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count;
                //HttpContext.Session.SetObject(SD.SShoppingCart, count);
                HttpContext.Session.SetInt32(SD.SShoppingCart, count);
            }
            return View(products);
        }
        public IActionResult Details(int id)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, includeProperties: "Category,CoverType");
            ShoppingCart cart = new ShoppingCart()
            {
                Id = 0,
                Product = product,
                ProductId = product.Id
            };
            return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Microsoft.AspNetCore.Authorization.Authorize]
        public IActionResult Details([Bind("ApplicationUserId,ProductId,Count,Price")]ShoppingCart cartObject)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartObject.ApplicationUserId = claim.Value;
                ShoppingCart cart = _unitOfWork.ShoppingCart.GetFirstOrDefault( x => x.ApplicationUserId == cartObject.ApplicationUserId && x.ProductId == cartObject.ProductId, includeProperties: "Product");
                if (cart == null)
                {
                    _unitOfWork.ShoppingCart.Add(cartObject);
                }
                else
                {
                    cart.Count += cartObject.Count;
                }
                _unitOfWork.SaveChanges();
                var count = _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == cartObject.ApplicationUserId).ToList().Count;
                //HttpContext.Session.SetObject(SD.SShoppingCart, count);
                HttpContext.Session.SetInt32(SD.SShoppingCart, count);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == cartObject.ProductId, includeProperties: "Category,CoverType");
                ShoppingCart cart = new ShoppingCart()
                {
                    Product = product,
                    ProductId = product.Id
                };
                return View(cart);
            }
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

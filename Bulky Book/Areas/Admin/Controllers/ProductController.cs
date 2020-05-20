using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book.DataAccess;
using Bulky_Book.DataAccess.Repository;
using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using Bulky_Book.Models.ViewModels;
using Bulky_Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bulky_Book.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            IEnumerable<Product> products = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = products });
        }
        public IActionResult Upsert(int? id)
        {
            ProductViewModel model = new ProductViewModel();
            var categories = _unitOfWork.Category.GetAll();
            var coverTypes = _unitOfWork.CoverType.GetAll();
            model.Categories = categories.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            model.CoverTypes = coverTypes.Select(x => new SelectListItem(x.Name, x.Id.ToString()));
            if (id != null)
            {
                model.Product = _unitOfWork.Product.Get(id.Value);
                return View(model);
            }
            model.Product = new Product();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ProductViewModel model, List<Microsoft.AspNetCore.Http.IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                //Create location if not exists
                if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Products")))
                {
                    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/Products"));
                }
                //Save image
                if (files.Count > 0)
                {
                    var path = "img/Products/" + files.FirstOrDefault().FileName;
                    var basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + path);
                    if (!string.IsNullOrEmpty(model.Product.ImageUrl))
                    {
                        if (System.IO.File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + model.Product.ImageUrl)))
                        {
                            System.IO.File.Delete(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/" + model.Product.ImageUrl));
                        }
                    }
                    using (var stream = new FileStream(basePath, FileMode.Create))
                    {
                        await files.FirstOrDefault().CopyToAsync(stream);
                    }
                    model.Product.ImageUrl = path;
                }
                if (model.Product.Id != 0)
                {
                    _unitOfWork.Product.Update(model.Product);
                }
                else
                {
                    _unitOfWork.Product.Add(model.Product);
                }
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            bool success = false;
            var model = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
            if (model != null)
            {
                try
                {
                    _unitOfWork.Product.Remove(model.Id);
                    _unitOfWork.SaveChanges();
                    success = true;
                }
                catch(Exception e)
                {
                    success = false;
                }
            }
            return Json(new { result = success });
        }
    }
}
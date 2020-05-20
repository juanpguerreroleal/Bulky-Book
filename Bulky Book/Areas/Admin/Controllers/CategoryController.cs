using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book.DataAccess;
using Bulky_Book.DataAccess.Repository;
using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using Bulky_Book.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky_Book.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = SD.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
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
            IEnumerable<Category> categories = _unitOfWork.Category.GetAll();
            return Json(new { data = categories });
        }
        public IActionResult Upsert(int? id)
        {
            if (id != null)
            {
                var model = _unitOfWork.Category.Get(id.Value);
                return View(model);
            }
            return View(new Category());
        }
        [HttpPost]
        public IActionResult Upsert(Category model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    _unitOfWork.Category.Update(model);
                }
                else
                {
                    _unitOfWork.Category.Add(model);
                }
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            bool success = false;
            var model = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            if (model != null)
            {
                try
                {
                    _unitOfWork.Category.Remove(model.Id);
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
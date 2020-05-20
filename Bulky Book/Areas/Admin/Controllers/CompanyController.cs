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
    [Authorize(Roles = SD.Role_Admin + "," + SD.Role_Employee)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
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
            IEnumerable<Company> categories = _unitOfWork.Company.GetAll();
            return Json(new { data = categories });
        }
        public IActionResult Upsert(int? id)
        {
            if (id != null)
            {
                var model = _unitOfWork.Company.Get(id.Value);
                return View(model);
            }
            return View(new Company());
        }
        [HttpPost]
        public IActionResult Upsert(Company model)
        {
            if (ModelState.IsValid)
            {
                if (model.Id != 0)
                {
                    _unitOfWork.Company.Update(model);
                }
                else
                {
                    _unitOfWork.Company.Add(model);
                }
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            bool success = false;
            var model = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
            if (model != null)
            {
                try
                {
                    _unitOfWork.Company.Remove(model.Id);
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
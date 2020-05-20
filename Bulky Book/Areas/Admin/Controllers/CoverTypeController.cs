using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book.DataAccess;
using Bulky_Book.DataAccess.Repository;
using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using Bulky_Book.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bulky_Book.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    [Authorize(Roles = SD.Role_Admin)]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CoverTypeController(IUnitOfWork unitOfWork)
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
            IEnumerable<CoverType> covertypes = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll,null);
            return Json(new { data = covertypes });
        }
        public IActionResult Upsert(int? id)
        {
            if (id != null)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Id", id);
                var model = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
                return View(model);
            }
            return View(new CoverType());
        }
        [HttpPost]
        public IActionResult Upsert(CoverType model)
        {
            if (ModelState.IsValid)
            {
                var parameter = new DynamicParameters();
                parameter.Add("@Name", model.Name);
                if (model.Id != 0)
                {

                    parameter.Add("@Id", model.Id);
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Update, parameter);
                }
                else
                {
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Create, parameter);
                }
                _unitOfWork.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var parameter = new DynamicParameters();
            parameter.Add("@id", id);
            bool success = false;
            var model = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, parameter);
            if (model != null)
            {
                try
                {
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete,parameter);
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
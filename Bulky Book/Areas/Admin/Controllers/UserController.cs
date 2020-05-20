using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bulky_Book.DataAccess;
using Bulky_Book.DataAccess.Repository;
using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bulky_Book.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _db.ApplicationUsers.Include(x => x.Company).ToList();
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            foreach (var user in users)
            {
                var roleId = userRole.FirstOrDefault(x => x.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(x => x.Id == roleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            return Json(new { data = users });
        }
        [HttpPost]
        public IActionResult LockUnlock([FromBody]string id)
        {
            var user = _db.ApplicationUsers.Where(x => x.Id == id).FirstOrDefault();
            if (user == null)
            {
                return Json(new { success = false, message = "Error while unlocking/locking" });
            }
            else if (user.LockoutEnd != null && user.LockoutEnd > DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now;
            }
            else
            {
                user.LockoutEnd = DateTime.Now.AddDays(1000);
            }
            _db.SaveChanges();
            return Json(new { success = true, message = "Operation successful" });
        }
    }
}
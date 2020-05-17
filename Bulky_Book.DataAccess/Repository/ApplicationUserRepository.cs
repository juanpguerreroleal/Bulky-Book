using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bulky_Book.DataAccess.Repository
{
    public class ApplicationUserRepository: Repository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
    }
}

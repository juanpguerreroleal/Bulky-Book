using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bulky_Book.DataAccess.Repository
{
    public class CompanyRepository: Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        public CompanyRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var objFromDb = _db.Companies.Where(x => x.Id == company.Id).FirstOrDefault();
            if (objFromDb != null)
            {
                objFromDb.Name = company.Name;
                objFromDb.IsAuthorizedCompany = company.IsAuthorizedCompany;
                objFromDb.PhoneNumber = company.PhoneNumber;
                objFromDb.PostalCode = company.PostalCode;
                objFromDb.State = company.State;
                objFromDb.StreetAddress = company.StreetAddress;
                objFromDb.City = company.City;
                _db.SaveChanges();
            }
        }
    }
}

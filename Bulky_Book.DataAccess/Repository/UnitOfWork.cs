using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bulky_Book.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            SP_Call = new SP_Call(_db);
            CoverType = new CoverTypeRepository(_db);
            Product = new ProductRepository(_db);
            Company = new CompanyRepository(_db);
        }
        public ICategoryRepository Category { get; private set; }
        public ISP_Call SP_Call { get; private set; }
        public ICoverTypeRepository CoverType { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICompanyRepository Company { get; set; }
        public void Dispose()
        {
            _db.Dispose();
        }
        public void SaveChanges()
        {
            _db.SaveChanges();
        }
    }
}

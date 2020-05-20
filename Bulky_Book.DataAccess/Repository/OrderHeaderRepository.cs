using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bulky_Book.DataAccess.Repository
{
    public class OrderHeaderRepository: Repository<OrderHeader>, IOrderHeaderRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader orderHeader)
        {
            var objFromDb = _db.OrderHeaders.Where(x => x.Id == orderHeader.Id).FirstOrDefault();
            if (objFromDb != null)
            {
                _db.Update(objFromDb);
                _db.SaveChanges();
            }
        }
    }
}

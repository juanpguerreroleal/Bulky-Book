using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bulky_Book.DataAccess.Repository
{
    public class OrderDetailsRepository: Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;
        public OrderDetailsRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(OrderDetails orderDetails)
        {
            var objFromDb = _db.OrderDetails.Where(x => x.Id == orderDetails.Id).FirstOrDefault();
            if (objFromDb != null)
            {
                _db.Update(objFromDb);
                _db.SaveChanges();
            }
        }
    }
}

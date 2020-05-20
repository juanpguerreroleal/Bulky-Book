using Bulky_Book.DataAccess.Repository.IRepository;
using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bulky_Book.DataAccess.Repository
{
    public class ShoppingCartRepository: Repository<ShoppingCart>, IShoppingCartRepository
    {
        private readonly ApplicationDbContext _db;
        public ShoppingCartRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }

        public void Update(ShoppingCart shoppingCart)
        {
            var objFromDb = _db.ShoppingCarts.Where(x => x.Id == shoppingCart.Id).FirstOrDefault();
            if (objFromDb != null)
            {
                _db.Update(objFromDb);
                _db.SaveChanges();
            }
        }
    }
}

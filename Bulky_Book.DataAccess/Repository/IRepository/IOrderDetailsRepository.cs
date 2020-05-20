using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bulky_Book.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository: IRepository<OrderDetails>
    {
        void Update(OrderDetails entity);
    }
}

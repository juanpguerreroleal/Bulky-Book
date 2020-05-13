﻿using Bulky_Book.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bulky_Book.DataAccess.Repository.IRepository
{
    public interface IProductRepository: IRepository<Product>
    {
        void Update(Product entity);
    }
}

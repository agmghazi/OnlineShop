﻿using DataModels.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModels.Dao
{
    public class ProductCategoryDao
    {
        OnlineShopingDbContext db = null;
        public ProductCategoryDao()
        {
            db = new OnlineShopingDbContext();
        }

        public List<ProductCategory> ListAll()
        {
            return db.ProductCategories.Where(x => x.Status == true).OrderBy(x => x.DisplayOrder).ToList();
        }

        public ProductCategory ViewDetails(long id)
        {
            return db.ProductCategories.Find(id);
        }
    }
}

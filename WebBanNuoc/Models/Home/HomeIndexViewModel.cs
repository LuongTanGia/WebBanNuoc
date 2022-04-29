using PagedList;
using PagedList.Mvc;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using WebBanNuoc.DAL;
using WebBanNuoc.Repository;

namespace WebBanNuoc.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _uniOfWork = new GenericUnitOfWork();
        DrinksStoreEntities1 context = new DrinksStoreEntities1();
        public IPagedList<Tbl_Product> ListOfProduct { get; set; }
        public HomeIndexViewModel CreateModel(string search, int pageSize, int? page )
        {
            SqlParameter[] pram = new SqlParameter[] {
                   new SqlParameter("@search",search??(object)DBNull.Value)
                   };
            IPagedList<Tbl_Product> data = context.Database.SqlQuery<Tbl_Product>("GetBySearch @search", pram).ToList().ToPagedList(page ?? 1, 4);
            return new HomeIndexViewModel
            {
                ListOfProduct = data
        };
    }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebBanNuoc.DAL;
using WebBanNuoc.Models;

namespace WebBanNuoc.Repository
{
   
    public class GenericUnitOfWork : IDisposable
    {
        public DrinksStoreEntities1 data = new DrinksStoreEntities1();
        public IRepository<Tbl_EntityType> GetRepositoryInstance<Tbl_EntityType>() where Tbl_EntityType : class
        {
            return new GenericRepository<Tbl_EntityType>(data);
        }

        public void SaveChanges()
        {
            data.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    data.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

    }
}
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientPortal.Models
{
   
    public class FormRepository : IFormRepository, IDisposable
    {
        private List<Form> forms = new List<Form>();
        private ClientPortalContext db = new ClientPortalContext();
        //private int _nextId = 1;

        public FormRepository()
        {

        }

        public IEnumerable<Form> GetAll()
        {

            return db.Set<Form>().ToList();
        }

        public Form Get(Guid id)
        {
            return db.Set<Form>().Find(id);
           // return forms.Find(p => p.Id == id);
        }

        public Form Add(Form item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
          //  item.Id = Guid.NewGuid();
          //  forms.Add(item);
            db.Set<Form>().Add(item);
            db.SaveChanges();
            return item;
        }

        public void Remove(Guid id)
        {
            Form form = db.Set<Form>().Find(id);
            db.Set<Form>().Remove(form);
            db.SaveChanges();
           // forms.RemoveAll(p => p.Id == id);
        }

        public bool Update(Form item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }
            int index = forms.FindIndex(p => p.CrmFormId == item.CrmFormId);
            if (index == -1)
            {
                return false;
            }
            forms.RemoveAt(index);
            forms.Add(item);
            return true;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
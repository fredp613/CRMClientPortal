using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientPortal.Models
{
    interface IFormRepository
    {
        IEnumerable<Form> GetAll();
        Form Get(Guid id);
        Form Add(Form item);
        void Remove(Guid id);
        bool Update(Form item);
        void Dispose();
    }
}

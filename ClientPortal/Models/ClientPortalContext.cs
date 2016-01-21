using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClientPortal.Models
{
    public class ClientPortalContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public ClientPortalContext() : base("name=DefaultConnection")
        {
        }

        public System.Data.Entity.DbSet<ClientPortal.Models.Form> Forms { get; set; }
        public System.Data.Entity.DbSet<ClientPortal.Models.FormField> FormFields { get; set; }
        public System.Data.Entity.DbSet<ClientPortal.Models.Address> Addresses { get; set; }
        public System.Data.Entity.DbSet<ClientPortal.Models.Organization> Organizations { get; set; }
    }
}

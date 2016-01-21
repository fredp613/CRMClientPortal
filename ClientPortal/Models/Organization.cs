using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientPortal.Models
{
    public enum OrganizationType
    {
        Government = 1,
        NGO = 2,
        ForProfit = 3
    }
    public class Organization
    {
        public Organization()
        {
            this.addresses = new HashSet<Address>();
        }
       
        [Key]
        public int Id { get; set; }
        public Guid CrmRecordId { get; set; }
        public string Name { get; set; }
        public Guid OrganizationType { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public virtual ICollection<Address> addresses { get; set; }
    }
}
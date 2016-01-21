using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientPortal.Models
{
    public enum AddressType
    {
        Mailing = 1,
        Main = 2,
        Payment = 3
    }
    public class Address
    {
        [Key]
        public int id { get; set;}
        public Guid CrmRecordId { get; set; }
        public string AddressType { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string AddressLine3 { get; set; }
        public string AddressLine4 { get; set; }
        public string City { get; set; }
        public string SubDivision { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public virtual ICollection<Organization> organizations { get; set; }
      

    }
}
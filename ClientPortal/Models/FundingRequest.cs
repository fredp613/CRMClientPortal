using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientPortal.Models
{
    public class FundingRequest
    {
        public int Id { get; set; }
        public Guid CrmRecordId { get; set;}
        public Guid CrmProgramId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid CreatedBy { get; set; }
        public string Status { get; set; }
        public virtual Organization organization { get; set; }

    }
}
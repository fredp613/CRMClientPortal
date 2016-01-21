using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ClientPortal.Models
{
    public enum formState
    {
        PendingActivation = 1,
        Activated = 2,
        Expired = 3,
        Removed = 4
    }
    public class Form
    {   
        public Form() {
            this.formFields = new HashSet<FormField>();
        }
        [Key]
        public int Id { get; set; }
        public Guid CrmFormId { get; set;}
        public string CrmFormName { get; set; }
        public int State
        {
            get; set;  
        }
        public DateTime? ActivationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public virtual ICollection<FormField> formFields { get; set; }
    }

    public class FormField
    {
        [Key]
        public int Id { get; set; }
        public string CrmfieldId { get; set; }
        public string CrmfieldName { get; set; }
        public int CrmFieldfieldType { get; set; }
        public int formId { get; set; }
        public virtual Form form { get; set; }
    }
   
}
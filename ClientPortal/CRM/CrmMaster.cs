using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata.Query;
using Microsoft.Xrm.Client;
using System.ServiceModel;
using System.Text;
using System.ServiceModel.Description;
using System.DirectoryServices.AccountManagement;
using Microsoft.Xrm.Sdk.Query;
using ClientPortal.ErrorMessages;
using System.Reflection;
using System.ComponentModel;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Metadata;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using ClientPortal.Models;

namespace ClientPortal.CRM
{
    public static class EnumHelper
    {

        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();

            MemberInfo[] memInfo = type.GetMember(en.ToString());

            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return en.ToString();
        }

    }
    public struct Contact
    {
        public string FirstName;
        public string LastName;
        public string Address1_Line1;
        public string Address1_City;
        public string Address1_StateOrProvince;
        public string Address1_PostalCode;
        public string Telephone1;
        public string EmailAddress1;
        public Guid Id;
    }

    public struct Account
    {
        public string Name;
        public string Address1_City;
        public Guid Id;
    }

    public struct ServerCredentials
    {

        public string UserName
        {
            get
            {
                return "Administrator";
            }          
        }
        public string Password
        {
            get
            {
                return "Fredp614$";
            }
        }       
    }

    public class CrmConnector
    {

        private Guid _accountId;
        private Guid _contactId;
        private OrganizationServiceProxy _serviceProxy;
        private OrganizationServiceContext _orgContext;
        private IOrganizationService _service;

        public CrmConnector()
        {
            try
            {
                var creds = new ClientCredentials();
                var serverCreds = new ServerCredentials();
                creds.UserName.UserName = serverCreds.UserName;
                creds.UserName.Password = serverCreds.Password;

                // Connect to the Organization service. 
                // The using statement assures that the service proxy will be properly disposed.
                var ouri = new Uri("http://myclerical/GnC/XRMServices/2011/Organization.svc");
                using (_serviceProxy = new OrganizationServiceProxy(ouri, null,
                                                                     creds, null))
                {
                    // This statement is required to enable early-bound type support.
                    _serviceProxy.EnableProxyTypes();
                    _service = (IOrganizationService)_serviceProxy;
                    _orgContext = new OrganizationServiceContext(_service);
                }
            }

            // Catch any service fault exceptions that Microsoft Dynamics CRM throws.
            catch (FaultException<Microsoft.Xrm.Sdk.OrganizationServiceFault>)
            {
                // You can handle an exception here or pass it back to the calling method.
                throw;
            }

        }

        public bool CreateContactFromRegistration(Contact contact)
        { 
            //contact does not exist - create in CRM and in Portal
            //contact exists in both CRM and in the Portal - return contact already exists
            try
            {
                //before creating check if the contact already exists in CRM using the email as a key
                //if the contact does exist, return false otherwise, continue to create contact
                var contactObj = new Entity("contact");
                contactObj["firstname"] = contact.FirstName;
                contactObj["lastname"] = contact.LastName;
                contactObj["address1_city"] = contact.Address1_City;
                contactObj["emailaddress1"] = contact.EmailAddress1;
                _service.Create(contactObj);
                return true; 
            } catch
            {
                return false;
            }
            
        }

        public void CreateAccountAndContactFromFirstLogin(Contact contact, Account account)
        {

            var contactObj = new Entity("contact");
            contactObj["firstname"] = contact.FirstName;
            contactObj["lastname"] = contact.LastName;
            contactObj["address1_city"] = contact.Address1_City;
            //_contactId = _service.Create(contactObj);
            contactObj.Id = Guid.NewGuid();
            contactObj.Id = _contactId;
            _orgContext.AddObject(contactObj);

            var accountObj = new Entity("account");
            accountObj["name"] = account.Name;
            accountObj["address1_city"] = account.Address1_City;
            accountObj.Id = account.Id;

            _orgContext.AddRelatedObject(
                contactObj,
                new Relationship("account_primary_contact"),
                accountObj);
            SaveChangesHelper(contactObj, accountObj);
        }

        private void SaveChangesHelper(params Entity[] entities)
        {
            SaveChangesResultCollection results = _orgContext.SaveChanges();
            if (results.HasError)
            {
                //  DeleteRequiredRecords(false);
                StringBuilder sb = new StringBuilder();
                foreach (var result in results)
                {
                    if (result.Error != null)
                    {
                        sb.AppendFormat("Error: {0}\n", result.Error.Message);
                    }
                }
                throw new InvalidOperationException("Failure occurred while " +
                    "calling OrganizationServiceContext.SaveChanges()\n" +
                    sb.ToString());
            }
            else
            {
                foreach (Entity e in entities)
                {
                    e.EntityState = null;
                }
            }
        }

        public List<FormField> getFormFields(string formName)
        {
            RetrieveEntityRequest retrieveFormEntityRequest = new RetrieveEntityRequest
            {
                EntityFilters = EntityFilters.Attributes,
                LogicalName = formName
            };
           
            RetrieveEntityResponse retrieveFormEntityResponse = (RetrieveEntityResponse)_service.Execute(retrieveFormEntityRequest);

            // string entityname = retrieveBankAccountEntityResponse.EntityMetadata.ManyToManyRelationships[0].IntersectEntityName;
            //var test1 = new List<string>();
            List<string> test1 = new List<string>();
            var test = retrieveFormEntityResponse.EntityMetadata.Attributes.ToArray();
            test.ToList().ForEach(i => test1.Add(i.LogicalName.ToString()));
            QueryExpression qe = new QueryExpression("systemform");
            qe.Criteria.AddCondition("type", ConditionOperator.Equal, 2); //main form
            qe.Criteria.AddCondition("objecttypecode", ConditionOperator.Equal, "contact");
            qe.ColumnSet.AddColumn("formxml");

          
          //  var test2 = _service.RetrieveMultiple(qe).Entities.ToArray();
          //  test2.ToList().ForEach(y => test3.Add(y.Attributes["formxml"].ToString()));

            string frmXml = _service.RetrieveMultiple(qe).Entities.First().Attributes["formxml"].ToString();

          //  StringBuilder output = new StringBuilder();

            XElement root = XElement.Parse(frmXml);

            List<FormField> controls = new List<FormField>();
            var query = root.Descendants("tabs")    
               .Concat(root.Descendants("control"))
               .Select(x => new {
                   control = (string)x.Attribute("datafieldname"),
                   id = (string)x.Attribute("id")         
               });
            Debug.WriteLine(query.Count());
            // test4.Add((string)el.Attribute("tab"));
            Form form = new Form
            {
                CrmFormName = formName,
            };

          
            foreach (var c in query)
            {
                FormField ff = new FormField
                {         
                    CrmfieldId = c.id,
                    CrmfieldName = c.control              
                   // form = form                    
                };
                controls.Add(ff);               
            }

            // OutputTextBlock.Text = output.ToString();

            return controls;
           // return frmXml;
        }

        /// <summary>
        /// Creates any entity records that this sample requires.
        /// </summary>
        //public void CreateRequiredRecords()
        //{
        //    // Create a second user that we will reference in our sample code.
        //    Guid userId = SystemUserProvider.RetrieveSalesManager(_serviceProxy);

        //    // Modify email address of user for sample.
        //    SystemUser systemUser = new SystemUser
        //    {
        //        Id = userId,
        //        InternalEMailAddress = "someone@example.com"
        //    };

        //    _service.Update(systemUser);
        //}

        /// <summary>
        /// Deletes any entity records that were created for this sample.
        /// <param name="prompt">Indicates whether to prompt the user 
        /// to delete the records created in this sample.</param>
        /// </summary>
        //public void DeleteRequiredRecords(bool prompt)
        //{
        //    // The system user named "Kevin Cook" that was created by this sample will
        //    // continue to exist on your system because system users cannot be deleted
        //    // in Microsoft Dynamics CRM.  They can only be enabled or disabled.

        //    bool deleteRecords = true;

        //    if (prompt)
        //    {
        //        Console.WriteLine("\nDo you want these entity records deleted? (y/n) [y]: ");
        //        String answer = Console.ReadLine();

        //        deleteRecords = (answer.StartsWith("y") || answer.StartsWith("Y") || answer == String.Empty);
        //    }

        //    if (deleteRecords)
        //    {
        //        _service.Delete(Incident.EntityLogicalName, _incidentId);
        //        _service.Delete(Opportunity.EntityLogicalName, _opportunityId);
        //        _service.Delete(QuoteDetail.EntityLogicalName, _quoteDetailId);
        //        _service.Delete(ProductPriceLevel.EntityLogicalName, _productPriceLevelId);
        //        _service.Delete(Product.EntityLogicalName, _productId);
        //        _service.Delete(Quote.EntityLogicalName, _quoteId);
        //        _service.Delete(PriceLevel.EntityLogicalName, _priceLevelId);
        //        _service.Delete(Annotation.EntityLogicalName, _annotationId);
        //        _service.Delete(Contact.EntityLogicalName, _contactId);
        //        _service.Delete(Account.EntityLogicalName, _accountId);
        //        Console.WriteLine("Entity record(s) have been deleted.");
        //    }
        //}


    }
}
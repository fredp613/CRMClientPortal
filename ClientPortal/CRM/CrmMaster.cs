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
    public struct WebInitiative
    {
        public Guid id;
        public string name;
        public string webdescription;
        public DateTime? StartDate;
        public DateTime? EndDate;
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

                // Connect to the CRM web service using a connection string.
                 //CrmServiceClient conn = new Xrm.Tooling.Connector.CrmServiceClient(connectionString);

                // Cast the proxy client to the IOrganizationService interface.
                //var _orgService = (IOrganizationService)conn.OrganizationWebProxyClient != null ? (IOrganizationService)conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

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
            //RetrieveEntityRequest retrieveFormEntityRequest = new RetrieveEntityRequest
            //{
            //    EntityFilters = EntityFilters.Attributes,
            //    LogicalName = formName
            //};
           
            //RetrieveEntityResponse retrieveFormEntityResponse = (RetrieveEntityResponse)_service.Execute(retrieveFormEntityRequest);

            // string entityname = retrieveBankAccountEntityResponse.EntityMetadata.ManyToManyRelationships[0].IntersectEntityName;
            //var test1 = new List<string>();
            List<string> test1 = new List<string>();
            //var test = retrieveFormEntityResponse.EntityMetadata.Attributes.ToArray();
            //test.ToList().ForEach(i => test1.Add(i.LogicalName.ToString()));
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

        public List<Entity> accounts()
        {
            var test = new List<string>();
            QueryExpression qe = new QueryExpression("account");
            qe.Criteria.AddCondition("statuscode", ConditionOperator.Equal, 1); //main form
            qe.ColumnSet.AddColumn("name");
            var result = _service.RetrieveMultiple(qe).Entities.ToList();
            return result;
        }

        public  List<WebInitiative> webinitiatives()
        {
            var wis = new List<WebInitiative>();
            QueryExpression qe = new QueryExpression("fp_webinitiative");
            qe.Criteria.AddCondition("statuscode", ConditionOperator.Equal, 1);
            qe.ColumnSet.AddColumns("fp_name", "fp_startdate", "fp_enddate", "fp_webdescription");
            var result = _service.RetrieveMultiple(qe).Entities.ToList();
            foreach(var r in result)
            {
                WebInitiative wi = new WebInitiative
                {
                    id = r.Id,
                    name = r.Attributes["fp_name"].ToString(),
                    webdescription = r.Attributes["fp_webdescription"].ToString(),
                    StartDate = (DateTime)r.Attributes["fp_startdate"],
                    EndDate = (DateTime)r.Attributes["fp_enddate"]
                };
                wis.Add(wi);
            }
            return wis;

        }

    




    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using ClientPortal.Models;
using System.Web.Mvc;
using ClientPortal.CRM;
using System.Collections;
using Microsoft.Xrm.Sdk;


namespace ClientPortal.Controllers
{
    public class HomeController : Controller
    {
        private CrmConnector _crmContext;
        public HomeController()
        {
            _crmContext = new CrmConnector();
        }
       // [Authorize]
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            var test = _crmContext.getFormFields("contact");
            ViewBag.data = test;
            return View();
        }
    }
    
    public class HomeAPIController : ApiController
    {
        private CrmConnector _crmContext;
        public HomeAPIController()
        {
            _crmContext = new CrmConnector();
        }
        [System.Web.Http.Route("api/webinitiatives")]
        [System.Web.Http.HttpGet]
        public /**IEnumerable<WebInitiative>**/ dynamic webinitiatives()
        {
            var test = _crmContext.getFormFields("contact");
            var test1 = _crmContext.accounts();
            var wis = _crmContext.webinitiatives();
           // return wis;
            return new { webinitiatives = wis };

        }
    }
}

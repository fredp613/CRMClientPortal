using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClientPortal.CRM;

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
}

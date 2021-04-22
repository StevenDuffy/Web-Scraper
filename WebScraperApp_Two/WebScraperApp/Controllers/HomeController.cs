using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebScraperApp.Business_Logic_Layer;
using WebScraperApp.Models;

namespace WebScraperApp.Controllers
{

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpGet]
        public ActionResult SearchResults(string searchTerm)
        {
            // validate input
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }

            _IndexSearchResultsPartialViewModel model = new _IndexSearchResultsPartialViewModel();
           
           WebScraper webScraper = new WebScraper(); 

            model.SearchResults = webScraper.GetSearchResults(searchTerm);

            return PartialView("_IndexSearchResultsPartial", model);

        }

    }
}
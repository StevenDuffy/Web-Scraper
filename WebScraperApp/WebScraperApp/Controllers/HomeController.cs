using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using WebScraperApp.Models;

namespace WebScraper.App.Controllers
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


            // Search engines are saved in Web.config with format of:
            // searchEngineName;url;CSSSelector,searchEngineName;url;CSSSelector
            List<string> searchEngines = ConfigurationManager.AppSettings["SearchEngines"].Split(',').ToList();

            using (ChromeDriver driver = new ChromeDriver())
            {
                foreach (string searchEngine in searchEngines)
                {
                    List<string> searchParameters = searchEngine.Split(';').ToList();


                    string searchEngineName = searchParameters[0];
                    string url = searchParameters[1];
                    string cssSelector = searchParameters[2];

                    url += HttpUtility.UrlEncode(searchTerm.Trim());

                    var elements = GetSearchResultElements(searchEngineName, driver, url, By.CssSelector(cssSelector));

                    foreach (IWebElement element in elements)
                    {
                        if (!String.IsNullOrEmpty(element.Text))
                        {
                            //string resultURL = element.FindElement(By.CssSelector("a.result__a")).GetAttribute("href").ToString().Trim();
                            model.SearchResults.Add(new SearchResultModel { SearchEngine = searchEngineName, ResultText = element.Text }); //, ResultURL = resultURL
                        }
                    }
                }
            };

            return PartialView("_IndexSearchResultsPartial", model);

        }

        private List<IWebElement> GetSearchResultElements(string searchEngineName, ChromeDriver driver, string url, By by)
        {
            List<IWebElement> elements = new List<IWebElement>();

            driver.Navigate().GoToUrl(url);

            PerformEngineSpecificActions(searchEngineName, driver);

            Stopwatch stopwatch = new Stopwatch();

            stopwatch.Start();

            while (stopwatch.ElapsedMilliseconds < (20 * 1000))
            {
                elements = driver.FindElements(by).ToList();

                if (elements.Count > 0)
                {
                    return elements;
                }

            }

            return elements;

        }

        private void PerformEngineSpecificActions(string searchEngineName, ChromeDriver driver)
        {
            switch (searchEngineName)
            {
                case "Google":
                    driver.FindElementById("zV9nZe").Click();
                    break;
                case "DuckDuckGo":
                    break;
            }
        }
    }
}
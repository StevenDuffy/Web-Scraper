using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Web;
using WebScraperApp.Models;

namespace WebScraperApp.Business_Logic_Layer
{
    public class WebScraper
    {
        private ChromeDriver driver;

        private readonly List<SearchEngine> searchEngines;

        public WebScraper()
        {
            searchEngines = new List<SearchEngine>();

            // Search engines are currently saved in Web.config with format of => searchEngineName;url;SelectorText;SelectorType[CSS || XPath ],
            List<string> searchEngineStrings = ConfigurationManager.AppSettings["SearchEngines"].Split(',').ToList();

            foreach (string searchEngineString in searchEngineStrings)
            {
                List<string> searchEngineParameters = searchEngineString.Split(';').ToList();

                SearchEngine searchEngine = new SearchEngine(
                    searchEngineParameters[0],
                    searchEngineParameters[1],
                    searchEngineParameters[2],
                    searchEngineParameters[3]
                    );

                searchEngines.Add(searchEngine);
            }
        }



        public List<SearchResultModel> GetSearchResults(string searchTerm)
        {
            List<SearchResultModel> searchResults = new List<SearchResultModel>();

            //ChromeOptions chromeOptions = new ChromeOptions();   -- pass this option to the driver constructor if you want to hide browsers opening

            //chromeOptions.AddArgument("--headless");

            using (driver = new ChromeDriver())
            {
                foreach (SearchEngine searchEngine in searchEngines)
                {
                    string completeURL = searchEngine.BaseURL + HttpUtility.UrlEncode(searchTerm.Trim());

                    var elements = GetSearchResultElements(searchEngine.Name, driver, completeURL, searchEngine.Selector);

                    foreach (IWebElement element in elements)
                    {
                        if (!String.IsNullOrEmpty(element.Text))
                        {
                            //string resultURL = element.FindElement(By.CssSelector("a.result__a")).GetAttribute("href").ToString().Trim();  - to be implemented
                            searchResults.Add(new SearchResultModel { SearchEngine = searchEngine.Name, ResultText = element.Text }); //, ResultURL = resultURL
                        }
                    }
                }
            };

            return searchResults;

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

        // Custom actions to be performed per search engine before results can be found (e.g close a pop up)
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
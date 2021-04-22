using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebScraperApp.Business_Logic_Layer
{
    public class SearchEngine
    {
        private readonly string name;
        private readonly string baseURL;
        private readonly By selector;

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public string BaseURL
        {
            get
            {
                return this.baseURL;
            }
        }

        public By Selector
        {
            get
            {
                return this.selector;
            }
        }
                
        public SearchEngine(string name, string baseURL, string selectorString, string selectorType)
        {
            this.name = name;
            this.baseURL = baseURL;

            // set selector type
            switch (selectorType)
            {
                case "CSS":
                    this.selector = By.CssSelector(selectorString);
                    break;
                case "XPath":
                    this.selector = By.XPath(selectorString);
                    break;
            }
        }

    }
}
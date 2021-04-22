using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebScraperApp.Models;

namespace WebScraperApp.Models
{
    public class _IndexSearchResultsPartialViewModel
    {
        public List<SearchResultModel> SearchResults { get; set; }

        public _IndexSearchResultsPartialViewModel()
        {
            SearchResults = new List<SearchResultModel>();
        }
    }
}
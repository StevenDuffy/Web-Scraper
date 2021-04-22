using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebScraperApp.Models
{
    public class IndexSearchViewModel
    {
        [Display(Name = "Search Term")]
        [Required]
        public string SearchTerm { get; set; }
    }
}
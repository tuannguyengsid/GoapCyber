using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class NewsCategoryViewModel
    {
        public List<News> LargeNews { get; set; }
        public List<NewsCategory> NewsCategories { get; set; }
        public NewsCategory NewsCategory { get; set; }
        public List<NewsCategory> FlagNewsCategories { get; set; }
        public News Post { get; set; }
        public List<News> Posts { get; set; }
        public List<News> LatestNews { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageVisit { get; set; }
        public int PageSize { get; set; }
        public int CountTotal { get; set; }
    }

    public class NewsViewModel
    {
        public News News { get; set; }
        public List<NewsCategory> NewsCategories { get; set; }
        public NewsCategory NewsCategory { get; set; }
        public string CategoryId { get; set; }
        public List<News> LatestNews { get; set; }
        public List<News> RelatedNewses { get; set; }

    }
}
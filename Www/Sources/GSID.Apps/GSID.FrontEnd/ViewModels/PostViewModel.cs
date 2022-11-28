using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.FrontEnd.ViewModels
{
    public class PostViewModel
    {
        public PostViewModel()
        {
            PageSize = 12;
            PageVisit = 8; 
            PageIndex = 1;
        }
        public List<News> Posts { get; set; }
        public List<News> Related { get; set; }
        public List<News> Latest { get; set; }
        public News Post { get; set; }
        public TagPost TagPost { get; set; }
        public List<TagPost> TagPosts { get; set; }

        public int PageSize { get; set; }
        public int PageVisit { get; set; }
        public int PageIndex { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int CountTotal { get; set; }
    }
}
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static GSID.FrontEnd.ViewModels.SearchAutocompleteViewModel;

namespace GSID.FrontEnd.ViewModels
{
    public class SearchViewModel
    {
        public List<ProductCategory> ProductCategories { get; set; }
        public string ProductCategory { get; set; }
        public string Keyword { get; set; }
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Url { get; set; }
        public SearchIsType Type { get; set; }
    }

    public class SearchAutocompleteViewModel
    {
        public enum SearchIsType : int
        {
            None = -1,
            Product = 1,// Trực tiếp
            Post = 2// Gián tiếp
        }
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public SearchIsType Type { get; set; }
        public string ProductCode { get; set; }
        public string ImageSrc { get; set; }
        public DateTime DateBy { get; set; }

    }
    
    public class SearchResultViewModel
    {
        public SearchResultViewModel()
        {

        }
        public string Keyword { get; set; }
        public string ProductCategoryName { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public List<Product> Products { get; set; }
        public double TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public int PageVisit { get; set; }
        public int PageSize { get; set; }
        public int CountTotal { get; set; }
    }

    public class SearchBoxViewModel
    {
        public string Keyword { get; set; }
    }

    public class SearchBoxAdvanceViewModel
    {
        public string Keyword { get; set; }
        public List<News> Posts { get; set; }
        public List<Recruitment> Recruitments { get; set; }
    }
}
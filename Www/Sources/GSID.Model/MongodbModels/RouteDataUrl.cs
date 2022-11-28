using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;
using System.ComponentModel.DataAnnotations;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public class RouteDataUrl : GSIDMongoEntity
    {
        public RouteDataUrl()
            : base()
        {
            this.Url = "";
            this.Controller = "";
            this.Action = "";
            this.Area = "";
            this.EditablePageId = "";
            this.Title = "";
        }

        public enum RouteDataIsLanguage : int
        {
            None = -1,
            Vn = 1,
            En = 2
        }

        public enum RouteDataIsType : int
        {
            None = -1,
            HomePage = 1,
            AboutUsPage = 2,
            Recruitment = 3,
            Gallery = 4,
            Post = 5,
            ContactUs = 6,
            CategoryDetail = 7,
            ProductDetail = 8,
            PostDetail = 9,
            PostTag = 10,
            PostCategory = 13,
            RecruitmentCareer = 11,
            RecruitmentDetail = 12,
            ProjectCategory = 14,
            ProjectDetail = 15
        }
        public enum RouteDataIsPolicy : int
        {
            None = -1,
            Edit = 1,
            NoEdit = 2,
        }

        [RegularExpression(@"^[a-z0-9-]+$")]
        [Display(Name = "SEO friendly url: only lowercase, number and dash (-) character allowed")]
        public string Url { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Area { get; set; }
        public string EditablePageId { get; set; }
        public bool Success { get; set; }
        public bool IsUrlPrefix { get; set; }
        public bool IsUrlRequired { get; set; }//Check pass url Null. If IsUrlRequired == false,=> url == null 
        public string RequestedUrl { get; set; }
        public string Title { get; set; }
        public string Keyword { get; set; }
        public string Content { get; set; }
        public string Description { get; set; }
        public string BodyCss { get; internal set; }
        public string H1 { get; set; }
        public string H2 { get; set; }
        public string H3 { get; set; }
        public string H4 { get; set; }
        public string H5 { get; set; }
        public string H6 { get; set; }
        public string HtmlMetaRaw { get; set; }
        public RouteDataIsLanguage IsLanguage { get; set; }
        public RouteDataIsType IsType { get; set; }
        public RouteDataIsPolicy IsPolicy { get; set; }

        //OpenGraphMetaData
        public string OgTitle { get; set; }//og:title
        public string OgSite_name { get; set; }//og:site_name
        public string OgDescription { get; set; }//og:description
        public string OgType { get; set; }//og:type website/article/product/video:other
        public string OgImageSrc { get; set; }//og:image
    }
}

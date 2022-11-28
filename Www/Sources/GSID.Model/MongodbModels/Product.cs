using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Product : GSIDMongoEntity
    {
        public enum IsOverviewViewType : int
        {
            None = -1,
            Editor = 1,
            Block = 2
        }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SlugVn { get; set; }
        public string SlugEn { get; set; }

        public string RouteDataUrlVnId { get; set; }
        public string RouteDataUrlEnId { get; set; }
   
        public string ShortDescriptionVn { get; set; }
        public string ShortDescriptionEn { get; set; }
        public string DescriptionVn { get; set; }
        public string DescriptionEn { get; set; }
        public string OverviewVn { get; set; }
        public IsOverviewViewType OverviewViewTypeVn { get; set; }
        public string OverviewEn { get; set; }
        public IsOverviewViewType OverviewViewTypeEn { get; set; }
        public string ProductCategoryId { get; set; }
        public string ImageSrc { get; set; }
        public string ImageEnSrc { get; set; }

        public List<ProductImage> Images { get; set; }
        public List<ProductImage> ImageEns { get; set; }


        //
        private RouteDataUrl _routeDataUrlVn;
        [BsonIgnore]
        public RouteDataUrl RouteDataUrlVn
        {
            get
            {
                if (_routeDataUrlVn == null && !string.IsNullOrEmpty(RouteDataUrlVnId))
                    _routeDataUrlVn = DbContext.Current.GetOne<RouteDataUrl>(u => u.Id.Equals(RouteDataUrlVnId));
                return _routeDataUrlVn;
            }
            set
            {
                _routeDataUrlVn = value;
            }
        }

        //
        private RouteDataUrl _routeDataUrlEn;
        [BsonIgnore]
        public RouteDataUrl RouteDataUrlEn
        {
            get
            {
                if (_routeDataUrlEn == null && !string.IsNullOrEmpty(RouteDataUrlEnId))
                    _routeDataUrlEn = DbContext.Current.GetOne<RouteDataUrl>(u => u.Id.Equals(RouteDataUrlEnId));
                return _routeDataUrlEn;
            }
            set
            {
                _routeDataUrlEn = value;
            }
        }
        private List<ProductOverviewBlock> _productOverviewBlockVn;
        [BsonIgnore]
        public List<ProductOverviewBlock> ProductOverviewBlockVn
        {
            get
            {
                if (_productOverviewBlockVn == null)
                    _productOverviewBlockVn = DbContext.Current.GetMany<ProductOverviewBlock>(u => u.ProductId.Equals(Id) && u.Language == ProductOverviewBlock.IsLanguage.Vietnamese).OrderBy(o=> o.Sort).ToList();

                return _productOverviewBlockVn;
            }
            set
            {
                _productOverviewBlockVn = value;
            }
        }

        private List<ProductOverviewBlock> _productOverviewBlockEn;
        [BsonIgnore]
        public List<ProductOverviewBlock> ProductOverviewBlockEn
        {
            get
            {
                if (_productOverviewBlockEn == null)
                    _productOverviewBlockEn = DbContext.Current.GetMany<ProductOverviewBlock>(u => u.ProductId.Equals(Id) && u.Language == ProductOverviewBlock.IsLanguage.English).OrderBy(o => o.Sort).ToList();

                return _productOverviewBlockEn;
            }
            set
            {
                _productOverviewBlockEn = value;
            }
        }

        private ProductCategory _productCategory;
        [BsonIgnore]
        public ProductCategory ProductCategory
        {
            get
            {
                if (_productCategory == null)
                    _productCategory = DbContext.Current.GetOne<ProductCategory>(u => u.Id.Equals(ProductCategoryId));

                return _productCategory;
            }
            set
            {
                _productCategory = value;
            }
        }

        //
        private User _addBy;
        [BsonIgnore]
        public User AddBy
        {
            get
            {
                if (_addBy == null && !string.IsNullOrEmpty(AddedBy))
                    _addBy = DbContext.Current.GetOne<User>(u => u.Id.Equals(AddedBy));
                return _addBy;
            }
            set
            {
                _addBy = value;
            }
        }

        //
        private User _editBy;
        [BsonIgnore]
        public User EditBy
        {
            get
            {
                if (_editBy == null && !string.IsNullOrEmpty(EditedBy))
                    _editBy = DbContext.Current.GetOne<User>(u => u.Id.Equals(EditedBy));
                return _editBy;
            }
            set
            {
                _editBy = value;
            }
        }
    }

    public class ProductImage
    {
        public Guid Id { get; set; }
        public string ImageSrc { get; set; }
        public int Sort { get; set; }
    }
}

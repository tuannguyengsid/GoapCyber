using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using GSID.Web.Core.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IProductService
    {
        Product GetBy(string id);
        Product GetRouterVn(string routerid);
        Product GetRouterEn(string routerid);
        Product GetBySlugEn(string slug);
        Product GetBySlugVn(string slug);
        Product IsNameVnAvailable(string name);
        Product IsNameVnAvailable(string name, string id);
        Product IsNameEnAvailable(string name);
        Product IsNameEnAvailable(string name, string id);
        List<Product> GetAll();
        List<Product> GetAll(bool isDeleted);
        List<Product> GetAllByRelated(string categoryId, string senprdId, bool isDeleted);
        List<Product> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] ProductCategoryId);
        List<Product> GetAllByCategory(string categoryId);
        List<Product> GetAllByCategory(string categoryId, bool isDeleted);
        List<Product> GetAllByCategory(List<string> categoryIds, bool isDeleted);
        string Create(Product obj);
        void Update(Product obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class ProductService : IProductService
    {
        IGSIDMongoRepository repository;

        public ProductService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Product GetBy(string id)
        {
            return repository.GetOne<Product>(id);
        }

        public Product GetRouterVn(string routerid)
        {
            return repository.GetOne<Product>(w => w.RouteDataUrlVnId == routerid);
        }

        public Product GetRouterEn(string routerid)
        {
            return repository.GetOne<Product>(w => w.RouteDataUrlEnId == routerid);
        }

        public Product GetBySlugEn(string slug)
        {
            return repository.GetOne<Product>(c => c.SlugEn == slug);
        }

        public Product GetBySlugVn(string slug)
        {
            return repository.GetOne<Product>(c => c.SlugVn == slug);
        }

        public Product IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Product>(w=> w.NameVn.ToLower() == name);
        }

        public Product IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Product>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public Product IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Product>(w => w.NameEn.ToLower() == name);
        }

        public Product IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Product>(w => w.NameEn.ToLower() == name && w.Id != id);
        }


        public List<Product> GetAll()
        {
            return repository.All<Product>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Product> GetAll(bool isDeleted)
        {
            return repository.GetMany<Product>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Product> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] ProductCategoryId)
        {
            ProductCategoryId = ProductCategoryId != null ? Array.FindAll(ProductCategoryId, s => !string.IsNullOrEmpty(s)) : ProductCategoryId;
            var _all = repository.All<Product>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(c => (!string.IsNullOrEmpty(keyword) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(keyword) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                    ).ToList();
            }

            if (ProductCategoryId != null && ProductCategoryId.Count() > 0)
                _all = _all.Where(c => ProductCategoryId.Contains(c.ProductCategoryId)).ToList();
            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public List<Product> GetAllByCategory(List<string> categoryIds, bool isDeleted)
        {
            return repository.GetMany<Product>(c => categoryIds.Contains(c.ProductCategoryId) && c.IsDeleted == isDeleted).ToList();
        }
        
        public List<Product> GetAllByCategory(string categoryId)
        {
            return repository.GetMany<Product>(c => c.ProductCategoryId == categoryId).ToList();
        }

        public List<Product> GetAllByCategory(string categoryId, bool isDeleted)
        {
            return repository.GetMany<Product>(c => c.ProductCategoryId == categoryId && c.IsDeleted == isDeleted).ToList();
        }

        public List<Product> GetAllByRelated(string categoryId, string senprdId, bool isDeleted)
        {
            return repository.GetMany<Product>(c => c.ProductCategoryId == categoryId && c.Id != senprdId && c.IsDeleted == isDeleted).ToList();
        }

        public string Create(Product obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Product>(obj);
        }

        public void Update(Product obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Product>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<Product>(id);
                if (obj != null)
                {
                    repository.Delete<Product>(id);
                    repository.Delete<ProductOverviewBlock>(w => w.ProductId == id);

                    if (!string.IsNullOrEmpty(obj.RouteDataUrlVnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlVnId);
                    if (!string.IsNullOrEmpty(obj.RouteDataUrlEnId))
                        repository.Delete<RouteDataUrl>(w => w.Id == obj.RouteDataUrlEnId);


                    result = true;
                }
            }
            catch
            {
            }
            return result;
        }

        public bool DeleteAll()
        {
            bool result = false;
            try
            {
                repository.Delete<Product>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

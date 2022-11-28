using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IProductCategoryService
    {
        ProductCategory GetBy(string id);
        ProductCategory GetRouterVn(string routerid);
        ProductCategory GetRouterEn(string routerid);
        ProductCategory GetBySlugEn(string slug);
        ProductCategory GetBySlugVn(string slug);
        ProductCategory IsNameVnAvailable(string name);
        ProductCategory IsNameVnAvailable(string name, string id);
        ProductCategory IsNameEnAvailable(string name);
        ProductCategory IsNameEnAvailable(string name, string id);
        ProductCategory GetBy(string id, bool isDeleted);
        List<ProductCategory> GetAll();
        List<ProductCategory> GetAll(bool isDeleted);
        List<ProductCategory> GetAllBySearch(string keyword, bool isDeleted);
        List<ProductCategory> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(ProductCategory obj);
        void Update(ProductCategory obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class ProductCategoryService : IProductCategoryService
    {
        IGSIDMongoRepository repository;

        public ProductCategoryService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public ProductCategory GetBy(string id)
        {
            return repository.GetOne<ProductCategory>(id);
        }

        public ProductCategory GetRouterVn(string routerid)
        {
            return repository.GetOne<ProductCategory>(w=> w.RouteDataUrlVnId == routerid);
        }

        public ProductCategory GetRouterEn(string routerid)
        {
            return repository.GetOne<ProductCategory>(w => w.RouteDataUrlEnId == routerid);
        }

        public ProductCategory GetBySlugEn(string slug)
        {
            return repository.GetOne<ProductCategory>(c => c.SlugEn == slug);
        }

        public ProductCategory GetBySlugVn(string slug)
        {
            return repository.GetOne<ProductCategory>(c => c.SlugVn == slug);
        }

        public ProductCategory IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProductCategory>(w => w.NameVn.ToLower() == name);
        }

        public ProductCategory IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProductCategory>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public ProductCategory IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProductCategory>(w => w.NameEn.ToLower() == name);
        }

        public ProductCategory IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<ProductCategory>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public ProductCategory GetBy(string id, bool isDeleted)
        {
            return repository.GetOne<ProductCategory>(c => c.Id == id && c.IsDeleted == isDeleted);
        }

        public List<ProductCategory> GetAll()
        {
            return repository.All<ProductCategory>().OrderBy(c => c.Sort).ToList();
        }

        public List<ProductCategory> GetAllBySearch(string keyword, bool isDeleted)
        {
            return repository.GetMany<ProductCategory>(c => ((!string.IsNullOrEmpty(keyword) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                                            || (!string.IsNullOrEmpty(keyword) && c.NameEn.ToLower().Contains(keyword.ToLower())))
                                                        && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort).ToList();
        }

        public List<ProductCategory> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<ProductCategory>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(c => (!string.IsNullOrEmpty(keyword) && c.NameVn.ToLower().Contains(keyword.ToLower()))
                                            || (!string.IsNullOrEmpty(keyword) && c.NameEn.ToLower().Contains(keyword.ToLower()))
                                    ).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderBy(c => c.Sort).ToList();
        }

        public List<ProductCategory> GetAll(bool isDeleted)
        {
            return repository.GetMany<ProductCategory>(c => c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }


        public string Create(ProductCategory obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<ProductCategory>(obj);
        }

        public void Update(ProductCategory obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<ProductCategory>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                var obj = repository.GetOne<ProductCategory>(id);
                if (obj != null)
                {
                    repository.Delete<ProductCategory>(id);

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
                repository.Delete<ProductCategory>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IProductOverviewBlockService
    {
        ProductOverviewBlock GetBy(string id);
        ProductOverviewBlock GetBy(string id, bool isDeleted);
        List<ProductOverviewBlock> GetAll();
        List<ProductOverviewBlock> GetAll(bool isDeleted);
        List<ProductOverviewBlock> GetAllByProduct(string prdId);
        List<ProductOverviewBlock> GetAllByProduct(string prdId, bool isDeleted);
        List<ProductOverviewBlock> GetAllByProduct(string prdId, ProductOverviewBlock.IsLanguage lang);
        List<ProductOverviewBlock> GetAllByProduct(string prdId, ProductOverviewBlock.IsLanguage lang, bool isDeleted);
        string Create(ProductOverviewBlock obj);
        void Update(ProductOverviewBlock obj);
        bool Delete(string id);
        bool DeleteByProduct(string prd);
        bool DeleteAll();
    }

    public class ProductOverviewBlockService : IProductOverviewBlockService
    {
        IGSIDMongoRepository repository;

        public ProductOverviewBlockService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public ProductOverviewBlock GetBy(string id)
        {
            return repository.GetOne<ProductOverviewBlock>(id);
        }

        public ProductOverviewBlock GetBy(string id, bool isDeleted)
        {
            return repository.GetOne<ProductOverviewBlock>(c => c.Id == id && c.IsDeleted == isDeleted);
        }

        public List<ProductOverviewBlock> GetAll()
        {
            return repository.All<ProductOverviewBlock>().OrderBy(c => c.Sort).ToList();
        }

        public List<ProductOverviewBlock> GetAll(bool isDeleted)
        {
            return repository.GetMany<ProductOverviewBlock>(c => c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }

        public List<ProductOverviewBlock> GetAllByProduct(string prdId)
        {
            return repository.GetMany<ProductOverviewBlock>(c => c.ProductId == prdId)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }

        public List<ProductOverviewBlock> GetAllByProduct(string prdId, bool isDeleted)
        {
            return repository.GetMany<ProductOverviewBlock>(c => c.ProductId == prdId && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }

        public List<ProductOverviewBlock> GetAllByProduct(string prdId, ProductOverviewBlock.IsLanguage lang)
        {
            return repository.GetMany<ProductOverviewBlock>(c => c.ProductId == prdId && c.Language == lang)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }
        public List<ProductOverviewBlock> GetAllByProduct(string prdId, ProductOverviewBlock.IsLanguage lang, bool isDeleted)
        {
            return repository.GetMany<ProductOverviewBlock>(c => c.ProductId == prdId && c.Language == lang && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.Sort)
                                        .ToList();
        }

        public string Create(ProductOverviewBlock obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<ProductOverviewBlock>(obj);
        }

        public void Update(ProductOverviewBlock obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<ProductOverviewBlock>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<ProductOverviewBlock>(id);
                result = true;
            }
            catch
            {
            }
            return result;
        }

        public bool DeleteByProduct(string prd)
        {
            bool result = false;
            try
            {
                repository.Delete<ProductOverviewBlock>(c => c.ProductId == prd);
                result = true;
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
                repository.Delete<ProductOverviewBlock>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

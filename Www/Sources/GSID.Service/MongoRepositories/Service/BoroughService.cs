
using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IBoroughService
    {
        Borough GetBy(string id);
        Borough GetBySync(string keywordSync);
        List<Borough> GetAll();
        List<Borough> GetAll(bool isDeleted);
        void Create(Borough obj);
        void Update(Borough obj);
        bool? Delete(string id);
        bool? DeleteAll();
    }

    public class BoroughService : IBoroughService
    {
        IGSIDMongoRepository repository;

        public BoroughService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Borough GetBy(string id)
        {
            return repository.GetOne<Borough>(id);
        }

        public Borough GetBySync(string keywordSync)
        {
            return repository.GetOne<Borough>(c => c.KeywordSync.Contains(keywordSync));
        }

        public List<Borough> GetAll()
        {
            return repository.All<Borough>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Borough> GetAll(bool isDeleted)
        {
            return repository.GetMany<Borough>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public void Create(Borough obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            repository.Insert<Borough>(obj);
        }

        public void Update(Borough obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Borough>(obj);
        }

        public bool? Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Borough>(id);
                result = true;
            }
            catch
            {
            }
            return result;
        }
        public bool? DeleteAll()
        {
            bool result = false;
            try
            {
                repository.Delete<Borough>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

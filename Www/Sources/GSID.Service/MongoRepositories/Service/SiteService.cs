using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface ISiteService
    {
        Site GetBy(string id);
        List<Site> GetAll();
        List<Site> GetAll(bool isDeleted);
        List<Site> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(Site obj);
        void Update(Site obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class SiteService : ISiteService
    {
        IGSIDMongoRepository repository;

        public SiteService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Site GetBy(string id)
        {
            return repository.GetOne<Site>(id);
        }

        public List<Site> GetAll()
        {
            return repository.All<Site>()
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Site> GetAll(bool isDeleted)
        {
            return repository.GetMany<Site>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.EditedByDate ?? DateTime.Now)
                                    .ThenByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Site> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<Site>();

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

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public string Create(Site obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Site>(obj);
        }

        public void Update(Site obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Site>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Site>(id);
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
                repository.Delete<Site>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}


using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IProvinceService
    {
        Province GetBy(string id);
        List<Province> GetAll();
        List<Province> GetAll(bool isDeleted);
        List<Province> GetAllSortName(bool isDeleted);
        List<Province> GetAllByCountryIsDeleted(string country, Nullable<bool> isDeleted);
        List<Province> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] CountryId);
        string Create(Province obj);
        void Update(Province obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class ProvinceService : IProvinceService
    {
        private readonly IGSIDMongoRepository repository;

        public ProvinceService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Province GetBy(string id)
        {
            return repository.GetOne<Province>(id);
        }

        public List<Province> GetAll()
        {
            return repository.All<Province>().OrderBy(c => c.Name).ToList();
        }

        public List<Province> GetAllSortName(bool isDeleted)
        {
            return repository.All<Province>().OrderBy(c => c.NameVn).ToList();
        }

        public List<Province> GetAll(bool isDeleted)
        {
            return repository.GetMany<Province>(c => c.IsDeleted == isDeleted).OrderBy(c => c.Name).ToList();
        }


        public List<Province> GetAllByCountryIsDeleted(string country, Nullable<bool> isDeleted)
        {
            return repository.GetMany<Province>(c => c.CountryId == country 
                                        && c.IsDeleted == isDeleted)
                                    .OrderBy(c => c.Name).ToList();
        }

        public List<Province> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate, string[] CountryId)
        {
            CountryId = CountryId != null ? Array.FindAll(CountryId, s => !string.IsNullOrEmpty(s)) : CountryId;

            var _all = repository.All<Province>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(w => (!string.IsNullOrEmpty(w.NameVn) && w.NameVn.ToLower().Contains(keyword))
                                        || (!string.IsNullOrEmpty(w.NameEn) && w.NameEn.ToLower().Contains(keyword))).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();
            if (CountryId != null && CountryId.Count() > 0)
                _all = _all.Where(c => CountryId.Contains(c.CountryId)).ToList();

            return _all.OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public string Create(Province obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted   = false;
            return repository.Insert<Province>(obj);
        }

        public void Update(Province obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Province>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Province>(id);
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
                repository.Delete<Province>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

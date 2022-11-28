using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface ICountryService
    {
        Country GetBy(string id);
        List<Country> GetAll();
        List<Country> GetAll(bool isDeleted);
        List<Country> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(Country obj);
        void Update(Country obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class CountryService : ICountryService
    {
        private readonly IGSIDMongoRepository repository;

        public CountryService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Country GetBy(string id)
        {
            return repository.GetOne<Country>(id);
        }

        public List<Country> GetAll()
        {
            return repository.All<Country>()
                                .OrderBy(c => c.NameEn).ToList();
        }

        public List<Country> GetAll(bool isDeleted)
        {
            return repository.GetMany<Country>(c => c.IsDeleted == isDeleted).OrderBy(c => c.NameEn).ToList();
        }

        public List<Country> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<Country>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(w => (!string.IsNullOrEmpty(w.NameVn) && w.NameVn.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.NameEn) && w.NameEn.ToLower().Contains(keyword))
                                    ).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderBy(c => c.NameVn).ToList();
        }

        public string Create(Country obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Country>(obj);
        }

        public void Update(Country obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Country>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Country>(id);
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
                repository.Delete<Country>();
                result = true;
            }
            catch (Exception ex)
            {
                string msg = ex.ToString();
            }
            return result;
        }
    }
}

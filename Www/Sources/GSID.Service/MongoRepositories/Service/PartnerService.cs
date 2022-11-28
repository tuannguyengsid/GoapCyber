using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IPartnerService
    {
        Partner GetBy(string id);
        Partner IsNameVnAvailable(string name);
        Partner IsNameVnAvailable(string name, string id);
        Partner IsNameEnAvailable(string name);
        Partner IsNameEnAvailable(string name, string id);
        List<Partner> GetAll();
        List<Partner> GetAll(bool isDeleted);
        List<Partner> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(Partner obj);
        void Update(Partner obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class PartnerService : IPartnerService
    {
        private readonly IGSIDMongoRepository repository;

        public PartnerService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Partner GetBy(string id)
        {
            return repository.GetOne<Partner>(id);
        }

        public Partner IsNameVnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Partner>(w => w.NameVn.ToLower() == name);
        }

        public Partner IsNameVnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Partner>(w => w.NameVn.ToLower() == name && w.Id != id);
        }

        public Partner IsNameEnAvailable(string name)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Partner>(w => w.NameEn.ToLower() == name);
        }

        public Partner IsNameEnAvailable(string name, string id)
        {
            name = !string.IsNullOrEmpty(name) ? name.Trim().ToLower() : "";
            return repository.GetOne<Partner>(w => w.NameEn.ToLower() == name && w.Id != id);
        }

        public List<Partner> GetAll()
        {
            return repository.All<Partner>()
                                .OrderBy(c => c.NameEn).ToList();
        }

        public List<Partner> GetAll(bool isDeleted)
        {
            return repository.GetMany<Partner>(c => c.IsDeleted == isDeleted).OrderBy(c => c.Sort).ToList();
        }

        public List<Partner> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<Partner>();

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

        public string Create(Partner obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Partner>(obj);
        }

        public void Update(Partner obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Partner>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Partner>(id);
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
                repository.Delete<Partner>();
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

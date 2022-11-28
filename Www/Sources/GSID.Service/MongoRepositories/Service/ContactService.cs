using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IContactService
    {
        Contact GetBy(string id); 
        Contact GetByEmail(string email);
        List<Contact> GetAll();
        List<Contact> GetAll(bool isDeleted);
        List<Contact> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        string Create(Contact obj);
        void Update(Contact obj);
        bool? Delete(string id);
        bool? DeleteAll();
    }

    public class ContactService : IContactService
    {
        private readonly IGSIDMongoRepository repository;

        public ContactService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Contact GetBy(string id)
        {
            return repository.GetOne<Contact>(id);
        }

        public Contact GetByEmail(string email)
        {
            return repository.GetOne<Contact>(c=> c.Email == email);
        }

        public List<Contact> GetAll()
        {
            return repository.All<Contact>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<Contact> GetAll(bool isDeleted)
        {
            return repository.GetMany<Contact>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<Contact> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<Contact>();

            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _all = _all.Where(w => (!string.IsNullOrEmpty(w.FullName) && w.FullName.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.Email) && w.Email.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.PhoneNumber) && w.PhoneNumber.ToLower().Contains(keyword))
                                    ).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderByDescending(c => c.AddedByDate).ToList();
        }

        public string Create(Contact obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Contact>(obj);
        }

        public void Update(Contact obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Contact>(obj);
        }

        public bool? Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Contact>(id);
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
                repository.Delete<Contact>();
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

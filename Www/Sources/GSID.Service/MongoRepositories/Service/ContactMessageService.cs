using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IContactMessageService
    {
        ContactMessage GetBy(string id);
        List<ContactMessage> GetAll();
        List<ContactMessage> GetAll(bool isDeleted);
        List<ContactMessage> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate);
        List<ContactMessage> GetAllByContact(string contactId);
        List<ContactMessage> GetAllByContact(string contactId, bool isDeleted);
        string Create(ContactMessage obj);
        void Update(ContactMessage obj);
        bool Delete(string id);
        bool DeleteByContact(string contactId);
        bool? DeleteAll();
    }

    public class ContactMessageService : IContactMessageService
    {
        private readonly IGSIDMongoRepository repository;

        public ContactMessageService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public ContactMessage GetBy(string id)
        {
            return repository.GetOne<ContactMessage>(id);
        }

        public List<ContactMessage> GetAll()
        {
            return repository.All<ContactMessage>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<ContactMessage> GetAll(bool isDeleted)
        {
            return repository.GetMany<ContactMessage>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<ContactMessage> GetAllBySearch(string keyword, DateTime? BeginAddDate, DateTime? EndAddDate)
        {
            var _all = repository.All<ContactMessage>();
            var _allContact = repository.All<Contact>();
            List<string> _contactIds = null;


            if (!string.IsNullOrEmpty(keyword))
            {
                keyword = keyword.Trim().ToLower();
                _allContact = _allContact.Where(w => (!string.IsNullOrEmpty(w.FullName) && w.FullName.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.Email) && w.Email.ToLower().Contains(keyword))
                                            || (!string.IsNullOrEmpty(w.PhoneNumber) && w.PhoneNumber.ToLower().Contains(keyword))
                                    ).ToList();
                _contactIds = _allContact != null ? _allContact.Select(s => s.Id).ToList() : null;

                _all = _all.Where(w => ((!string.IsNullOrEmpty(w.Message) && w.Message.ToLower().Contains(keyword))) || ((_contactIds != null) && _contactIds.Contains(w.ContactId))).ToList();
            }

            if (BeginAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value >= BeginAddDate.Value).ToList();
            if (EndAddDate.HasValue)
                _all = _all.Where(w => w.AddedByDate.HasValue && w.AddedByDate.Value <= EndAddDate.Value).ToList();

            return _all.OrderByDescending(c => c.AddedByDate ?? DateTime.MaxValue).ToList();
        }

        public List<ContactMessage> GetAllByContact(string contactId)
        {
            return repository.GetMany<ContactMessage>(c => c.ContactId == contactId)
                                .OrderBy(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public List<ContactMessage> GetAllByContact(string contactId, bool isDeleted)
        {
            return repository.GetMany<ContactMessage>(c => c.ContactId == contactId && c.IsDeleted == isDeleted)
                                .OrderBy(c => c.AddedByDate ?? DateTime.Now)
                                    .ToList();
        }

        public string Create(ContactMessage obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<ContactMessage>(obj);
        }

        public void Update(ContactMessage obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<ContactMessage>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<ContactMessage>(id);
                result = true;
            }
            catch
            {
            }
            return result;
        }

        public bool DeleteByContact(string contactId)
        {

            bool result = false;
            try
            {
                repository.Delete<ContactMessage>(c=> c.ContactId == contactId);
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
                repository.Delete<ContactMessage>();
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

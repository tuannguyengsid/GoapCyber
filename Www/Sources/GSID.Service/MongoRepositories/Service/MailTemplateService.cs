using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface IMailTemplateService
    {
        MailTemplate GetBy(string id);
        MailTemplate GetByCode(string code);
        List<MailTemplate> GetAll();
        List<MailTemplate> GetAll(bool isDeleted);
        string Create(MailTemplate obj);
        void Update(MailTemplate obj);
        bool Delete(string id);
        bool DeleteAll();
    }

    public class MailTemplateService : IMailTemplateService
    {
        IGSIDMongoRepository repository;

        public MailTemplateService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public MailTemplate GetBy(string id)
        {
            return repository.GetOne<MailTemplate>(id);
        }

        public MailTemplate GetByCode(string code)
        {
            return repository.GetOne<MailTemplate>(c => c.Code == code);
        }

        public List<MailTemplate> GetAll()
        {
            return repository.All<MailTemplate>().OrderBy(c => c.AddedByDate ?? DateTime.MaxValue).ToList();
        }

        public List<MailTemplate> GetAll(bool isDeleted)
        {
            return repository.GetMany<MailTemplate>(c => c.IsDeleted == isDeleted).OrderBy(c => c.AddedByDate ?? DateTime.MaxValue).ToList();
        }

        public string Create(MailTemplate obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<MailTemplate>(obj);
        }

        public void Update(MailTemplate obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<MailTemplate>(obj);
        }

        public bool Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<MailTemplate>(id);
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
                repository.Delete<MailTemplate>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

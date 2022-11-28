using GSID.Data.Mongodb.MongoCore;
using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Service.MongoRepositories.Service
{
    public interface ICustomerService
    {
        Customer GetBy(string id);
        Customer GetByEmail(string email);
        Customer GetByTokenResetPassword(string token);
        Customer GetByPhone(string phonenumber);
        Customer GetByEmailOrPhone(string email, string phonenumber);
        Customer GetByTokenRegistration(string token);
        Customer VerifiedAccountEmailOrPhone(string emailphone);
        List<Customer> GetAll();
        List<Customer> GetAll(bool isDeleted);
        string Create(Customer obj);
        void Update(Customer obj);
        bool? Delete(string id);
        bool? DeleteAll();
    }

    public class CustomerService : ICustomerService
    {
        IGSIDMongoRepository repository;

        public CustomerService(IGSIDMongoRepository _repository)
        {
            this.repository = _repository;
        }

        public Customer GetBy(string id)
        {
            return repository.GetOne<Customer>(id);
        }

        public Customer GetByEmail(string email)
        {
            return repository.GetOne<Customer>(c => c.Email == email);
        }

        public Customer GetByTokenResetPassword(string token)
        {

            return repository.GetOne<Customer>(c => c.ResetPasssToken.ToLower() == token);
        }

        public Customer VerifiedAccountEmailOrPhone(string emailphone)
        {
            return repository.GetOne<Customer>(c => c.Email == emailphone || c.PhoneNumber == emailphone);
        }

        public Customer GetByPhone(string phonenumber)
        {
            return repository.GetOne<Customer>(c => c.PhoneNumber == phonenumber);
        }

        public Customer GetByEmailOrPhone(string email, string phonenumber)
        {
            return repository.GetOne<Customer>(c => c.Email == email || c.PhoneNumber == phonenumber);
        }

        public Customer GetByTokenRegistration(string token)
        {
            return repository.GetOne<Customer>(c => c.AccountVerifyToken == token);
        }

        public List<Customer> GetAll()
        {
            return repository.All<Customer>()
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public List<Customer> GetAll(bool isDeleted)
        {
            return repository.GetMany<Customer>(c => c.IsDeleted == isDeleted)
                                .OrderByDescending(c => c.AddedByDate ?? DateTime.Now)
                                        .ToList();
        }

        public string Create(Customer obj)
        {
            obj.AddedByDate = DateTime.Now;
            obj.IsDeleted = false;
            return repository.Insert<Customer>(obj);
        }

        public void Update(Customer obj)
        {
            obj.EditedByDate = DateTime.Now;
            repository.Update<Customer>(obj);
        }

        public bool? Delete(string id)
        {
            bool result = false;
            try
            {
                repository.Delete<Customer>(id);
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
                repository.Delete<Customer>();
                result = true;
            }
            catch
            {
            }
            return result;
        }
    }
}

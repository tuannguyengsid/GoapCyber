using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class ContactMessage : GSIDMongoEntity
    {
        public string Message { get; set; }
        public string ContactId { get; set; }
        public string Ip { get; set; }
        #region not map
        private Contact _contact;
        [BsonIgnore]
        public Contact Contact
        {
            get
            {
                if (_contact == null)
                    _contact = DbContext.Current.GetOne<Contact>(u => u.Id.Equals(ContactId));

                return _contact;
            }
            set
            {
                _contact = value;
            }
        }

        //
        private User _addBy;
        [BsonIgnore]
        public User AddBy
        {
            get
            {
                if (_addBy == null && !string.IsNullOrEmpty(AddedBy))
                    _addBy = DbContext.Current.GetOne<User>(u => u.Id.Equals(AddedBy));
                return _addBy;
            }
            set
            {
                _addBy = value;
            }
        }

        //
        private User _editBy;
        [BsonIgnore]
        public User EditBy
        {
            get
            {
                if (_editBy == null && !string.IsNullOrEmpty(EditedBy))
                    _editBy = DbContext.Current.GetOne<User>(u => u.Id.Equals(EditedBy));
                return _editBy;
            }
            set
            {
                _editBy = value;
            }
        }
        #endregion
    }
}

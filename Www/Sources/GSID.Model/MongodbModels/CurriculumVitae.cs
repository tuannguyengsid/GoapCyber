using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class CurriculumVitae : GSIDMongoEntity
    {
        public string ContactId { get; set; }
        public string FileSrc { get; set; }
        public string CareerId { get; set; }
        public string RecruitmentId { get; set; }
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

        private Career _career;
        [BsonIgnore]
        public Career Career
        {
            get
            {
                if (_career == null)
                    _career = DbContext.Current.GetOne<Career>(u => u.Id.Equals(CareerId));

                return _career;
            }
            set
            {
                _career = value;
            }
        }

        private Recruitment _recruitment;
        [BsonIgnore]
        public Recruitment Recruitment
        {
            get
            {
                if (_recruitment == null)
                    _recruitment = DbContext.Current.GetOne<Recruitment>(u => u.Id.Equals(RecruitmentId));

                return _recruitment;
            }
            set
            {
                _recruitment = value;
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

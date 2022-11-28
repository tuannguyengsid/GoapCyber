using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class MailTemplate : GSIDMongoEntity
    {
        public string Code { get; set; }
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public string SubjectVn { get; set; }
        public string SubjectEn { get; set; }
        public string BodyVn { get; set; }
        public string BodyEn { get; set; }

        #region not map

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

    public class MailTemplateCode
    {
        public static readonly string CONFIRM_NEW_ACCOUNT = "MAILTEMPLATE_CONFIRM_NEW_ACCOUNT";
        public static readonly string FORGOT_PASSWPRD_ACCOUNT = "MAILTEMPLATE_FORGOT_PASSWPRD_ACCOUNT";
        
        
        public static readonly string NOTIFICATION_CONTACTMESSAGE = "MAILTEMPLATE_NOTIFICATION_CONTACTMESSAGE";
        public static readonly string NOTIFICATION_CURRICULUMVITAE = "MAILTEMPLATE_NOTIFICATION_CURRICULUMVITAE";
    }
}

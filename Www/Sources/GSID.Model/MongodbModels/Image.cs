using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using GSID.Data.Mongodb.FrameworkCore;
using GSID.Data.Mongodb;
using System.Linq;

namespace GSID.Model.MongodbModels
{
    [BsonIgnoreExtraElements]
    public partial class Image : GSIDMongoEntity
    {
        public string[] ImageLibraryIds { get; set; }
        public string ImageSrc { get; set; }
        #region not map

        private List<ImageLibrary> _imageLibraries;
        [BsonIgnore]
        public List<ImageLibrary> ImageLibraries
        {
            get
            {
                if (_imageLibraries == null && ImageLibraryIds != null)
                {
                    _imageLibraries = new List<ImageLibrary>();
                    foreach (var x in ImageLibraryIds)
                    {
                        try
                        {
                            var imageLibrary = DbContext.Current.GetOne<ImageLibrary>(u => ImageLibraryIds.Contains(u.Id));
                            if (imageLibrary != null)
                                _imageLibraries.Add(imageLibrary);
                        }
                        catch
                        {
                        }
                    }
                }    

                return _imageLibraries;
            }
            set
            {
                _imageLibraries = value;
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

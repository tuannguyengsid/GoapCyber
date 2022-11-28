using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.FrameworkCore
{
    public interface IGSIDEntity
    {
        string Id { get; set; }
        long Version { get; set; }
        string AddedBy { get; set; }
        Nullable<System.DateTime> AddedByDate { get; set; }
        string EditedBy { get; set; }
        Nullable<System.DateTime> EditedByDate { get; set; }
        string DeletedBy { get; set; }
        Nullable<System.DateTime> DeletedByDate { get; set; }
        bool IsDeleted { get; set; }
    }
}

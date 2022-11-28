using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GSID.Admin.ViewModels.MongoModels
{
    public abstract class SystemEntity
    {
        public virtual Guid Id { get; set; }
    }
}
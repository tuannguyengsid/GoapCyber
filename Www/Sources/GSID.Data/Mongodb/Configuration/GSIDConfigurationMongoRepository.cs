using GSID.Data.Mongodb.FrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.ConfigurationsCore
{
    /// <summary>
    /// This is specific to flag settings or other configurations that can drive the apllication. That's the reason this comes as a part of core framework.
    /// Advantage: Even releasing changes to Core framework can be flag protected.
    /// </summary>
    public class GSIDConfigurationMongoRepository : GSIDMongoRepository, IGSIDConfigurationMongoRepository
    {
      
        public GSIDConfigurationMongoRepository()
        {
            connectionUrl = new Lazy<string>(() => ConfigurationManager.AppSettings["mongoConnUrl"].ToString());
            databaseName = new Lazy<string>(() => ConfigurationManager.AppSettings["GSIDConfigurationDatabaseName"].ToString());
        }

    }
}

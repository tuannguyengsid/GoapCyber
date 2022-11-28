using GSID.Data.Mongodb.MongoCore;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Data.Mongodb.FrameworkCore
{
    public static class MongoLazyLoad
    {
        /// <summary>
        /// Initializes StructureMap (dependency injector) to setup our concrete database provider.
        /// </summary>
        public static void Initialize()
        {
            // Initialize our concrete database provider type.
            ObjectFactory.Initialize(x => { x.For<IGSIDMongoRepository>().Use<GSIDMongoRepository>(); });
        }

        /// <summary>
        /// Disposes the database provider context.
        /// </summary>
        public static void Close()
        {
            if (DbContext.IsOpen)
            {
                DbContext.Current.Dispose();
            }
        }
    }
}

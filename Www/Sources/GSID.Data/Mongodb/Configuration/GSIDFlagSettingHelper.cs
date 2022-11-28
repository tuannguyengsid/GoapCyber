using System;
using System.Linq;
//using System.Runtime.Caching;
//using System.Configuration;
using MongoDB.Driver;

namespace GSID.Data.Mongodb.ConfigurationsCore
{
    //Well, such a flag setting can even be driven by an xml file. I'm using database here so that you actually avoid blasting that Xml file to all servers(in a web farm).
    public class GSIDFlagSettingHelper
    {
        //private GSIDFlagSettingHelper() { }
        //public static string redisConnectionString { get; set; }

        //public static T Get<T>(string key) where T : IConvertible
        //{ 
        //    //old code
        //    //GSIDRedisCacheRepository _redisCache = new GSIDRedisCacheRepository(ConfigurationManager.AppSettings["redisConnectionString"].ToString(), 
        //    //                                        GSIDCacheDatabaseName.FlagSettings);
        //    GSIDRedisCacheRepository _redisCache = new GSIDRedisCacheRepository(redisConnectionString,
        //                                            GSIDCacheDatabaseName.FlagSettings);

        //    //Using distributed caching products such as Redis
        //    if (_redisCache.Exists(key))
        //    {
        //        return _redisCache.GetValue<T>(key);
        //    }
        //    else
        //    {
        //        GSIDConfigurationMongoRepository repository = new GSIDConfigurationMongoRepository();

        //        var collection = repository.DbContext.GetCollection<FlagSetting>(typeof(FlagSetting).Name);

        //        var flagValue = collection.Find<FlagSetting>(c => c.Name == key).FirstOrDefault().FlagValue;

        //        //set the value into redis
        //        _redisCache.SetValue(key, flagValue);

        //        return (T)Convert.ChangeType(flagValue, typeof(T));
        //    }
        //}

    }//End of class
}

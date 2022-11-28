using GSID.Model.MongodbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Extensions
{
    public static class EntityHelper
    {
        //public static T ShallowCopy<T>(this T entity) where T : SystemGuidEntity
        //{
        //    var type = entity.GetType();
        //    var propertyInfos =
        //        type.GetProperties()
        //            .Where(
        //                info =>
        //                info.CanWrite &&
        //                !(info.PropertyType.IsGenericType &&
        //                  info.PropertyType.GetGenericArguments().Any(arg => arg.IsSubclassOf(typeof(SystemGuidEntity)))) &&
        //                !info.PropertyType.IsSubclassOf(typeof(SystemGuidEntity)));

        //    var shallowCopy = Activator.CreateInstance(type) as T;
        //    foreach (var propertyInfo in propertyInfos)
        //    {
        //        propertyInfo.SetValue(shallowCopy, propertyInfo.GetValue(entity, null), null);
        //    }
        //    shallowCopy.Id = Guid.NewGuid();
        //    return shallowCopy;
        //}
    }
}

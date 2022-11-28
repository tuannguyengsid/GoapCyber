using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GSID.Web.Core.Extensions
{
    public static class EnumerableHelper
    {
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action.Invoke(item);
            }
        }

        public static List<T> ToList<T>(this IEnumerable<T> list, Func<T, bool> predicate)
        {
            return list.Where(predicate).ToList();
        }

        /// <summary>
        /// Break a list of items into chunks of a specific size
        /// </summary>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int chunksize)
        {
            while (source.Any())
            {
                yield return source.Take(chunksize);
                source = source.Skip(chunksize).ToList();
            }
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int page, int? pageSize = null)
        {
            return new PagedList<T>(source, page, pageSize ?? 20);
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}

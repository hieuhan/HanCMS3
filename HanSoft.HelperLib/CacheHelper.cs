using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace HanSoft.HelperLib
{
    public class CacheHelper
    {
        #region Constructor
        public CacheHelper()
        {
        }
        #endregion

        #region Methods

        public static object GetCache(string cacheKey)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            return objCache[cacheKey];
        }

        public static void SetCache(string cacheKey, object objObject)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject);
        }

        public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            System.Web.Caching.Cache objCache = HttpRuntime.Cache;
            objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        public static void RemoveAllCache(string cacheKey)
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            cache.Remove(cacheKey);
        }

        public static void RemoveAllCache()
        {
            System.Web.Caching.Cache cache = HttpRuntime.Cache;
            IDictionaryEnumerator cacheEnum = cache.GetEnumerator();
            while (cacheEnum.MoveNext())
            {
                if (cacheEnum.Key != null) cache.Remove(cacheEnum.Key.ToString());
            }
        }
        #endregion
    }
}

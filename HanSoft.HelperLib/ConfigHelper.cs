using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanSoft.HelperLib
{
    public class ConfigHelper
    {
        #region Constructor
        public ConfigHelper()
        {
        }
        #endregion

        #region Methods
        public static string GetConfigString(string key)
        {
            string cacheKey = "AppSettings-" + key;
            object objModel = CacheHelper.GetCache(cacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = ConfigurationManager.AppSettings[key];
                    if (objModel != null)
                    {
                        CacheHelper.SetCache(cacheKey, objModel, DateTime.Now.AddMinutes(180), TimeSpan.Zero);
                    }
                }
                catch
                { }
            }
            return objModel.ToString();
        }
        #endregion
    }
}

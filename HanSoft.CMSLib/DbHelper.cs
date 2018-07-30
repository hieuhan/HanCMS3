using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanSoft.HelperLib;

namespace HanSoft.CMSLib
{
    public class DbHelper
    {
        #region Private attribute

        private static string _defaultProviderName = ConfigHelper.GetConfigString("dbProviderName");
        private static string _defaultConnectionString = ConfigHelper.GetConfigString("dbConnection");
        private static DbHelper _instance = new DbHelper(_defaultConnectionString, _defaultProviderName);

        public static DbHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        private string _providerName;
        private string _connectionString;

        public string ProviderName
        {
            get { return _providerName; }
            set { _providerName = value; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        #endregion

        #region Constructor
        public DbHelper()
        {

        }

        public DbHelper(string connectionString, string providerName)
        {
            this._connectionString = connectionString;
            this._providerName = providerName;
        }

        #endregion

        #region Create object
        public DbConnection CreateConnection()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            DbConnection dbconn = dbfactory.CreateConnection();
            dbconn.ConnectionString = _connectionString;
            return dbconn;
        }

        public DbCommand CreateCommand()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            return dbfactory.CreateCommand();
        }

        public DbCommand CreateSqlsCommand(string sqls)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            DbCommand cmd = dbfactory.CreateCommand();
            cmd.CommandText = sqls;
            cmd.CommandType = CommandType.Text;
            return cmd;
        }

        public DbCommand CreateProcCommand(string proc)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            DbCommand cmd = dbfactory.CreateCommand();
            cmd.CommandText = proc;
            cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        public DbParameter CreateParameter()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            return dbfactory.CreateParameter();
        }

        public DbParameter CreateParameter(string name, object value)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            DbParameter p = dbfactory.CreateParameter();
            p.ParameterName = name;
            p.Value = value;
            return p;
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            DbParameter p = dbfactory.CreateParameter();
            p.ParameterName = name;
            p.DbType = dbType;
            p.Value = value;
            return p;
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType, int size)
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_providerName);
            DbParameter p = dbfactory.CreateParameter();
            p.ParameterName = name;
            p.DbType = dbType;
            p.Value = value;
            p.Size = size;
            return p;
        }

        #endregion

    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HanSoft.HelperLib
{
    public class CommandHelper
    {
        #region Private attribute
        private static string _defaultProviderName = DbHelper.Instance.ProviderName;
        private static string _defaultConnectionString = DbHelper.Instance.ConnectionString;
        private static CommandHelper _instance = new CommandHelper(_defaultConnectionString, _defaultProviderName);
        public static CommandHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        private DbHelper _dbHelper;

        public string ProviderName
        {
            get { return _dbHelper.ProviderName; }
            set { _dbHelper.ProviderName = value; }
        }

        public string ConnectionString
        {
            get { return _dbHelper.ConnectionString; }
            set { _dbHelper.ConnectionString = value; }
        }

        #endregion

        #region Constructor
        public CommandHelper()
        {
            _dbHelper = DbHelper.Instance;
        }

        public CommandHelper(string connectionString, string providerName)
        {
            _dbHelper = new DbHelper(connectionString, providerName);
        }
        #endregion

        #region Create object
        public DbCommand CreateCommand()
        {
            DbProviderFactory dbfactory = DbProviderFactories.GetFactory(_defaultProviderName);
            return dbfactory.CreateCommand();
        }
        #endregion

        #region Execute
        /// <summary>
        /// Run a SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns>returns the number of rows affected</returns>
        public int RunSql(string sql, params DbParameter[] ps)
        {
            int num=0;
            DbCommand cmd = _dbHelper.CreateSqlsCommand(sql);
            try
            {
                cmd.Parameters.AddRange(ps);
                cmd.Connection = _dbHelper.CreateConnection();
                num = _dbHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return num;
        }


        /// <summary>
        /// Run a SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>returns the number of rows affected</returns>
        public int RunSql(string sql)
        {
            int num = 0;
            DbCommand cmd = _dbHelper.CreateSqlsCommand(sql);
            try
            {
                cmd.Connection = _dbHelper.CreateConnection();
                num = _dbHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return num;
        }


        /// <summary>
        /// Run a stored procedure
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="ps"></param>
        /// <returns>returns the affected row</returns>
        public int RunProc(string procName, ref DbCommand cmd, params DbParameter[] ps)
        {
            int num = 0;
            cmd = _dbHelper.CreateProcCommand(procName);
            try
            {
                cmd.Parameters.AddRange(ps);
                cmd.Connection = _dbHelper.CreateConnection();
                num = _dbHelper.ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return num;
        }

        /// <summary>
        /// Get the value of a field
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public object GetVar(string sql, params DbParameter[] ps)
        {
            object o = null;
            DbCommand cmd = _dbHelper.CreateSqlsCommand(sql);
            try
            {
                cmd.Parameters.AddRange(ps);
                cmd.Connection = _dbHelper.CreateConnection();
                o = _dbHelper.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return o;
        }

        /// <summary>
        /// Get the value of a field
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetVar(string sql)
        {
            object o = null;
            DbCommand cmd = _dbHelper.CreateSqlsCommand(sql);
            try
            {
                cmd.Connection = _dbHelper.CreateConnection();
                o = _dbHelper.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return o;
        }


        /// <summary>
        /// Get the value of a field
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetVar(DbCommand cmd)
        {
            object o = null;
            try
            {
                cmd.Connection = _dbHelper.CreateConnection();
                o = _dbHelper.ExecuteScalar(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return o;
        }

        /// <summary>
        /// Get a row of data
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public DataRow GetRow(string sql, params DbParameter[] ps)
        {
            DataTable dataTable = Select(sql, ps);
            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    return Select(sql, ps).Rows[0];
                }
            }
            return null;
        }


        /// <summary>
        /// Get a row of data
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public DataRow GetRow(DbCommand cmd)
        {
            DataTable dataTable = Select(cmd);
            if (dataTable != null)
            {
                if (dataTable.Rows.Count > 0)
                {
                    return Select(cmd).Rows[0];
                }
            }
            return null;
        }

        /// <summary>
        /// Insert a piece of data
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns>returns the automatically generated identifier</returns>
        public object Insert(string sql, params DbParameter[] ps)
        {
            return GetVar(sql + ";select SCOPE_IDENTITY();", ps);
        }

        public int Update(string sql, params DbParameter[] ps)
        {
            return RunSql(sql, ps);
        }

        /// <summary>
        /// Perform a delete operation
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public int Delete(string sql, params DbParameter[] ps)
        {
            return RunSql(sql, ps);
        }


        /// <summary>
        /// Perform a delete operation
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="ps"></param>
        /// <returns></returns>
        public int Delete(string sql)
        {
            return RunSql(sql);
        }

        public DataTable Select(string sql, params DbParameter[] ps)
        {

            DbCommand cmd = _dbHelper.CreateSqlsCommand(sql);
            DataTable dt = new DataTable();
            try
            {
                cmd.Parameters.AddRange(ps);
                cmd.Connection = _dbHelper.CreateConnection();
                dt = _dbHelper.ExecuteDataTable(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return dt;
        }

        public DataTable Select(string sql)
        {

            DbCommand cmd = _dbHelper.CreateSqlsCommand(sql);
            DataTable Dt = new DataTable();
            try
            {
                cmd.Connection = _dbHelper.CreateConnection();
                Dt = _dbHelper.ExecuteDataTable(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return Dt;
        }

        public DataTable Select(DbCommand cmd)
        {
            DataTable dt = new DataTable();
            try
            {
                cmd.Connection = _dbHelper.CreateConnection();
                dt = _dbHelper.ExecuteDataTable(cmd);
            }
            catch (Exception ex)
            {
                Logger.InstanceError.Write(ex);
            }
            finally
            {
                cmd.Dispose();
            }
            return dt;
        }

        public DbDataReader ExecuteReader(string sql, params DbParameter[] ps)
        {
            DbDataReader dr = null;
            DbCommand cmd = _dbHelper.CreateSqlsCommand(sql);
            try
            {
                cmd.Parameters.AddRange(ps);
                cmd.Connection = _dbHelper.CreateConnection();
                dr = _dbHelper.ExecuteReader(cmd);
            }
            catch (Exception ex)
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                {
                    cmd.Connection.Close();
                }
                Logger.InstanceError.Write(ex);
            }
            return dr;
        }

        public DbParameter CreateParameter()
        {
            return _dbHelper.CreateParameter();
        }

        public DbParameter CreateParameter(string name, object value)
        {
            return _dbHelper.CreateParameter(name, value);
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType)
        {
            return _dbHelper.CreateParameter(name, value, dbType);
        }

        public DbParameter CreateParameter(string name, object value, DbType dbType, int size)
        {
            return _dbHelper.CreateParameter(name, value, dbType, size);
        }
        #endregion
    }
}

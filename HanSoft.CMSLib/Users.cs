using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HanSoft.HelperLib;

namespace HanSoft.CMSLib
{
    public class Users
    {
        #region Private Properties
        private int _UserId;
        private string _UserName;
        private string _Password;
        private string _FullName;
        private string _Email;
        private string _Address;
        private string _Mobile;
        private byte _GenderId;
        private byte _UserStatusId;
        private byte _UserTypeId;
        private short _DefaultActionId;
        private DateTime _BirthDay;
        private DateTime _CrDateTime;
        

        #endregion

        #region Public Properties

        //-----------------------------------------------------------------
        public Users()
        {
            
        }
        //-----------------------------------------------------------------        
        public static Users Instance
        {
            get
            {
                return new Users();
            }
        }
        //-----------------------------------------------------------------
        ~Users()
        {

        }
        //-----------------------------------------------------------------
        public virtual void Dispose()
        {

        }

        //-----------------------------------------------------------------    
        public int UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                _UserId = value;
            }
        }

        public string UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                _UserName = value;
            }
        }
        public string Password
        {
            get
            {
                return _Password;
            }
            set
            {
                _Password = value;
            }
        }
        public string FullName
        {
            get
            {
                return _FullName;
            }
            set
            {
                _FullName = value;
            }
        }

        public string Email
        {
            get
            {
                return _Email;
            }
            set
            {
                _Email = value;
            }
        }

        public string Address
        {
            get
            {
                return _Address;
            }
            set
            {
                _Address = value;
            }
        }
        public string Mobile
        {
            get
            {
                return _Mobile;
            }
            set
            {
                _Mobile = value;
            }
        }
        public byte GenderId
        {
            get
            {
                return _GenderId;
            }
            set
            {
                _GenderId = value;
            }
        }
        public byte UserStatusId
        {
            get
            {
                return _UserStatusId;
            }
            set
            {
                _UserStatusId = value;
            }
        }
        public byte UserTypeId
        {
            get
            {
                return _UserTypeId;
            }
            set
            {
                _UserTypeId = value;
            }
        }
        public short DefaultActionId
        {
            get
            {
                return _DefaultActionId;
            }
            set
            {
                _DefaultActionId = value;
            }
        }
        public DateTime BirthDay
        {
            get
            {
                return _BirthDay;
            }
            set
            {
                _BirthDay = value;
            }
        }
        public DateTime CrDateTime
        {
            get
            {
                return _CrDateTime;
            }
            set
            {
                _CrDateTime = value;
            }
        }



        #endregion
        //-----------------------------------------------------------
        #region Method
        private List<Users> Init(SqlCommand cmd)
        {
            SqlConnection con = db.GetConnection();
            cmd.Connection = con;
            List<Users> l_Users = new List<Users>();
            try
            {
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(reader);
                while (smartReader.Read())
                {
                    Users m_Users = new Users();
                    m_Users.UserId = smartReader.GetInt32("UserId");
                    m_Users.UserName = smartReader.GetString("UserName");
                    m_Users.Password = smartReader.GetString("Password");
                    m_Users.FullName = smartReader.GetString("FullName");
                    m_Users.Email = smartReader.GetString("Email");
                    m_Users.Address = smartReader.GetString("Address");
                    m_Users.Mobile = smartReader.GetString("Mobile");
                    m_Users.GenderId = smartReader.GetByte("GenderId");
                    m_Users.UserStatusId = smartReader.GetByte("UserStatusId");
                    m_Users.UserTypeId = smartReader.GetByte("UserTypeId");
                    m_Users.DefaultActionId = smartReader.GetInt16("DefaultActionId");
                    m_Users.BirthDay = smartReader.GetDateTime("BirthDay");
                    m_Users.CrDateTime = smartReader.GetDateTime("CrDateTime");
                    l_Users.Add(m_Users);
                }
                reader.Close();
                return l_Users;
            }
            catch (SqlException err)
            {
                throw new ApplicationException("Data error: " + err.Message);
            }
            finally
            {
                db.closeConnection(con);
            }
        }
        //-----------------------------------------------------------
        public int Insert(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                string proc = "Users_Insert";
                CommandHelper db = new CommandHelper();
                DbCommand cmd = null;
                DbParameter[] ps = new DbParameter[5];

                ps[0] = db.CreateParameter("@UserName", this.UserName, DbType.String);
                ps[1] = db.CreateParameter("@Password", this.Password, DbType.String);
                ps[2] = db.CreateParameter("@Fullname", this.FullName, DbType.String);
                ps[3] = db.CreateParameter("@Email", this.Email, DbType.String);
                ps[4] = db.CreateParameter("@UserId", this.UserId, DbType.Int32);

                return Convert.ToInt32(db.RunProc(proc, ref cmd, ps));

                //SqlCommand sqlCommand = new SqlCommand("Users_Insert");
                //sqlCommand.CommandType = CommandType.StoredProcedure;
                //sqlCommand.Parameters.Add(new SqlParameter("@UserName", this.UserName));
                //sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
                //sqlCommand.Parameters.Add(new SqlParameter("@Fullname", this.FullName));
                //sqlCommand.Parameters.Add(new SqlParameter("@Email", this.Email));
                //sqlCommand.Parameters.Add(new SqlParameter("@Address", this.Address));
                //sqlCommand.Parameters.Add(new SqlParameter("@Mobile", this.Mobile));
                //sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", this.UserStatusId));
                //sqlCommand.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
                //sqlCommand.Parameters.Add(new SqlParameter("@DefaultActionId", this.DefaultActionId));
                //sqlCommand.Parameters.Add(BirthDay == DateTime.MinValue
                //    ? new SqlParameter("@Birthday", DBNull.Value)
                //    : new SqlParameter("@Birthday", this.BirthDay));
                //sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", this.UserTypeId));
                //sqlCommand.Parameters.Add("@UserId", SqlDbType.Int).Direction = ParameterDirection.Output;
                //sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                //sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                //this.db.ExecuteSQL(sqlCommand);
                //this.UserId = Convert.ToInt32(sqlCommand.Parameters["@UserId"].Value ?? "0");
                //sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value ?? "0");
                //retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value ?? "0");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //--------------------------------------------------------------
        public byte Update(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_Update") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@Username", this.UserName));
                sqlCommand.Parameters.Add(new SqlParameter("@Password", this.Password));
                sqlCommand.Parameters.Add(new SqlParameter("@Fullname", this.FullName));
                sqlCommand.Parameters.Add(new SqlParameter("@Email", this.Email));
                sqlCommand.Parameters.Add(new SqlParameter("@Address", this.Address));
                sqlCommand.Parameters.Add(new SqlParameter("@Mobile", this.Mobile));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", this.UserStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
                sqlCommand.Parameters.Add(new SqlParameter("@DefaultActionId", this.DefaultActionId));
                sqlCommand.Parameters.Add(BirthDay == DateTime.MinValue
                    ? new SqlParameter("@Birthday", DBNull.Value)
                    : new SqlParameter("@Birthday", this.BirthDay));
                sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", this.UserTypeId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                if (sqlCommand.Parameters["@SysMessageId"].Value != DBNull.Value)
                {
                    sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value);
                }
                if (sqlCommand.Parameters["@SysMessageTypeId"].Value != DBNull.Value)
                {
                    retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }
        //-----------------------------------------------------------
        public byte InsertOrUpdate(byte Replicated, int ActUserId, ref int SysMessageId)
        {
            byte RetVal = 0;
            try
            {
                SqlCommand cmd = new SqlCommand("Users_InsertOrUpdate");
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@Replicated", Replicated));
                cmd.Parameters.Add(new SqlParameter("@ActUserId", ActUserId));
                cmd.Parameters.Add(new SqlParameter("@UserName", this.UserName));
                cmd.Parameters.Add(new SqlParameter("@Password", this.Password));
                cmd.Parameters.Add(new SqlParameter("@FullName", this.FullName));
                cmd.Parameters.Add(new SqlParameter("@Address", this.Address));
                cmd.Parameters.Add(new SqlParameter("@Mobile", this.Mobile));
                cmd.Parameters.Add(new SqlParameter("@GenderId", this.GenderId));
                cmd.Parameters.Add(new SqlParameter("@UserStatusId", this.UserStatusId));
                cmd.Parameters.Add(new SqlParameter("@UserTypeId", this.UserTypeId));
                cmd.Parameters.Add(new SqlParameter("@DefaultActionId", this.DefaultActionId));
                cmd.Parameters.Add(BirthDay == DateTime.MinValue
                    ? new SqlParameter("@Birthday", DBNull.Value)
                    : new SqlParameter("@Birthday", this.BirthDay));
                cmd.Parameters.Add(new SqlParameter("@UserId", this.UserId)).Direction = ParameterDirection.InputOutput;
                cmd.Parameters.Add("@SysMessageId", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                db.ExecuteSQL(cmd);
                this.UserId = int.Parse(cmd.Parameters["@UserId"].Value.ToString());
                SysMessageId = Convert.ToInt32((cmd.Parameters["@SysMessageId"].Value == null) ? "0" : cmd.Parameters["@SysMessageId"].Value);
                RetVal = Convert.ToByte((cmd.Parameters["@SysMessageTypeId"].Value == null) ? "0" : cmd.Parameters["@SysMessageTypeId"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RetVal;
        }
        //--------------------------------------------------------------            
        public byte Delete(int actUserId, ref short sysMessageId)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_Delete") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                sqlCommand.Parameters.Add("@SysMessageId", SqlDbType.SmallInt).Direction = ParameterDirection.Output;
                sqlCommand.Parameters.Add("@SysMessageTypeId", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                if (sqlCommand.Parameters["@SysMessageId"].Value != DBNull.Value)
                {
                    sysMessageId = Convert.ToInt16(sqlCommand.Parameters["@SysMessageId"].Value);
                }
                if (sqlCommand.Parameters["@SysMessageTypeId"].Value != DBNull.Value)
                {
                    retVal = Convert.ToByte(sqlCommand.Parameters["@SysMessageTypeId"].Value);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public Users Get(int userId)
        {
            try
            {
                if (userId > 0)
                {
                    string cmdText = "SELECT * FROM Users WHERE (UserId =" + userId + " )";
                    SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                    List<Users> list = this.Init(sqlCommand);
                    if (list.Count >= 1)
                    {
                        return list[0];
                    }
                    return new Users();
                }
                return new Users();
            }
            catch
            {
                return new Users();
            }
        }

        public Users Get(string constr, int userId, List<Users> list)
        {
            Users users = new Users(constr);
            try
            {
                if (userId > 0 && list.Count > 0)
                {
                    int num = 0;
                    while (num < list.Count)
                    {
                        users = list[num];
                        if (userId == users.UserId)
                        {
                            num = list.Count;
                        }
                        else
                        {
                            num++;
                            if (num == list.Count)
                            {
                                users.UserId = -1;
                            }
                        }
                    }
                }
                else
                {
                    users = new Users(constr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return users;
        }

        public Users Get(string userName)
        {
            string cmdText = "SELECT * FROM Users WHERE (username ='" + userName.Trim() + "')";
            try
            {
                SqlCommand sqlCommand = new SqlCommand(cmdText) { CommandType = CommandType.Text };
                List<Users> list = this.Init(sqlCommand);
                if (list.Count >= 1)
                {
                    return list[0];
                }
                return new Users();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //-------------------------------------------------------------- 

        public List<Users> GetPage(string userName, string fullName, string address, string email, string mobile, byte genderId, byte userStatusId, byte userTypeId, string dateFrom, string dateTo, string orderBy, int pageSize, int pageIndex, ref int rowCount)
        {
            List<Users> result = new List<Users>();
            try
            {
                string cmdText = "Users_GetPage";
                SqlCommand sqlCommand = new SqlCommand(cmdText);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.Add(new SqlParameter("@UserName", userName));
                sqlCommand.Parameters.Add(new SqlParameter("@FullName", fullName));
                sqlCommand.Parameters.Add(new SqlParameter("@Address", address));
                sqlCommand.Parameters.Add(new SqlParameter("@Mobile", mobile));
                sqlCommand.Parameters.Add(new SqlParameter("@GenderId", genderId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserStatusId", userStatusId));
                sqlCommand.Parameters.Add(new SqlParameter("@UserTypeId", userTypeId));
                sqlCommand.Parameters.Add(new SqlParameter("@DateFrom", dateFrom));
                sqlCommand.Parameters.Add(new SqlParameter("@DateTo", dateTo));
                sqlCommand.Parameters.Add(new SqlParameter("@OrderBy", orderBy));
                sqlCommand.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                sqlCommand.Parameters.Add(new SqlParameter("@PageNumber", pageIndex));
                sqlCommand.Parameters.Add("@RowCount", SqlDbType.Int).Direction = ParameterDirection.Output;
                result = this.Init(sqlCommand);
                rowCount = Convert.ToInt32(sqlCommand.Parameters["@RowCount"].Value);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        //--------------------------------------------------------------
        public static string Static_GetDisplayString(int UserId)
        {
            string RetVal = "";
            Users m_Users = new Users();
            m_Users = m_Users.Get(UserId);
            RetVal = m_Users.UserName;
            return RetVal;
        }

        public List<Users> GetAll()
        {
            List<Users> result = new List<Users>();
            try
            {
                string cmdText = "SELECT * FROM Users";
                result = this.Init(new SqlCommand(cmdText)
                {
                    CommandType = CommandType.Text
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }
        //--------------------------------------------------------------
        public UserLoginResult Login(string userName, string password)
        {
            UserLoginResult retVal = new UserLoginResult();
            try
            {
                SqlCommand cmd = new SqlCommand("User_Login") { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.Add(new SqlParameter("@UserName", StringUtil.InjectionString(userName)));
                cmd.Parameters.Add(new SqlParameter("@Password", StringUtil.InjectionString(password)));
                cmd.Parameters.Add("@LoginStatus", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@LoginMessage", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;
                List<Users> listUsers = Init(cmd);
                retVal.User = listUsers.Count == 0 ? new Users() : listUsers[0];
                if (cmd.Parameters["@LoginStatus"].Value != DBNull.Value) retVal.ActionStatus = cmd.Parameters["@LoginStatus"].Value.ToString();
                if (cmd.Parameters["@LoginMessage"].Value != DBNull.Value) retVal.ActionMessage = cmd.Parameters["@LoginMessage"].Value.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public bool HasPriv(int userId, string url)
        {
            byte retVal = 0;
            try
            {
                SqlCommand sqlCommand = new SqlCommand("Users_HasPriv") { CommandType = CommandType.StoredProcedure };
                sqlCommand.Parameters.Add(new SqlParameter("@url", url));
                sqlCommand.Parameters.Add(new SqlParameter("@UserId", userId));
                sqlCommand.Parameters.Add("@HasPriv", SqlDbType.TinyInt).Direction = ParameterDirection.Output;
                this.db.ExecuteSQL(sqlCommand);
                retVal = (byte)sqlCommand.Parameters["@HasPriv"].Value;
            }
            catch
            {
                retVal = 0;
            }
            return retVal == 1;
        }

        public static Users Static_Get(int UserId, List<Users> l_Users)
        {
            Users RetVal;
            RetVal = l_Users.Find(x => x.UserId == UserId);
            if (RetVal == null)
                RetVal = new Users();
            return RetVal;
        }
        #endregion
    }
}

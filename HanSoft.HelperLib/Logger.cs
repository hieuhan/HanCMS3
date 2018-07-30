using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HanSoft.HelperLib
{
    public sealed class Logger
    {

        private static Logger _instance;
        private ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private readonly FileInfo _logFileInfo;
        private readonly long _maxLogFileSize = 0;
        private const string _strLineBreak = "\n========================\n";
        private const string _strLineBreakCustom = "\n*********************************\n\n\n\n";
        private const string _strLineBreakEnd = "\n----------------------------------------------------------\n\n\n";
        private readonly string _strLogFilePath;

        public static Logger InstanceInfo()
        {
            return _instance ?? (_instance = new Logger(string.Concat(AppDomain.CurrentDomain.BaseDirectory , "Logs\\", LogLevel.Info,"_", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss.fff"), ".log") , 0));
        }

        public static Logger InstanceError()
        {
            return _instance ?? (_instance = new Logger(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Logs\\", LogLevel.Error, "_", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss.fff"), ".log"), 0));
        }

        public static Logger InstanceWarning()
        {
            return _instance ?? (_instance = new Logger(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Logs\\", LogLevel.Warning, "_", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm:ss.fff"), ".log"), 0));
        }

        public Logger(string strLogFilePath, long maxLogFileSize)
        {
            _maxLogFileSize = maxLogFileSize;
            _strLogFilePath = strLogFilePath;
            _logFileInfo = new FileInfo(strLogFilePath);
        }


        internal Logger()
        {
            
        }

        private bool CheckLogSize()
        {
            try
            {
                if (_maxLogFileSize != 0)
                {
                    if (_logFileInfo.Length > _maxLogFileSize)
                    {
                        File.Delete(_strLogFilePath);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Write(Exception ex, string userdetails = null)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                CheckLogSize();
                if (File.Exists(_strLogFilePath))
                {
                    File.AppendAllText(_strLogFilePath, DateTime.UtcNow.ToString()
                                                        + " : Exception :"
                                                        + ex.Message + "\n"
                                                        + "Inner Exception : " + _strLineBreak
                                                        + ex.InnerException + "\n"
                                                        + "Stack Trace :" + _strLineBreak
                                                        + ex.StackTrace + "\n"
                                                        + "Date:" + _strLineBreak
                                                        + DateTime.Now.ToString() + "\n"
                                                        + " UserDetails :" + userdetails
                                                        + "Source : " + _strLineBreak
                                                        + ex.Source + _strLineBreakEnd);
                    return true;
                }
                File.WriteAllText(_strLogFilePath, DateTime.UtcNow.ToString()
                                                   + " : Exception :" + _strLineBreak
                                                   + ex.Message + "\n"
                                                   + "Inner Exception :" + _strLineBreak
                                                   + ex.InnerException + "\n"
                                                   + "Stack Trace :" + _strLineBreak
                                                   + ex.StackTrace + "\n"
                                                   + "Date:" + _strLineBreak
                                                   + DateTime.Now.ToString() + "\n"
                                                   + " UserDetails :" + userdetails
                                                   + "Source :" + _strLineBreak
                                                   + ex.Source + _strLineBreakEnd);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public bool Write(string strMessage, string userdetails = null)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                if (File.Exists(_strLogFilePath))
                {
                    File.AppendAllText(_strLogFilePath, _strLineBreak
                                                        + DateTime.UtcNow.ToString()
                                                        + "; UserDetails :" + userdetails
                                                        + " : " + strMessage + _strLineBreakCustom);
                    return true;
                }
                File.WriteAllText(_strLogFilePath, _strLineBreak
                                                   + DateTime.UtcNow.ToString()
                                                   + "; UserDetails :" + userdetails
                                                   + " : " + strMessage + _strLineBreakCustom);
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }

        public enum LogLevel
        {
            Warning,
            Error,
            Info
        }
    }
}

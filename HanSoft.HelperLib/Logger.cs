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
        private ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();
        private static FileInfo _logFileInfo;
        private static DirectoryInfo LogDirectory { get { return _logFileInfo?.Directory; } }
        private readonly long _maxLogFileSize = 0;
        private const string StrLineBreakCustom = "\r\n************************************************************\r\n";
        private const string StrLineBreak = "\r\n----------------------------------------------------------\r\n";
        private const string StrLineBreakEnd = "\r\n==========================================================\r\n";
        private readonly string _strLogFilePath;


        //public static Logger Instance;

        public static Logger Instance;

        public static Logger InstanceInfo
        {
            get
            {
                return Instance ?? (Instance = new Logger(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Logs\\", LogLevel.Info, "_", DateTime.UtcNow.ToString("ddMMyyyyHHmmssfff"), ".log"), 0));
            }
        }

        public static Logger InstanceWarning
        {
            get
            {
                return Instance ?? (Instance = new Logger(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Logs\\", LogLevel.Warning, "_", DateTime.UtcNow.ToString("ddMMyyyyHHmmssfff"), ".log"), 0));
            }
        }
        public static Logger InstanceError
        {
            get
            {
                return Instance ?? (Instance = new Logger(string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Logs\\", LogLevel.Error, "_", DateTime.UtcNow.ToString("ddMMyyyyHHmmssfff"), ".log"), 0));
            }
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

        private static void VerifyTargetDirectory()
        {
            if (LogDirectory != null)
            {
                LogDirectory.Refresh();
                if (!LogDirectory.Exists)
                    LogDirectory.Create();
            }
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
                    File.AppendAllText(_strLogFilePath, "Log time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\r\n"
                                                        + "User :" + userdetails + StrLineBreak
                                                        + "Exception :" + "\r\n"
                                                        + ex.Message + StrLineBreak
                                                        + "Inner Exception : " + "\r\n"
                                                        + ex.InnerException + StrLineBreak
                                                        + "Stack Trace :" + "\r\n"
                                                        + ex.StackTrace + StrLineBreak
                                                        + "Source : " + "\r\n"
                                                        + ex.Source + StrLineBreakEnd);
                    return true;
                }
                VerifyTargetDirectory();
                File.WriteAllText(_strLogFilePath, "Log time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\r\n"
                                                   + "User :" + userdetails + StrLineBreak
                                                   + "Exception :" + "\r\n"
                                                   + ex.Message + StrLineBreak
                                                   + "Inner Exception :" + "\r\n"
                                                   + ex.InnerException + StrLineBreak
                                                   + "Stack Trace :" + "\r\n"
                                                   + ex.StackTrace + StrLineBreak
                                                   + "Source :" + "\r\n"
                                                   + ex.Source + StrLineBreakEnd);
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

        public bool Write(string logMessage, string userDetails = null)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                if (File.Exists(_strLogFilePath))
                {
                    File.AppendAllText(_strLogFilePath, "Log time: " +
                                                        DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\r\n"
                                                        + "User :" + userDetails + "\r\n"
                                                        + "Message : " + logMessage + StrLineBreakEnd);
                    return true;
                }

                VerifyTargetDirectory();

                File.WriteAllText(_strLogFilePath, 
                                                   "Log time: " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") + "\r\n"
                                                   + "User :" + userDetails + "\r\n"
                                                   + "Message : " + logMessage + StrLineBreakEnd);
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

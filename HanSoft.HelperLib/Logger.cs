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
        private static DirectoryInfo _logDirectory { get { return _logFileInfo?.Directory; } }
        private readonly long _maxLogFileSize = 0;
        private const string _strLineBreak = "\n========================\n";
        private const string _strLineBreakCustom = "\n*********************************\n\n\n\n";
        private const string _strLineBreakEnd = "\n----------------------------------------------------------\n\n\n";
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
            if (_logDirectory != null)
            {
                _logDirectory.Refresh();
                if (!_logDirectory.Exists)
                    _logDirectory.Create();
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
                VerifyTargetDirectory();
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

        public bool Write(string logMessage, string userDetails = null)
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                if (File.Exists(_strLogFilePath))
                {
                    File.AppendAllText(_strLogFilePath, _strLineBreak
                                                        + DateTime.UtcNow.ToString()
                                                        + "; UserDetails :" + userDetails
                                                        + " : " + logMessage + _strLineBreakCustom);
                    return true;
                }

                VerifyTargetDirectory();

                File.WriteAllText(_strLogFilePath, _strLineBreak
                                                   + DateTime.UtcNow.ToString()
                                                   + "; UserDetails :" + userDetails
                                                   + " : " + logMessage + _strLineBreakCustom);
                return true;
            }
            catch(Exception e)
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

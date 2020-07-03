/********************************************************
 * FileName:    Debuger.cs
 * Description: 游戏基础架构---基础库---日志系统      
 ********************************************************/
using System;
using System.IO;
using System.Diagnostics;
using System.Text;
using Object = System.Object;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharpLogSystem
{
    public static class Debuger
    {
        private static bool EnableLog;
        private static bool EnableTime = true;
        private static bool EnableSave = true;
        private static bool EnableStack = false;
        private static string LogFileDir = "";
        private static string LogFileName = "";
        public readonly static string Prefix = ">>";
        private static StreamWriter LogFileWriter = null;
        private static IDebugerConsole m_console;

        public static void Init(bool enableLog, bool enableTime,bool enableSave, 
                                bool enableStack, string logFileDir, IDebugerConsole console)
        {
            EnableLog = enableLog;
            EnableTime = enableTime;
            EnableSave = enableSave;
            EnableStack = enableStack;
            LogFileDir = logFileDir;
            m_console = console;

            if (string.IsNullOrEmpty(LogFileDir))
            {
                string path = System.AppDomain.CurrentDomain.BaseDirectory;
                LogFileDir = path + "/DebugerLog/";
            }
            else
            {
                LogFileDir = LogFileDir.Replace('\\', '/');
                if (!LogFileDir.EndsWith("/")) LogFileDir += "/";
            }
            LogLogHead();
        }

        private static void LogLogHead()
        {
            DateTime now = DateTime.Now;
            string timeStr = now.ToString("HH:mm:ss.fff") + " ";

            Internal_Log("================================================================================");
            Internal_Log("                              CSharp LogSystem                                  ");
            Internal_Log("————————————————————————————————————————");
            Internal_Log("Time:\t" + timeStr);
            Internal_Log("Path:\t" + LogFileDir);
            Internal_Log("================================================================================");
        }

        private static void Internal_Log(string msg)
        {
            if (Debuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                msg = now.ToString("HH:mm:ss.fff") + " " + msg;
            }

            if (m_console == null) m_console = new NativeLogConsole();

            m_console.Log(msg);

            LogToFile("[I]" + msg);
        }

        private static void Internal_LogWarning(string msg)
        {
            if (Debuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                msg = now.ToString("HH:mm:ss.fff") + " " + msg;
            }
            if (m_console == null) m_console = new NativeLogConsole();

            m_console.LogWarning(msg);

            LogToFile("[W]" + msg);
        }

        private static void Internal_LogError(string msg)
        {
            if (Debuger.EnableTime)
            {
                DateTime now = DateTime.Now;
                msg = now.ToString("HH:mm:ss.fff") + " " + msg;
            }

            if (m_console == null) m_console = new NativeLogConsole();

            m_console.LogError(msg);

            LogToFile("[E]" + msg, true);
        }

        public static void Log(string message = "")
        {
            if (!Debuger.EnableLog) return;
            message = GetLogText(GetLogCaller(true), message);
            Internal_Log(Prefix + message);
        }

        public static void Log(string format, params object[] args)
        {
            if (!Debuger.EnableLog) return;
            string message = GetLogText(GetLogCaller(true), string.Format(format, args));
            Internal_Log(Prefix + message);
        }

        public static void LogError(string format, params object[] args)
        {
            string message = GetLogText(GetLogCaller(true), string.Format(format, args));
            Internal_LogError(Prefix + message);
        }

        public static void LogWarning(string format, params object[] args)
        {
            string message = GetLogText(GetLogCaller(true), string.Format(format, args));
            Internal_LogWarning(Prefix + message);
        }

        #region 工具函数

        private static Assembly ms_Assembly;

        private static string GetLogCaller(bool bIncludeClassName = false)
        {
            StackTrace st = new StackTrace(2, false);
            if (st != null)
            {
                if (null == ms_Assembly)
                {
                    ms_Assembly = typeof(Debuger).Assembly;
                }

                int currStackFrameIndex = 0;
                while (currStackFrameIndex < st.FrameCount)
                {
                    StackFrame oneSf = st.GetFrame(currStackFrameIndex);
                    MethodBase oneMethod = oneSf.GetMethod();

                    if (oneMethod.Module.Assembly != ms_Assembly)
                    {
                        if (bIncludeClassName)
                        {
                            return oneMethod.DeclaringType.Name + "::" + oneMethod.Name;
                        }
                        else
                        {
                            return oneMethod.Name;
                        }
                    }
                    currStackFrameIndex++;
                }
            }
            return "";
        }

        private static string GetLogText(string caller, string message)
        {
            return caller + "() " + message;
        }

        internal static string CheckLogFileDir()
        {
            if (string.IsNullOrEmpty(LogFileDir))
            {
                Internal_LogError("Debuger::CheckLogFileDir() LogFileDir is NULL!");
                return "";
            }

            try
            {
                if (!Directory.Exists(LogFileDir))
                {
                    Directory.CreateDirectory(LogFileDir);
                }
            }
            catch (Exception e)
            {
                Internal_LogError("Debuger::CheckLogFileDir() " + e.Message + e.StackTrace);
                return "";
            }

            return LogFileDir;
        }

        internal static string GenLogFileName()
        {
            DateTime now = DateTime.Now;
            string filename = now.GetDateTimeFormats('s')[0].ToString();//2005-11-05T14:06:25
            filename = filename.Replace("-", "_");
            filename = filename.Replace(":", "_");
            filename = filename.Replace(" ", "");
            filename += ".log";
            return filename;
        }

        private static void LogToFile(string message, bool EnableStack = false)
        {
            if (!EnableSave) return;

            if (LogFileWriter == null)
            {
                LogFileName = GenLogFileName();
                LogFileDir = CheckLogFileDir();
                if (string.IsNullOrEmpty(LogFileDir)) return;

                string fullpath = LogFileDir + LogFileName;
                try
                {
                    LogFileWriter = File.AppendText(fullpath);
                    LogFileWriter.AutoFlush = true;
                }
                catch (Exception e)
                {
                    LogFileWriter = null;
                    Internal_LogError("Debuger::LogToFile() " + e.Message + e.StackTrace);
                    return;
                }
            }

            if (LogFileWriter != null)
            {
                try
                {
                    LogFileWriter.WriteLine(message);
                    if ((EnableStack || Debuger.EnableStack))
                    {
                        StackTrace st = new StackTrace(2, false);
                        LogFileWriter.WriteLine(st.ToString());
                    }
                }
                catch (Exception)
                {
                    return;
                }
            }
        }

        #endregion
    }
}

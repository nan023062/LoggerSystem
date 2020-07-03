using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace CSharpLogSystem
{
    public static class DebugerExtension
    {
        public static void Log(this object obj, string message = "")
        {
            Debuger.Log(message);
        }

        public static void Log(this object obj, string format, params object[] args)
        {
            Debuger.Log(format, args);
        }

        public static void LogError(this object obj, string format, params object[] args)
        {
            Debuger.LogError(format, args);
        }

        public static void LogWarning(this object obj, string format, params object[] args)
        {
            Debuger.LogWarning(format, args);
        }

        public static void Log(this IDebugerTag obj, string message = "")
        {
            Debuger.Log(obj.Log_Tag + message);
        }

        public static void Log(this IDebugerTag obj, string format, params object[] args)
        {
            Debuger.Log(obj.Log_Tag + format, args);
        }

        public static void LogError(this IDebugerTag obj, string format, params object[] args)
        {
            Debuger.LogError(obj.Log_Tag + format, args);
        }

        public static void LogWarning(this IDebugerTag obj, string format, params object[] args)
        {
            Debuger.LogWarning(obj.Log_Tag + format, args);
        }

        #region Object 2 ListString 

        /// <summary>
        /// 将容器序列化成字符串
        /// 格式：{a, b, c}
        /// </summary>
        public static string ListToString<T>(this IEnumerable<T> source)
        {
            if (source == null)
            {
                return "null";
            }

            if (source.Count() == 0)
            {
                return "[]";
            }

            if (source.Count() == 1)
            {
                return "[" + source.First() + "]";
            }

            var s = "";

            s += source.ButFirst().Aggregate(s, (res, x) => res + ", " + x.ToListString());
            s = "[" + source.First().ToListString() + s + "]";

            return s;
        }


        /// <summary>
        /// 将容器序列化成字符串
        /// </summary>
        public static string ToListString(this object obj)
        {
            if (obj is string)
            {
                return obj.ToString();
            }
            else
            {
                var objAsList = obj as IEnumerable;
                return objAsList == null ? obj.ToString() : objAsList.Cast<object>().ListToString();
            }
        }

        public static IEnumerable<T> ButFirst<T>(this IEnumerable<T> source)
        {
            return source.Skip(1);
        }

        #endregion 
    }
}

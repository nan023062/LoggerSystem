using System;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

namespace CSharpLogSystem
{
    public class UnityDebugerConsole : IDebugerConsole
    {
        private object[] mTempArgs = new object[] { "" };
        private MethodInfo m_miLog;
        private MethodInfo m_miLogWarning;
        private MethodInfo m_miLogError;

        public UnityDebugerConsole()
        {
            Type type = Type.GetType("UnityEngine.Debug, UnityEngine");
            m_miLog = type.GetMethod("Log", new Type[] { typeof(object) });
            m_miLogWarning = type.GetMethod("LogWarning", new Type[] { typeof(object) });
            m_miLogError = type.GetMethod("LogError", new Type[] { typeof(object) });
        }

        public void Log(string msg)
        {
            mTempArgs[0] = msg;
            m_miLog.Invoke(null, mTempArgs);
        }

        public void LogWarning(string msg)
        {
            mTempArgs[0] = msg;
            m_miLogWarning.Invoke(null, mTempArgs);
        }

        public void LogError(string msg)
        {
            mTempArgs[0] = msg;
            m_miLogError.Invoke(null, mTempArgs);
        }
    }
}

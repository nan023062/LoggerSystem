using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSharpLogSystem
{
    public interface IDebugerConsole
    {
        void Log(string msg);
        void LogWarning(string msg);
        void LogError(string msg);
    }
}

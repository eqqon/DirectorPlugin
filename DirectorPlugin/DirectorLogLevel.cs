using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DirectorPlugin
{
    public enum DirectorLogLevel
    {
        DEBUG = 30000,
        INFO = 40000,
        WARN = 60000,
        ERROR = 70000,
        FATAL = 110000,
        OFF = int.MaxValue,
    }
}

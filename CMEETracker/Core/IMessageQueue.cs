using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMEETracker.Core
{
    internal interface IMessageQueue
    {
        void Connect();
        void Disconnect();
        void Open();
        void Close();
        void Put();
        void Get();
    }
}

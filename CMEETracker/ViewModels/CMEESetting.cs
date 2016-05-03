using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMEETracker.ViewModels
{
    public class CMEESetting
    {
        public string QueueName { get; set; }
        public string IPAddress { get; set; }
        public int PortNumber { get; set; }
        public int McmqMaxSize { get; set; }
        public int McmqMaxCount { get; set; }
        public int McmqTimeOut { get; set; }

        public CMEESetting()
        {
            McmqTimeOut = 3 * 10000;
        }
    }
}

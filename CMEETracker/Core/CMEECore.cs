using CMEEAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMEETracker.Core
{
    public class CMEECore //: IMessageQueue
    {
        public readonly string RETURN_CODE_SUCCESS = "0000";  // FROM mcmq的programming code
        private cmeeAPI _mcmqApi;
        public bool IsConnected { get; protected set; }
        public bool IsOpened { get; protected set; }

        private string QueueName;
        private string IPAddress;
        private int PortNumber;
        private int McmqMaxSize;
        private int McmqMaxCount;
        private int McmqTimeOut;

        public CMEECore()
        {
            
        }

        public void Init(string queueName, string ip, int port, int maxSize, int maxCount, int timeOut)
        {
            //_mcmqApi = new cmeeAPI
            //{
            //    //queueHandle = "",
            //    //queueDescription = "",
            //    //maxCount = 0,
            //    //maxSize = 0,
            //};
            _mcmqApi = new cmeeAPI();
            QueueName = queueName;
            IPAddress = ip;
            PortNumber = port;
            McmqMaxSize = maxSize;
            McmqMaxCount = maxCount;
            McmqTimeOut = timeOut;
        }

        public void Connect()
        {
            string returnCode = _mcmqApi.connect(IPAddress, PortNumber, false);

            if (returnCode.Equals(RETURN_CODE_SUCCESS, StringComparison.OrdinalIgnoreCase))
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
                throw new Exception("Connect Error Code: " + returnCode);
            }
        }

        public void Disconnect()
        {
            string returnCode = _mcmqApi.disconnect();

            if (returnCode.Equals(RETURN_CODE_SUCCESS, StringComparison.OrdinalIgnoreCase))
            {
                IsConnected = false;
            }
            else
            {
                IsConnected = true;
                throw new Exception("Disconnect Error Code: " + returnCode);
            }
        }

        public void Open()
        {
            string returnCode = _mcmqApi.openQueue(QueueName, McmqMaxSize, McmqMaxCount,
                500000, 2500, cmeeAPI.cmeeQueueType.NonPersistent, "Event+的接收Queue");

            if (returnCode.Equals(RETURN_CODE_SUCCESS, StringComparison.OrdinalIgnoreCase))
            {
                IsOpened = true;
            }
            else
            {
                IsOpened = true;
                throw new Exception("Open Error Code: " + returnCode);
            }
        }

        public void Close()
        {
            string returnCode = _mcmqApi.closeQueue(_mcmqApi.queueHandle);

            if (returnCode.Equals(RETURN_CODE_SUCCESS, StringComparison.OrdinalIgnoreCase))
            {
                IsOpened = false;
            }
            else
            {
                IsOpened = true;
                throw new Exception("Close Error Code: " + returnCode);
            }
        }

        // void SendPrimary(string m_ChannelId, string m_MessageId, string m_TargetModule, KXmlItem m_Message);
        public void Put(string queueName, string message)
        {
            string returnCode = _mcmqApi.putQueue(queueName, Encoding.UTF8.GetBytes(message), QueueName, _mcmqApi.getCorrelationID, cmeeAPI.cmeeMsgEncrypted.NONE);
            
            if (returnCode.Equals(RETURN_CODE_SUCCESS, StringComparison.OrdinalIgnoreCase) == false)
            {
                throw new Exception("Put Error Code: " + returnCode);
            }
        }

        public string Get()
        {
            string returnCode = _mcmqApi.getQueue(_mcmqApi.queueHandle, McmqTimeOut, cmeeAPI.cmeeMsgEncrypted.NONE);

            var a = _mcmqApi.getMessage;
            var b = _mcmqApi.getCorrelationID;
            var c = _mcmqApi.getReplyQueue;
            var d = _mcmqApi.getCorrelationID;

            if (returnCode.Equals(RETURN_CODE_SUCCESS, StringComparison.OrdinalIgnoreCase) == false)
            {
                throw new Exception("Get Error Code: " + returnCode);
            }

            return Encoding.UTF8.GetString(_mcmqApi.getMessage).Trim();
        }
    }
}

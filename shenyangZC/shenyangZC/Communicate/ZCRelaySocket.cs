using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace shenyangZC.Communicate
{
    class ZCRelaySocket
    {
        public Socket socketMain;
        private IPAddress HostIP;
        private int HostPort;
        public bool RunningFlag = true;
        byte[] ReceiveDataArray;
        IPAddress ZCIP;
        int ZCPort;
        Thread ReceiveThread;


        public ZCRelaySocket()
        {
            ReceiveDataArray = new byte[200];
            IPList ZCiplist = IPConfigure.IPList.Find((IPList iplist) => { return iplist.DeviceName == "ZCToATSAndCI"; });
            ZCIP = ZCiplist.IP;
            ZCPort = ZCiplist.Port;
            IPList ipandport = IPConfigure.IPList.Find((IPList iplist) => { return iplist.DeviceName == "ZCToATP"; });
            HostIP = ipandport.IP;
            HostPort = ipandport.Port;

            IPEndPoint IPEP = new IPEndPoint(HostIP, HostPort);
            EndPoint EP = (EndPoint)IPEP;
            socketMain = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketMain.Bind(EP);
            CancelReceiveException();
        }

        public void Start()
        {
            RunningFlag = true;
            ReceiveThread = new Thread(ListenControlData);
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();
        }

        public void CancelReceiveException()
        {
            uint IOC_IN = 0x80000000; uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            socketMain.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
        }

        public void ListenControlData()
        {
            IPEndPoint ipEP = new IPEndPoint(HostIP, HostPort);
            EndPoint remoteEP = (EndPoint)ipEP;

            while (RunningFlag)
            {
                int nRecv = socketMain.ReceiveFrom(ReceiveDataArray, ref remoteEP);
                Handle(ReceiveDataArray, nRecv, remoteEP);
            }
        }

        private void Handle(byte[] receiveDataArray, int nRecv, EndPoint remoteEP)
        {
            if (receiveDataArray != null)
            {
                if ((remoteEP as IPEndPoint).Address.ToString() == ZCIP.ToString() && (remoteEP as IPEndPoint).Port == ZCPort)
                {
                    IPList list = IPConfigure.IPList.Find((IPList iplist) => { return iplist.DeviceID == receiveDataArray[0]; });
                    SendControlData(receiveDataArray, nRecv - 1,1, list.IP, list.Port);
                }
                else
                {
                    SendControlData(receiveDataArray, nRecv, 0, ZCIP, ZCPort);
                }
            }
            Array.Clear(receiveDataArray, 0, receiveDataArray.Length);
        }

        public void SendControlData(byte[] sendControlPacket, int packetLength, int offset, IPAddress DIP, int Dport)
        {
            IPEndPoint ipep = new IPEndPoint(DIP, Dport);
            socketMain.SendTo(sendControlPacket, offset, packetLength, SocketFlags.None, ipep);
        }
    }
}

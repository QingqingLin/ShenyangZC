using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace shenyangZC
{
    public class ZCSocket
    {
        public static Socket socketMain;
        private IPAddress HostIP;
        private int HostPort;
        public bool RunningFlag;
        byte[] ReceiveDataArray;
        public Thread ReceiveThread;

        public ZCSocket()
        {
            ReceiveDataArray = new byte[200];
            SetHostIPAndPort();

            IPEndPoint IPEP = new IPEndPoint(IPAddress.Any, HostPort);
            EndPoint EP = (EndPoint)IPEP;
            socketMain = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socketMain.Bind(EP);
            CancelReceiveException();
        }

        public void CancelReceiveException()
        {
            uint IOC_IN = 0x80000000; uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            socketMain.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
        }

        public void SetHostIPAndPort()
        {
            foreach (var item in IPConfigure.IPList)
            {
                if (item.DeviceName == "ZCToATSAndCI")
                {
                    HostIP = item.IP;
                    HostPort = item.Port;
                }
            }
        }

        public void Start()
        {
            RunningFlag = true;
            ReceiveThread = new Thread(ListenControlData);
            ReceiveThread.IsBackground = true;
            ReceiveThread.Start();
        }

        public void ListenControlData()
        {
            IPEndPoint ipEP = new IPEndPoint(IPAddress.Any, HostPort);
            EndPoint EP = (EndPoint)ipEP;

            while (RunningFlag)
            {
                int nRecv = socketMain.ReceiveFrom(ReceiveDataArray, ref EP);
                SaveData(ReceiveDataArray);
            }
        }
        
        private void SaveData(byte[] receiveDataArray)
        {
            if (receiveDataArray != null)
            {
                HandleData VOBCorCI = new HandleData(receiveDataArray);
            }
            Array.Clear(receiveDataArray, 0, receiveDataArray.Length);
        }

        public static void SendControlData(byte[] sendControlPacket, int packetLength, IPAddress DIP, int Dport)
        {
            IPEndPoint ipep = new IPEndPoint(DIP, Dport);
            socketMain.SendTo(sendControlPacket, 0, packetLength, SocketFlags.None, ipep);
        }
    }
}

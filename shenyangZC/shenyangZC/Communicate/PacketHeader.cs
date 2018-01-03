using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenyangZC
{
    class PacketHeader
    {
        private UInt16 _cycleNumber;

        public UInt16 CycleNumber
        {
            get { return _cycleNumber; }
            set { _cycleNumber = value; }
        }

        private DataType _type;

        public DataType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private DeviceID _senderID;

        public DeviceID SenderID
        {
            get { return _senderID; }
            set { _senderID = value; }
        }

        private DeviceID _receiveID;

        public DeviceID ReceiveID
        {
            get { return _receiveID; }
            set { _receiveID = value; }
        }

        private UInt16 _dataLength;

        public UInt16 DataLength
        {
            get { return _dataLength; }
            set { _dataLength = value; }
        }
    }
}

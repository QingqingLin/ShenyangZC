using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenyangZC.ATS
{
    class InfoToATS
    {
        private UInt16 trainID_;

        public UInt16 TrainID
        {
            get { return trainID_; }
            set { trainID_ = value; }
        }

        private bool isQuit_;

        public bool IsQuit
        {
            get { return isQuit_; }
            set { isQuit_ = value; }
        }

        private UInt16 offset_;

        public UInt16 Offset
        {
            get { return offset_; }
            set { offset_ = value; }
        }

        private Byte type_;

        public Byte Type
        {
            get { return type_; }
            set { type_ = value; }
        }

        private UInt16 positionID_;

        public UInt16 PositionID
        {
            get { return positionID_; }
            set { positionID_ = value; }
        }

        private Byte direction_;

        public Byte Direction
        {
            get { return direction_; }
            set { direction_ = value; }
        }

        private bool isStop_;

        public bool IsStop
        {
            get { return isStop_; }
            set { isStop_ = value; }
        }

    }
}

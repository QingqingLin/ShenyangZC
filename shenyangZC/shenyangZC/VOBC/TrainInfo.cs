using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC
{
    class TrainInfo
    {
        private UInt16 NIDTrain_;

        public UInt16 NIDTrain
        {
            get { return NIDTrain_; }
            set { NIDTrain_ = value; }
        }

        private NCTrain NCTrain_;

        public NCTrain NcTrain
        {
            get { return NCTrain_; }
            set { NCTrain_ = value; }
        }

        private TrainDir TrainDirection_;

        public TrainDir TrainDirection
        {
            get { return TrainDirection_; }
            set { TrainDirection_ = value; }
        }

        private Device HeadPosition_;

        public Device HeadPosition
        {
            get { return HeadPosition_; }
            set { HeadPosition_ = value; }
        }

        private UInt32 HeadOffset_;

        public UInt32 HeadOffset
        {
            get { return HeadOffset_; }
            set { HeadOffset_ = value; }
        }

        private Device TailPosition_;

        public Device  TailPosition
        {
            get { return TailPosition_; }
            set { TailPosition_ = value; }
        }

        private UInt32 TailOffset_;

        public UInt32 TailOffset
        {
            get { return TailOffset_; }
            set { TailOffset_ = value; }
        }

        private UInt16 speed_;

        public UInt16 Speed
        {
            get { return speed_; }
            set { speed_ = value; }
        }

        private bool PSDoor_;

        public bool PSDoor
        {
            get { return PSDoor_; }
            set { PSDoor_ = value; }
        }


        public bool EqualTo(TrainInfo obj)
        {
            TrainInfo trainInfo = obj as TrainInfo;
            if (trainInfo.NIDTrain_ == this.NIDTrain_ && trainInfo.TrainDirection_ == this.TrainDirection_ && trainInfo.HeadPosition_ == this.HeadPosition_
                && trainInfo.HeadOffset_ == this.HeadOffset_ && trainInfo.TailOffset_ == this.TailOffset_ && trainInfo.TailPosition_ == this.TailPosition_)
            {
                return true;
            }
            return false;
        }
    }
}

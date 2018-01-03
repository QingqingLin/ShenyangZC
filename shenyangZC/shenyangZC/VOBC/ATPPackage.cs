using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenyangZC.VOBC
{
    class ATPPackage
    {
        public Pack ATPPack = new Pack();

        PacketHeader Header_ = new PacketHeader();
        public PacketHeader Header
        {
            get { return Header_; }
            set { Header_ = value; }
        }

        UInt16 NID_ZC_ = 1;
        public UInt16 NID_ZC { set { NID_ZC_ = value; } }

        UInt16 NID_Train_;
        public UInt16 NID_Train { set { NID_Train_ = value; } }

        Byte NC_ZC_;
        public Byte NC_ZC
        {
            get { return NC_ZC_; }
            set { NC_ZC_ = value; }
        }

        Byte NC_StopEnsure_;
        public Byte NC_StopEnsure { set { NC_StopEnsure_ = value; } }

        UInt64 NID_DataBase_;
        public UInt64 NID_DataBase { set { NID_DataBase_ = value; } }

        UInt16 NID_ARButton_;
        public UInt16 NID_ARButton { set { NID_ARButton_ = value; } }

        Byte Q_ARButtonStatus_;
        public Byte Q_ARButtonStatus { set { Q_ARButtonStatus_ = value; } }

        UInt16 NID_LoginZCNext_;
        public Byte NID_LoginZCNext { set { NID_LoginZCNext_ = value; } }

        Byte N_Length_;
        public Byte N_Length
        {
            get { return N_Length_; }
            set { N_Length_ = value; }
        }

        Byte NC_MAEndType_;
        public Byte NC_MAEndType { set { NC_MAEndType_ = value; } }

        Byte D_MAHeadType_;
        public Byte D_MAHeadType { set { D_MAHeadType_ = value; } }

        Byte D_MAHeadId_;
        public Byte D_MAHeadId { set { D_MAHeadId_ = value; } }

        Byte D_MATailType_;
        public Byte D_MATailType { set { D_MATailType_ = value; } }

        Byte D_MATailId_;
        public Byte D_MATailId { set { D_MATailId_ = value; } }

        UInt32 D_MAHeadOff_;
        public UInt32 D_MAHeadOff { set { D_MAHeadOff_ = value; } }

        Byte Q_MAHeadDir_;
        public Byte Q_MAHeadDir { set { Q_MAHeadDir_ = value; } }

        UInt32 D_MATailOff_;
        public UInt32 D_MATailOff
        {
            get { return D_MATailOff_; }
            set { D_MATailOff_ = value; }
        }

        Byte Q_MATailDir_;
        public Byte Q_MATailDir { set { Q_MATailDir_ = value; } }

        Byte N_Obstacle_;
        public Byte N_Obstacle
        {
            get { return N_Obstacle_; }
            set { N_Obstacle_ = value; }
        }

        List<ObstacleInfo> Obstacle_ = new List<ObstacleInfo>();
        public List<ObstacleInfo> Obstacle
        {
            get { return Obstacle_; }
            set { Obstacle_ = value; }
        }

        Byte N_TSR_;
        public Byte N_TSR { set { N_TSR_ = value; } }

        UInt32 Q_ZC_;
        public UInt32 Q_ZC { set { Q_ZC_ = value; } }

        Byte EB_Type_;
        public Byte EB_Type { set { EB_Type_ = value; } }

        Byte EB_DEV_Type_;
        public Byte EB_DEV_Type { set { EB_DEV_Type_ = value; } }

        UInt16 EB_DEV_Name_;
        public UInt16 EB_DEV_Name { set { EB_DEV_Name_ = value; } }

        public void PackATP()
        {
            ATPPack.PackUint16(Header_.CycleNumber);
            ATPPack.PackUint16((UInt16)Header_.Type);
            ATPPack.PackByte((Byte)Header_.SenderID);
            ATPPack.PackByte((Byte)Header_.ReceiveID);
            ATPPack.PackUint16(Header_.DataLength);
            ATPPack.PackUint16(NID_ZC_);
            ATPPack.PackUint16(NID_Train_);
            ATPPack.PackByte(NC_ZC_);
            ATPPack.PackByte(NC_StopEnsure_);
            ATPPack.PackUint64(NID_DataBase_);
            ATPPack.PackUint16(NID_ARButton_);
            ATPPack.PackByte(Q_ARButtonStatus_);
            ATPPack.PackUint16(NID_LoginZCNext_);
            ATPPack.PackByte(N_Length_);
            ATPPack.PackByte(NC_MAEndType_);
            ATPPack.PackByte(D_MAHeadType_);
            ATPPack.PackByte(D_MAHeadId_);
            ATPPack.PackUint32(D_MAHeadOff_);
            ATPPack.PackByte(Q_MAHeadDir_);
            ATPPack.PackByte(D_MATailType_);
            ATPPack.PackByte(D_MATailId_);
            ATPPack.PackUint32(D_MATailOff_);
            ATPPack.PackByte(Q_MATailDir_);
            ATPPack.PackByte(N_Obstacle_);
            foreach (var ObstacleInfo in Obstacle_)
            {
                ATPPack.PackByte(ObstacleInfo.NC_Obstacle);
                ATPPack.PackUint16(ObstacleInfo.NID_Obstacle);
                ATPPack.PackByte(ObstacleInfo.Q_Obstacle_Now);
                ATPPack.PackByte(ObstacleInfo.Q_Obstacle_CI);
            }
            ATPPack.PackByte(N_TSR_);
            ATPPack.PackUint32(Q_ZC_);
            ATPPack.PackByte(EB_Type_);
            ATPPack.PackByte(EB_DEV_Type_);
            ATPPack.PackUint16(EB_DEV_Name_);
        }
    }
}

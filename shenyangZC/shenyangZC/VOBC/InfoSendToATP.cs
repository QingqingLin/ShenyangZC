using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenyangZC.VOBC
{
    class InfoSendToATP
    {
        public SetMA MA;
        public ATPPackage atpPackage;
        TrainInfo trainInfo;
        DeviceID senderID;

        public InfoSendToATP(TrainInfo trainInfo, DeviceID senderID)
        {
            atpPackage = new ATPPackage();
            this.trainInfo = trainInfo;
            this.senderID = senderID;
            SetInfoToVOBC();
        }

        private void SetInfoToVOBC()
        {
            SetVOBCHead();
            SetID();
            SetNCofZC();
            SetMAHead();
            SetMATail();
            SetMAEndType();
            SetMAObstacle();
            SetNumberOfObstacle();
            SetMALength();
            atpPackage.Header.DataLength = Convert.ToUInt16(atpPackage.ATPPack.byteFlag_);
        }

        private void SetMAObstacle()
        {
            atpPackage.Obstacle = new List<ObstacleInfo>();
            MA.SetMAObstacle(atpPackage.Obstacle);
        }

        private void SetMAEndType()
        {
            atpPackage.NC_MAEndType = (Byte)MA.MaEndType;
        }

        private void SetMALength()
        {
            int NumOfBarrier = atpPackage.N_Obstacle;
            atpPackage.N_Length = Convert.ToByte(17 + 5 * NumOfBarrier);
        }

        private void SetNumberOfObstacle()
        {
            atpPackage.N_Obstacle = Convert.ToByte(atpPackage.Obstacle.Count);
        }

        private void SetMATail()
        {
            MA = new SetMA();
            if (MA.DetermineMA(trainInfo))
            {
                atpPackage.D_MATailType = Convert.ToByte(MA.MAEndDevice is Section ? 0x01 : 0x02);
                atpPackage.D_MATailId = Convert.ToByte(MA.MAEndDevice.ID);
                atpPackage.D_MATailOff = MA.MAOffset;
                atpPackage.Q_MATailDir = (Byte)trainInfo.TrainDirection;
            }
        }

        private void SetMAHead()
        {
            atpPackage.D_MAHeadType = Convert.ToByte(trainInfo.HeadPosition is Section ? 0x01 : 0x02);
            atpPackage.D_MAHeadId = Convert.ToByte(trainInfo.HeadPosition.ID);
            atpPackage.D_MAHeadOff = trainInfo.HeadOffset;
            atpPackage.Q_MAHeadDir = (Byte)trainInfo.TrainDirection;
        }

        private void SetNCofZC()
        {
            switch (trainInfo.NcTrain)
            {
                case NCTrain.AskMA:
                    atpPackage.NC_ZC = 0x01;
                    break;
                case NCTrain.SwitchLogOutZC:
                    atpPackage.NC_ZC = 0x02;
                    break;
                case NCTrain.QuitCBTCLogOutZC:
                    atpPackage.NC_ZC = 0x03;
                    break;
                case NCTrain.ReentryLogOutZC:
                    atpPackage.NC_ZC = 0x04;
                    break;
                case NCTrain.ReentryComplete:
                    atpPackage.NC_ZC = 0x05;
                    break;
                default:
                    break;
            }
        }

        private void SetID()
        {
            atpPackage.NID_ZC = 1;
            atpPackage.NID_Train = trainInfo.NIDTrain;
        }

        private void SetVOBCHead()
        {
            atpPackage.Header.Type = DataType.ZCToATP;
            atpPackage.Header.SenderID = DeviceID.ZC;
            atpPackage.Header.ReceiveID = senderID;
        }
    }
}

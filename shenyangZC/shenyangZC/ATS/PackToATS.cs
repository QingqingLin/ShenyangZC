using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZCToATS;

namespace shenyangZC.ATS
{
    class PackToATS
    {
        InfoToATS infoToATS;
        TrainInfo trainInfo;

        public PackToATS(InfoToATS infoToATS, TrainInfo trainInfo)
        {
            this.trainInfo = trainInfo;
            this.infoToATS = infoToATS;
            infoToATS.Direction = (Byte)trainInfo.TrainDirection;
            infoToATS.IsQuit = trainInfo.NcTrain == NCTrain.QuitCBTCLogOutZC;
            infoToATS.IsStop = trainInfo.Speed == 0 ? true : false;
            infoToATS.Offset = trainInfo.HeadOffset;
            infoToATS.PositionID = (UInt16)trainInfo.HeadPosition.ID;
            infoToATS.TrainID = trainInfo.NIDTrain;
            infoToATS.Type = (Byte)(trainInfo.HeadPosition is Section ? SectionOrSwitch.Section : SectionOrSwitch.Switch);
        }
    }
}

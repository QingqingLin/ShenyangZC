using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shenyangZC
{
    public enum DataType : UInt16
    {
        Default = 0,
        CIToZC = 1,
        ZCToCI = 2,
        ATPToZC = 8,
        ZCToATP = 9,
        ATSToZC = 10,
        ZCToATS = 11
    }

    public enum DeviceID : Byte
    {
        CI1 = 1,
        CI2 = 2,
        CI3 = 3,
        CI4 = 4,
        ZC = 5,
        ATP1 = 6,
        ATP2 = 7,
        ATP3 = 8,
        ATP4 = 9,
        ATS = 19
    }

    public enum SectionOrSwitch : Byte
    {
        Section = 0x01,
        Switch = 0x02
    }

    public enum MAEndType : Byte
    {
        CBTCEnd = 0x01,
        ReturnEnd = 0x02,
        ElseEnd = 0x03
    }

    public enum NCTrain : Byte
    {
        /// <summary>
        /// 申请MA
        /// </summary>
        AskMA = 0x01,
        /// <summary>
        /// 切换时注销ZC
        /// </summary>
        SwitchLogOutZC = 0x02,
        /// <summary>
        /// 退出CBTC时注销ZC
        /// </summary>
        QuitCBTCLogOutZC = 0x03,
        /// <summary>
        /// 折返时注销ZC
        /// </summary>
        ReentryLogOutZC = 0x04,
        /// <summary>
        /// 折返换端完成
        /// </summary>
        ReentryComplete = 0x05
    }

    public enum TrainDir : Byte
    {
        Right = 0x55,
        Left = 0xaa
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.CI
{
    class PackToCI
    {
        public Pack CIPacket = new Pack();
        string[] Station = new string[] { "108", "109", "202", "205", "305", "314", "407", "410" };
        HandleCIData CI;

        public PackToCI(DeviceID SenderID, HandleCIData CI)
        {
            this.CI = CI;

            WriteCIHead(SenderID);
            SetLogicState(CI.InSections, CI.InRailSwitchs);
            CIPacket.Skip();

            SetLogicStopInfo(CI.InSections, CI.InRailSwitchs);
            CIPacket.Skip();

            SetSectionARB(CI.InSections, CI.InRailSwitchs);
            CIPacket.Skip();

            SetTrainAccessInfo();
            CIPacket.Skip();

            SetPSDReq();
            CIPacket.Skip();

            SetDataLength();
        }

        private void SetPSDReq()
        {
            foreach (var psd in CI.InPSDoors)
            {
                CIPacket.SetBit((psd as PSDoor).ShouldOpen);
            }
        }

        private void SetSectionARB(List<GraphicElement> Sections, List<GraphicElement> Switchs)
        {
            foreach (var item in Sections)
            {
                if (item is Section)
                {
                    CIPacket.SetBit(false);
                }
            }
            List<string> railswitchs = new List<string>();
            foreach (var item in Switchs)
            {
                if (item is RailSwitch)
                {
                    RailSwitch railswitch = item as RailSwitch;
                    if (!railswitchs.Contains(railswitch.SectionName))
                    {
                        CIPacket.SetBit(false);
                        railswitchs.Add(railswitch.SectionName);
                    }
                }
            }
        }

        private void SetLogicStopInfo(List<线路绘图工具.GraphicElement> Sections, List<线路绘图工具.GraphicElement> Switchs)
        {
            foreach (var item in Sections)
            {
                if (item is Section)
                {
                    if ((item as Section).LogicCount == 1)
                    {
                        CIPacket.SetBit(false);
                    }
                    else
                    {
                        CIPacket.SetBit(false);
                        CIPacket.SetBit(false);
                    }
                }
            }
            List<string> railswitchs = new List<string>();
            foreach (var item in Switchs)
            {
                if (item is RailSwitch)
                {
                    RailSwitch railswitch = item as RailSwitch;
                    if (!railswitchs.Contains(railswitch.SectionName))
                    {
                        CIPacket.SetBit(false);
                        railswitchs.Add(railswitch.SectionName);
                    }
                }
            }
        }

        private void SetDataLength()
        {
            ushort Length = (ushort)CIPacket.byteFlag_;
            CIPacket.byteFlag_ = 6;
            CIPacket.PackUint16(Length);
            CIPacket.byteFlag_ = Length;
        }

        public void WriteCIHead(DeviceID SenderID)
        {
            PacketHeader head = new PacketHeader();
            CIPacket.PackUint16(0);
            CIPacket.PackUint16((UInt16)DataType.ZCToCI);
            CIPacket.PackByte((byte)DeviceID.ZC);
            CIPacket.PackByte((byte)SenderID);
            CIPacket.PackUint16(0);
        }

        private void SetTrainAccessInfo()
        {
            for (int i = CI.startRouteNum; i < CI.startRouteNum + CI.inRoutesNum; i++)
            {
                Device accessDevice = MainWindow.routeList_.Routes[i].IncomingSections[0];
                if (accessDevice is Section)
                {
                    CIPacket.SetBit((accessDevice as Section).HasNonComTrain.Count == 0 ? true : false);
                }
                if (accessDevice is RailSwitch)
                {
                    CIPacket.SetBit((accessDevice as RailSwitch).HasNonComTrain.Count == 0 ? true : false);
                }
            }
        }

        public void SetLogicState(List<线路绘图工具.GraphicElement> Sections, List<线路绘图工具.GraphicElement> Switchs)
        {
            foreach (var item in Sections)
            {
                if (item is Section)
                {
                    if ((item as Section).LogicCount == 1)
                    {
                        bool bitFront = (item as Section).IsFrontLogicOccupy.Count == 0 ? false : true;
                        CIPacket.SetBit(!bitFront);
                    }
                    else
                    {
                        bool bitFront = (item as Section).IsFrontLogicOccupy.Count == 0 ? false : true;
                        CIPacket.SetBit(!bitFront);
                        bool bitLast = (item as Section).IsLastLogicOccupy.Count == 0 ? false : true;
                        CIPacket.SetBit(!bitLast);
                    }
                }
            }
            List<string> railswitchs = new List<string>();
            foreach (var item in Switchs)
            {
                if (item is RailSwitch)
                {
                    RailSwitch railswitch = item as RailSwitch;
                    if (!railswitchs.Contains(railswitch.SectionName))
                    {
                        bool bit = (item as RailSwitch).TrainOccupy.Count != 0 ? false : true;
                        CIPacket.SetBit(bit);
                        railswitchs.Add(railswitch.SectionName);
                    }
                }
            }
        }
    }
}

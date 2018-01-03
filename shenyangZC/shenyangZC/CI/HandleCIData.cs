using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using 线路绘图工具;

namespace shenyangZC.CI
{
    class HandleCIData : Load.Update
    {
        Unpack unpack;
        List<bool> Info = new List<bool>();
        int Num;
        int stationID;
        public int startRouteNum;
        public int inRoutesNum;
        public List<GraphicElement> InRailSwitchs;
        public List<GraphicElement> InSections;
        public List<GraphicElement> InSignals;
        public List<GraphicElement> InPSDoors;

        public HandleCIData(Unpack unpack, DeviceID senderID)
        {
            this.unpack = unpack;
            unpack.SkipBytes(4);
            switch (senderID)
            {
                case DeviceID.CI1:
                    stationID = 0;
                    startRouteNum = 0;
                    inRoutesNum = 26;
                    break;
                case DeviceID.CI2:
                    stationID = 1;
                    startRouteNum = 26;
                    inRoutesNum = 16;
                    break;
                case DeviceID.CI3:
                    startRouteNum = 42;
                    inRoutesNum = 11;
                    stationID = 2;
                    break;
                case DeviceID.CI4:
                    startRouteNum = 53;
                    inRoutesNum = 13;
                    stationID = 3;
                    break;
                default:
                    break;
            }
            FindInDevices();
            UnpackSwitchStatus();
            UnPackSection();            
            int nSignal = UnpackSignal();
            UnPackProtectionSection(nSignal);
            //UnPackEB();
            //UnPackPSDoor();
            UnPackAccessState();
        }

        private void FindInDevices()
        {
            InRailSwitchs = MainWindow.stationElements_.Elements.FindAll((GraphicElement railswitch) =>
            {
                if (railswitch is RailSwitch)
                {
                    return (railswitch as RailSwitch).StationID == stationID;
                }
                return false;
            });

            InSections = MainWindow.stationElements_.Elements.FindAll((GraphicElement section) =>
            {
                if (section is Section)
                {
                    return (section as Section).StationID == stationID;
                }
                return false;
            });

            InSignals = MainWindow.stationElements_.Elements.FindAll((GraphicElement signal) =>
            {
                if (signal is Signal)
                {
                    return (signal as Signal).StationID == stationID;
                }
                return false;
            });

            InPSDoors = MainWindow.stationElements_.Elements.FindAll((GraphicElement psdoor) =>
            {
                if (psdoor is PSDoor)
                {
                    return (psdoor as PSDoor).StationID == stationID;
                }
                return false;
            });
        }

        private void UnPackAccessState()
        {
            Info.Clear();
            for (int i = 0; i < inRoutesNum; i++)
            {
                bool IsAccessOpen = unpack.GetBit();
                Info.Add(IsAccessOpen);
            }
            SetAccessState();
        }

        private void SetAccessState()
        {
            Num = 0;
            for (int i = startRouteNum; i < startRouteNum + inRoutesNum; i++)
            {
                MainWindow.routeList_.Routes[i].RouteOpenState = Info[Num] ? Load.RouteOpenState.NormalOpen : Load.RouteOpenState.NotOpen;
                Num++;
            }
        }

        private void UnPackProtectionSection(int nSignal)
        {
            for (int i = 0; i < nSignal; i++)
            {
                bool InfoOfProtectSection = unpack.GetBit();
            }
            unpack.Skip();
        }

        private int UnpackSignal()
        {
            int nSignal = unpack.GetUint16();
            Info.Clear();
            for (int i = 0; i < nSignal; i++)
            {
                bool isSignalOpen = unpack.GetBit();
                Info.Add(isSignalOpen);
            }
            SetSignalState();
            unpack.Skip();
            return nSignal;
        }

        private void UnPackSection()
        {
            Info.Clear();
            int nSection = unpack.GetUint16();

            for (int i = 0; i < nSection; i++)
            {
                bool isUpward = unpack.GetBit();
                Info.Add(isUpward);
                bool isDownward = unpack.GetBit();
                Info.Add(isDownward);
            }
            SetSectionRunDirection();
            unpack.Skip();

            int nRai = 0;
            List<String> Sectionname = new List<string>();
            foreach (var item in InRailSwitchs)
            {
                if (!Sectionname.Contains((item as RailSwitch).SectionName))
                {
                    Sectionname.Add((item as RailSwitch).SectionName);
                    nRai++;
                }
            }
            int nSec = InSections.Count + nRai;
            Info.Clear();
            for (int i = 0; i < nSec; i++)
            {
                bool isOccupied = unpack.GetBit();
                Info.Add(isOccupied);
            }
            SetSectionAxleState();
            Info.Clear();
            for (int i = 0; i < InPSDoors.Count; i++)
            {
                bool isEB = !unpack.GetBit();
                Info.Add(isEB);
            }
            SetEB();
            Info.Clear();
            for (int i = 0; i < InPSDoors.Count; i++)
            {
                unpack.GetBit();
            }
            for (int i = 0; i < InPSDoors.Count; i++)
            {
                bool isDoorOpen = !unpack.GetBit();
                Info.Add(isDoorOpen);
            }
            SetPSDoor();
            for (int i = 0; i < InPSDoors.Count; i++)
            {
                unpack.GetBit();
            }
            unpack.Skip();

            Info.Clear();
            for (int i = 0; i < nSection; i++)
            {
                bool isRouteLocked = unpack.GetBit();
                Info.Add(isRouteLocked);
            }
            SetAxleSectionAccessLock();
            unpack.Skip();
        }

        private void UnPackPSDoor()
        {
            //int nPSDoor = unpack.GetUint16();
            Info.Clear();
            for (int i = 0; i < InPSDoors.Count; i++)
            {
                bool isPSDoorOpen = unpack.GetBit();
                Info.Add(isPSDoorOpen);
            }
            SetPSDoor();
            unpack.Skip();
        }

        private void UnpackSwitchStatus()
        {
            int nSwitch = unpack.GetUint16();
            Info.Clear();
            for (int i = 0; i < nSwitch; i++)
            {
                bool isNormal = unpack.GetBit();
                Info.Add(isNormal);
                bool isReverse = unpack.GetBit();
                Info.Add(isReverse);
            }
            unpack.Skip();
            SetRailSwithPosition();

            Info.Clear();
            for (int i = 0; i < nSwitch; i++)
            {
                bool isLocked = unpack.GetBit();
                Info.Add(isLocked);
            }
            unpack.Skip();
            SetRailSwithLock();
        }

        private void SetPSDoor()
        {
            Num = 0;
            foreach (var PSDoor in InPSDoors)
            {
                PSDoor psdoor = PSDoor as PSDoor;
                if (psdoor.IsOpen != Info[Num])
                {
                    psdoor.IsOpen = Info[Num];
                    UpdateDevice(psdoor);
                }
                Num++;
            }
        }

        private void SetEB()
        {
            Num = 0;
            foreach (var PSDoor in InPSDoors)
            {
                PSDoor psdoor = PSDoor as PSDoor;
                if (psdoor.IsEB != Info[Num])
                {
                    psdoor.IsEB = Info[Num];
                    //UpdateDevice(psdoor);
                }
                Num++;
            }
        }

        private void SetRailSwithPosition()
        {
            Num = 0;
            foreach (var item in InRailSwitchs)
            {
                RailSwitch rail = item as RailSwitch;
                rail.IsPositionNormal = Info[Num];
                Num++;
                rail.IsPositionReverse = Info[Num];
                Num++;
                UpdateDevice(item as RailSwitch);
            }
        }

        public void SetRailSwithLock()
        {
            Num = 0;
            foreach (var item in InRailSwitchs)
            {
                (item as RailSwitch).Islock = Info[Num];
                Num++;
            }
        }

        public void SetSectionRunDirection()
        {
            Num = 0;
            foreach (var item in InSections)
            {
                (item as Section).Direction = Info[Num] == true ? 0 : 1;
                Num = Num + 2;
            }
            Dictionary<string, int> RailSwitchDir = new Dictionary<string, int>();
            foreach (var item in InRailSwitchs)
            {
                if (RailSwitchDir.Keys.Contains((item as RailSwitch).SectionName))
                {
                    (item as RailSwitch).Direction = RailSwitchDir[(item as RailSwitch).SectionName];
                }
                else
                {
                    (item as RailSwitch).Direction = Info[Num] == true ? 0 : 1;
                    RailSwitchDir.Add((item as RailSwitch).SectionName, (item as RailSwitch).Direction);
                    Num = Num + 2;
                }
            }
        }

        public void SetSectionAxleState()
        {
            Num = 0;
            foreach (var item in InSections)
            {
                Section section = item as Section;
                if (section.AxleOccupy != !Info[Num])
                {
                    section.AxleOccupy = !Info[Num];
                    UpdateDevice(section);
                }
                Num++;
            }
            Dictionary<string, bool> RailSwitchAxleState = new Dictionary<string, bool>();
            foreach (var item in InRailSwitchs)
            {
                RailSwitch railswitch = item as RailSwitch;
                if (RailSwitchAxleState.Keys.Contains(railswitch.SectionName))
                {
                    railswitch.AxleOccupy = RailSwitchAxleState[railswitch.SectionName];
                    UpdateDevice(railswitch);
                }
                else
                {
                    if (railswitch.AxleOccupy != !Info[Num])
                    {
                        railswitch.AxleOccupy = !Info[Num];
                        UpdateDevice(railswitch);
                    }
                    RailSwitchAxleState.Add(railswitch.SectionName, railswitch.AxleOccupy);
                    Num++;
                }                  
                
            }
        }

        public void SetAxleSectionAccessLock()
        {
            Num = 0;
            foreach (var item in InSections)
            {
                (item as Section).IsAccessLock = Info[Num];
                Num++;
            }
            Dictionary<string, bool> RailSwitchAccessLock = new Dictionary<string, bool>();
            foreach (var item in InRailSwitchs)
            {
                RailSwitch railswitch = item as RailSwitch;
                if (RailSwitchAccessLock.Keys.Contains(railswitch.SectionName))
                {
                    railswitch.IsAccessLock = RailSwitchAccessLock[railswitch.SectionName];
                }
                else
                {
                    railswitch.IsAccessLock = Info[Num];
                    RailSwitchAccessLock.Add(railswitch.SectionName, railswitch.IsAccessLock);
                    Num++;
                }                
            }
        }

        public void SetSignalState()
        {
            Num = 0;
            foreach (var item in InSignals)
            {
                Signal signal = item as Signal;
                if (signal.IsSignalOpen != Info[Num])
                {
                    signal.IsSignalOpen = Info[Num];
                    UpdateDevice(signal);
                }
                Num++;
            }
        }
    }
}

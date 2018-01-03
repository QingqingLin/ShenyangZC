using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.VOBC
{
    class HandleVobc
    {
        public static List<UInt16> VOBCNonCom = new List<UInt16>();
        Unpack unPack;
        public TrainInfo trainInfo;

        public HandleVobc(Unpack unPack)
        {
            this.unPack = unPack;
            trainInfo = new TrainInfo();
            GetTrainID();
            GetNcTrain();
            GetPosition();
            GetDirection();
            GetSpeed();
            GetDoorInfo();

            lock (NonCommunicationTrain.LoseTrain)
            {
                if (NonCommunicationTrain.LoseTrain.Keys.Contains(trainInfo.NIDTrain))
                {
                    Load.Update.CancelNonComTrain(trainInfo);
                }
            }

            int num = VOBCNonCom.IndexOf(trainInfo.NIDTrain);
            if (num == -1)
            {
                VOBCNonCom.Add(trainInfo.NIDTrain);
            }
        }

        private void GetDoorInfo()
        {
            bool shouldDoorOpen = unPack.GetByte() == 1 ? true : false;
            trainInfo.PSDoor = shouldDoorOpen;
            if (trainInfo.HeadPosition is Section)
            {
                if ((trainInfo.HeadPosition as Section).LogicCount == 1)
                {
                    PSDoor psdoor = Load.FindDoor.FindRelayPSDoor(trainInfo.HeadPosition as Section);
                    if (psdoor != null)
                    {
                        psdoor.ShouldOpen = shouldDoorOpen;
                    }
                }
            }
        }

        private void GetSpeed()
        {
            unPack.SkipBytes(2);
            trainInfo.Speed = unPack.GetUint16();
        }

        private void GetDirection()
        {
            unPack.SkipBytes(1);
            trainInfo.TrainDirection = (TrainDir)unPack.GetByte();
        }

        private void GetPosition()
        {
            unPack.SkipBytes(7);
            SectionOrSwitch sectionOrSwitch = (SectionOrSwitch)unPack.GetByte();
            int DeviceId = unPack.GetByte();
            trainInfo.HeadPosition = FindDevice(sectionOrSwitch, DeviceId);
            trainInfo.HeadOffset = unPack.GetUint32();

            sectionOrSwitch = (SectionOrSwitch)unPack.GetByte();
            DeviceId = unPack.GetByte();
            trainInfo.TailPosition = FindDevice(sectionOrSwitch, DeviceId);
            trainInfo.TailOffset = unPack.GetUint32();
        }

        private Device FindDevice(SectionOrSwitch sectionOrSwitch, int DeviceId)
        {
            Device device = MainWindow.stationElements_.Elements.Find((GraphicElement elements) =>
            {
                if (elements is Device)
                {
                    Device dev = elements as Device;
                    if (sectionOrSwitch == SectionOrSwitch.Section)
                    {
                        if (dev is Section)
                        {
                            if ((dev as Section).ID == DeviceId)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (dev is RailSwitch)
                        {
                            if ((dev as RailSwitch).ID == DeviceId)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }) as Device;
            return device;
        }

        private void GetNcTrain()
        {
            unPack.SkipBytes(2);
            trainInfo.NcTrain = (NCTrain)unPack.GetByte();
        }

        private void GetTrainID()
        {
            trainInfo.NIDTrain = unPack.GetUint16();
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.VOBC
{
    class NonCommunicationTrain : Load.Update
    {
        public static Dictionary<UInt16, TrainInfo> LoseTrain = new Dictionary<UInt16, TrainInfo>();
        System.Threading.Timer CheckNonCommunicateTrain ;

        public NonCommunicationTrain()
        {
            CheckNonCommunicateTrain = new System.Threading.Timer(new TimerCallback(JudgeLostTrain), null, 2000, 2000);
        }


        public void JudgeLostTrain(object o)
        {
            lock (HandleVobc.VOBCNonCom)
            {
                Judge(HandleVobc.VOBCNonCom);
                HandleVobc.VOBCNonCom.Clear();
            }
            UpdateLostTrain();
            UpdateAccessOfTrain();
        }

        private void Judge(List<UInt16> VOBCList)
        {
            lock (UpdateTrainPosition.PreTrainPosition)
            {
                foreach (var item in UpdateTrainPosition.PreTrainPosition.Keys)
                {
                    if (!VOBCList.Contains(item))
                    {
                        if (!LoseTrain.Keys.Contains(item))
                        {
                            LoseTrain.Add(item, UpdateTrainPosition.PreTrainPosition[item]);
                        }
                    }
                }
            }
        }

        private void UpdateLostTrain()
        {
            foreach (var trainID in LoseTrain.Keys)
            {
                HandlePosition(trainID, LoseTrain[trainID].HeadPosition);
                HandlePosition(trainID, LoseTrain[trainID].TailPosition);
            }
        }

        private void HandlePosition(ushort trainID, Device device)
        {
            if (device is Section)
            {
                (device as Section).SetNonCommunicateTrain(trainID, true);
                UpdateDevice(device);
            }
            else if (device is RailSwitch)
            {
                RailSwitch railswitch = device as RailSwitch;
                railswitch.SetNonCommunicateTrain(trainID, true);
                UpdateDevice(device);

                RailSwitch railNeedToChange = FindAnotherRailswitch(railswitch);
                if (railNeedToChange != null)
                {
                    railNeedToChange.SetNonCommunicateTrain(trainID, true);
                    UpdateDevice(railNeedToChange);
                }
            }
        }

        private RailSwitch FindAnotherRailswitch(RailSwitch railswitch)
        {
            RailSwitch railNeedToChange = MainWindow.stationElements_.Elements.Find((GraphicElement element) =>
            {
                if (element is RailSwitch)
                {
                    if ((element as RailSwitch).SectionName == railswitch.SectionName && (element as RailSwitch).Name != railswitch.Name)
                    {
                        return true;
                    }
                }
                return false;
            }) as RailSwitch;
            return railNeedToChange;
        }

        private void UpdateAccessOfTrain()
        {
            foreach (var trainID in LoseTrain.Keys)
            {
                lock (UpdateRoute.PreAccess)
                {
                    if (UpdateRoute.PreAccess.Keys.Contains(trainID))
                    {
                        foreach (var device in UpdateRoute.PreAccess[trainID])
                        {
                            if (device is Section)
                            {
                                Section section = device as Section;
                                section.SetRouteOpen(trainID, false, LogicSection.Left);
                                section.SetRouteOpen(trainID, false, LogicSection.Right);
                            }
                            else if (device is RailSwitch)
                            {
                                (device as RailSwitch).SetRouteOpen(trainID, false);
                            }
                            UpdateDevice(device);
                        }
                        UpdateRoute.PreAccess.Remove(trainID);
                    }
                }
            }
        }
    }
}

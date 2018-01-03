using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.VOBC
{
    class VOBCQuit : Load.Update
    {
        public VOBCQuit(TrainInfo trainInfo)
        {
            CancelOccupy(trainInfo.NIDTrain);
            CancelPreAccess(trainInfo.NIDTrain);
        }

        private void CancelPreAccess(UInt16 trainID)
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

        private void CancelOccupy(UInt16 trainID)
        {
            TrainInfo trainInfo = UpdateTrainPosition.PreTrainPosition[trainID];
            HandlePosition(trainInfo.TrainDirection, trainInfo.HeadOffset, false, trainInfo.NIDTrain, trainInfo.HeadPosition);
            HandlePosition(trainInfo.TrainDirection, trainInfo.TailOffset, false, trainInfo.NIDTrain, trainInfo.TailPosition);
        }

        private void HandlePosition(TrainDir trainDir, UInt32 offset, bool isOccupy, UInt16 trainID, Device device)
        {
            if (device is Section)
            {
                (device as Section).SetTrainOccupy(trainDir,offset, isOccupy, trainID);
                UpdateDevice(device);
            }
            else if (device is RailSwitch)
            {
                RailSwitch railswitch = device as RailSwitch;
                railswitch.SetTrainOccupy(trainID, isOccupy);
                UpdateDevice(device);

                RailSwitch railNeedToChange = FindAnotherRailswitch(railswitch);
                if (railNeedToChange != null)
                {
                    railNeedToChange.SetTrainOccupy(trainID, isOccupy);
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
    }
}

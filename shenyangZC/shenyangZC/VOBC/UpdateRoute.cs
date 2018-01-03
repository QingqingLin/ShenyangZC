using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.VOBC
{
    class UpdateRoute:shenyangZC.Load.Update
    {
        public static Dictionary<UInt16, List<Device>> PreAccess = new Dictionary<UInt16, List<Device>>();
        TrainInfo trainInfo;
        SetMA MA;

        public UpdateRoute(TrainInfo trainInfo, SetMA MA)
        {
            this.trainInfo = trainInfo;
            this.MA = MA;
            CancelPreAccess();
            UpdateAccess();
            UpdatePreAccess();
        }

        private void UpdatePreAccess()
        {
            if (PreAccess.Keys.Contains(trainInfo.NIDTrain))
            {
                PreAccess[trainInfo.NIDTrain].Clear();
                foreach (var item in MA.AdvanceRoute)
                {
                    PreAccess[trainInfo.NIDTrain].Add(item);
                }
            }
            else
            {
                List<Device> Route = new List<Device>();
                foreach (var item in MA.AdvanceRoute)
                {
                    Route.Add(item);
                }
                PreAccess.Add(trainInfo.NIDTrain, Route);
            }
        }

        private void UpdateAccess()
        {
            if (MA.AdvanceRoute.Count > 0)
            {
                HandleCurrentDevice();
                if (MA.AdvanceRoute.Count > 2)
                {
                    HandleMiddleDevice();
                }
                if (MA.AdvanceRoute.Count > 1)
                {
                    HandleLastDevice();
                }
            }
        }

        private void HandleLastDevice()
        {
            Device device = MA.AdvanceRoute.Last();
            if (device is RailSwitch)
            {
                (device as RailSwitch).SetRouteOpen(trainInfo.NIDTrain, true);
            }
            else if (device is Section)
            {
                Section section = device as Section;
                if (section.LogicCount == 1)
                {
                    section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Left);
                    section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Right);
                }
                else
                {
                    if (MA.MAOffset == section.Distance)
                    {
                        section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Left);
                        section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Right);
                    }
                    else
                    {
                        if (trainInfo.TrainDirection == TrainDir.Right)
                        {
                            section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Left);
                            section.SetRouteOpen(trainInfo.NIDTrain, false, LogicSection.Right);
                        }
                        else if (trainInfo.TrainDirection == TrainDir.Left)
                        {
                            section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Right);
                            section.SetRouteOpen(trainInfo.NIDTrain, false, LogicSection.Left);
                        }
                    }
                }
            }
            UpdateDevice(device);
        }

        private void HandleMiddleDevice()
        {
            for (int i = 1; i < MA.AdvanceRoute.Count - 1; i++)
            {
                if (MA.AdvanceRoute[i] is RailSwitch)
                {
                    (MA.AdvanceRoute[i] as RailSwitch).SetRouteOpen(trainInfo.NIDTrain, true);
                }
                else
                {
                    Section section = MA.AdvanceRoute[i] as Section;
                    section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Left);
                    section.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Right);
                }
                UpdateDevice(MA.AdvanceRoute[i]);
            }
        }

        private void HandleCurrentDevice()
        {
            if (MA.AdvanceRoute[0] is Section)
            {
                Section CurrentPosition = MA.AdvanceRoute[0] as Section;
                if (trainInfo.TrainDirection == TrainDir.Right)
                {
                    if (CurrentPosition.IsFrontLogicOccupy.Contains(trainInfo.NIDTrain))
                    {
                        CurrentPosition.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Right);
                    }
                    if (CurrentPosition.IsLastLogicOccupy.Contains(trainInfo.NIDTrain))
                    {
                        CurrentPosition.SetRouteOpen(trainInfo.NIDTrain, false, LogicSection.Left);
                    }
                }
                else
                {
                    if (CurrentPosition.IsFrontLogicOccupy.Contains(trainInfo.NIDTrain))
                    {
                        CurrentPosition.SetRouteOpen(trainInfo.NIDTrain, false, LogicSection.Right);
                    }
                    if (CurrentPosition.IsLastLogicOccupy.Contains(trainInfo.NIDTrain))
                    {
                        CurrentPosition.SetRouteOpen(trainInfo.NIDTrain, true, LogicSection.Left);
                    }
                }
            }
            UpdateDevice(MA.AdvanceRoute[0]);
        }

        private void CancelPreAccess()
        {
            if (PreAccess.Keys.Contains(trainInfo.NIDTrain))
            {
                foreach (var device in PreAccess[trainInfo.NIDTrain])
                {
                    if (!MA.AdvanceRoute.Contains(device))
                    {
                        if (device is Section)
                        {
                            Section sec = device as Section;
                            sec.SetRouteOpen(trainInfo.NIDTrain, false,LogicSection.Left);
                            sec.SetRouteOpen(trainInfo.NIDTrain, false, LogicSection.Right);
                        }
                        else if (device is RailSwitch)
                        {
                            (device as RailSwitch).SetRouteOpen(trainInfo.NIDTrain, false);
                        }
                        UpdateDevice(device);
                    }
                }
            }
        }
    }
}

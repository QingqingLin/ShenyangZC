using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.Load
{
    class Update
    {
        public static void UpdateDevice(Device device)
        {
            try
            {
                System.Windows.Application.Current.Dispatcher.Invoke(new Action(delegate
                {
                    device.InvalidateVisual();
                }));
            }
            catch { }

        }

        public static void CancelNonComTrain(TrainInfo trainInfo)
        {
            Device PreNonComTrainHeadPosition;
            Device PreNonComTrainTailPosition;
            lock (VOBC.NonCommunicationTrain.LoseTrain)
            {
                PreNonComTrainHeadPosition = trainInfo.HeadPosition;
                PreNonComTrainTailPosition = trainInfo.TailPosition;
                VOBC.NonCommunicationTrain.LoseTrain.Remove(trainInfo.NIDTrain);
            }
            lock (PreNonComTrainHeadPosition)
            {
                HandlePosition(trainInfo, PreNonComTrainHeadPosition);
            }
            lock (PreNonComTrainTailPosition)
            {
                HandlePosition(trainInfo, PreNonComTrainTailPosition);
            }

        }

        private static void HandlePosition(TrainInfo trainInfo, Device position)
        {
            if (position is Section)
            {
                Section section = position as Section;
                section.HasNonComTrain.Remove(trainInfo.NIDTrain);
                UpdateDevice(position);
            }
            else if (position is RailSwitch)
            {
                RailSwitch railSwitch = position as RailSwitch;
                railSwitch.HasNonComTrain.Remove(trainInfo.NIDTrain);
                UpdateDevice(position);

                RailSwitch railSwitchNeedToChange = FindAnotherRailswitch(railSwitch);
                if (railSwitchNeedToChange != null)
                {
                    railSwitchNeedToChange.HasNonComTrain.Remove(trainInfo.NIDTrain);
                    UpdateDevice(position);
                }
            }
        }

        private static RailSwitch FindAnotherRailswitch(RailSwitch railswitch)
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

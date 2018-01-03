using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.VOBC
{
    class UpdateTrainPosition:shenyangZC.Load.Update
    {
        TrainInfo CurrentTrain;
        public static Dictionary<UInt16, TrainInfo> PreTrainPosition = new Dictionary<UInt16, TrainInfo>();
        List<Device> DeviceNeedToChange = new List<Device>();

        public UpdateTrainPosition(TrainInfo VOBC)
        {
            this.CurrentTrain = VOBC;
            if (PreTrainPosition.Keys.Contains(CurrentTrain.NIDTrain) && PreTrainPosition[CurrentTrain.NIDTrain].EqualTo(CurrentTrain)){}
            else
            {
                CancelPreTrainPosition();
                UpDataCurrentTrainPosition();
                UpdataLine();
                UpdatePrePosition();
            }
        }

        private void UpdatePrePosition()
        {
            if (PreTrainPosition.Keys.Contains(CurrentTrain.NIDTrain))
            {
                PreTrainPosition[CurrentTrain.NIDTrain] = CurrentTrain;
            }
            else
            {
                PreTrainPosition.Add(CurrentTrain.NIDTrain, CurrentTrain);
            }
        }

        private void UpdataLine()
        {
            foreach (var item in DeviceNeedToChange)
            {
                UpdateDevice(item);
            }
        }

        private void UpDataCurrentTrainPosition()
        {

            ChangeDeviceOccupy(CurrentTrain.HeadPosition, CurrentTrain.HeadOffset, CurrentTrain.TrainDirection, true);
            ChangeDeviceOccupy(CurrentTrain.TailPosition, CurrentTrain.TailOffset, CurrentTrain.TrainDirection, true);

        }

        private void CancelPreTrainPosition()
        {
            if (PreTrainPosition.Keys.Contains(CurrentTrain.NIDTrain))
            {
                ChangeDeviceOccupy(PreTrainPosition[CurrentTrain.NIDTrain].HeadPosition, PreTrainPosition[CurrentTrain.NIDTrain].HeadOffset, PreTrainPosition[CurrentTrain.NIDTrain].TrainDirection, false);
                ChangeDeviceOccupy(PreTrainPosition[CurrentTrain.NIDTrain].TailPosition, PreTrainPosition[CurrentTrain.NIDTrain].TailOffset, PreTrainPosition[CurrentTrain.NIDTrain].TrainDirection, false);
            }
        }

        private void ChangeDeviceOccupy(Device position, uint offset, TrainDir trainDirection, bool isOccupy)
        {
            if (position is Section)
            {
                (position as Section).SetTrainOccupy(trainDirection, offset, isOccupy, CurrentTrain.NIDTrain);
                AddToDeviceNeedToChange(position);
            }
            else if (position is RailSwitch)
            {
                ChangeRailSwitchOccupy(position as RailSwitch, isOccupy);
            }
        }

        private void ChangeRailSwitchOccupy(RailSwitch railSwitch, bool isOccupy)
        {
            railSwitch.SetTrainOccupy(CurrentTrain.NIDTrain, isOccupy);
            AddToDeviceNeedToChange(railSwitch);
        }

        private void AddToDeviceNeedToChange(Device device)
        {
            if (!DeviceNeedToChange.Contains(device))
            {
                DeviceNeedToChange.Add(device);
            }
        }
    }
}
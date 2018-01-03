using System;

namespace shenyangZC
{
    public class TopolotyNode : 线路绘图工具.TopolotyNode
    {
        internal 线路绘图工具.Device FindDeviceByDistance(double distance)
        {
            if ((NodeDevice as ICheckDistance).IsDistanceIn(distance))
            {
                return NodeDevice;
            }
            else
            {
                foreach (TopolotyNode node in RightNodes)
                {
                    线路绘图工具.Device resultDevice = node.FindDeviceByDistance(distance);
                    if (resultDevice != null)
                    {
                        return resultDevice;
                    }
                }
            }
            return null;
        }
    }
}

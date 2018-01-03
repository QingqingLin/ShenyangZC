using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.Load
{
    public enum RouteOpenState : byte
    {
        NormalOpen = 0x01,
        ReturnOpen = 0x02,
        NotOpen = 0x03
    }
    public class Route : 线路绘图工具.Route
    {
        private TrainDir RouteDirection_;

        public TrainDir RouteDirection
        {
            get { return RouteDirection_; }
            set { RouteDirection_ = value; }
        }

        private RouteOpenState RouteOpenState_ = RouteOpenState.NotOpen;

        public RouteOpenState RouteOpenState
        {
            get { return RouteOpenState_; }
            set { RouteOpenState_ = value; }
        }

        private TopolotyNode FindNode(Device device)
        {
            TopolotyNode Node = MainWindow.stationTopoloty_.Nodes.Find((TopolotyNode node) =>
            {
                if (node.ConnectableDevice == device)
                {
                    return true;
                }
                return false;
            });
            return Node;
        }

        public void InitializeDirection()
        {
            TopolotyNode inCommingNode = FindNode(this.IncomingSections[0]);
            TopolotyNode inSectionFirst = FindNode(this.InSections[0]);
            if (inCommingNode.ConnectableDevice is Section)
            {
                this.RouteDirection_ = inCommingNode.RightNodes.Contains(inSectionFirst) ? TrainDir.Right : TrainDir.Left;
            }
            else if (inCommingNode.ConnectableDevice is RailSwitch)
            {
                List<TopolotyNode> CommingRightNode = inCommingNode.RightNodes;
                foreach (var item in CommingRightNode)
                {
                    if (item.ConnectableDevice is RailSwitch)
                    {
                        if ((item.ConnectableDevice as RailSwitch).SectionName == (inCommingNode.ConnectableDevice as RailSwitch).SectionName)
                        {
                            inCommingNode = item;
                        }
                    }
                }
                this.RouteDirection_ = inCommingNode.RightNodes.Contains(inSectionFirst) ? TrainDir.Right : TrainDir.Left;
            }
        }

        private void SetDirection(TopolotyNode incommingnode, TopolotyNode inSectionFirst)
        {
            foreach (var node in incommingnode.RightNodes)
            {
                this.RouteDirection_ = node.ConnectableDevice == inSectionFirst.ConnectableDevice ? TrainDir.Right : TrainDir.Left;
            }
        }
    }
}

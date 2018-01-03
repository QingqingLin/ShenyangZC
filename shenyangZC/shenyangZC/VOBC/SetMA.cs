using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.VOBC
{
    class SetMA
    {
        public Device MAEndDevice;
        public UInt32 MAOffset;
        public MAEndType MaEndType = MAEndType.CBTCEnd;
        public List<Device> AdvanceRoute;
        List<Load.Route> CurTrainInRoutes;
        TrainInfo trainInfo;
        bool FindEnd;
        Load.Route CurHandleRoute;
        
        public bool DetermineMA(TrainInfo trainInfo)
        {
            this.trainInfo = trainInfo;
            FindEnd = false;
            CurTrainInRoutes = new List<Load.Route>();
            AdvanceRoute = new List<Device>();
            FindCurrentIn();
            if (CurTrainInRoutes.Count > 0)
            {
                CurHandleRoute = CurTrainInRoutes[0];
                int inNum = CurHandleRoute.InSections.IndexOf(trainInfo.HeadPosition);
                while (true)
                {
                    if (CurHandleRoute.RouteOpenState == Load.RouteOpenState.ReturnOpen)
                    {
                        HandleReturnRoute(CurHandleRoute, inNum);
                        break;
                    }
                    else
                    {
                        if (HandleGeneralRoute(CurHandleRoute, inNum))
                        {
                            break;
                        }
                    }
                    inNum = 0;
                }
            }
            return SetMAByAdvanceRoute();
        }        

        /// <summary>
        /// 将普通进路的内方区段添加到AdvanceRoute中
        /// </summary>
        /// <param 要处理的普通进路="route"></param>
        /// <param 从普通进路中的哪一项开始添加="inNum"></param>
        /// <returns 是否找到进路重点></returns>
        private bool HandleGeneralRoute(Load.Route route, int inNum)
        {
            for (int i = inNum; i < route.InSections.Count; i++)
            {
                if (AddToAdvanceRoute(route.InSections[i]))
                {
                    return true;
                }
                else
                {
                    CurHandleRoute = IsApproachSection(route.InSections[i]);
                    if (CurHandleRoute != null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 判断device是不是某一条进路的接近区段
        /// </summary>
        /// <param 要判断的区段="device"></param>
        /// <returns 找到的进路></returns>
        public Load.Route IsApproachSection(Device device)
        {
            foreach (var route in MainWindow.routeList_.Routes)
            {
                #region 适应时序之前
                //if (route.IncomingSections.Contains(device) && trainInfo.TrainDirection == route.RouteDirection 
                //    && (route.RouteOpenState == Load.RouteOpenState.NormalOpen || route.RouteOpenState == Load.RouteOpenState.ReturnOpen)
                //    && (route.StartSignal as Signal).IsSignalOpen)
                //{
                //    return route;
                //}
                #endregion
                #region 适应时序之后
                if (route.IncomingSections.Contains(device) && trainInfo.TrainDirection == route.RouteDirection
                    && (route.RouteOpenState == Load.RouteOpenState.NormalOpen || route.RouteOpenState == Load.RouteOpenState.ReturnOpen))
                {
                    if ((route.StartSignal as Signal).IsSignalOpen)
                    {
                        return route;
                    }
                    else 
                    {
                        if (route.InSections[0] is Section)
                        {
                            if ((route.InSections[0] as Section).AxleOccupy)
                            {
                                return route;
                            }
                        }
                        else
                        {
                            if ((route.InSections[0] as RailSwitch).AxleOccupy)
                            {
                                return route;
                            }
                        }
                    }
                }
                #endregion
            }
            return null;
        }

        /// <summary>
        /// 将折返进路的内方区段添加到AdvanceRoute中
        /// </summary>
        /// <param 折返进路="route"></param>
        /// <param 从折返进路内方区段的哪一项开始添加="inNum"></param>
        private void HandleReturnRoute(Load.Route route, int inNum)
        {
            MaEndType = MAEndType.ReturnEnd;
            for (int i = inNum; i < route.InSections.Count; i++)
            {
                AddToAdvanceRoute(route.InSections[i]);
            }
        }

        /// <summary>
        /// 将区段或道岔加到AdvanceRoute中
        /// </summary>
        /// <param 要处理的device="CurrentRoute"></param>
        /// <returns 如果遇到MA终点，返回true，否则返回false></returns>
        private bool AddToAdvanceRoute(Device device)
        {
            List<TopolotyNode> NextNodes = FindNextNode();

            if (device is Section)
            {
                Section inSection = device as Section;
                if (inSection.HasNonComTrain.Count != 0)
                {
                    DeletePreSection(inSection);
                    return true;
                }
                else if (inSection.GetIsEB() || inSection.GetIsDoorOpen())
                {
                    return true;
                }
                else if (inSection.AxleOccupy && device != trainInfo.HeadPosition && inSection.GetTrainOccupy() == TrainOccupy.NonOccupy)
                {
                    #region 适应时序前
                    //return true;
                    #endregion
                    #region 适应时序后
                    if (NextNodes.Contains(MainWindow.stationTopoloty_.Nodes.Find((TopolotyNode inputNode) => { return inputNode.NodeDevice == inSection; })))
                    {
                        Add(inSection);
                    }
                    else
                    {
                        return true;
                    }
                    #endregion
                }
                else if (inSection.GetTrainOccupy(trainInfo.NIDTrain) == TrainOccupy.AllOccupy)
                {
                    return true;
                }
                else if (trainInfo.TrainDirection == TrainDir.Right && inSection.GetTrainOccupy(trainInfo.NIDTrain) == TrainOccupy.LeftOccupy)
                {
                    return true;
                }
                else if (trainInfo.TrainDirection == TrainDir.Left && inSection.GetTrainOccupy(trainInfo.NIDTrain) == TrainOccupy.RightOccupy)
                {
                    return true;
                }
                else if (inSection.GetTrainOccupy(trainInfo.NIDTrain) == TrainOccupy.NonOccupy)
                {
                    Add(device);
                }
                else
                {
                    Add(device);
                    return true;
                }
            }
            else if (device is RailSwitch)
            {
                RailSwitch inRailSwitch = device as RailSwitch;
                if (inRailSwitch.HasNonComTrain.Count != 0)
                {
                    DeletePreSection(inRailSwitch);
                    return true;
                }
                else if (inRailSwitch.AxleOccupy && inRailSwitch != trainInfo.HeadPosition)
                {
                    #region 适应时序前
                    //return true;
                    #endregion
                    #region 适应时序后
                    if (inRailSwitch.TrainOccupy.Count != 0)
                    {
                        return true;
                    }
                    else
                    {
                        if (NextNodes.Contains(MainWindow.stationTopoloty_.Nodes.Find((TopolotyNode inputNode) => { return inputNode.NodeDevice == inRailSwitch; })))
                        {
                            Add(inRailSwitch);
                        }
                        else
                        {
                            return true;
                        }
                    }
                    #endregion
                }
                else if (inRailSwitch.GetTrainOccupy(trainInfo.NIDTrain))
                {
                    return true;
                }
                else
                {
                    Add(device);
                }
            }
            return false;
        }

        private List<TopolotyNode> FindNextNode()
        {
            Device curPos = trainInfo.HeadPosition;
            TopolotyNode curNode = MainWindow.stationTopoloty_.Nodes.Find((TopolotyNode node) =>
            {
                return curPos == node.NodeDevice;
            });
            List<TopolotyNode> NextNodes = null;
            NextNodes = trainInfo.TrainDirection == TrainDir.Right ? curNode.RightNodes : curNode.LeftNodes;
            TopolotyNode relayNode = null;
            foreach (var node in NextNodes)
            {
                if (node != null)
                {
                    if (node.NodeDevice is RailSwitch)
                    {
                        relayNode = MainWindow.stationTopoloty_.Nodes.Find((TopolotyNode reNode) => { return reNode.NodeDevice == node.NodeDevice; });
                    }
                }
            }
            if (relayNode != null)
            {
                NextNodes.Add(relayNode);
            }
            return NextNodes;
        }

        private void DeletePreSection(Device currentDevice)
        {
            List<TopolotyNode> preNodes;
            TopolotyNode currentNode = FindNode(currentDevice);
            switch (trainInfo.TrainDirection)
            {
                case TrainDir.Right:
                    preNodes = currentNode.LeftNodes;
                    break;
                case TrainDir.Left:
                    preNodes = currentNode.RightNodes;
                    break;
                default:
                    preNodes = null;
                    break;
            }
            if (AdvanceRoute.Count != 0)
            {
                TopolotyNode lastNode = FindNode(AdvanceRoute.Last());
                if (preNodes.Contains(lastNode))
                {
                    if (AdvanceRoute.Last() is Section)
                    {
                        AdvanceRoute.Remove(AdvanceRoute.Last());
                    }
                    else if (AdvanceRoute.Last() is RailSwitch)
                    {
                        if (AdvanceRoute.Count > 1)
                        {
                            if (AdvanceRoute[AdvanceRoute.Count - 2] is RailSwitch)
                            {
                                if ((AdvanceRoute[AdvanceRoute.Count - 2] as RailSwitch).SectionName == (AdvanceRoute.Last() as RailSwitch).SectionName)
                                {
                                    AdvanceRoute.Remove(AdvanceRoute[AdvanceRoute.Count - 2]);
                                }
                            }
                        }
                        AdvanceRoute.Remove(AdvanceRoute.Last());
                    }
                }
            }
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

        private void Add(Device NeedToAdd)
        {
            if (!AdvanceRoute.Contains(NeedToAdd))
            {
                AdvanceRoute.Add(NeedToAdd);
            }
        }

        /// <summary>
        /// 找到列车当前位置所在进路
        /// </summary>
        private void FindCurrentIn()
        {
            CurTrainInRoutes = MainWindow.routeList_.Routes.FindAll((Load.Route route) =>
            {
                if (route.InSections.Contains(trainInfo.HeadPosition)
                && (route.RouteOpenState == Load.RouteOpenState.NormalOpen || route.RouteOpenState == Load.RouteOpenState.ReturnOpen)
                && route.RouteDirection == trainInfo.TrainDirection)
                {
                    return true;
                }
                return false;
            });
        }

        /// <summary>
        /// 将AdvanceRoute中最后一项作为进路终点
        /// </summary>
        /// <returns 是否为空MA></returns>
        private bool SetMAByAdvanceRoute()
        {
            if (AdvanceRoute.Count > 0)
            {
                MAEndDevice = AdvanceRoute.Last();
                MAOffset = SetOffset(AdvanceRoute.Last());
                return true;
            }
            else
            {
                return false;
            }
        }

        private UInt16 SetOffset(Device device)
        {
            if (device is Section)
            {
                return (device as Section).GetOffset(trainInfo.TrainDirection,trainInfo.NIDTrain);
            }
            else if(device is RailSwitch)
            {
                return (device as RailSwitch).GetOffset(trainInfo.TrainDirection);
            }
            else
            {
                return 0;
            }
        }

        public void SetMAObstacle(List<ObstacleInfo> obstacle)
        {
            for (int i = 0; i < AdvanceRoute.Count; i++)
            {
                if (i == 0)
                {
                    if (AdvanceRoute[i] is RailSwitch)
                    {
                        AddObstacleCollection(obstacle, AdvanceRoute[i] as RailSwitch);
                    }
                }
                else
                {
                    if (AdvanceRoute[i] is RailSwitch)
                    {
                        RailSwitch nowRailSwitch = AdvanceRoute[i] as RailSwitch;
                        if (AdvanceRoute[i - 1] is RailSwitch)
                        {
                            RailSwitch lastRailSwitch = AdvanceRoute[i - 1] as RailSwitch;
                            if (nowRailSwitch.SectionName == lastRailSwitch.SectionName) { }
                            else
                            {
                                AddObstacleCollection(obstacle, AdvanceRoute[i] as RailSwitch);
                            }
                        }
                        else
                        {
                            AddObstacleCollection(obstacle, AdvanceRoute[i] as RailSwitch);
                        }
                    }
                }
            }
        }

        public void AddObstacleCollection(List<ObstacleInfo> Obstacle, RailSwitch railSwitch)
        {
            ObstacleInfo railSwitchObstacle = new ObstacleInfo();
            railSwitchObstacle.NC_Obstacle = 4;
            railSwitchObstacle.NID_Obstacle = Convert.ToUInt16(railSwitch.ID);
            railSwitchObstacle.Q_Obstacle_Now = (railSwitch.IsPositionNormal == true ? Convert.ToByte(1) : Convert.ToByte(2));
            railSwitchObstacle.Q_Obstacle_CI = railSwitchObstacle.Q_Obstacle_Now;
            Obstacle.Add(railSwitchObstacle);
        }
    }
}

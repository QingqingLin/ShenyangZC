using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using 线路绘图工具;
using shenyangZC.VOBC;

namespace shenyangZC
{
    public enum LogicSection
    {
        Left,
        Right
    }

    public enum TrainOccupy
    {
        LeftOccupy,
        RightOccupy,
        AllOccupy,
        NonOccupy
    }

    public class Section : 线路绘图工具.Section, IRailway
    {
        static Pen DefaultPen_ = new Pen(Brushes.Cyan, 2.9);
        static Pen AxleOccupyPen_ = new Pen(Brushes.Red, 3);
        static Pen TrainOccpyPen_ = new Pen(Brushes.Yellow, 3);
        static Pen NonComTrainOccupy_ = new Pen(Brushes.Purple, 3);
        static Pen AccessPen_ = new Pen(Brushes.Green, 3);

        public bool IsAccessLock;
        public int Direction;


        #region 有用的属性
        private PSDoor relayPSDoor_;

        public PSDoor RelayPSDoor
        {
            get { return relayPSDoor_; }
            set { relayPSDoor_ = value; }
        }

        private bool axleOccpy_ = true;
        public bool AxleOccupy 
        {
            get {return axleOccpy_; } 
            set 
            {                 
                if (axleOccpy_ != value)
                {
                    axleOccpy_ = value;
                }
            } 
        }

        List<UInt16> hasNonComTrain_ = new List<UInt16>();
        public List<UInt16> HasNonComTrain
        {
            get { return hasNonComTrain_; }
            set
            {
                if (hasNonComTrain_ != value)
                {
                    hasNonComTrain_ = value;
                }
            }
        }

        List<UInt16> isFrontLogicOccupy_ = new List<UInt16>();
        public List<UInt16> IsFrontLogicOccupy
        {
            get { return isFrontLogicOccupy_; }
        }

        List<UInt16> isLastLogicOccupy_ = new List<UInt16>();
        public List<UInt16> IsLastLogicOccupy
        {
            get { return isLastLogicOccupy_; }
        }

        List<UInt16> isFrontRouteOpen_ = new List<UInt16>();
        public List<UInt16> IsFrontRouteOpen
        {
            get { return isFrontRouteOpen_; }
        }

        List<UInt16> isLastRouteOpen_ = new List<UInt16>();
        public List<UInt16> IsLastRouteOpen
        {
            get { return isLastRouteOpen_; }
        }
        #endregion

        public Graphic InsuLine { get; set; }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            Point startPoint;
            Point MiddlePoint;
            Point EndPoint;
            if (this.Name != "T0115" && this.Name != "T0114")
            {
                Line line = graphics_.Find((Graphic graphic) => { return graphic is Line; }) as Line;
                startPoint = line.Points[0];
                EndPoint = line.Points[1];
                MiddlePoint = new System.Windows.Point((line.Points[0].X + line.Points[1].X) / 2, (line.Points[0].Y + line.Points[1].Y) / 2);
            }
            else
            {
                List<Graphic> lines = graphics_.FindAll((Graphic graphic) => { return graphic is Line; });
                startPoint = (lines[0] as Line).Points[0];
                MiddlePoint = (lines[0] as Line).Points[1];
                EndPoint = (lines[1] as Line).Points[1];
            }

            bool isFrontDraw = false;
            bool isLastDraw = false;
            dc.DrawLine(DefaultPen_, startPoint, MiddlePoint);
            dc.DrawLine(DefaultPen_, MiddlePoint, EndPoint);
            if (hasNonComTrain_.Count != 0)
            {
                dc.DrawLine(NonComTrainOccupy_, startPoint, MiddlePoint);
                dc.DrawLine(NonComTrainOccupy_, MiddlePoint, EndPoint);
            }
            else
            {
                if (axleOccpy_)
                {
                    dc.DrawLine(AxleOccupyPen_, startPoint, MiddlePoint);
                    dc.DrawLine(AxleOccupyPen_, MiddlePoint, EndPoint);
                }
                if (isFrontLogicOccupy_.Count != 0 || isLastLogicOccupy_.Count != 0)
                {
                    if (isFrontLogicOccupy_.Count != 0)
                    {
                        dc.DrawLine(TrainOccpyPen_, startPoint, MiddlePoint);
                        isFrontDraw = true;
                    }
                    if (isLastLogicOccupy_.Count != 0)
                    {
                        dc.DrawLine(TrainOccpyPen_, MiddlePoint, EndPoint);
                        isLastDraw = true;
                    }
                }
                if (!isFrontDraw)
                {
                    if (isFrontRouteOpen_.Count != 0)
                    {
                        dc.DrawLine(AccessPen_, startPoint, MiddlePoint);
                    }
                }
                if (!isLastDraw)
                {
                    if (isLastRouteOpen_.Count != 0)
                    {
                        dc.DrawLine(AccessPen_, MiddlePoint, EndPoint);
                    }
                }
            }

            dc.DrawText(formattedName_, namePoint_);

            if (InsuLine is Line)
            {
                (InsuLine as Line).OnRender(dc, DefaultPen_);
            }
        }

        public void AddInsulation()
        {
            List<Point> leftPts = new List<Point>();
            GetLeftPoints(leftPts);

            InsuLine = new Line()
            {
                Pt0 = new Point(leftPts[0].X, leftPts[0].Y - Line.LineThickness),
                Pt1 = new Point(leftPts[0].X, leftPts[0].Y + Line.LineThickness),
            };
        }

        public void SetTrainOccupy(TrainDir trainDir, UInt32 offset, bool isOccupy, UInt16 trainID)
        {
            if (this.LogicCount == 1)
            {
                SetTrainList(this.isFrontLogicOccupy_, isOccupy, trainID);
                SetTrainList(this.isLastLogicOccupy_, isOccupy, trainID);
            }
            else
            {
                if (trainDir == TrainDir.Right)
                {
                    if (offset < (this.Distance / 2))
                    {
                        SetTrainList(this.isFrontLogicOccupy_, isOccupy, trainID);
                    }
                    else
                    {
                        SetTrainList(this.isLastLogicOccupy_, isOccupy, trainID);
                    }
                }
                else if (trainDir == TrainDir.Left)
                {
                    if (offset < (this.Distance / 2))
                    {
                        SetTrainList(this.isFrontLogicOccupy_, isOccupy, trainID);
                    }
                    else
                    {
                        SetTrainList(this.isLastLogicOccupy_, isOccupy, trainID);
                    }
                }
            }
        }        

        public TrainOccupy GetTrainOccupy()
        {
            if (this.LogicCount == 1)
            {
                if (isFrontLogicOccupy_.Count == 0)
                {
                    return TrainOccupy.NonOccupy;
                }
                else
                {
                    return TrainOccupy.AllOccupy;
                }
            }
            else
            {
                if (isFrontLogicOccupy_.Count == 0 && isLastLogicOccupy_.Count == 0)
                {
                    return TrainOccupy.NonOccupy;
                }
                else if (isFrontLogicOccupy_.Count != 0 && isLastLogicOccupy_.Count == 0)
                {
                    return TrainOccupy.LeftOccupy;
                }
                else if (isFrontLogicOccupy_.Count == 0 && isLastLogicOccupy_.Count != 0)
                {
                    return TrainOccupy.RightOccupy;
                }
                else
                {
                    return TrainOccupy.AllOccupy;
                }
            }
        }

        public TrainOccupy GetTrainOccupy(UInt16 trainID)
        {
            if (this.LogicCount == 1)
            {
                if (!HasOtherTrain(isFrontLogicOccupy_, trainID))
                {
                    return TrainOccupy.NonOccupy;
                }
                else
                {
                    return TrainOccupy.AllOccupy;
                }
            }
            else
            {
                if (!HasOtherTrain(isFrontLogicOccupy_, trainID) && !HasOtherTrain(isLastLogicOccupy_, trainID))
                {
                    return TrainOccupy.NonOccupy;
                }
                else if (HasOtherTrain(isFrontLogicOccupy_, trainID) && !HasOtherTrain(isLastLogicOccupy_, trainID))
                {
                    return TrainOccupy.LeftOccupy;
                }
                else if (!HasOtherTrain(isFrontLogicOccupy_,trainID) && HasOtherTrain(isLastLogicOccupy_, trainID))
                {
                    return TrainOccupy.RightOccupy;
                }
                else
                {
                    return TrainOccupy.AllOccupy;
                }
            }
        }

        private bool HasOtherTrain(List<UInt16> list, UInt16 trainID)
        {
            if (list.Count == 0)
            {
                return false;
            }
            else if (list.Count == 1 && list.Contains(trainID))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void SetTrainList(List<UInt16> trainList, bool isOccupy, UInt16 trainID)
        {
            if (isOccupy)
            {
                if (!trainList.Contains(trainID))
                {
                    trainList.Add(trainID);
                }
            }
            else
            {
                if (trainList.Contains(trainID))
                {
                    trainList.Remove(trainID);
                }
            }
        }

        public UInt16 GetOffset(TrainDir Direction, UInt16 trainID)
        {
            if (this.LogicCount == 1)
            {
                return Convert.ToUInt16(this.Distance);
            }
            switch (Direction)
            {
                case TrainDir.Right:
                    switch (GetTrainOccupy(trainID))
                    {
                        case TrainOccupy.NonOccupy:
                            return Convert.ToUInt16(this.Distance);
                        case TrainOccupy.RightOccupy:
                            return Convert.ToUInt16(this.Distance / 2);
                        default:
                            return 0;
                    }
                case TrainDir.Left:
                    switch (GetTrainOccupy(trainID))
                    {
                        case TrainOccupy.NonOccupy:
                            return Convert.ToUInt16(this.Distance);
                        case TrainOccupy.LeftOccupy:
                            return Convert.ToUInt16(this.Distance / 2);
                        default:
                            return 0;
                    }
                default:
                    return 0; 
            }
        }

        public void SetRouteOpen(UInt16 trainID, bool isOpen, LogicSection logicSection)
        {
            if (logicSection == LogicSection.Left)
            {
                SetTrainList(this.isFrontRouteOpen_, isOpen, trainID);
            }
            else if (logicSection == LogicSection.Right)
            {
                SetTrainList(this.isLastRouteOpen_, isOpen, trainID);
            }
        }

        public void SetNonCommunicateTrain(UInt16 trainID, bool isNonCom)
        {
            SetTrainList(hasNonComTrain_, isNonCom, trainID);
        }

        public bool GetIsDoorOpen()
        {
            if (this.LogicCount == 2)
            {
                return false;
            }
            else
            {
                PSDoor psdoor = Load.FindDoor.FindRelayPSDoor(this);
                if (psdoor != null)
                {
                    return psdoor.IsOpen;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool GetIsEB()
        {
            if (this.LogicCount == 2)
            {
                return false;
            }
            else
            {
                PSDoor psdoor = Load.FindDoor.FindRelayPSDoor(this);
                if (psdoor != null)
                {
                    return psdoor.IsEB;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using 线路绘图工具;

namespace shenyangZC
{
    public class RailSwitch : 线路绘图工具.RailSwitch, IRailway
    {
        static Pen YellowPen_ = new Pen(Brushes.Yellow,3);
        static Pen RedPen_ = new Pen(Brushes.Red, 3);
        static Pen DefaultPen_ = new Pen(Brushes.Cyan, 3);
        static Pen PurplePen_ = new Pen(Brushes.Purple, 3);
        static Pen GreenPen_ = new Pen(Brushes.Green, 3);

        Point sectionStartPoint;
        Point sectionEndPoint;
        Point normalStartPoint;
        Point normalEndPoint;
        Point reverseStartPoint;
        Point reverseEndPoint;

        RailSwitch()
        {
            IsPositionNormal = false;
            IsPositionReverse = false;
            axleOccupy_ = true;
        }

        #region 有用的属性
        public bool IsPositionNormal { get; set; }
        public bool IsPositionReverse { get; set; }
        public bool Islock { get; set; }
        public bool IsAccessLock { get; set; }

        private int _Offset = 45;
        public int Offset
        {
            get { return _Offset;}
            set
            {
                if (_Offset != value)
                {
                    _Offset = value;
                }
            }
        }
        public int Direction { get; set; }

        private bool axleOccupy_;
        public bool AxleOccupy
        {
            get { return axleOccupy_; }
            set
            {
                if (axleOccupy_ != value)
                {
                    axleOccupy_ = value;
                }
            }
        }

        private List<UInt16> trainOccupy_ = new List<UInt16>();
        public List<UInt16> TrainOccupy
        {
            get { return trainOccupy_; }
        }

        private List<UInt16> isAccess_ = new List<UInt16>();
        public List<UInt16> IsAccess
        {
            get { return isAccess_; }
            set
            {
                if (isAccess_ != value)
                {
                    isAccess_ = value;
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
        #endregion


        public Graphic InsuLine { get; set; }

        protected override void OnRender(System.Windows.Media.DrawingContext dc)
        {
            List<int> sectionIndexs = SectionIndexList[0];
            List<int> normalIndexs = SectionIndexList[1];
            List<int> reverseIndexs = SectionIndexList[2];

            sectionStartPoint = (graphics_[sectionIndexs[0]] as Line).Points[0];
            sectionEndPoint = (graphics_[sectionIndexs[0]] as Line).Points[1];

            normalStartPoint = (graphics_[normalIndexs[0]] as Line).Points[0];
            normalEndPoint = (graphics_[normalIndexs[0]] as Line).Points[1];

            reverseStartPoint = (graphics_[reverseIndexs[0]] as Line).Points[0];
            reverseEndPoint = (graphics_[reverseIndexs[0]] as Line).Points[1];

            if (HasNonComTrain.Count != 0)
            {
                DrawLine(dc, PurplePen_, PurplePen_);
            }
            else
            {
                if (trainOccupy_.Count > 0)
                {
                    DrawLine(dc, YellowPen_, DefaultPen_);
                }
                else if (axleOccupy_)
                {
                    DrawLine(dc, RedPen_, DefaultPen_);
                }
                else if (isAccess_.Count != 0)
                {
                    DrawLine(dc, GreenPen_, DefaultPen_);
                }
                else
                {
                    DrawLine(dc, DefaultPen_, DefaultPen_);
                }
            }

            if (InsuLine is Line)
            {
                (InsuLine as Line).OnRender(dc, DefaultPen_);
            }
        }

        private void DrawLine(DrawingContext dc, Pen relatedPen, Pen unreletedPen)
        {
            dc.DrawLine(relatedPen, sectionStartPoint, sectionEndPoint);
            if (IsPositionNormal == true && IsPositionReverse == false)
            {
                dc.DrawLine(relatedPen, sectionEndPoint, normalEndPoint);
                dc.DrawLine(unreletedPen, reverseStartPoint, reverseEndPoint);
            }
            else if (IsPositionNormal == false && IsPositionReverse == true)
            {
                dc.DrawLine(relatedPen, sectionEndPoint, reverseEndPoint);
                dc.DrawLine(unreletedPen, normalStartPoint, normalEndPoint);
            }
            else
            {
                dc.DrawLine(relatedPen, sectionStartPoint, sectionEndPoint);
                dc.DrawLine(relatedPen, normalStartPoint, normalEndPoint);
                dc.DrawLine(relatedPen, reverseStartPoint, reverseEndPoint);
            }
        }

        private void DrawSwitchLine(DrawingContext dc, List<int> sectionIndexs, List<int> normalIndexs, List<int> reverseIndexs,
            Pen sectionPen, Pen normalPen, Pen reversePen)
        {
            foreach (int index in sectionIndexs)
            {
                Line line = graphics_[index] as Line;
                dc.DrawLine(sectionPen, line.Points[0], line.Points[1]);
            }
            foreach (int index in normalIndexs)
            {
                Line line = graphics_[index] as Line;
                dc.DrawLine(normalPen, line.Points[0], line.Points[1]);
            }
            foreach (int index in reverseIndexs)
            {
                Line line = graphics_[index] as Line;
                dc.DrawLine(reversePen, line.Points[0], line.Points[1]);
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

        public void SetTrainOccupy(UInt16 trainID, bool isOccupy)
        {
            SetTrainList(trainOccupy_, isOccupy, trainID);
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

        public bool GetTrainOccupy(UInt16 trainID)
        {
            if (HasOtherTrain(trainOccupy_,trainID))
            {
                return true;
            }
            else
            {
                return false;
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


        public UInt16 GetOffset(TrainDir Direction)
        {
            if (this.SectionName == "W0105" || this.SectionName == "W0106" || this.SectionName == "W0411" || this.SectionName == "W0414")
            {
                if (this.IsPositionNormal && !this.IsPositionReverse)
                {
                    return Convert.ToUInt16(2 * this.NormalDistance);
                }
                else if (this.IsPositionReverse && !this.IsPositionNormal)
                {
                    return Convert.ToUInt16(2 * this.ReverseDistance);
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                if (this.IsPositionNormal && !this.IsPositionReverse)
                {
                    return Convert.ToUInt16(this.NormalDistance);
                }
                else if (!this.IsPositionNormal && this.IsPositionReverse)
                {
                    return Convert.ToUInt16(this.ReverseDistance);
                }
                else
                {
                    return 0;
                }
            }
        }

        public void SetRouteOpen(UInt16 trainID, bool isOpen)
        {
            SetTrainList(isAccess_, isOpen, trainID);
        }

        public void SetNonCommunicateTrain(UInt16 trainID, bool isNonCom)
        {
            SetTrainList(hasNonComTrain_, isNonCom, trainID);
        }
    }
}

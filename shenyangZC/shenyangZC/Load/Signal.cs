using System;
using System.Collections.Generic;
using System.Windows.Media;
using 线路绘图工具;
using System.Xml;


namespace shenyangZC
{
    public class Signal : 线路绘图工具.Signal
    {
        static Pen linePen = new Pen(Brushes.White, 3);
        
        List<Circle> lights_ = new List<Circle>();
        List<Line> lines_ = new List<Line>();

        private bool isSignalOpen_;

        public bool IsSignalOpen
        {
            get { return isSignalOpen_; }
            set { isSignalOpen_ = value; }
        }



        protected override void OnRender(DrawingContext dc)
        {
            foreach (var item in lines_)
            {
                item.OnRender(dc, linePen);
            }

            lights_[0].OnRender(dc, isSignalOpen_ ? Brushes.Green : Brushes.Red);
            DrawEmptyLight_1(dc);

            dc.DrawText(formattedName_, namePoint_);
        }

        private void DrawEmptyLight_1(DrawingContext dc)
        {
            if (lights_.Count > 1)

            {
                lights_[1].OnRender(dc, Brushes.Black);
            }
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);

            foreach (var item in graphics_)
            {
                if (item is Circle)
                {
                    lights_.Add(item as Circle);
                }
                else
                {
                    lines_.Add(item as Line);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using 线路绘图工具;

namespace shenyangZC
{
    public class PSDoor : 线路绘图工具.PSDoor
    {
        static Pen WhitePen_ = new Pen(Brushes.White, 3);
        
        public PSDoor()
        {
            isOpen_ = false;
        }

        #region 有用的属性
        private bool shouldOpen_;

        public bool ShouldOpen
        {
            get { return shouldOpen_; }
            set { shouldOpen_ = value; }
        }


        private bool isEB_;

        public bool IsEB
        {
            get { return isEB_; }
            set { isEB_ = value; }
        }

        private bool isOpen_ = false;

        public bool IsOpen
        {
            get { return isOpen_; }
            set { isOpen_ = value; }
        }
        #endregion

        protected override void OnRender(DrawingContext dc)
        {
            List<Graphic> lines = graphics_.FindAll((Graphic graphic) => { return graphic is Line; });

            if (isOpen_)
            {
                dc.DrawLine(WhitePen_, (lines[0] as Line).Points[0], (lines[0] as Line).Points[1]);
                dc.DrawLine(WhitePen_, (lines[2] as Line).Points[0], (lines[2] as Line).Points[1]);
            }
            else
            {
                dc.DrawLine(WhitePen_, (lines[0] as Line).Points[0], (lines[0] as Line).Points[1]);
                dc.DrawLine(WhitePen_, (lines[1] as Line).Points[0], (lines[1] as Line).Points[1]);
                dc.DrawLine(WhitePen_, (lines[2] as Line).Points[0], (lines[2] as Line).Points[1]);
            }
        }
    }
}

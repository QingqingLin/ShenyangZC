using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC
{
    interface IRailway
    {
        void AddInsulation();
        Graphic InsuLine { get; set; }

    }
}

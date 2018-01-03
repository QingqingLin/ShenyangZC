using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.Load
{
    class FindDoor
    {
        public static PSDoor FindRelayPSDoor(Section section)
        {
            Section sec = MainWindow.stationElements_.Elements.Find((GraphicElement element) => {
                if (element is Section)
                {
                    return (element as Section) == section;
                }
                return false;
            }) as Section;
            return sec.RelayPSDoor;
        }
    }
}

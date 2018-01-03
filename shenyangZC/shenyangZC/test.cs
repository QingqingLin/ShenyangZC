using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;
using shenyangZC.VOBC;

namespace shenyangZC
{
    class test
    {
        public void finddevice()
        {
            SectionOrSwitch sos = SectionOrSwitch.Switch;
            int DeviceId = 0;
            Device device = MainWindow.stationElements_.Elements.Find((GraphicElement elements) =>
            {
                if (elements is Device)
                {
                    Device dev = elements as Device;
                    if (sos == SectionOrSwitch.Section)
                    {
                        if (dev is Section)
                        {
                            if ((dev as Section).ID == DeviceId)
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (dev is RailSwitch)
                        {
                            if ((dev as RailSwitch).ID == DeviceId)
                            {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }) as Device;
            List<int> num = new List<int>();
            foreach (var item in MainWindow.routeList_.Routes)
            {
                if (item.IncomingSections[0] == device)
                {
                    int a = 9;
                }
                foreach (var dev in item.InSections)
                {
                    if (device == dev)
                    {
                        int a = 0;
                    }
                }
            }
        }

        //public void testDeviceID()
        //{
        //    List<Route> CurTrainIn = MainWindow.routeList_.routes_.FindAll((Route route) =>
        //    {
        //        foreach (var sec in route.InSections)
        //        {
        //            if (sec.ID == 2)
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    });
        //}

        public void testPSD()
        {
            Section section = MainWindow.stationElements_.Elements.Find((GraphicElement element) =>
            {
                if (element is Section)
                {
                    if ((element as Section).Name == "T0107")
                    {
                        return true;
                    }
                }
                return false;
            }) as Section;

            PSDoor psdoor = section.RelayPSDoor;
        }
    }
}

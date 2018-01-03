using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using 线路绘图工具;

namespace shenyangZC.Load
{
    public class RouteCreator
    {
        [XmlElement("Route", typeof(Route))]
        public List<Route> Routes { get; set; }

        static internal RouteCreator Open()
        {
            using (StreamReader sr = new StreamReader("src\\RouteList.xml"))
            {
                return new XmlSerializer(typeof(RouteCreator)).Deserialize(sr) as RouteCreator;
            }
        }

        public void LoadDevices(List<GraphicElement> elements)
        {
            foreach (var item in Routes)
            {
                item.LoadDevices(elements);
            }
        }

        public void InitializeDirection()
        {
            foreach (var item in this.Routes)
            {
                item.InitializeDirection();
            }
        }
    }
}

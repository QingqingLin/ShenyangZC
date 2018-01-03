using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using 线路绘图工具;

namespace shenyangZC.Load
{
    class LoadSectionRelay
    {
        public static List<LoadSectionRelay> SectionRelay = new List<LoadSectionRelay>();

        public LoadSectionRelay()
        {
            List<string> list = new List<string>();
            using (StreamReader sr = new StreamReader("src\\SectionRelay.txt", Encoding.Default))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line);
                }
            }
            foreach (var line in list)
            {
                string[] item = line.Split(':');
                string secID = item[0].Split('_')[1];
                int sectionID = Convert.ToInt16(secID);
                string[] psdID = System.Text.RegularExpressions.Regex.Split(item[1], @"\s+");
                int psdoorID = Convert.ToInt16(psdID[1].Split('_')[2]);

                Section section = MainWindow.stationElements_.Elements.Find((GraphicElement element) =>
                {
                    if (element is Section)
                    {
                        if ((element as Section).ID == sectionID)
                        {
                            return true;
                        }
                    }
                    return false;
                }) as Section;

                PSDoor psdoor = MainWindow.stationElements_.Elements.Find((GraphicElement element) =>
                {
                    if (element is PSDoor)
                    {
                        if ((element as PSDoor).ID == psdoorID)
                        {
                            return true;
                        }
                    }
                    return false;
                }) as PSDoor;

                section.RelayPSDoor = psdoor;
            }
        }
    }
}

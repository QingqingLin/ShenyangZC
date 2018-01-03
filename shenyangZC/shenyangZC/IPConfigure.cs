using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace shenyangZC
{
    class IPConfigure
    {
         [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key,
           string val, string filePath);

        [DllImport("kernel32")]
        private static extern long GetPrivateProfileString(string section, string key,
            string def, StringBuilder retVal, int size, string filePath);

        public IPConfigure()
        {
            foreach (string Device in Section)
            {
                IPList IP = new IPList();
                IP.DeviceName = Device;
                IP.DeviceID = Convert.ToInt16(ReadIniData(Device, "DeviceID", "", IPConfigPath));
                IP.IP = IPAddress.Parse(ReadIniData(Device, "IP", "", IPConfigPath));
                IP.Port = Convert.ToInt16(ReadIniData(Device, "Port", "", IPConfigPath));
                IPList.Add(IP);
            }
        }

        public static List<IPList> IPList = new List<IPList>();
        StringBuilder temp = new StringBuilder(1024);
        string IPConfigPath = System.AppDomain.CurrentDomain.BaseDirectory +"IP-PortList.ini";
        string[] Section = { "ZCToATP", "ZCToATSAndCI", "ATS", "CI1", "CI2", "CI3", "CI4", "ATP1", "ATP2", "ATP3", "ATP4" };
        string[] Key = { "DeviceID", "IP", "Port" };
        
        #region 读Ini文件
        public string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                GetPrivateProfileString(Section, Key, NoText, temp, 1024, iniFilePath);

                return temp.ToString();
            }
            else
            {
                return String.Empty;
            }
        }
        #endregion
    }

    class IPList
    {
        public string DeviceName;
        public int DeviceID;
        public IPAddress IP;
        public int Port;
    }
}

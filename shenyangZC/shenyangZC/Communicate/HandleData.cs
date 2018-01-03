using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using shenyangZC.VOBC;
using System.Net;
using ZCToATS;
using System.IO;
using 线路绘图工具;

namespace shenyangZC
{
    class HandleData
    {
        private byte[] Data;
        private Unpack UnPack;
        static string consoleConcentToCI;
        static string consoleConcentToATP;
        static string consoleFromATP;
        public HandleData(byte[] data)
        {
            this.Data = data;
            AnalysisHead();
        }

        private void AnalysisHead()
        {
            UnPack = new Unpack();
            UnPack.Reset(Data);
            JudgeDevice(new PacketHeader()
            {
                CycleNumber = UnPack.GetUint16(),
                Type = (DataType)UnPack.GetUint16(),
                SenderID = (DeviceID)UnPack.GetByte(),
                ReceiveID = (DeviceID)UnPack.GetByte(),
                DataLength = UnPack.GetUint16()
            });
        }

        private void JudgeDevice(PacketHeader head)
        {
            if (head.SenderID == DeviceID.ATP1 || head.SenderID == DeviceID.ATP2 || head.SenderID == DeviceID.ATP3 || head.SenderID == DeviceID.ATP4)
            {
                HandleVobc VOBC = new HandleVobc(UnPack);
                ConsoleOut(VOBC.trainInfo, null, false);
                if (VOBC.trainInfo.NcTrain == NCTrain.QuitCBTCLogOutZC)
                {
                    ATPPackage InfoSendToATP = new ATPPackage();
                    InfoSendToATP.NC_ZC = 0x03;
                    VOBCQuit Quit = new VOBCQuit(VOBC.trainInfo);
                    UpdateTrainPosition.PreTrainPosition.Remove(VOBC.trainInfo.NIDTrain);
                    UpdateRoute.PreAccess.Remove(VOBC.trainInfo.NIDTrain);
                    InfoSendToATP.PackATP();
                    Send(InfoSendToATP.ATPPack.buf_, IPConfigure.IPList[0].IP, IPConfigure.IPList[0].Port, InfoSendToATP.ATPPack.byteFlag_);

                    InfoToATS infoToATS = new InfoToATS();
                    new ATS.PackToATS(infoToATS, VOBC.trainInfo);
                    Send(MySerialize.serializeObject(infoToATS), GetIPByDataType((int)DeviceID.ATS), GetPortByDataType((int)DeviceID.ATS), MySerialize.serializeObject(infoToATS).Length);
                }
                else if (VOBC.trainInfo.NcTrain == NCTrain.AskMA)
                {
                    new UpdateTrainPosition(VOBC.trainInfo);
                    InfoSendToATP infoSendToATP = new InfoSendToATP(VOBC.trainInfo, head.SenderID);
                    new UpdateRoute(VOBC.trainInfo, infoSendToATP.MA);
                    infoSendToATP.atpPackage.PackATP();
                    Send(infoSendToATP.atpPackage.ATPPack.buf_, GetIPByDataType((int)head.SenderID), GetPortByDataType((int)head.SenderID), infoSendToATP.atpPackage.ATPPack.byteFlag_);

                    InfoToATS infoToATS = new InfoToATS();
                    new ATS.PackToATS(infoToATS, VOBC.trainInfo);
                    Send(MySerialize.serializeObject(infoToATS), GetIPByDataType((int)DeviceID.ATS), GetPortByDataType((int)DeviceID.ATS), MySerialize.serializeObject(infoToATS).Length);
                    ConsoleOut(VOBC.trainInfo, infoSendToATP, true);
                }
            }
            else if (head.SenderID == DeviceID.CI1 || head.SenderID == DeviceID.CI2 || head.SenderID == DeviceID.CI3 || head.SenderID == DeviceID.CI4)
            {
                CI.HandleCIData CI = new CI.HandleCIData(UnPack, head.SenderID);
                shenyangZC.CI.PackToCI packToCI = new shenyangZC.CI.PackToCI(head.SenderID,CI);
                Send(packToCI.CIPacket.buf_, GetIPByDataType((int)head.SenderID), GetPortByDataType((int)head.SenderID), packToCI.CIPacket.byteFlag_);
            }
        }

        private void Send(byte[] Data, IPAddress IP, int port, int DataSize)
        {
            ZCSocket.SendControlData(Data, DataSize,IP,port);
        }

        public IPAddress GetIPByDataType(int SenderID)
        {
            foreach (var item in IPConfigure.IPList)
            {
                if (item.DeviceID == SenderID)
                {
                    return item.IP;
                }
            }
            return null;
        }

        public int GetPortByDataType(int SenderID)
        {
            foreach (var item in IPConfigure.IPList)
            {
                if (item.DeviceID == SenderID)
                {
                    return item.Port;
                }
            }
            return 0;
        }

        private void ConsoleOut(TrainInfo trainInfo, InfoSendToATP infoSendToATP, bool isComplete)
        {
            if (MainWindow.IsShowLog)
            {
                string content = null;
                if (!isComplete)
                {
                    content = "Receive from ATP:"
                                + "\r\n" + "NID_Train：" + trainInfo.NIDTrain +
                                " NC_Train：" + trainInfo.NcTrain +
                                " HeadPosition：" + (trainInfo.HeadPosition is Section ? trainInfo.HeadPosition.Name : ((trainInfo.HeadPosition as RailSwitch).SectionName + "-" + (trainInfo.HeadPosition as RailSwitch).Name)) + "+" + trainInfo.HeadOffset +
                                " TailPosition：" + (trainInfo.TailPosition is Section ? trainInfo.TailPosition.Name : ((trainInfo.TailPosition as RailSwitch).SectionName + "-" + (trainInfo.TailPosition as RailSwitch).Name)) + "+" + trainInfo.TailOffset +
                                " Speed:" + trainInfo.Speed + " Direction:" + (trainInfo.TrainDirection == TrainDir.Right ? "Up" : "Down") + " PSDoor:" + (trainInfo.PSDoor ? "Open" : "Close") +
                                "\r\n";
                    MainWindow.AllocConsole();
                    if (content != consoleFromATP)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(content);
                        consoleFromATP = content;
                        OutPutLogs(consoleFromATP);
                    }
                }
                else
	            {
                    content = "Send to ATP:" + "\r\n" +
                                "NC_ZC:" + infoSendToATP.atpPackage.NC_ZC + " MALength:" + infoSendToATP.atpPackage.N_Length +
                                " EndMA:" + (infoSendToATP.MA.MAEndDevice is Section ? infoSendToATP.MA.MAEndDevice.Name :(infoSendToATP.MA.MAEndDevice == null? "null":((infoSendToATP.MA.MAEndDevice as RailSwitch).SectionName) + "-" + infoSendToATP.MA.MAEndDevice.Name)) +
                                " EndMAOffset:" + infoSendToATP.atpPackage.D_MATailOff +
                                " NumOfObstacle:" + infoSendToATP.atpPackage.N_Obstacle + "  {  ";
                    foreach (var item in infoSendToATP.atpPackage.Obstacle)
                    {
                        RailSwitch obsta = MainWindow.stationElements_.Elements.Find((GraphicElement element) =>
                        {
                            if (element is RailSwitch)
                            {
                                return (element as RailSwitch).ID == item.NID_Obstacle;
                            }
                            return false;
                        }) as RailSwitch;

                        content += obsta.SectionName + "-" + obsta.Name + "(" + item.Q_Obstacle_Now + ")  ";
                    }
                    content = content + "}";                
                    MainWindow.AllocConsole();
                    if (content != consoleConcentToATP)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine(content);
                        consoleConcentToATP = content;
                        OutPutLogs(consoleConcentToATP);
                    }
                }
            }
        }

        private void ConsoleOut()
        {
            if (MainWindow.IsShowLog)
            {
                MainWindow.AllocConsole();
                string content = null;
                if (content != consoleConcentToCI)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(consoleConcentToCI);
                    consoleConcentToCI = content;
                    OutPutLogs(consoleConcentToCI);
                }
            }
        }

        private void OutPutLogs(string content)
        {
            if (MainWindow.IsSaveLog)
            {
                string path = "Log\\" + DateTime.Now.Year + " " + DateTime.Now.Month + "." + DateTime.Now.Day + ".txt";
                File.AppendAllText(path, content);
            }
        }
    }
}

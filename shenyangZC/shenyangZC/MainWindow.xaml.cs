using System.Windows;
using System.Windows.Input;
using 线路绘图工具;
using shenyangZC.Load;
using shenyangZC.VOBC;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System;
using System.ComponentModel;

namespace shenyangZC
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static StationElements stationElements_;
        public static StationTopoloty stationTopoloty_;
        public static Load.RouteCreator routeList_;
        private Point lastPosition_;
        public static bool IsShowLog = false;
        public static bool IsSaveLog = false;

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool AllocConsole();

        // 释放控制台
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FreeConsole();

        public MainWindow()
        {
            InitializeComponent();

            

            LoadGraphicElements("src\\StationElements.xml");
            LoadStationTopo("src\\StationTopoloty.xml");

            IPConfigure LoadIPConfig = new IPConfigure();
            routeList_ = Load.RouteCreator.Open();
            routeList_.LoadDevices(stationElements_.Elements);
            routeList_.InitializeDirection();
            new LoadSectionRelay();

            new ZCSocket().Start();
            //new Communicate.ZCRelaySocket().Start(); 
            new NonCommunicationTrain();
        }

        private void LoadStationTopo(string path)
        {
            stationTopoloty_ = 线路绘图工具.StationTopoloty.Open(path, stationElements_.Elements);
        }

        private void LoadGraphicElements(string path)
        {
            stationElements_ = StationElements.Open(path);
            stationElements_.AddElementsToCanvas(MainCanvas);
        }

        internal void ScrollStationCanvas(Vector vMouseMove)
        {
            MainScroll.ScrollToHorizontalOffset(MainScroll.HorizontalOffset - vMouseMove.X);
            MainScroll.ScrollToVerticalOffset(MainScroll.VerticalOffset - vMouseMove.Y);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentposition = e.GetPosition(this);
                Vector vMouseMove = currentposition - lastPosition_;
                this.ScrollStationCanvas(vMouseMove);
                lastPosition_ = currentposition;
            }
        }

        private void MainCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            lastPosition_ = e.GetPosition(this);
        }

        private void Out_Click(object sender, RoutedEventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void Net_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void OpenLogs_Click(object sender, RoutedEventArgs e)
        {
            if (this.OpenLogs.IsChecked)
            {
                IsShowLog = true;
            }
            else if (!this.OpenLogs.IsChecked)
            {
                IsShowLog = false;
            }
        }

        private void SaveLogs_Click(object sender, RoutedEventArgs e)
        {
            if (this.SaveLogs.IsChecked)
            {
                IsSaveLog = true;
            }
            else if (!this.SaveLogs.IsChecked)
            {
                IsSaveLog = false;
            }
        }
    }
}

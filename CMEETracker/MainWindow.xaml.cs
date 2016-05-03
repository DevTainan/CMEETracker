using CMEETracker.Core;
using CMEETracker.ViewModels;
using System;
using System.Configuration;
using System.Windows;
using System.Windows.Media;

namespace CMEETracker
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        private CMEECore _cmee;
        private CMEESetting _cmeeSetting;

        public MainWindow()
        {
            InitializeComponent();

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledException);

            LoadConfigToUI();
            Init();
        }

        private void UnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            lblMsg.Text = e.Message;
        }

        private void LoadConfigToUI()
        {
            //txtQueueName.Text = ConfigurationManager.AppSettings["ServerQueue"];
            txtQueueName.Text = "VA7_test";
            txtIP.Text = ConfigurationManager.AppSettings["IPAddress"];
            txtPort.Text = ConfigurationManager.AppSettings["PortNumber"];
            txtMaxSize.Text = ConfigurationManager.AppSettings["maxSize"];
            txtMaxCount.Text = ConfigurationManager.AppSettings["maxCount"];
            txtTimeOut.Text = (3 * 10000).ToString();
        }

        private void Init()
        {
            _cmeeSetting = new CMEESetting
            {
                //QueueName = txtQueueName.Text,
                QueueName = ConfigurationManager.AppSettings["ServerQueue"],
                IPAddress = txtIP.Text,
                PortNumber = Convert.ToInt32(txtPort.Text),
                McmqMaxSize = Convert.ToInt32(txtMaxSize.Text),
                McmqMaxCount = Convert.ToInt32(txtMaxCount.Text),
                McmqTimeOut = Convert.ToInt32(txtTimeOut.Text),
            };
        }

        private void InitCMEE()
        {
            string QueueName = _cmeeSetting.QueueName;
            string IPAddress = _cmeeSetting.IPAddress;
            int PortNumber = _cmeeSetting.PortNumber;
            int McmqMaxSize = _cmeeSetting.McmqMaxSize;
            int McmqMaxCount = _cmeeSetting.McmqMaxCount;
            int McmqTimeOut = _cmeeSetting.McmqTimeOut;

            _cmee = new CMEECore();
            _cmee.Init(QueueName, IPAddress, PortNumber, McmqMaxSize, McmqMaxCount, McmqTimeOut);
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if (_cmee != null && _cmee.IsConnected)
            {
                _cmee.Close();
                _cmee.Disconnect();
            }
            else
            {
                Init();
                InitCMEE();
                _cmee.Connect();
                _cmee.Open();
            }

            ChangeConnectStatus();
        }

        private void ChangeConnectStatus()
        {
            if (_cmee.IsConnected)
            {
                lblConnectTip.Text = "Connected";
                lblConnectTip.Background = new SolidColorBrush(Colors.Lime);
            }
            else
            {
                lblConnectTip.Text = "Disconnected";
                lblConnectTip.Background = new SolidColorBrush(Colors.Red);
            }
        }

        private void btnPut_Click(object sender, RoutedEventArgs e)
        {
            string message = txtPut.Text;
            _cmee.Put(txtQueueName.Text, message);
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            string message = _cmee.Get();
            txtGet.Text = message;
        }
    }
}

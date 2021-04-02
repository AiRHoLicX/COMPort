using System;
using System.Windows;
using System.IO.Ports;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.Windows.Input;

namespace COMPort
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static SerialPort _comPort;
        private String port;
        
        public MainWindow()
        {
            InitializeComponent();

            foreach (String s in SerialPort.GetPortNames())
            {
                comPorts.Items.Add(s);
            }
            //comPorts.Items.Add("test");
            btStop.IsEnabled = false;

            if(Properties.Settings.Default.StartByStart == true)
            {
                startBy.IsChecked = true;
                comPorts.Text = Properties.Settings.Default.COMPort;
                StartRead();
            }

        }

        

        private void Write()
        {
            _comPort.WriteLine("test_547896_text");
        }

        private void btStart_Click(object sender, RoutedEventArgs e)
        {
            StartRead();            

        }
        
        private void StartRead()
        {
            _comPort = new SerialPort();
            port = comPorts.Text;
            if (port == "") { MessageBox.Show("Trage Bitte eine COM Schnittstelle ein!!"); return; }
            _comPort.PortName = port;
            _comPort.BaudRate = 9600;
            _comPort.StopBits = StopBits.One;
            _comPort.DataBits = 8;
            _comPort.Parity = Parity.None;
            _comPort.DtrEnable = false;
            _comPort.RtsEnable = false;

            _comPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedEventHandler);

            btStart.IsEnabled = false;
            comPorts.IsEnabled = false;
            btStop.IsEnabled = true;

            try
            {
                _comPort.Open();


            }
            catch (Exception ex)
            {

                MessageBox.Show("Start: " + ex.ToString());
            }
        }
        private void btStop_Click(object sender, RoutedEventArgs e)
        {
            btStart.IsEnabled = true;
            comPorts.IsEnabled = true;
            btStop.IsEnabled = false;
            try
            {
               
                _comPort.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Stop: " + ex.ToString());
            }

        }

        private void DataReceivedEventHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                SendKeys.SendWait(sp.ReadLine());
                sp = null;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Lesen: " + ex.ToString());
            }
            

        }

       

        private void startBy_Click(object sender, RoutedEventArgs e)
        {
            if(comPorts.Text != "")
            {
                if(startBy.IsChecked == true)
                {
                    Properties.Settings.Default.StartByStart = true;
                    Properties.Settings.Default.COMPort = comPorts.Text;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    Properties.Settings.Default.StartByStart = false;
                    Properties.Settings.Default.COMPort = "";
                    Properties.Settings.Default.Save();
                }
            }
            else
            {
                MessageBox.Show("Trage Bitte eine COM Schnittstelle ein!!");
                startBy.IsChecked = false;
            }
        }
    }
}

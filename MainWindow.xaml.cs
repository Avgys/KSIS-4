using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace WpfApp1
{
    using MySockets;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySocket sock;
        public MainWindow()
        {
            InitializeComponent();
            sock = new MySocket();
            output = "";
            sock.Connect("aspmx.l.google.com", 25);
            Thread RecThread = new Thread(RecFromServer);
            RecThread.Start();
            //string input = "HELO 185.152.139.93\r\n";
            //sock.Send(input);
            //output = "";
        }

        string output;

        private void RecFromServer()
        {
            int bytesRec;
            byte[] temp = new byte[1024];
            List<byte> buf = new List<byte>();
           
            while ((bytesRec = sock.Receive(temp)) != 0)
            {
                buf.Clear();
                for (int i = 0; i < bytesRec; i++)
                    buf.Add(temp[i]);
                temp = buf.ToArray();  
                    output += Encoding.UTF8.GetString(temp, 0, temp.Length);
                    //ServerToClient.Text = outp;
                    Dispatcher.Invoke(() => { 
                                                ServerToClient.Text = output;
                                            });               
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string input = ClientToServer.Text;
            input += "\r\n";
            byte[] temp = Encoding.UTF8.GetBytes(input);
            output = "";
            sock.Send(temp);
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            string input = "HELO 185.152.139.93\r\n";
            sock.Send(input);
            input = "MAIL FROM: <" + FromEmail.Text + ">";
            input += "\r\n";            
            sock.Send(input);
            input = "RCPT TO: <" + ToEmail.Text + ">"; 
            input += "\r\n";
            sock.Send(input);
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string input = "DATA";
            input += "\r\n";
            sock.Send(input);
            input = ClientInput.Text;
            input += "\r\n.\r\n";
            sock.Send(input);
            ClientInput.Text = "";
        }
    }
}

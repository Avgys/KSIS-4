using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;

namespace WpfApp1
{
    using MySockets;
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MySocket sock;
        string ip;
        public MainWindow()
        {
            InitializeComponent();
            //var req = WebRequest.Create("https://api.ipify.org/?");
            //string reqstring;

            //using (var reader = new StreamReader(req.GetResponse().GetResponseStream()))
            //{
            //    reqstring = reader.ReadToEnd();
            //}
            ////string[] a = reqstring.Split(':');
            ////string a2 = a[1].Substring(1);
            ////string[] a3 = a2.Split('<');
            //ip = reqstring;
            //sock = new MySocket(ip);
            //output = "";
            //sock.Connect("aspmx.l.google.com", 25);
            //Thread RecThread = new Thread(RecFromServer);
            //RecThread.Start();
            //string input = "HELO 185.152.139.93\r\n";
            //sock.Send(input);
            //output = "";
        }

        //private void RecFromServer()
        //{
        //    int bytesRec;
        //    byte[] temp = new byte[1024];
        //    List<byte> buf = new List<byte>();
           
        //    while ((bytesRec = sock.Receive(temp)) != 0)
        //    {
        //        buf.Clear();
        //        for (int i = 0; i < bytesRec; i++)
        //            buf.Add(temp[i]);
        //        temp = buf.ToArray();  
        //            output += Encoding.UTF8.GetString(temp, 0, temp.Length);
        //            //ServerToClient.Text = outp;
        //            Dispatcher.Invoke(() => { 
        //                                        ServerToClient.Text = output;
        //                                    });               
        //    }
        //}

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string input = ClientToServer.Text;
            SendMessage(input);
        }

        private void Assign_Click(object sender, RoutedEventArgs e)
        {
            //SslStream mainStream = new SslStream(client.GetStream());

            //mainStream.ReadTimeout = 1000;
            //mainStream.WriteTimeout = 1000;
            ////authPasswordForSmtpMailRu
            //mainStream.AuthenticateAsClient(host, null, System.Security.Authentication.SslProtocols.Tls, false);

            //string input = "EHLO " + ip + "\r\n";
            //sock.Send(input);
            //input = "MAIL FROM: <" + FromEmail.Text + ">";
            //input += "\r\n";            
            //sock.Send(input);
            //input = "RCPT TO: <" + ToEmail.Text + ">"; 
            //input += "\r\n";
            //sock.Send(input);
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        SslStream stream;

        public bool SendMessage(string Message)
        {
            try
            {
                const string CRLF = "\r\n";

                byte[] msgbytes = Encoding.UTF8.GetBytes(Message + CRLF);
                stream.Write(msgbytes, 0, msgbytes.Length);
                ServerToClient.Text = ReadAnswer();
                return true;
            }
            catch
            {
                return false;
            }

        }

        public string ReadAnswer()
        {
            try
            {

                byte[] msgbytes = new byte[1024];
                int byteread = this.stream.Read(msgbytes, 0, 1024);
                string answer = Encoding.ASCII.GetString(msgbytes, 0, byteread);
                return answer;
            }
            catch
            {
                return "";
            }

        }

        StringBuilder output;

        private void Send_Click(object sender, RoutedEventArgs e)
        {

            var req = WebRequest.Create("https://api.ipify.org/?");
            string reqstring;

            using (var reader = new StreamReader(req.GetResponse().GetResponseStream()))
            {
                reqstring = reader.ReadToEnd();
            }
            //string[] a = reqstring.Split(':');
            //string a2 = a[1].Substring(1);
            //string[] a3 = a2.Split('<');
            ip = reqstring;
            TcpClient sock = new TcpClient();
            output = new StringBuilder();

            string host = "smtp.gmail.com";
            sock.Connect(host, 465);
            ////RecFromServer();
            //byte[] temp = new byte[1024];
            //int bytesRec = sock.Receive(temp);
            //ServerToClient.Text += Encoding.UTF8.GetString(temp, 0, temp.Length);
            stream = new SslStream(sock.GetStream());

            stream.ReadTimeout = 1000;
            stream.WriteTimeout = 1000;
            //authPasswordForSmtpMailRu
            stream.AuthenticateAsClient(host, null, System.Security.Authentication.SslProtocols.Tls, false);      

            string input = "EHLO " + ip;
            SendMessage(input);
            
            output.AppendLine(ReadAnswer());


            SendMessage("AUTH LOGIN");
            SendMessage(Base64Encode(FromEmail.Text));
            SendMessage(Base64Encode(clientPassword.Text));
            output.AppendLine(ReadAnswer());
            
            SendMessage("MAIL FROM:<" + FromEmail.Text + ">");

            output.AppendLine(ReadAnswer());
            SendMessage("RCPT TO:<" + ToEmail.Text + ">");

            output.AppendLine(ReadAnswer());
            SendMessage("DATA");
            SendMessage("Subject: tema");
            output.AppendLine(ReadAnswer());

            SendMessage(ClientInput.Text);
            SendMessage(".");
            output.AppendLine(ReadAnswer());
            ClientInput.Text = "";
            SendMessage("QUIT");
            output.AppendLine(ReadAnswer());
            stream.Close();
            sock.Close();
            output.AppendLine(ReadAnswer());
            ServerToClient.Text = output.ToString();
        }
    }
}

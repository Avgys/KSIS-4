using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace MySockets
{
     class MySocket{

        public Socket _Sock;

        public MySocket(string Ip = "localhost", SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp)
        {
            IPAddress ipAddr = Dns.GetHostEntry(Ip).AddressList[0];
            _Sock = new Socket(ipAddr.AddressFamily, socketType, protocolType);
        }

        public Socket CreateSocket(string Ip = "localhost",  SocketType socketType = SocketType.Stream, ProtocolType protocolType = ProtocolType.Tcp)
        {            
            IPAddress ipAddr = Dns.GetHostEntry(Ip).AddressList[0];
            return new Socket(ipAddr.AddressFamily, socketType, protocolType);
        }

        public int Send(byte[] msg)
        {
            return _Sock.Send(msg);
        }

        public int Send(string msg)
        {
            return _Sock.Send(Encoding.UTF8.GetBytes(msg));
        }

        public int Receive(byte[] bytes)
        {
            return _Sock.Receive(bytes);
        }

        public void Listen(int count)
        {
            _Sock.Listen(count);
        }

        public void Bind(string input = "localhost", int port = 513)
        {
            IPEndPoint ipEndPoint = new((Dns.GetHostEntry("localhost")).AddressList[0], port);
            _Sock.Bind(ipEndPoint);
            Listen(4);
        }

        public void Connect(string input = "localhost", int port = 25)
        {
            IPEndPoint ipEndPoint = new((Dns.GetHostEntry(input)).AddressList[0], port);
            _Sock.Connect(ipEndPoint);
        }
        
        public Socket Accept()
        {
            return _Sock.Accept();
        }

        public void ShutDown(int way = 3)
        {
            if (way == 3)
            _Sock.Shutdown(SocketShutdown.Both);
        }

        public void Close()
        {
            _Sock.Close();
        }
    }
}
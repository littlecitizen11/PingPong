using Ping;
using Pong;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PingPong
{

    class Program
    {
   
        static void Main(string[] args)
        {
            ClientPing cp = new ClientPing();
            ServerPong sp = new ServerPong();
        }
    }
}

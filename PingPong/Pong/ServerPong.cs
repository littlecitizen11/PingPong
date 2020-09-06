using Common;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Pong
{
    public class ServerPong
    {
        public static void StartServer()
        {
            IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
            IPAddress ipAddress = host.AddressList[0];
            Console.WriteLine("Enter port please");
            int port = int.Parse(Console.ReadLine());
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port);
            try
            {
                var server = new TcpListener(ipAddress, port);
                server.Start();
                Console.WriteLine("Waiting for a connection...");
                Serializations serializations = new Serializations();
                byte[] bytes = null;
                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Task t = Task.Factory.StartNew(() =>
                    {
                        using (NetworkStream networkStream = client.GetStream())
                        {
                            while (true)
                            {
                                bytes = new byte[client.ReceiveBufferSize];
                                networkStream.Read(bytes, 0, bytes.Length);
                                Person newPerson = (Person)serializations.ByteArrayToObject(bytes);
                                Console.WriteLine(newPerson.ToString());
                                bytes = serializations.ObjectToByteArray(newPerson);
                                networkStream.Write(bytes, 0, bytes.Length);
                                if (newPerson.ToString().Contains("<EOF>"))
                                {
                                    break;
                                }
                            }

                        }
                        client.Dispose();
                        client.Close();

                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\n Press any key to continue...");
            Console.ReadKey();
        }
        static void Main(string[] args)
        {
            StartServer();
        }
    }
}

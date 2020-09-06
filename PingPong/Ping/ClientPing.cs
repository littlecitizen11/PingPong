using System;
using Common;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Ping
{
    public class ClientPing
    {



        public static void StartClient()
        {
            try
            {
                IPHostEntry host = Dns.GetHostEntry("127.0.0.1");
                IPAddress ipAddress = host.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 1234);
                using var client = new TcpClient();
                try
                {
                    while (client.Connected == false)
                    {
                        try
                        {
                            client.Connect(remoteEP);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Waiting for server...");
                        }
                    }
                    Console.WriteLine("Enter Name and age to client to end enter EOF in name or -1 in age");
                    string name;
                    int age;
                    byte[] msg;
                    Serializations serializations = new Serializations();
                    NetworkStream networkStream = client.GetStream();
                    while ((name = Console.ReadLine()) != "EOF" && (age = int.Parse(Console.ReadLine())) != -1)
                    {
                        Person person = new Person(name, age);
                        msg = serializations.ObjectToByteArray(person);
                        networkStream.Write(msg, 0, msg.Length);
                        networkStream.Read(msg, 0, msg.Length);
                        Person newPerson = (Person)serializations.ByteArrayToObject(msg);
                        Console.WriteLine(newPerson.ToString());
                        Console.WriteLine("Enter Name and age to client to end enter EOF in name or -1 in age");
                    }
                    client.Dispose();
                    client.Close();
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
        static void Main(string[] args)
        {
            StartClient();
        }
    }
}

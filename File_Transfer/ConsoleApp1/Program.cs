using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1

{

    class Program

    {

        public static void doChat(Socket clientSocket,string n)

        {
            Console.WriteLine("getting file....");
            byte[] clientData = new byte[1024 * 5000];
            int receivedBytesLen = clientSocket.Receive(clientData);
            int fileNameLen = BitConverter.ToInt32(clientData, 0);
            string fileName = Encoding.ASCII.GetString(clientData, 4, fileNameLen);
            BinaryWriter bWrite = new BinaryWriter(File.Open(fileName + n, FileMode.Create));
            bWrite.Write(clientData, 4 + fileNameLen, receivedBytesLen - 4 - fileNameLen);
            bWrite.Close();
            clientSocket.Close();

            //[0]filenamelen[4]filenamebyte[*]filedata   

        }




        static void Main(string[] args)

        {


            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

            Console.WriteLine("Starting TCP listener...");

            IPEndPoint  ipEnd = new IPEndPoint(ipAddress, 3004); 
            Socket serverSocket= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);;
            serverSocket.Bind(ipEnd); 

            int counter = 0;

            serverSocket.Listen(3004);

            Console.WriteLine(" >> " + "Server Started"); 

            while (true)

            {

                counter += 1;


                Socket clientSocket = serverSocket.Accept();

                Console.WriteLine(" >> " + "Client No:" + Convert.ToString(counter) + " started");


                new Thread(delegate () {
                    doChat(clientSocket, Convert.ToString(counter));
                }).Start();



            }


        }

    }

}


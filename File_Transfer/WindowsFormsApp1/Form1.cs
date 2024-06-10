using System;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Text;
using System.IO;
using System.Net;

namespace WindowsFormsApp1

{

    public partial class Form1 : Form

    {





        public Form1()

        {

           InitializeComponent();

        }



        private void Form1_Load(object sender, EventArgs e)

        {






        }

        private void sendfile(string fn)

        {


            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint ipEnd = new IPEndPoint(ipAddress, 3004);
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);

            string fileName = fn;// "c:\\filetosend.txt";
            byte[] fileNameByte = Encoding.ASCII.GetBytes(fileName);
            byte[] fileNameLen = BitConverter.GetBytes(fileNameByte.Length);
            byte[] fileData = File.ReadAllBytes(fileName);
            byte[] clientData = new byte[4 + fileNameByte.Length + fileData.Length];
            
            fileNameLen.CopyTo(clientData, 0);
            fileNameByte.CopyTo(clientData, 4);
            fileData.CopyTo(clientData, 4 + fileNameByte.Length);
            clientSocket.Connect(ipEnd);
            clientSocket.Send(clientData);
            clientSocket.Close();

            //[0]filenamelen[4]filenamebyte[*]filedata     


        }



        private void button1_Click(object sender, EventArgs e)

        {


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_DragDrop(object sender, DragEventArgs e)
        {


            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                Console.WriteLine(files[0]);
                sendfile(files[0]);
            }


        }

        private void textBox1__DragEnter(object sender, DragEventArgs e)
        {

            if (e.Data.GetDataPresent(DataFormats.FileDrop, false) == true)
            {
                e.Effect = DragDropEffects.All;
            }

        }
    }

}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using System.Collections;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;// socket.io for .NET (Client)
using System.Net;

namespace Web_Server_Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = content_send.Text;
            socketIoManager(1, str + ":");//发送 数据队列
        }


        private void Connect_Click(object sender, EventArgs e)
        {
            string data = null;
            socketIoManager(0, data);
        }

        private void socketIoManager(int send, string dataToSend)
        {

            //Instantiate the socket.io connection
            string serveraddress = IP.Text;
            string serverport = port.Text;

            var socket = IO.Socket(serveraddress + ":" + serverport);
            //Upon a connection event, update our status

            if (send == 0)
            {
                //timer2.Enabled = true;
                socket.On(Socket.EVENT_CONNECT, () =>
                {
                    MessageBox.Show("connect server successfully");
                    //btn_download.Visible = true;
                });

                socket.On("result", (data) =>
                {
                    // var temperature = new { temperature = "" };
                    // MessageBox.Show((string)data);
                });
            }
            else
            {
                socket.Emit("result", dataToSend);
            }
        }

        private void FDA_S_Click(object sender, EventArgs e)
        {
            int length = Convert.ToInt32(FDA_S_times.Text) ;
            string str = "";
            int y1, y2, y3;
            for (int i = 0; i < length; i++)
            {
                y1 = Convert.ToInt32(Math.Truncate(3000 + 1000 * Math.Sin(2 * 3.14 * i * 1000)));
                y2 = y1 + 1000;
                y3 = y1 - 1000;
                str = str + y1.ToString() + ',';
                str = str + y2.ToString() + ',';
                str = str + y3.ToString() + ',';
                str += '0';
                socketIoManager(1, "AA,FF,FF,AA,1,"+str + ":");//发送 数据队列
                Thread.Sleep(10);
                str = "";
            }
        }

        private void FDA_M_Click(object sender, EventArgs e)
        {
            int length = Convert.ToInt32(FDA_M_times.Text);
            string str = "";
            int cnt = 0;
            int y1, y2, y3;
            for (int i = 0; i < length; i++)
            {
                cnt++;
                y1 = Convert.ToInt32(Math.Truncate(3000 + 1000 * Math.Sin(2 * 3.14 * i * 1000)));
                y2 = y1 + 1000;
                y3 = y1 - 1000;
                str = str + y1.ToString() + ',';
                str = str + y2.ToString() + ',';
                str = str + y3.ToString() + ',';
                str += cnt.ToString();
                socketIoManager(1, "AA,FF,FF,AA,1," + str + ":");//发送 数据队列
                Thread.Sleep(20);
                str = "";
                if (cnt > 49)
                    cnt = 0;
            }
        }

        private void TD_Click(object sender, EventArgs e)
        {
            int length = Convert.ToInt32(FDA_S_times.Text);
            string str = "";
            int y1, y2, y3;
            for (int i = 0; i < length; i++)
            {
                y1 = Convert.ToInt32(Math.Truncate(3000 + 1000 * Math.Sin(2 * 3.14 * i * 1000)));
                y2 = y1 + 1000;
                y3 = y1 - 1000;
                str = str + y1.ToString() + ',';
                str = str + y2.ToString() + ',';
                str = str + y3.ToString() + ',';
                str += '0';
                socketIoManager(1, "AA,FF,FF,AA,5," + str + ":");//发送 数据队列
                Thread.Sleep(10);
                str = "";
            }
        }

        private void Comb_Click(object sender, EventArgs e)
        {
            int length = Convert.ToInt32(FDA_M_times.Text);
            string str = "";
            int cnt = 0;
            int y1, y2, y3;
            for (int i = 0; i < length; i++)
            {
                cnt++;
                y1 = Convert.ToInt32(Math.Truncate(3000 + 1000 * Math.Sin(2 * 3.14 * i * 1000)));
                y2 = y1 + 1000;
                y3 = y1 - 1000;
                str = str + y1.ToString() + ',';
                str = str + y2.ToString() + ',';
                str = str + y3.ToString() + ',';
                str += cnt.ToString();
                socketIoManager(1, "AA,FF,FF,AA,4," + str + ":");//发送 数据队列
                Thread.Sleep(20);
                str = "";
                if (cnt > 49)
                    cnt = 0;
            }
        }

        private void DC_Click(object sender, EventArgs e)
        {
            int length = Convert.ToInt32(FDA_S_times.Text);
            string str = "";
            int y1, y2;
            for (int i = 0; i < length; i++)
            {
                str = str + '0' + ',';
                y1 = Convert.ToInt32(Math.Truncate(3000 + 1000 * Math.Sin(2 * 3.14 * i * 1000)));
                y2 = y1 + 1000;
                str = str + y1.ToString() + ',';
                str = str + y2.ToString() ;               
                socketIoManager(1, "AA,FF,FF,AA,3," + str + ":");//发送 数据队列
                Thread.Sleep(10);
                str = "";
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}

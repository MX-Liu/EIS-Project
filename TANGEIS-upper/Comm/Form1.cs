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
//using Quobject.SocketIoClientDotNet.Client;// socket.io for .NET (Client)
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;



namespace Comm
{

    public partial class Form1 : Form
    {
        bool ACM = false;

        //串口数据定义
        static bool PortIsOpen = false;
        private static byte[] result = new byte[1024];
        
        //图形数据定义
        private int time = 0;
        private float vol = 0;
        private float fre = 1;
        private float mag = 0;
        private float pha = 0;
        private Int64 cnt_x = 0;
        private Queue chart_x = new Queue();
        private Queue chart_y = new Queue();
        private Queue data_queue = new Queue();

        //无线数据定义
        private IPAddress serverIP;
        private IPEndPoint serverFullAddr;
        private Socket sock;
        private Socket newSocket;
        Thread myThread = null;



        private string fest_str(string s,int n)
        {
            string feststr;

                feststr = Convert.ToInt32(s).ToString().PadLeft(n, '0');

            return feststr;
        }


        private string strToHex(string s)
        {
            if ((s.Length % 2) != 0)
                s = fest_str(s, s.Length + 1);
            int b = Convert.ToInt32(s);

            string HexStr = b.ToString("X");
            return HexStr;

        }
        private byte[] exchange(byte[] array)
        { 
            byte n;

                for (int i=0; i<array.Length/2; i++)

                {
                    n=array[i];
                    array[i]=array[array.Length - i - 1];
                    array[array.Length - i - 1]=n;
                }
            return array;
        }

        private byte[] strToByte(string s)
        {
            byte[] b = Encoding.ASCII.GetBytes(s);//按照指定编码将string编程字节数组
            byte[] result = new byte[s.Length ];
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                if ((b[i]=='A') || b[i] == 'B' || b[i] == 'C' || b[i] == 'D' || b[i] == 'E' || b[i] == 'F')
                {
                    result[i] = Convert.ToByte(Convert.ToInt32(b[i]) - 55);
                }
                else
                {
                    result[i] = Convert.ToByte(Convert.ToInt32(b[i]) - 48);
                }

            }

            return result;
        }



        private string StringToHexString(string s)

        {
            //byte[] b = Encoding.ASCII.GetBytes(s);//按照指定编码将string编程字节数组
            char[] c = s.ToCharArray() ;
            string result = string.Empty;
            for (int i = 0; i < c.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                //result += Convert.ToString(int.Parse(c[i].ToString()));
                result += Convert.ToString(Convert.ToInt32(c[i])-48);
                //result += Convert.ToString(c[i], 16);

            }
            test.AppendText(Convert.ToString(c[0]));
            return result;
        }

        /// 16进制编码发送
        //private void SendHexByte(string send_data)
        private byte[] strToHexByte(string Str)
        {

            //Str = Str.Replace(" ", "");
            string hexString = strToHex(Str);
            //string hexString = Str;


            //string hexString = StringToHexString(Str);
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0"+  hexString;
            test.AppendText(hexString);
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;


            //if ((hexString.Length % 2) != 0)
            //    hexString = fest_str(hexString, hexString.Length + 1);
            ////returnBytes[0] = Convert.ToByte(hexString.Substring(Str.Length - 1, 1), 16);
            //byte[] returnBytes = new byte[hexString.Length / 2];
            ////serialPort1.Write(returnBytes, 0, 1);  //循环发送
            //for (int i = 0; i < returnBytes.Length; i++)
            //{
            //    //hexString = fest_str(hexString, Str.Length + 1);
            //    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2));
            //    test.AppendText(returnBytes[0].ToString());
            //}
            //    //test.AppendText(temp2[1].ToString());
            ////serialPort1.Write(returnBytes, 0, 1);  //循环发送
            //return returnBytes;
        }


        //*********************************************//
        //**************数据初始化********************//
        //*******************************************//
        public Form1()
        {
            
            InitializeComponent();
            COMChoose.Text = "COM3";
            CheckBitChoose.Text = "NONE";
            StopBitChoose.Text = "1";
            DataBitChoose.Text = "8";
            BaudrateChoose.Text = "9600";
            rb_Frequncy.Checked = true;
            cb_freq.Text = "Hz";
            cb_freq_f.Text = "Hz";
            cb_freq_t.Text = "Hz";
            cb_dor.Text = "5";
            cb_tia.Text = "0";
            cb_dft.Text = "1";
            cb_days.Text = "7";

            SaveFilePath.Visible = true;
            ChooseFile.Visible = true;
            CheckForIllegalCrossThreadCalls = false;
            enableButtons(true, true, true, true, false, false,true);
            //SaveFilePath.Visible = false;
            //ChooseFile.Visible = false;
            //var chart = new LightningChartUltimate();

        }

 


        //*********************************************//
        //**************界面清空**********************//
        //*******************************************//

        private void ClearReceive_Click(object sender, EventArgs e)
        {
            ReceiveArea.Clear();
            this.chart1.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart2.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart4.ChartAreas[0].AxisX.IsLogarithmic = false;
            // this.chart5.ChartAreas[0].AxisX.IsLogarithmic = false;
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            // chart5.Series[0].Points.Clear();
        }

        private void ClearSendArea_Click(object sender, EventArgs e)
        {
            SendArea.Clear();
        }


        //*********************************************//
        //**************串口传输**********************//
        //*******************************************//

        //打开串口
        private void OpenPort_Click(object sender, EventArgs e)
        {
            int PortBaudrate, PortDataBits;
            Parity PortParity;
            StopBits PortStopBits;
            if (!PortIsOpen)
            {
                PortBaudrate = Convert.ToInt32(BaudrateChoose.Text);
                PortDataBits = Convert.ToInt32(DataBitChoose.Text);
                PortParity = (Parity)Enum.Parse(typeof(Parity), CheckBitChoose.Text);
                PortStopBits = (StopBits)Enum.Parse(typeof(StopBits), StopBitChoose.Text);
                //获取串口设置
                try
                {
                    serialPort1.PortName = COMChoose.Text;
                    serialPort1.BaudRate = PortBaudrate;
                    serialPort1.DataBits = PortDataBits;
                    serialPort1.Parity = PortParity;
                    serialPort1.StopBits = PortStopBits;
                    //设置串口
                }
                catch
                {
                    MessageBox.Show("COM configure ERROR");
                    PortIsOpen = false;
                    OpenPortButton.Text = "Open COM";
                    return;
                }

                try
                {
                    serialPort1.Open();
                    PortIsOpen = true;
                    OpenPortButton.Text = "Close COM";
                }
                catch
                {
                    MessageBox.Show("Fail to open COM");
                    PortIsOpen = false;
                    OpenPortButton.Text = "Open COM";
                }
            }
            else
            {
                serialPort1.Close();
                timer2.Enabled = false;
                PortIsOpen = false;
                OpenPortButton.Text = "Open COM";
            }
        }
        
        //发送数据
        private void button3_Click(object sender, EventArgs e)//对应Send（button）
        {
            byte[] sendbuff = new byte[1];
            if (PortIsOpen == true)
            {
                if (SendArea.Text != "")
                {
                    try
                    {
                        if (!HexSend.Checked)
                        {
                            serialPort1.Write(SendArea.Text);
                            //字符发送
                        }
                        else
                        {
                            for (int i = 0; i < (SendArea.Text.Length - SendArea.Text.Length % 2) / 2; i++)
                            {
                                sendbuff[0] = Convert.ToByte(SendArea.Text.Substring(i * 2, 2), 16);
                                serialPort1.Write(sendbuff, 0, 1);
                            }
                            if (SendArea.Text.Length % 2 != 0)
                            {
                                sendbuff[0] = Convert.ToByte(SendArea.Text.Substring(SendArea.Text.Length - SendArea.Text.Length % 2, SendArea.Text.Length % 2));
                                serialPort1.Write(sendbuff, 0, 1);
                            }
                            //十六进制发送，先分开每两位一组发送，如果为奇数位，则最后一位单独发送
                            string temp = SendArea.Text;
                        }
                    }
                    catch
                    {
                        MessageBox.Show("data erorr");
                        serialPort1.Close();
                        PortIsOpen = false;
                        OpenPortButton.Text = "Port Open";
                    }
                }

            }
        }

        //定义串口接收事件
        private void Port_DataRecevied(object sender, SerialDataReceivedEventArgs e)
        {
           
            if (!PutIntoFile.Checked)
            {

                if (!Hexshow.Checked)
                {
                    //ACM = true;
                    if (ACM == true)//cfg界面
                    {
                        string strv = serialPort1.ReadExisting();
                        //string strv = serialPort1.ReadLine();
                        ReceiveArea.AppendText(strv);
                        cnt_x++;
                        this.chart4.Series[0].Points.AddXY(cnt_x, strv);                   
                    }
                   else //非cfg界面   （暂时为频域）
                    { 
                        string str = serialPort1.ReadExisting();
                        //string str = serialPort1.ReadLine();
                        ReceiveArea.AppendText(str);
                        string[] strArr = str.Split(',');
                        int length = strArr.Length;

                        //string length1 = Convert.ToString(length);
                        //test.AppendText(length1);

                        if (length == 3 && strArr[0] != "200000.00")
                        {
                            try
                            {
                                fre = Convert.ToSingle(strArr[0]);
                                mag = Convert.ToSingle(strArr[1]);
                                pha = Convert.ToSingle(strArr[2]);

                                int fre_int = (int)(fre * 100);
                                fre = fre_int / 100;



                                double real = mag * Math.Cos(pha * Math.PI / 180);
                                double img = mag * Math.Sin(-pha * Math.PI / 180);

                                int real_int = (int)(real * 100);
                                real = real_int / 100;

                                int img_int = (int)(img * 100);
                                img = img_int / 100;

                                this.chart1.Series[0].Points.AddXY(fre, mag);
                                this.chart2.Series[0].Points.AddXY(fre, pha);
                                this.chart3.Series[0].Points.AddXY(real, img);
                                this.chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
                                this.chart2.ChartAreas[0].AxisX.IsLogarithmic = true;

                                data_queue.Enqueue(str + ":");
                                //socketIoManager(1, str);

                            }
                            catch
                            {
                                MessageBox.Show("图表未工作");
                                Console.WriteLine("error");

                            }
                        }

                    }
                    
                }
                else
                {
                    byte data;
                    data = (byte)serialPort1.ReadByte();
                    string datareceive = Convert.ToString(data, 16).ToUpper();
                    ReceiveArea.AppendText("0x" + (datareceive.Length == 1 ? "0" + datareceive : " "));
                    //十六进制接收，分开，填“0x”
                }
            }
            else
            {
                //接收到文件，方式同上
                if (!Hexshow.Checked)
                {
                    string datareceive = serialPort1.ReadExisting();
                    File.AppendAllText(SaveFilePath.Text, datareceive);
                }
                else
                {
                    byte data;
                    data = (byte)serialPort1.ReadByte();
                    string datareceive = Convert.ToString(data, 16).ToUpper();
                    File.AppendAllText(SaveFilePath.Text, "0x" + (datareceive.Length == 1 ? "0" + datareceive : " "));
                }
            }

        }

        //接收事件委托
        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(Port_DataRecevied);
            
        }

        //从文件发送
        private void SendFile_Click(object sender, EventArgs e)
        {
            
            int SendBit = Convert.ToInt32(ByteNum.Text);
            int WaitTime = Convert.ToInt32(Time.Text);
            int i;
            byte[] str = File.ReadAllBytes(SendFileName.Text);
            try
            {
                for (i = 0; i < (str.Length / SendBit); i++)
                {
                    serialPort1.Write(str, i, SendBit);
                    Thread.Sleep(WaitTime);
                }
                serialPort1.Write(str, i, str.Length - (str.Length / SendBit) * SendBit);
            }
            catch
            {
                MessageBox.Show("Send Data Error");
                serialPort1.Close();
                PortIsOpen = false;
                OpenPortButton.Text = "Open COM";
            }

        }


        //*********************************************//
        //**************无线传输**********************//
        //*******************************************//

        //建立连接
        private void button1_Click_1(object sender, EventArgs e)
        {
            string data = null;
            serverIP = IPAddress.Parse(ipaddress.Text);
            try
            {
                serverFullAddr = new IPEndPoint(serverIP, int.Parse(Port.Text));//设置IP，端口
                sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                sock.Connect(serverFullAddr);
                socket.Enabled = false;
                MessageBox.Show("connect server successfully");
                btnStop.Enabled = true;
                myThread = new Thread(new ThreadStart(Listen));
                myThread.Start();
            }
            catch (Exception ee)
            {
                MessageBox.Show("connect server failed");
            }
        }

        //读取字节
        private delegate void PrintRecvMssgDelegate(string s);
        private void Listen()
        {
            byte[] message = new byte[1024];
            string mess = "";
            while (true)
            {
                int bytes = sock.Receive(message);
                mess = Encoding.Default.GetString(message, 0, bytes);
                Invoke(new PrintRecvMssgDelegate(PrintRecvMssg), new object[] { mess });
            }
        }
        private void PrintRecvMssg(string info)
        {
            ReceiveArea.Text += string.Format("{0}\r\n", info);
        }

        //关闭连接
        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                newSocket.Close();
                sock.Close();
                myThread.Abort();
                //               btnStart.Enabled = true;
                btnStop.Enabled = false;
            }
            catch (Exception ee)
            {
                //label3.Text = "未检测Client接入" + ee;
            }
        }

        //socketIoManager(0,data);


        /*
                private void socketIoManager(int send, string dataToSend)
                {

                    // Instantiate the socket.io connection
                    string serveraddress = ipaddress.Text;
                    string serverport = Port.Text;


                    var socket = IO.Socket(serveraddress + ":" + serverport);
                    // Upon a connection event, update our status

                    if (send == 0)
                    {
                        timer2.Enabled = true;
                        socket.On(Socket.EVENT_CONNECT, () =>
                        {

                            MessageBox.Show("connect server successfully");


                        });

                        socket.On("result", (data) =>
                        {
                            //var temperature = new { temperature = "" };
                            //MessageBox.Show((string)data);
                        });
                    }
                    else
                    {
                        socket.Emit("result", dataToSend);

                    }

                }*/

        //*********************************************//
        //**************文件存储**********************//
        //*******************************************//

        //打开存储文件选项
        private void PutIntoFile_CheckedChanged(object sender, EventArgs e)
        {
            if (PutIntoFile.Checked)
            {
                SaveFilePath.Visible = true;
                ChooseFile.Visible = true;
            }
            else
            {
                SaveFilePath.Visible = false;
                ChooseFile.Visible = false;
            }
        }

        //打开文件
        private void button2_Click(object sender, EventArgs e)//未找到
        {
            OpenFileDialog OpenFilePath = new OpenFileDialog();
            OpenFilePath.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            OpenFilePath.AddExtension = true;
            if (OpenFilePath.ShowDialog() == DialogResult.OK)
            {
                SendFileName.Text = OpenFilePath.FileName;
            }
        }

        //保存文件
        private void button1_Click(object sender, EventArgs e)//Choosefile
        {

            SaveFileDialog savefilepath = new SaveFileDialog();
            savefilepath.Filter = "(*.txt)|*.txt|(*.*)|*.*";
            savefilepath.AddExtension = true;
            if (savefilepath.ShowDialog() == DialogResult.OK)
            {
                SaveFilePath.Text = savefilepath.FileName;
            }
        }

        //bode图幅值保存
        private void button1_Click_2(object sender, EventArgs e)//button1 bode图幅值save
        {
            SaveFileDialog savefilepath = new SaveFileDialog();
            savefilepath.Filter = "(*.png)|*.png|(*.*)|*.*";
            savefilepath.AddExtension = true;
            if (savefilepath.ShowDialog() == DialogResult.OK)
            {
                SaveFilePath.Text = savefilepath.FileName;
            }
            this.chart1.SaveImage(SaveFilePath.Text, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            MessageBox.Show("you have stored this picture");
        }
       
        //bode图相位保存
        private void SaveBode2_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefilepath = new SaveFileDialog();
            savefilepath.Filter = "(*.png)|*.png|(*.*)|*.*";
            savefilepath.AddExtension = true;
            if (savefilepath.ShowDialog() == DialogResult.OK)
            {
                SaveFilePath.Text = savefilepath.FileName;
            }
            this.chart2.SaveImage(SaveFilePath.Text, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            MessageBox.Show("you have stored this picture");
        }
        
        //Nyquist图保存
        private void SaveNyq_Click(object sender, EventArgs e)
        {
            SaveFileDialog savefilepath = new SaveFileDialog();
            savefilepath.Filter = "(*.png)|*.png|(*.*)|*.*";
            savefilepath.AddExtension = true;
            if (savefilepath.ShowDialog() == DialogResult.OK)
            {
                SaveFilePath.Text = savefilepath.FileName;
            }
            this.chart3.SaveImage(SaveFilePath.Text, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            MessageBox.Show("you have stored this picture");
        }

 
        //*********************************************//
        //**************界面切换**********************//
        //*******************************************//

       // 频域
        private void btn_freq_Click(object sender, EventArgs e)
        {
            ACM = false;
            panel_switch.Height = btn_freq.Height;
            panel_switch.Top = btn_freq.Top;
            enableButtons(false,true,true,false,true,true,false);
            rb_Frequncy.Checked = false;
            rb_Sweep.Checked = true;

        }

        // 初始化
        private void btn_cfg_Click_1(object sender, EventArgs e)
        {
            ACM = true;
            panel_switch.Height = btn_cfg.Height;
            panel_switch.Top = btn_cfg.Top;
            enableButtons(true, true, true,true,false,false,true);
        }

        // 时域
        private void btn_TD_Click_1(object sender, EventArgs e)
        {
            panel_switch.Height = btn_TD.Height;
            panel_switch.Top = btn_TD.Top;
            gb_ac.Text = "AC Control";
            enableButtons(false, true, true, false, true,true,false);
            rb_Frequncy.Checked = true;
            rb_Sweep.Checked = false;
            
        }

        // 交流
        private void btn_AC_Click(object sender, EventArgs e)
        {
            panel_switch.Height = btn_AC.Height;
            panel_switch.Top = btn_AC.Top;
            gb_ac.Text = "AC Control";
            enableButtons(false, true, true, false, true, true, false);
            ACM = true;
        }

        // 直流
        private void btn_DC_Click(object sender, EventArgs e)
        {
            panel_switch.Height = btn_DC.Height;
            panel_switch.Top = btn_DC.Top;
            gb_ac.Text = "DC Control";
            enableButtons(false, true, true, false, true, true,false);
        }
       
        
        // function to control buttons（groupBox控制函数）
        private void enableButtons(bool show_groupBox1, bool show_groupBox2, bool show_groupBox4,
                                   bool show_gb1,bool show_gb_ac,bool show_gb_exp,bool show_tabControl1)
        {
            groupBox1.Visible = show_groupBox1;
            groupBox2.Visible = show_groupBox2;
            groupBox4.Visible = show_groupBox4;
            gb1.Visible = show_gb1;
            gb_ac.Visible = show_gb_ac;
            gb_exp.Visible = show_gb_exp;
            tabControl1.Visible = show_tabControl1;
        }


        //*********************************************//
        //****************定时器**********************//
        //*******************************************//

       //时钟显示
        private void timer1_Tick(object sender, EventArgs e)
        {
            //lab_day.Text = DateTime.Now.ToString("");
            lab_date.Text = DateTime.Now.ToString("");
        }

        //wifi定时器
        private void timer2_Tick(object sender, EventArgs e)
        {
            if (data_queue.Count != 0)
            {
                string data;
                data = (string)data_queue.Dequeue();
                for (int i = 0; i < 7; i++)
                {
                    data += (string)data_queue.Dequeue();
                }

                //socketIoManager(1, data);
            }


        }

        //关闭窗口后关闭后台进程
        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill(); 
        }


  
        //*********************************************//
        //****************频域窗口********************//
        //*******************************************//


        //输入指令
        //标志位         频率(Hz/kHz)   AMP(mV)   DOR(sps)   RTIA       DFT  
        //A(固定频率)       X            N          M         O          P
        //B（扫频）        Y-Z           N          M         O          P
        //例：A 1000 5 0 5 2
        //    B 1 1000 5 0 5 2
        //工作时间
        //Repeat：最多7天 每天最多5次 每天包含小时与分钟，标志位a→1次，b→2次，...e→5次
        //例：7 5 e 05 06 08 05 12 23 18 35 23 45
        //Duration:只记持续时间，单位s
        //例：2：5：25输出为7525
        private void btn_start_Click(object sender, EventArgs e)
        {

            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x01 }, 0, 5);

            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    int temp_h = 0;
                    int temp_m = 0;
                    int temp_s = 0;
                    int sum = 0;
                    byte[] flag = new byte[2];
                    flag[0] = 00;
                    flag[1] = 01;
                    byte[] flag1 = new byte[2];
                    flag1[0] = 00;
                    flag1[1] = 6;

                    byte[] Fre = new byte[12];
                    for (int i = 0; i < 12; i++)
                    {
                        Fre[i] = 0x00;

                    }
                    byte[] Sum = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        Sum[i] = 0x00;
                    }

                    byte[] amp = new byte[] { 0x00, 0x00 };

                    try
                    {
                        serialPort1.Write(strToHexByte(cb_dor.Text.Trim()), 0, 1);//dor


                        byte[] tmp2 = strToHexByte(tb_amp.Text);
                        byte[] tmp = exchange(tmp2);
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            amp[1 - i] = tmp[i];
                        }

                        serialPort1.Write(amp, 0, 2);//amp


                        serialPort1.Write(strToHexByte(cb_dft.Text.Trim()), 0, 1);//dft



                        //固定频率
                        if (rb_Frequncy.Checked)
                        {
                            serialPort1.Write(flag, 0, 1);
                            tb_Sweep_f.Enabled = false;
                            tb_Sweep_t.Enabled = false;
                            cb_freq_f.Enabled = false;
                            cb_freq_t.Enabled = false;

                            if (cb_freq.Text.Equals("kHz"))
                            {


                                byte[] temp1 = strToHexByte((Convert.ToInt32(tb_freq.Text) * 1000).ToString());
                                byte[] temp = exchange(temp1);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[3 - i] = temp[i];
                                }


                            }
                            else
                            {

                                byte[] temp2 = strToHexByte(tb_freq.Text);
                                byte[] temp = exchange(temp2);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[3 - i] = temp[i];
                                }
                            }

                        }
                        else//扫频
                        {
                            serialPort1.Write(flag, 1, 1);
                            tb_freq.Enabled = false;
                            cb_freq.Enabled = false;
                            //serialPort1.Write(" ");
                            if (cb_freq_f.Text.Equals("kHz"))
                            {
                                byte[] temp1 = strToHexByte((Convert.ToInt32(tb_Sweep_f.Text) * 1000).ToString());
                                byte[] temp = exchange(temp1);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[7 - i] = temp[i];
                                }
                                serialPort1.Write(temp1, 0, temp1.Length);

                            }
                            else
                            {
                                
                                byte[] temp2 = strToHexByte(tb_Sweep_f.Text);
                                byte[] temp = exchange(temp2);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[7 - i] = temp[i];
                                }
                                serialPort1.Write(temp2, 0, temp2.Length);


                            }
                            if (cb_freq_t.Text.Equals("kHz"))
                            {
                                byte[] temp1 = strToHexByte((Convert.ToInt32(tb_Sweep_t.Text) * 1000).ToString());
                                byte[] temp = exchange(temp1);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[11 - i] = temp[i];
                                }
                            }
                            else
                            {
                                byte[] temp2 = strToHexByte(tb_Sweep_t.Text);
                                byte[] temp = exchange(temp2);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[11 - i] = temp[i];
                                }
                            }

                        }

                        serialPort1.Write(Fre, 0, 12);

                        serialPort1.Write(flag1, 0, 1);//log  暂时没用
                        //serialPort1.Write(strToByte(cb_tia.Text.Trim()), 0, cb_tia.Text.Length);//rtia
                        serialPort1.Write(strToHexByte(cb_tia.Text.Trim()), 0, 1);//rtia

                        //repeat
                        if (rb_rep.Checked)
                        {
                            serialPort1.Write(strToHexByte(cb_days.Text), 0, 1);
                            serialPort1.Write(strToHexByte(cb_times.Text), 0, 1);
                            //serialPort1.Write(cb_times.Text);
                            if (cb_times.Text.Equals("1"))
                            {
                                enablePanel(true, false, false, false, false);

                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_1.Text, 2)), 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);

                                serialPort1.Write(Sum, 0, 4);
                            }
                            else if (cb_times.Text.Equals("2"))
                            {
                                enablePanel(true, true, false, false, false);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_2.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_2.Text, 2)), 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(Sum, 0, 4);
                                //serialPort1.Write(strToHexByte(fest_str(Convert.ToString(sum), 8)), 0, 4);
                            }
                            else if (cb_times.Text.Equals("3"))
                            {
                                enablePanel(true, true, true, false, false);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_2.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_2.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_3.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_3.Text, 2)), 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(Sum, 0, 4);
                                //serialPort1.Write(strToHexByte(fest_str(Convert.ToString(sum), 8)), 0, 4);
                            }
                            else if (cb_times.Text.Equals("4"))
                            {
                                enablePanel(true, true, true, true, false);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_2.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_2.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_3.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_3.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_4.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_4.Text, 2)), 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(Sum, 0, 4);
                                //serialPort1.Write(strToHexByte(fest_str(Convert.ToString(sum), 8)), 0, 4);
                            }
                            else
                            {
                                enablePanel(true, true, true, true, true);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_1.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_2.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_2.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_3.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_3.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_4.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_4.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_h_5.Text, 2)), 0, 1);
                                serialPort1.Write(strToHexByte(fest_str(tb_f_m_5.Text, 2)), 0, 1);
                                serialPort1.Write(Sum, 0, 4);
                                //serialPort1.Write(strToHexByte(fest_str(Convert.ToString(sum), 8)), 0, 4);
                            }
                        }
                        else//duration
                        {
                                serialPort1.Write(flag1, 1, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                serialPort1.Write(flag1, 0, 1);
                                temp_h = Convert.ToInt16(tb_d_h.Text);
                                temp_m = Convert.ToInt16(tb_d_m.Text);
                                temp_s = Convert.ToInt16(tb_d_s.Text);
                                sum = 3600 * temp_h + 60 * temp_m + temp_s;

                                byte[] temp2 = strToHexByte(Convert.ToString(sum));
                                byte[] temp = exchange(temp2);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Sum[3 - i] = temp[i];
                                }
                                //serialPort1.Write(strToHexByte(fest_str(Convert.ToString(sum), 8)), 0, 4);
                                serialPort1.Write(Sum, 0, 4);
                        }
                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("请将参数填写完整");
                    
                    }
                }
            }
            

            catch (Exception ex)
            {
                serialPort1.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();

                MessageBox.Show(ex.Message);

            }
        }


        //固定频率操作框
        private void rb_Frequncy_CheckedChanged(object sender, EventArgs e)
        {
            tb_Sweep_f.Enabled = false;
            tb_Sweep_f.Text = null;
            tb_Sweep_t.Enabled = false;
            tb_Sweep_t.Text = null;
            cb_freq_f.Enabled = false;
            cb_freq_t.Enabled = false;
            tb_freq.Enabled = true;
            cb_freq.Enabled = true;
        }

        //扫频操作框
        private void rb_Sweep_CheckedChanged(object sender, EventArgs e)
        {
            tb_freq.Enabled = false;
            tb_freq.Text = null;
            cb_freq.Enabled = false;
            tb_Sweep_f.Enabled = true;
            tb_Sweep_t.Enabled = true;
            cb_freq_f.Enabled = true;
            cb_freq_t.Enabled = true;
        }

        //repeat 操作框
        private void rb_rep_CheckedChanged(object sender, EventArgs e)
        {
            cb_days.Enabled = true;
            cb_times.Enabled = true;
            tb_f_h_1.Enabled = true;
            tb_f_h_2.Enabled = true;
            tb_f_h_3.Enabled = true;
            tb_f_h_4.Enabled = true;
            tb_f_h_5.Enabled = true;
            tb_f_m_1.Enabled = true;
            tb_f_m_2.Enabled = true;
            tb_f_m_3.Enabled = true;
            tb_f_m_4.Enabled = true;
            tb_f_m_5.Enabled = true;
            tb_d_h.Enabled = false;
            tb_d_m.Enabled = false;
            tb_d_s.Enabled = false;
            tb_d_h.Text = null;
            tb_d_m.Text = null;
            tb_d_s.Text = null;
        }

        //duration 操作框
        private void rb_dur_CheckedChanged(object sender, EventArgs e)
        {
            cb_days.Enabled = false;
            cb_times.Enabled = false;
            tb_f_h_1.Enabled = false;
            tb_f_h_2.Enabled = false;
            tb_f_h_3.Enabled = false;
            tb_f_h_4.Enabled = false;
            tb_f_h_5.Enabled = false;
            tb_f_m_1.Enabled = false;
            tb_f_m_2.Enabled = false;
            tb_f_m_3.Enabled = false;
            tb_f_m_4.Enabled = false;
            tb_f_m_5.Enabled = false;
            tb_f_h_1.Text = null;
            tb_f_h_2.Text = null;
            tb_f_h_3.Text = null;
            tb_f_h_4.Text = null;
            tb_f_h_5.Text = null;
            tb_f_m_1.Text = null;
            tb_f_m_2.Text = null;
            tb_f_m_3.Text = null;
            tb_f_m_4.Text = null;
            tb_f_m_5.Text = null;
            cb_days.Text = null;
            cb_times.Text = null;
            tb_d_h.Enabled = true;
            tb_d_m.Enabled = true;
            tb_d_s.Enabled = true;
        }

        //天数控制函数
        private void enablePanel(bool show_panel_fst, bool show_panel_sec, bool show_panel_thd,
                                   bool show_panel_fourth, bool show_panel_fifth)
        {
            panel_fst.Visible = show_panel_fst;
            panel_sec.Visible = show_panel_sec;
            panel_thd.Visible = show_panel_thd;
            panel_fourth.Visible = show_panel_fourth;
            panel_fifth.Visible = show_panel_fifth;

        }

        //天数显示函数
        private void cb_times_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cb_times.Text.Equals("1"))
            {
                enablePanel(true, false, false, false, false);
            }
            else if (cb_times.Text.Equals("2"))
            {
                enablePanel(true, true, false, false, false);
            }
            else if (cb_times.Text.Equals("3"))
            {
                enablePanel(true, true, true, false, false);
            }
            else if (cb_times.Text.Equals("4"))
            {
                enablePanel(true, true, true, true, false);
            }
            else
            {
                enablePanel(true, true, true, true, true);
            }
        }

        //*********************************************//
        //****************未使用**********************//
        //*******************************************//
        private void panel_thd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ReceiveArea_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void HexSend_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void timRead_Tick(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void gb1_Enter(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void File_TextChanged(object sender, EventArgs e)
        {

        }

        private void BaudrateChoose_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void DataBitChoose_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel_switch_Paint(object sender, PaintEventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void Port_TextChanged(object sender, EventArgs e)
        {

        }

        private void chart4_Click(object sender, EventArgs e)
        {

        }

       
    }
}
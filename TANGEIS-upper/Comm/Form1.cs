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
using System.Windows.Forms.DataVisualization.Charting;
using System.Net;
//using System.Net.Sockets;
using System.Text.RegularExpressions;




namespace Comm
{



    public partial class Form1 : Form
    {

        //*********************************************//
        //**************数据定义**********************//
        //*******************************************//


        bool FDA = false;
        bool TD = false;
        bool ACM = false;
        bool DCM = false;

        //串口数据定义
        static bool PortIsOpen = false;
        private static byte[] result = new byte[1024];

        //图形数据定义
        private int time = 0;
        private float vol = 0;
        private float cur = 0;
        private float fre = 1;
        private float mag = 0;
        private float pha = 0;
        private float X = 0;
        private float Y = 0;
        private float btn_X = 0;
        private float btn_Y = 0;
        private Int32 Num_FDA1 = 0;
        private Int32 Num_FDA2 = 0;
        private Int32 Num_COMB1 = 0;
        private Int32 Num_COMB2 = 0;
        private Int64 cnt = 0;
        private Int64 cnt1 = 0;
        private Int64 cnt2 = 0;
        private Int64 cnt3 = 0;
        private Int64 cnt4 = 0;
        private Int64 cnt5 = 0;
        private Int64 cnt6 = 0;
        private Int64 cnt7 = 0;
        private Int64 cnt8 = 0;
        private Int64 cnt9 = 0;
        private Int64 cnt10 = 0;
        private Int64 cnt11 = 0;
        private Int64 cnt12 = 0;

        private Int64 cnt_comb = 0;
        private Int64 cnt_FDA1 = 0;
        private Int64 cnt_FDA2 = 0;
        private Int64 cnt_TD = 0;
        private Int64 cnt_UIR = 0;
        private Queue chart_x = new Queue();
        private Queue chart_y = new Queue();
        private Queue data_queue = new Queue();

        //无线数据定义
        //private IPAddress serverIP;
        //private IPEndPoint serverFullAddr;
        //private Socket sock;
        //private Socket newSocket;
        //Thread myThread = null;


        string parameter1 = "";
        string parameter21 = "";
        string parameter22 = "";
        string parameter3 = "";
        string parameter4 = "";
        string pathString1 = "";
        string pathString2 = "";
        string pathString3 = "";
        string pathString4 = "";
        string Single_m = "";
        string Single_m1 = "";
        string Multiple_m = "";
        string U_I_R = "";
        string foldPath = "";
        string ID_Num = "";
        string Combination_m = "";
        string Temperature = "℃";

        Int32 tempt = 37;


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
            tb_Sweep_f.Text = "10";
            tb_Sweep_t.Text = "100";
            tb_times_D1.Text = "0";
            tb_times_D2.Text = "0";
            tb_times_T1.Text = "0";
            tb_times_T2.Text = "0";
            cb_days.Text = "0";
            rb_Frequncy.Checked = true;
            rb_rep.Checked = true;
            rb_dur.Checked = true;
            rb_Square.Checked = true;
            panel_sec.Visible = false;
            checkBox_SEC.Checked = false;
            btn_freq.Visible = false;
            btn_TD.Visible = false;
            btn_DC.Visible = false;
            btn_AC.Visible = false;
            btn_cfg.Visible = false;
            panel_switch.Visible = false;
            btn_s.Visible = false;
            btn_m.Visible = false;
            panel_switch.Visible = false;
            btn_comb_start.Enabled = true;
            btn_stop.Enabled = false;
            btn_stop2.Enabled = false;
            btn_stop1.Enabled = false;
            //rb_Sawtooth.Checked = true;
            cb_freq.Text = "Hz";
            cb_freq_f.Text = "Hz";
            cb_freq_t.Text = "Hz";
            cb_dor.Text = "5";
            cb_tia.Text = "0";
            cb_dft.Text = "1";
            cb_days.Text = "7";


            cb_freq_f2.Text = "Hz";
            cb_freq_t2.Text = "Hz";
            cb_dor2.Text = "5";
            cb_tia2.Text = "0";
            cb_dft2.Text = "1";
            cb_days2.Text = "7";

            SaveFilePath.Visible = true;
            ChooseFile.Visible = true;
            CheckForIllegalCrossThreadCalls = false;
            panel_load.Visible = false;
            enableButtons(false, false, false, false, false, false, false, false, false, false);



        }

        //事件添加
        private void Form1_Load(object sender, EventArgs e)
        {
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(Port_DataRecevied);
            Init_Chart();
            chart1.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            chart2.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            chart3.MouseWheel += new MouseEventHandler(chart_MouseWheelXY);
            chart4.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            chart_u_i_r.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            this.Resize += new EventHandler(modular_calEchoPhaseFromSignal1_Resize);//窗体调整大小时引发事件

            X = this.Width;//获取窗体的宽度

            Y = this.Height;//获取窗体的高度

            btn_X = btn_comb_start.Width;

            btn_Y = btn_comb_start.Height;

            setTag(this);//调用方法

        }




        //*********************************************//
        //**************自编函数**********************//
        //*******************************************//



        private void setTag(Control cons)

        {

            //遍历窗体中的控件

            foreach (Control con in cons.Controls)

            {

                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size + ":" + con.Name;

                if (con.Controls.Count > 0)

                    setTag(con);

            }

        }
        private void setControls(float newx, float newy, Control cons)

        {
            //foreach (Control con in cons.Controls)

            //{
            //    con.Visible = false;

            //}
            //遍历窗体中的控件，重新设置控件的值

            foreach (Control con in cons.Controls)

            {

                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//获取控件的Tag属性值，并分割后存储字符串数组

                float a = Convert.ToSingle(mytag[0]) * newx;//根据窗体缩放比例确定控件的值，宽度

                con.Width = (int)a;//宽度

                a = Convert.ToSingle(mytag[1]) * newy;//高度

                con.Height = (int)(a);

                a = Convert.ToSingle(mytag[2]) * newx;//左边距离

                con.Left = (int)(a);

                a = Convert.ToSingle(mytag[3]) * newy;//上边缘距离

                con.Top = (int)(a);

                Single currentSize = Convert.ToSingle(mytag[4]) * newx;//字体大小

                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);

                if (con.Controls.Count > 0)

                {

                    setControls(newx, newy, con);

                }

            }

        }

        void modular_calEchoPhaseFromSignal1_Resize(object sender, EventArgs e)

        {


            float newx = (this.Width) / X; //窗体宽度缩放比例

            float newy = this.Height / Y;//窗体高度缩放比例

            setControls(newx, newy, this);//随窗体改变控件大小

            setControls(newx*X, newy*Y, btn_comb_start);//随窗体改变控件大小

            setControls(newx * X, newy * Y, btn_stop);//随窗体改变控件大小

        }

        //字符串定长
        private string fest_str(string s, int n)
        {
            string feststr;

            feststr = Convert.ToInt32(s).ToString().PadLeft(n, '0');

            return feststr;
        }

        //字符串转16进制
        private string strToHex(string s)
        {
            if ((s.Length % 2) != 0)
                s = fest_str(s, s.Length + 1);
            int b = Convert.ToInt32(s);

            string HexStr = b.ToString("X");
            return HexStr;

        }

        //数字倒序排列
        private byte[] exchange(byte[] array)
        {
            byte n;

            for (int i = 0; i < array.Length / 2; i++)

            {
                n = array[i];
                array[i] = array[array.Length - i - 1];
                array[array.Length - i - 1] = n;
            }
            return array;
        }

        private string TimeToSec(string tmp1)
        {
            tmp1 = tmp1.Replace(":", "");
            //this.textBox1_test.Text = tmp1;
            int tmp2 = Convert.ToInt32(tmp1);
            int hh = tmp2 / 10000;
            int tmp3 = tmp2 % 10000;
            int mm = tmp3 / 100;
            int ss = tmp3 % 100;
            int sum = hh * 3600 + mm * 60 + ss;
            string Zeit = sum.ToString();
            return Zeit;
        }


        private string TimeDifference(string Zeit)
        {
            int cc = 0;
            string z = Convert.ToString(cc);
            //int n = Convert.ToInt32(days);
            int tmp = Convert.ToInt32(TimeToSec(Zeit));
            string dt1 = System.DateTime.Now.ToString("HH:mm:ss");
            int tmp1 = Convert.ToInt32(TimeToSec(dt1));
            int TD = tmp - tmp1;
            if (TD < 0)
            {
                TD = TD + 86400;
            }

            string TD1 = z;
            //try
            //{
            if (TD >= 60)
            {
                TD1 = TD.ToString();

            }
            else
            {
                MessageBox.Show("开始时间至少在一分钟之后");

            }
            ////}
            ////catch
            ////{
            ////    MessageBox.Show("开始时间至少在一小时之后");
            ////    TD1 = null;
            ////}
            return TD1;
        }

        //字符串转byte（不用）
        private byte[] strToByte(string s)
        {
            byte[] b = Encoding.ASCII.GetBytes(s);//按照指定编码将string编程字节数组
            byte[] result = new byte[s.Length];
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                if ((b[i] == 'A') || b[i] == 'B' || b[i] == 'C' || b[i] == 'D' || b[i] == 'E' || b[i] == 'F')
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


        //字符串转16进制字符串(不用)
        private string StringToHexString(string s)

        {
            //byte[] b = Encoding.ASCII.GetBytes(s);//按照指定编码将string编程字节数组
            char[] c = s.ToCharArray();
            string result = string.Empty;
            for (int i = 0; i < c.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                //result += Convert.ToString(int.Parse(c[i].ToString()));
                result += Convert.ToString(Convert.ToInt32(c[i]) - 48);
                //result += Convert.ToString(c[i], 16);

            }
            test.AppendText(Convert.ToString(c[0]));
            return result;
        }

        /// 字符串转16进制byte[]编码发送
        //private void SendHexByte(string send_data)
        private byte[] strToHexByte(string Str)
        {

            //Str = Str.Replace(" ", "");
            string hexString = strToHex(Str);
            //string hexString = Str;


            //string hexString = StringToHexString(Str);
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            test.AppendText(hexString);
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;

        }

        private double trianglewav(double t, double p)

        {
            //三角波函数，p：0到1占空比
            //trianglewav(t,p)的周期为2pi
            //返回值在-1到+1间
            double y;
            //将t归化于0到2pi区间
            if (t >= 0)
                t = (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            else
                t = 2 * Math.PI + (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            //检查占空比参数范围是否合法
            if (p < 0 | p > 1)
            {
                MessageBox.Show("fun trianglewav error: p<0 or p>1\n");
            }
            p = p * 2 * Math.PI;
            //当p为0或2*pi时的近似处理
            p = (p - 2 * Math.PI == 0) ? 2 * Math.PI - 1e-10 : p;
            p = (p == 0) ? 1e-10 : p;

            if (t < p)
            {
                y = 2 * t / p - 1;//计算上升沿
            }
            else
            {
                y = -2 * t / (2 * Math.PI - p) + (2 * Math.PI + p) / (2 * Math.PI - p);//下降沿
            }
            return y;
        }

        private double Squarewav(double t, double p)

        {
            //三角波函数，p：0到1占空比
            //trianglewav(t,p)的周期为2pi
            //返回值在-1到+1间
            double y;
            //将t归化于0到2pi区间
            if (t >= 0)
                t = (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            else
                t = 2 * Math.PI + (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            //检查占空比参数范围是否合法
            if (p < 0 | p > 1)
            {
                MessageBox.Show("fun trianglewav error: p<0 or p>1\n");
            }
            p = p * 2 * Math.PI;
            //当p为0或2*pi时的近似处理
            p = (p - 2 * Math.PI == 0) ? 2 * Math.PI - 1e-10 : p;
            p = (p == 0) ? 1e-10 : p;

            if (t < p)
            {
                y = 1;//计算上升沿
            }
            else
            {
                y = 0;//下降沿
            }
            return y;
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
            this.chart_u_i_r.ChartAreas[0].AxisX.IsLogarithmic = false;
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart_u_i_r.Series[0].Points.Clear();
            // chart5.Series[0].Points.Clear();

        }

        private void ChartClear()
        {
            ReceiveArea.Clear();
            this.chart1.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart2.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart4.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart_u_i_r.ChartAreas[0].AxisX.IsLogarithmic = false;
            chart1.Series[0].Points.Clear();
            chart2.Series[0].Points.Clear();
            chart3.Series[0].Points.Clear();
            chart4.Series[0].Points.Clear();
            chart_u_i_r.Series[0].Points.Clear();
            // chart5.Series[0].Points.Clear();
        }


        private void ClearSendArea_Click(object sender, EventArgs e)
        {
            SendArea.Clear();
        }


        //*********************************************//
        //**************串口传输+画图*****************//
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



            //ID send
            byte[] ID_N = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                ID_N[i] = 0x00;
            }

            byte[] Zeros = new byte[7];
            for (int i = 0; i < 7; i++)
            {
                Zeros[i] = 0x00;
            }
            if (serialPort1.IsOpen)

            {
                serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x00 }, 0, 5);
                // MessageBox.Show("COM is already!");
                byte[] temp2 = strToHexByte(ID_Num);
                byte[] temp = exchange(temp2);
                for (int i = 0; i < temp.Length; i++)
                {
                    ID_N[3 - i] = temp[i];
                }
                ReceiveArea.Text = ID_N.ToString();
                serialPort1.Write(ID_N, 0, 4);
                serialPort1.Write(Zeros, 0, 7);
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

                    if (TD == true)//cfg界面
                    {

                        string strv = serialPort1.ReadLine();
                        ReceiveArea.AppendText(strv);
                        cnt++;
                        this.chart4.Series[0].Points.AddXY(cnt, strv);
                        this.chart4.ChartAreas[0].AxisX.IsLogarithmic = true;
                        FileStream fs = new FileStream(Single_m1, FileMode.Append);
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(cnt + "\t" + strv + "\t");
                        sw.Flush();
                        sw.Close();
                        fs.Close();

                    }
                    else if (DCM == true) //非cfg界面   （暂时为频域）
                    {
                        //string stru = serialPort1.ReadExisting();
                        string stru = serialPort1.ReadLine();
                        ReceiveArea.AppendText(stru);
                        string[] strArry = stru.Split(',');
                        int length_u = strArry.Length;
                        time++;
                        string length1 = Convert.ToString(length_u);
                        //test.AppendText(length1);

                        if (length_u == 2 && strArry[0] != "200000.00")
                        {
                            try
                            {
                                vol = Convert.ToSingle(strArry[0]);
                                cur = Convert.ToSingle(strArry[1]);


                                double res = vol / cur;


                                this.chart_u_i_r.Series[0].Points.AddXY(time, vol);
                                this.chart_u_i_r.Series[1].Points.AddXY(time, cur);
                                this.chart_u_i_r.Series[2].Points.AddXY(time, res);
                                this.chart_u_i_r.ChartAreas[0].AxisX.IsLogarithmic = true;

                                tb_u.Text = vol.ToString();
                                tb_I.Text = cur.ToString();
                                tb_R.Text = res.ToString();

                                data_queue.Enqueue(stru + ":");

                                FileStream fs = new FileStream(U_I_R, FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.WriteLine(time.ToString() + "\t" + tb_u.Text + "\t" + tb_I.Text + "\t" + tb_R.Text);
                                sw.Flush();
                                sw.Close();
                                fs.Close();

                            }

                            catch
                            {

                                Console.WriteLine("error");

                            }
                        }
                    }

                    else if (FDA == true)
                    {
                        //string str = serialPort1.ReadExisting();
                        string str = serialPort1.ReadLine();
                        ReceiveArea.AppendText(str);

                        string[] strArr = str.Split(',');
                        int length = strArr.Length;



                        if (length == 4 && strArr[0] != "200000.00")
                        {
                            try
                            {
                                fre = Convert.ToSingle(strArr[0]);
                                mag = Convert.ToSingle(strArr[1]);
                                pha = Convert.ToSingle(strArr[2]);
                                Num_FDA1 = Convert.ToInt32(strArr[3]);
                                if (Num_FDA1 > Num_FDA2)
                                {
                                    cnt6++;
                                    chart1.Series.Add((cnt6).ToString());//添加
                                    chart2.Series.Add((cnt6).ToString());//添加
                                    chart3.Series.Add((cnt6).ToString());//添加
                                    this.chart1.Series[(cnt6).ToString()].ChartType = SeriesChartType.Point;
                                    this.chart2.Series[(cnt6).ToString()].ChartType = SeriesChartType.Point;
                                    this.chart3.Series[(cnt6).ToString()].ChartType = SeriesChartType.Point;
                                }
                                Num_FDA2 = Convert.ToInt32(strArr[3]);

                                int fre_int = (int)(fre * 100);
                                fre = fre_int / 100;



                                double real = mag * Math.Cos(pha * Math.PI / 180);
                                double img = mag * Math.Sin(-pha * Math.PI / 180);

                                int real_int = (int)(real * 100);
                                real = real_int / 100;

                                int img_int = (int)(img * 100);
                                img = img_int / 100;

                                this.chart1.Series[cnt6.ToString()].Points.AddXY(fre, mag);
                                this.chart2.Series[cnt6.ToString()].Points.AddXY(fre, pha);
                                this.chart3.Series[cnt6.ToString()].Points.AddXY(real, img);
                                //this.chart1.ChartAreas[cnt6.ToString()].AxisX.ScaleView.Scroll(ScrollType.Last);
                                //this.chart2.ChartAreas[cnt6.ToString()].AxisX.ScaleView.Scroll(ScrollType.Last);


                                data_queue.Enqueue(str + ":");

                                if (rb_dur.Checked)
                                {
                                    FileStream fs = new FileStream(Single_m, FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[0] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t");
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();
                                }
                                else if (rb_rep.Checked)
                                {
                                    FileStream fs = new FileStream(Multiple_m, FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[0] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t" + Num_FDA1.ToString());
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            catch
                            {
                                MessageBox.Show("图表未工作");
                                Console.WriteLine("error");

                            }
                        }
                    }

                    else if (ACM == true)
                    {
                        string strc = serialPort1.ReadLine();
                        ReceiveArea.AppendText(strc);

                        string[] strArr = strc.Split(',');
                        int length = strArr.Length;



                        if (length == 4 && strArr[0] != "200000.00")
                        {
                            try
                            {
                                fre = Convert.ToSingle(strArr[0]);
                                mag = Convert.ToSingle(strArr[1]);
                                pha = Convert.ToSingle(strArr[2]);
                                Num_COMB1 = Convert.ToInt32(strArr[3]);
                                if (Num_COMB1 > Num_COMB2)
                                {
                                    cnt11++;
                                    chart1.Series.Add((cnt11).ToString());//添加
                                    chart2.Series.Add((cnt11).ToString());//添加
                                    chart3.Series.Add((cnt11).ToString());//添加
                                    this.chart1.Series[(cnt11).ToString()].ChartType = SeriesChartType.Point;
                                    this.chart2.Series[(cnt11).ToString()].ChartType = SeriesChartType.Point;
                                    this.chart3.Series[(cnt11).ToString()].ChartType = SeriesChartType.Point;
                                }
                                Num_COMB2 = Convert.ToInt32(strArr[3]);
                                int fre_int = (int)(fre * 100);
                                fre = fre_int / 100;



                                double real = mag * Math.Cos(pha * Math.PI / 180);
                                double img = mag * Math.Sin(-pha * Math.PI / 180);

                                int real_int = (int)(real * 100);
                                real = real_int / 100;

                                int img_int = (int)(img * 100);
                                img = img_int / 100;

                                this.chart1.Series[(cnt11).ToString()].Points.AddXY(fre, mag);
                                this.chart2.Series[(cnt11).ToString()].Points.AddXY(fre, pha);
                                this.chart3.Series[(cnt11).ToString()].Points.AddXY(real, img);


                                data_queue.Enqueue(strc + ":");


                                FileStream fs = new FileStream(Combination_m, FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.WriteLine(strArr[0] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t" + cnt11.ToString());
                                sw.Flush();
                                sw.Close();
                                fs.Close();

                            }
                            catch
                            {
                                MessageBox.Show("图表未工作");
                                Console.WriteLine("error");

                            }
                        }
                    }
                    else
                    {
                        string strvv = serialPort1.ReadExisting();
                        //string strvv = serialPort1.ReadLine();
                        ReceiveArea.AppendText(strvv);
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


        //鼠标滚轮改变图像大小
        void chart_MouseWheel(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)(sender);
            double zoomfactor = 2;   //设置缩放比例
            double xstartpoint = chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum;      //获取当前x轴最小坐标
            double xendpoint = chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum;      //获取当前x轴最大坐标
            double xmouseponit = chart.ChartAreas[0].AxisX.PixelPositionToValue(e.X);    //获取鼠标在chart中x坐标
            double xratio = (xendpoint - xmouseponit) / (xmouseponit - xstartpoint);      //计算当前鼠标基于坐标两侧的比值，后续放大缩小时保持比例不变

            if (e.Delta > 0)    //滚轮上滑放大
            {
                if (chart.ChartAreas[0].AxisX.ScaleView.Size > 5)     //缩放视图不小于5
                {
                    if ((xmouseponit >= chart.ChartAreas[0].AxisX.ScaleView.ViewMinimum) && (xmouseponit <= chart.ChartAreas[0].AxisX.ScaleView.ViewMaximum)) //判断鼠标位置不在x轴两侧边沿
                    {
                        double xspmovepoints = Math.Round((xmouseponit - xstartpoint) * (zoomfactor - 1) / zoomfactor, 1);    //计算x轴起点需要右移距离,保留一位小数
                        double xepmovepoints = Math.Round(xendpoint - xmouseponit - xratio * (xmouseponit - xstartpoint - xspmovepoints), 1);    //计算x轴末端左移距离，保留一位小数
                        double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                        chart.ChartAreas[0].AxisX.ScaleView.Size -= viewsizechange;        //设置x轴缩放视图大小
                        chart.ChartAreas[0].AxisX.ScaleView.Position += xspmovepoints;        //设置x轴缩放视图起点，右移保持鼠标中心点

                    }
                }
            }
            else     //滚轮下滑缩小
            {

                if (chart.ChartAreas[0].AxisX.ScaleView.Size < chart.ChartAreas[0].AxisX.Maximum)
                {
                    double xspmovepoints = Math.Round((zoomfactor - 1) * (xmouseponit - xstartpoint), 1);   //计算x轴起点需要左移距离
                    double xepmovepoints = Math.Round((zoomfactor - 1) * (xendpoint - xmouseponit), 1);    //计算x轴末端右移距离
                    if (chart.ChartAreas[0].AxisX.ScaleView.Size + xspmovepoints + xepmovepoints < chart.ChartAreas[0].AxisX.Maximum)  //判断缩放视图尺寸是否超过曲线尺寸
                    {
                        if ((xstartpoint - xspmovepoints <= 0) || (xepmovepoints + xendpoint >= chart.ChartAreas[0].AxisX.Maximum))  //判断缩放值是否达到曲线边界
                        {
                            if (xstartpoint - xspmovepoints <= 0)    //缩放视图起点小于等于0
                            {
                                xspmovepoints = xstartpoint;
                                chart.ChartAreas[0].AxisX.ScaleView.Position = 0;    //缩放视图起点设为0
                            }
                            else
                                chart.ChartAreas[0].AxisX.ScaleView.Position -= xspmovepoints;  //缩放视图起点大于0，按比例缩放
                            if (xepmovepoints + xendpoint >= chart.ChartAreas[0].AxisX.Maximum)  //缩放视图终点大于曲线最大值
                                chart.ChartAreas[0].AxisX.ScaleView.Size = chart.ChartAreas[0].AxisX.Maximum - chart.ChartAreas[0].AxisX.ScaleView.Position;  //设置缩放视图尺寸=曲线最大值-视图起点值
                            else
                            {
                                double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                                chart.ChartAreas[0].AxisX.ScaleView.Size += viewsizechange;   //按比例缩放视图大小
                            }
                        }
                        else
                        {
                            double viewsizechange = xspmovepoints + xepmovepoints;         //计算x轴缩放视图缩小变化尺寸
                            chart.ChartAreas[0].AxisX.ScaleView.Size += viewsizechange;   //按比例缩放视图大小
                            chart.ChartAreas[0].AxisX.ScaleView.Position -= xspmovepoints;   //按比例缩放视图大小
                        }
                    }
                    else
                    {
                        chart.ChartAreas[0].AxisX.ScaleView.Size = chart.ChartAreas[0].AxisX.Maximum;
                        chart.ChartAreas[0].AxisX.ScaleView.Position = 0;
                    }
                }
            }
        }

        void chart_MouseWheelXY(object sender, MouseEventArgs e)
        {
            Chart chart = (Chart)(sender);

            if (e.Delta > 0)    //滚轮上滑放大
            {
                if (chart.ChartAreas[0].AxisX.ScaleView.Size > 5)     //缩放视图不小于5
                {
                    ((Chart)(sender)).ChartAreas[0].AxisX.ScaleView.Size = ((Chart)(sender)).ChartAreas[0].AxisX.ScaleView.Size * 0.8;
                    ((Chart)(sender)).ChartAreas[0].AxisY.ScaleView.Size = ((Chart)(sender)).ChartAreas[0].AxisY.ScaleView.Size * 0.8;

                }
            }
            else     //滚轮下滑缩小
            {
                ((Chart)(sender)).ChartAreas[0].AxisX.ScaleView.Size = ((Chart)(sender)).ChartAreas[0].AxisX.ScaleView.Size * 1.2;
                ((Chart)(sender)).ChartAreas[0].AxisY.ScaleView.Size = ((Chart)(sender)).ChartAreas[0].AxisY.ScaleView.Size * 1.2;

            }
        }


        //chart init.
        private void Init_Chart()
        {

            chart1.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart2.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart3.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart3.ChartAreas[0].AxisY.ScaleView.Size = 3000;
            chart4.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart_u_i_r.ChartAreas[0].AxisX.ScaleView.Size = 3000;



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


        //********************************************/
        //**************无线传输**********************//
        //*******************************************//

        //建立连接
        private void button1_Click_1(object sender, EventArgs e)
        {
            string data = null;
            socketIoManager(0, data);
        }

        //关闭连接
        private void btnStop_Click(object sender, EventArgs e)
        {

        }

        //服务器数据传输
        private void socketIoManager(int send, string dataToSend)
        {

            //Instantiate the socket.io connection
            string serveraddress = ipaddress.Text;
            string serverport = Port.Text;


            var socket = IO.Socket(serveraddress + ":" + serverport);
            //Upon a connection event, update our status

            if (send == 0)
            {
                timer2.Enabled = true;
                socket.On(Socket.EVENT_CONNECT, () =>
                {

                    MessageBox.Show("connect server successfully");


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

                socketIoManager(1, data);
            }


        }



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


            if (FDA == true)
            {
                string bode_amp = pathString1 + "/bode_amp" + cnt1 + ".png";
                this.chart1.SaveImage(bode_amp, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt1++;
            }
            else if (ACM == true)
            {
                string bode_amp = pathString4 + "/bode_amp" + cnt8 + ".png";
                this.chart1.SaveImage(bode_amp, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt8++;
            }
        }
            

        //bode图相位保存
        private void SaveBode2_Click(object sender, EventArgs e)
        {
            if (FDA == true)
            {

                string bode_pha = pathString1 + "/bode_pha" + cnt2 + ".png";
                this.chart2.SaveImage(bode_pha, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt2++;
            }
            else if (ACM == true)
            {
                string bode_pha = pathString4 + "/bode_pha" + cnt9 + ".png";
                this.chart2.SaveImage(bode_pha, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt9++;
            }

        }

        //Nyquist图保存
        private void SaveNyq_Click(object sender, EventArgs e)
        {
            if (FDA == true)
            {
                string Nyqst = pathString1 + "/Nyqst" + cnt3 + ".png";
                this.chart3.SaveImage(Nyqst, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt3++;
            }
            else if (ACM == true)
            {
                string Nyqst = pathString4 + "/Nyqst" + cnt10 + ".png";
                this.chart3.SaveImage(Nyqst, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt10++;
            }
        }

        //时域曲线保存
        private void SaveTD_Click(object sender, EventArgs e)
        {
            string TD_Wave = pathString2 + "/TD_Wave" + cnt4 + ".png";
            this.chart4.SaveImage(TD_Wave, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            MessageBox.Show("you have stored this picture");
            cnt4++;
        }

        //U_I_R曲线保存
        private void button2_Click_1(object sender, EventArgs e)
        {
            string U_I_R_Wave = pathString3 + "/U_I_R_Wave" + cnt5 + ".png";
            this.chart_u_i_r.SaveImage(U_I_R_Wave, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            MessageBox.Show("you have stored this picture");
            cnt5++;
        }


        //*********************************************//
        //**************界面切换**********************//
        //*******************************************//

        // 频域
        private void btn_freq_Click(object sender, EventArgs e)
        {
            FDA = true;
            TD = false;
            ACM = false;
            DCM = false;
            rb_rep.Enabled = true;
            panel_switch.Height = btn_freq.Height;
            panel_switch.Top = btn_freq.Top;
            enableButtons(false, true, false, false, true, false, true, false, false, false);
            rb_Frequncy.Checked = false;
            rb_Sweep.Checked = true;
            //serialPort1.Write("Open COM");

        }

        // 初始化
        private void btn_cfg_Click_1(object sender, EventArgs e)
        {
            FDA = false;
            TD = false;
            ACM = false;
            DCM = false;
            panel_switch.Height = btn_cfg.Height;
            panel_switch.Top = btn_cfg.Top;
            enableButtons(true, true, true, true, false, false, false, true, false, false);
            //string length2 = Convert.ToString(FDA);
            //test.AppendText(length2);
        }

        // 时域
        private void btn_TD_Click_1(object sender, EventArgs e)
        {
            FDA = false;
            TD = true;
            ACM = false;
            DCM = false;
            rb_dur.Enabled = true;
            rb_rep.Enabled = false;
            panel_switch.Height = btn_TD.Height;
            panel_switch.Top = btn_TD.Top;
            gb_ac.Text = "AC Control";
            enableButtons(false, true, false, false, true, false, true, false, false, false);
            rb_Frequncy.Checked = true;
            rb_Sweep.Checked = false;
            //serialPort1.Write("Open COM");
        }

        // 交流+直流
        private void btn_AC_Click(object sender, EventArgs e)
        {
           

            gb_ac2.Visible = true;
            gb_rep2.Visible = true;
            gb_dc2.Visible = true;

            dateTimePicker_f_4.Enabled = false;
            dateTimePicker_t_4.Enabled = false;
            tb_times_D4.Enabled = false;
            tb_times_T4.Enabled = false;
            if (checkBox_SEC2.Checked)
            {
                dateTimePicker_f_4.Enabled = true;
                dateTimePicker_t_4.Enabled = true;
                tb_times_D4.Enabled = true;
                tb_times_T4.Enabled = true;
            }

            else
            {
                dateTimePicker_f_4.Enabled = false;
                dateTimePicker_t_4.Enabled = false;
                tb_times_D4.Enabled = false;
                tb_times_T4.Enabled = false;
            }
            FDA = false;
            TD = false;
            ACM = true;
            DCM = false;
            panel_switch.Height = btn_AC.Height;
            panel_switch.Top = btn_AC.Top;
            gb_ac.Text = "AC Control";
            enableButtons(false, true, false, false, false, false, false, false, false, true);
            //serialPort1.Write("Open COM");

        }

        // 直流
        private void btn_DC_Click(object sender, EventArgs e)
        {
            panel_switch.Height = btn_DC.Height;
            panel_switch.Top = btn_DC.Top;
            FDA = false;
            TD = false;
            DCM = true;
            ACM = false;
            string length2 = Convert.ToString(FDA);
            test.AppendText(length2);
            if (rb_Sweep1.Checked)
            {

                panel1.Visible = true;
                tb_s_p1.Enabled = true;
                tb_d_h1.Enabled = false;
                tb_d_m1.Enabled = false;
                tb_d_s1.Enabled = false;
                tb_times.Enabled = false;
            }
            else
            {
                panel1.Visible = false;
                tb_s_p1.Enabled = false;
                tb_d_h1.Enabled = true;
                tb_d_m1.Enabled = true;
                tb_d_s1.Enabled = true;
                tb_times.Enabled = true;
            }

            gb_ac.Text = "DC Control";
            enableButtons(false, false, false, false, false, true, false, false, true, false);
            //serialPort1.Write("Open COM");
        }


        // function to control buttons（groupBox控制函数）
        private void enableButtons(bool show_groupBox1, bool show_groupBox2, bool show_groupBox4,
                                   bool show_gb1, bool show_gb_ac, bool show_gb_dc, bool show_gb_exp, bool show_tabControl1, bool show_chart_ui, bool show_Comb)
        {
            groupBox1.Visible = show_groupBox1;
            groupBox2.Visible = show_groupBox2;
            groupBox4.Visible = show_groupBox4;
            gb1.Visible = show_gb1;
            gb_ac.Visible = show_gb_ac;
            gb_dc.Visible = show_gb_dc;
            gb_exp.Visible = show_gb_exp;
            tabControl1.Visible = show_tabControl1;
            groupBox_UI.Visible = show_chart_ui;
            gb_comb.Visible = show_Comb;
        }


        //*********************************************//
        //****************定时器**********************//
        //*******************************************//

        //时钟显示
        private void timer1_Tick(object sender, EventArgs e)
        {
            //lab_day.Text = DateTime.Now.ToString("");
            lab_date.Text = System.DateTime.Now.ToString("");
        }



        //关闭窗口后关闭后台进程
        private void CloseForm(object sender, FormClosedEventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }



        //*********************************************//
        //****************频域窗口********************//
        //*******************************************//


        //FDA+TD send
        private void btn_start_Click(object sender, EventArgs e)
        {
            btn_start.Enabled = false;
            btn_stop.Enabled = true;
            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x01 }, 0, 5);

            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    //int num = 0;   //获取本次发送字节数
                    //串口处于开启状态，将发送区文本发送test_var
                    //string temp1 = 0;
                    int temp_h = 0;
                    int temp_m = 0;
                    int temp_s = 0;
                    int sum = 0;

                    //bool flag_repeat = false;
                    byte[] flag = new byte[2];
                    flag[0] = 00;
                    flag[1] = 01;
                    byte[] flag1 = new byte[2];
                    flag1[0] = 0x00;
                    flag1[1] = 6;

                    byte[] Fre = new byte[12];
                    for (int i = 0; i < 12; i++)
                    {
                        Fre[i] = 0x00;

                    }
                    byte[] Start_send1 = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        Start_send1[i] = 0x00;
                    }

                    byte[] Start_send2 = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        Start_send2[i] = 0x00;
                    }

                    byte[] End_send1 = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        End_send1[i] = 0x00;
                    }

                    byte[] End_send2 = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        End_send2[i] = 0x00;
                    }

                    byte[] Sum = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        Sum[i] = 0x00;
                    }

                    byte[] Zeros = new byte[14];
                    for (int i = 0; i < 14; i++)
                    {
                        Zeros[i] = 0x00;
                    }

                    byte[] amp = new byte[] { 0x00, 0x00 };
                    byte[] points = new byte[] { 0x00, 0x00 };

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
                            if (cb_freq_f.Text.Equals("kHz"))
                            {
                                byte[] temp1 = strToHexByte((Convert.ToInt32(tb_Sweep_f.Text) * 1000).ToString());
                                byte[] temp = exchange(temp1);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[7 - i] = temp[i];
                                }
                            }
                            else
                            {

                                byte[] temp2 = strToHexByte(tb_Sweep_f.Text);
                                byte[] temp = exchange(temp2);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre[7 - i] = temp[i];
                                }
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

                        serialPort1.Write(strToHexByte(cb_tia.Text.Trim()), 0, 1);//rtia


                        serialPort1.Write(strToHexByte(tb_s_p.Text.Trim()), 0, 1);//points




                        //repeat
                        if (rb_rep.Checked)
                        {


                            //计算天数

                            string dt1 = System.DateTime.Now.ToString("yyyy/MM/dd");
                            DTP_Start.Text = dt1;
                            DateTime d1 = DateTime.Parse(DTP_Start.Text);
                            DateTime d2 = DateTime.Parse(DTP_End.Text);
                            System.TimeSpan ND = d2 - d1;
                            int ts1 = ND.Days;   //天数差
                            int ts2 = 0;
                            if ((ts1 > 0) && (ts1 < 8))
                            {
                                ts2 = ts1;
                            }

                            string ts = Convert.ToString(ts2);
                            textBox1_test.Text = ts;
                            cb_days.Text = ts;

                            serialPort1.Write(strToHexByte(cb_days.Text), 0, 1);

                            //开始时间

                            string start_time1 = dateTimePicker_f_1.Text;
                            string start_send1 = TimeDifference(start_time1);

                            byte[] temp_s1 = strToHexByte(start_send1);
                            byte[] temp_se1 = exchange(temp_s1);
                            for (int i = 0; i < temp_se1.Length; i++)
                            {
                                Start_send1[3 - i] = temp_se1[i];
                            }

                            serialPort1.Write(Start_send1, 0, 4);//开始时间1发送

                            string end_time1 = dateTimePicker_t_1.Text;
                            string end_send1 = TimeDifference(end_time1);

                            byte[] temp_e1 = strToHexByte(end_send1);
                            byte[] temp_ee1 = exchange(temp_e1);
                            for (int i = 0; i < temp_ee1.Length; i++)
                            {
                                End_send1[3 - i] = temp_ee1[i];
                            }

                            serialPort1.Write(End_send1, 0, 4);//结束时间1发送

                            serialPort1.Write(strToHexByte(tb_times_D1.Text.Trim()), 0, 1);//次数1/天发送
                            serialPort1.Write(strToHexByte(tb_times_T1.Text.Trim()), 0, 1);//次数1/次发送

                            if (checkBox_SEC.Checked)
                            {
                                string start_time2 = dateTimePicker_f_2.Text;
                                string start_send2 = TimeDifference(start_time2);
                                byte[] temp_s2 = strToHexByte(start_send2);
                                byte[] temp_se2 = exchange(temp_s2);
                                for (int i = 0; i < temp_se2.Length; i++)
                                {
                                    Start_send2[3 - i] = temp_se2[i];
                                }

                                serialPort1.Write(Start_send2, 0, 4);//开始时间2发送

                                string end_time2 = dateTimePicker_t_2.Text;
                                string end_send2 = TimeDifference(end_time2);

                                byte[] temp_e2 = strToHexByte(end_send2);
                                byte[] temp_ee2 = exchange(temp_e2);
                                for (int i = 0; i < temp_ee1.Length; i++)
                                {
                                    End_send2[3 - i] = temp_ee2[i];
                                }

                                serialPort1.Write(End_send2, 0, 4);//结束时间2发送

                                serialPort1.Write(strToHexByte(tb_times_D2.Text.Trim()), 0, 1);//次数2/天发送
                                serialPort1.Write(strToHexByte(tb_times_T2.Text.Trim()), 0, 1);//次数2/次发送

                                //this.textBox1_test.Text = end_send2;


                            }
                            else
                            {

                                serialPort1.Write(Start_send2, 0, 4);//开始时间2发送
                                serialPort1.Write(End_send2, 0, 4);//结束时间2发送
                                serialPort1.Write(strToHexByte(tb_times_D2.Text.Trim()), 0, 1);//次数2/天发送
                                serialPort1.Write(strToHexByte(tb_times_T2.Text.Trim()), 0, 1);//次数2/次发送
                            }

                            serialPort1.Write(Sum, 0, 4);
                        }
                        else//duration
                        {

                            serialPort1.Write(strToHexByte(cb_days.Text), 0, 1);//天数空值
                            serialPort1.Write(Start_send2, 0, 4);//开始时间2发送
                            serialPort1.Write(End_send2, 0, 4);//结束时间2发送
                            serialPort1.Write(strToHexByte(tb_times_D1.Text.Trim()), 0, 1);//次数2/天发送
                            serialPort1.Write(strToHexByte(tb_times_T1.Text.Trim()), 0, 1);//次数2/次发送
                            serialPort1.Write(Start_send2, 0, 4);//开始时间2发送
                            serialPort1.Write(End_send2, 0, 4);//结束时间2发送
                            serialPort1.Write(strToHexByte(tb_times_D2.Text.Trim()), 0, 1);//次数2/天发送
                            serialPort1.Write(strToHexByte(tb_times_T2.Text.Trim()), 0, 1);//次数2/次发送

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

                            serialPort1.Write(Sum, 0, 4);
                        }

                        serialPort1.Write(Zeros, 0, 14);

                        if (rb_Sweep.Checked)
                        {
                            if (rb_dur.Checked)
                            {
                                FileStream fs = new FileStream(parameter21, FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.WriteLine("Start at:   " + "\t" + System.DateTime.Now + "\t");
                                sw.WriteLine("Frequency:  " + "\t" + tb_freq.Text + "\t" + cb_freq.Text);
                                sw.WriteLine("Sweep from: " + "\t" + tb_Sweep_f.Text + "\t" + cb_freq_f.Text);
                                sw.WriteLine("to:         " + "\t" + tb_Sweep_t.Text + "\t" + cb_freq_t.Text);
                                sw.WriteLine("Amplitude:  " + "\t" + tb_amp.Text + "\t" + "mV");
                                sw.WriteLine("ODR:        " + "\t" + cb_dor.Text + "\t" + "sps");
                                sw.WriteLine("RTIA:       " + "\t" + cb_tia.Text + "\t");
                                sw.WriteLine("DFT:        " + "\t" + cb_dft.Text + "\t");
                                sw.WriteLine("Sweep:      " + "\t" + tb_s_p.Text + "\t" + "Points");
                                sw.WriteLine("Single_m:   " + "\t" + rb_dur.Checked + "\t");
                                sw.WriteLine("Multiple_m: " + "\t" + rb_rep.Checked + "\t");
                                sw.WriteLine("Duration_h: " + "\t" + tb_d_h.Text + "\t");
                                sw.WriteLine("Duration_m: " + "\t" + tb_d_m.Text + "\t");
                                sw.WriteLine("Duration_s: " + "\t" + tb_d_s.Text + "\t");
                                sw.WriteLine("Start_Datum:" + "\t" + DTP_Start.Text + "\t");
                                sw.WriteLine("End_Datum:  " + "\t" + DTP_End.Text + "\t");
                                sw.WriteLine("Repeat:     " + "\t" + cb_days.Text + "\t" + "days");
                                sw.WriteLine("From:       " + "\t" + dateTimePicker_f_1.Text + "\t");
                                sw.WriteLine("to:         " + "\t" + dateTimePicker_t_1.Text + "\t");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_D1.Text + "\t" + "Times/day");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_T1.Text + "\t" + "Times/Cycle");
                                sw.WriteLine("Second:    " + "\t" + checkBox_SEC.Checked + "\t");
                                sw.WriteLine("From:       " + "\t" + dateTimePicker_f_2.Text + "\t");
                                sw.WriteLine("to:         " + "\t" + dateTimePicker_t_2.Text + "\t");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_D2.Text + "\t" + "Times/day");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_T2.Text + "\t" + "Times/Cycle");
                                sw.Flush();
                                sw.Close();
                            }
                            else
                            {
                                FileStream fs = new FileStream(parameter22, FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.WriteLine("Start at:   " + "\t" + System.DateTime.Now + "\t");
                                sw.WriteLine("Frequency:  " + "\t" + tb_freq.Text + "\t" + cb_freq.Text);
                                sw.WriteLine("Sweep from: " + "\t" + tb_Sweep_f.Text + "\t" + cb_freq_f.Text);
                                sw.WriteLine("to:         " + "\t" + tb_Sweep_t.Text + "\t" + cb_freq_t.Text);
                                sw.WriteLine("Amplitude:  " + "\t" + tb_amp.Text + "\t" + "mV");
                                sw.WriteLine("ODR:        " + "\t" + cb_dor.Text + "\t" + "sps");
                                sw.WriteLine("RTIA:       " + "\t" + cb_tia.Text + "\t");
                                sw.WriteLine("DFT:        " + "\t" + cb_dft.Text + "\t");
                                sw.WriteLine("Sweep:      " + "\t" + tb_s_p.Text + "\t" + "Points");
                                sw.WriteLine("Single_m:   " + "\t" + rb_dur.Checked + "\t");
                                sw.WriteLine("Multiple_m: " + "\t" + rb_rep.Checked + "\t");
                                sw.WriteLine("Duration_h: " + "\t" + tb_d_h.Text + "\t");
                                sw.WriteLine("Duration_m: " + "\t" + tb_d_m.Text + "\t");
                                sw.WriteLine("Duration_s: " + "\t" + tb_d_s.Text + "\t");
                                sw.WriteLine("Start_Datum:" + "\t" + DTP_Start.Text + "\t");
                                sw.WriteLine("End_Datum:  " + "\t" + DTP_End.Text + "\t");
                                sw.WriteLine("Repeat:     " + "\t" + cb_days.Text + "\t" + "days");
                                sw.WriteLine("From:       " + "\t" + dateTimePicker_f_1.Text + "\t");
                                sw.WriteLine("to:         " + "\t" + dateTimePicker_t_1.Text + "\t");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_D1.Text + "\t" + "Times/day");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_T1.Text + "\t" + "Times/Cycle");
                                sw.WriteLine("Second:    " + "\t" + checkBox_SEC.Checked + "\t");
                                sw.WriteLine("From:       " + "\t" + dateTimePicker_f_2.Text + "\t");
                                sw.WriteLine("to:         " + "\t" + dateTimePicker_t_2.Text + "\t");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_D2.Text + "\t" + "Times/day");
                                sw.WriteLine("repeat:     " + "\t" + tb_times_T2.Text + "\t" + "Times/Cycle");
                                sw.Flush();
                                sw.Close();

                            }
                        }
                        else if (rb_Frequncy.Checked)
                        {
                            FileStream fs = new FileStream(parameter1, FileMode.Append);
                            StreamWriter sw = new StreamWriter(fs);

                            sw.WriteLine("Start at:   " + "\t" + System.DateTime.Now + "\t");
                            sw.WriteLine("Frequency:  " + "\t" + tb_freq.Text + "\t" + cb_freq.Text);
                            sw.WriteLine("Sweep from: " + "\t" + tb_Sweep_f.Text + "\t" + cb_freq_f.Text);
                            sw.WriteLine("to:         " + "\t" + tb_Sweep_t.Text + "\t" + cb_freq_t.Text);
                            sw.WriteLine("Amplitude:  " + "\t" + tb_amp.Text + "\t" + "mV");
                            sw.WriteLine("ODR:        " + "\t" + cb_dor.Text + "\t" + "sps");
                            sw.WriteLine("RTIA:       " + "\t" + cb_tia.Text + "\t");
                            sw.WriteLine("DFT:        " + "\t" + cb_dft.Text + "\t");
                            sw.WriteLine("Sweep:      " + "\t" + tb_s_p.Text + "\t" + "Points");
                            sw.WriteLine("Single_m:   " + "\t" + rb_dur.Checked + "\t");
                            sw.WriteLine("Multiple_m: " + "\t" + rb_rep.Checked + "\t");
                            sw.WriteLine("Duration_h: " + "\t" + tb_d_h.Text + "\t");
                            sw.WriteLine("Duration_m: " + "\t" + tb_d_m.Text + "\t");
                            sw.WriteLine("Duration_s: " + "\t" + tb_d_s.Text + "\t");
                            sw.WriteLine("Start_Datum:" + "\t" + DTP_Start.Text + "\t");
                            sw.WriteLine("End_Datum:  " + "\t" + DTP_End.Text + "\t");
                            sw.WriteLine("Repeat:     " + "\t" + cb_days.Text + "\t" + "days");
                            sw.WriteLine("From:       " + "\t" + dateTimePicker_f_1.Text + "\t");
                            sw.WriteLine("to:         " + "\t" + dateTimePicker_t_1.Text + "\t");
                            sw.WriteLine("repeat:     " + "\t" + tb_times_D1.Text + "\t" + "Times/day");
                            sw.WriteLine("repeat:     " + "\t" + tb_times_T1.Text + "\t" + "Times/Cycle");
                            sw.WriteLine("Second:    " + "\t" + checkBox_SEC.Checked + "\t");
                            sw.WriteLine("From:       " + "\t" + dateTimePicker_f_2.Text + "\t");
                            sw.WriteLine("to:         " + "\t" + dateTimePicker_t_2.Text + "\t");
                            sw.WriteLine("repeat:     " + "\t" + tb_times_D2.Text + "\t" + "Times/day");
                            sw.WriteLine("repeat:     " + "\t" + tb_times_T2.Text + "\t" + "Times/Cycle");
                            sw.Flush();
                            sw.Close();
                        }


                    }
                    catch (Exception ee)
                    {
                        MessageBox.Show("Please fill in the parameters completely");

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


        //FDA+TD stop
        private void btn_stop_Click(object sender, EventArgs e)
        {

            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x09 }, 0, 5);
            btn_start.Enabled = true;
            btn_stop.Enabled = false;


            if (rb_Frequncy.Checked)
            {

                FileStream fs = new FileStream(parameter1, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
                sw.WriteLine("\n");
                sw.Flush();
                sw.Close();

                cnt_TD++;

                if (!File.Exists(pathString2 + "\\" + cnt_TD + "Single_Measurement1.txt"))
                {

                    StreamWriter sw1 = new StreamWriter(pathString2 + "\\" + cnt_TD + "Single_Measurement1.txt");
                    sw1.WriteLine("Time" + "\t" + "Voltag" + "\t" + "Current" + "\t" + "Impedence");
                    sw1.Flush();
                    sw1.Close();
                }
            }
            else
            {
                if (rb_dur.Checked)
                {
                    FileStream fs = new FileStream(parameter21, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
                    sw.WriteLine("\n");
                    sw.Flush();
                    sw.Close();

                    cnt_FDA1++;

                    if (!File.Exists(pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt"))
                    {

                        StreamWriter sw1 = new StreamWriter(pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt");
                        sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                        sw1.Flush();
                        sw1.Close();
                    }

                }
                else
                {
                    FileStream fs = new FileStream(parameter22, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
                    sw.WriteLine("\n");
                    sw.Flush();
                    sw.Close();

                    cnt_FDA2++;

                    if (!File.Exists(pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt"))
                    {

                        StreamWriter sw1 = new StreamWriter(pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt");
                        sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                        sw1.Flush();
                        sw1.Close();
                    }
                }
            }
        }

        //固定频率操作框
        private void rb_Frequncy_CheckedChanged(object sender, EventArgs e)
        {
            tb_Sweep_f.Enabled = false;
            tb_Sweep_f.Visible = false;
            tb_Sweep_f.Text = null;
            tb_Sweep_t.Enabled = false;
            tb_Sweep_t.Visible = false;
            tb_Sweep_t.Text = null;
            cb_freq_f.Enabled = false;
            cb_freq_t.Enabled = false;
            cb_freq_f.Visible = false;
            cb_freq_t.Visible = false;
            tb_freq.Enabled = true;
            cb_freq.Enabled = true;
            tb_freq.Visible= true;
            cb_freq.Visible = true;
            label13.Visible = false;
            label11.Visible = false;

            rb_Sweep.Visible = false;
            rb_Frequncy.Visible = true;
        }

        //扫频操作框
        private void rb_Sweep_CheckedChanged(object sender, EventArgs e)
        {
            tb_freq.Enabled = false;
            tb_freq.Visible = false;
            tb_freq.Text = null;
            cb_freq.Enabled = false;
            cb_freq.Visible = false;
            tb_Sweep_f.Enabled = true;
            tb_Sweep_t.Enabled = true;
            tb_Sweep_f.Visible = true;
            tb_Sweep_t.Visible = true;
            cb_freq_f.Enabled = true;
            cb_freq_t.Enabled = true;
            cb_freq_f.Visible = true;
            cb_freq_t.Visible = true;
            rb_Sweep.Visible = true;
            rb_Frequncy.Visible = false;
            label13.Visible = true;
            label11.Visible = true;
        }

        //repeat 操作框
        private void rb_rep_CheckedChanged(object sender, EventArgs e)
        {
            cb_days.Enabled = false;
            tb_d_h.Enabled = false;
            tb_d_m.Enabled = false;
            tb_d_s.Enabled = false;
            tb_d_h.Text = null;
            tb_d_m.Text = null;
            tb_d_s.Text = null;
            gb_mulitply.Visible = true;
            gb_Single.Visible = false;
        }

        //duration 操作框
        private void rb_dur_CheckedChanged(object sender, EventArgs e)
        {
            cb_days.Enabled = false;
            tb_d_h.Enabled = true;
            tb_d_m.Enabled = true;
            tb_d_s.Enabled = true;
            gb_mulitply.Visible = false;
            gb_Single.Visible = true;
        }

        //second choice in FDA
        private void checkBox_SEC_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SEC.Checked)
            {
                panel_sec.Visible = true;
            }
            else
            {
                panel_sec.Visible = false;
            }

        }

        private void dateTimePicker_f_1_ValueChanged(object sender, EventArgs e)
        {
            String tmp1 = dateTimePicker_f_1.Text;
            string tmp3 = TimeDifference(tmp1);
        }


        private void checkBox_SEC_CheckedChanged_1(object sender, EventArgs e)
        {
            if (checkBox_SEC.Checked)
            {
                panel_sec.Visible = true;
            }
            else
            {
                panel_sec.Visible = false;
            }
        }



        //*********************************************//
        //****************DC窗口**********************//
        //*******************************************//

         // DC send
        private void btn_start1_Click(object sender, EventArgs e)
        {
            btn_start1.Enabled = false;
            btn_stop1.Enabled = true;
            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x03 }, 0, 5);

            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    int temp_h1 = 0;
                    int temp_m1 = 0;
                    int temp_s1 = 0;
                    int sum1 = 0;

                    byte[] flag = new byte[2];
                    flag[0] = 0x00;
                    flag[1] = 0x01;

                    byte[] Sum1 = new byte[4];
                    for (int i = 0; i < 4; i++)
                    {
                        Sum1[i] = 0x00;
                    }


                    byte[] Zeros = new byte[15];
                    for (int i = 0; i < 15; i++)
                    {
                        Zeros[i] = 0x00;
                    }

                    byte[] amp1 = new byte[] { 0x00, 0x00 };
                    byte[] Start_v = new byte[] { 0x00, 0x00 };
                    try
                    {
                        byte[] tmp_sv = strToHexByte(tb_start.Text);
                        byte[] tmp_sv1 = exchange(tmp_sv);
                        for (int i = 0; i < tmp_sv1.Length; i++)
                        {
                            Start_v[1 - i] = tmp_sv1[i];
                        }

                        serialPort1.Write(Start_v, 0, 2);//Start_v


                        byte[] tmp2 = strToHexByte(tb_amp1.Text);
                        byte[] tmp = exchange(tmp2);
                        for (int i = 0; i < tmp.Length; i++)
                        {
                            amp1[1 - i] = tmp[i];
                        }

                        serialPort1.Write(amp1, 0, 2);//amp


                        if (rb_Sweep1.Checked)
                        {
                            serialPort1.Write(flag, 0, 1);//Sweep


                            serialPort1.Write(strToHexByte(tb_s_p1.Text), 0, 1);//points

                            if (rb_Square.Checked)
                            {
                                serialPort1.Write(flag, 0, 1);//Sawtooth
                            }
                            else
                            {
                                serialPort1.Write(flag, 1, 1);//Triangle
                            }

                            serialPort1.Write(Sum1, 0, 4);
                            serialPort1.Write(strToHexByte(tb_times.Text.Trim()), 0, 1);//Times

                        }
                        else
                        {

                            serialPort1.Write(flag, 1, 1);//Duration 
                            serialPort1.Write(flag, 0, 1);//points空值
                            serialPort1.Write(flag, 0, 1);//波形空值

                            temp_h1 = Convert.ToInt16(tb_d_h1.Text);
                            temp_m1 = Convert.ToInt16(tb_d_m1.Text);
                            temp_s1 = Convert.ToInt16(tb_d_s1.Text);
                            sum1 = 3600 * temp_h1 + 60 * temp_m1 + temp_s1;

                            byte[] temp_s = strToHexByte(Convert.ToString(sum1));
                            byte[] temp_ss = exchange(temp_s);
                            for (int i = 0; i < temp_ss.Length; i++)
                            {
                                Sum1[3 - i] = temp_ss[i];
                            }

                            serialPort1.Write(Sum1, 0, 4);
                            serialPort1.Write(strToHexByte(tb_times.Text.Trim()), 0, 1);//Times
                        }

                        serialPort1.Write(Zeros, 0, 15);
                    }

                    catch
                    {
                        MessageBox.Show("Please fill in the parameters completely!");
                    }
                }


                FileStream fs = new FileStream(parameter3, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Start at:   " + "\t" + System.DateTime.Now + "\t");
                sw.WriteLine("Strat at:   " + "\t" + tb_start.Text + "\t" + "mV");
                sw.WriteLine("Amplitude:  " + "\t" + tb_amp1.Text + "\t" + "mV");
                sw.WriteLine("Sweep:      " + "\t" + rb_Sweep1.Checked + "\t");
                sw.WriteLine("Square:     " + "\t" + rb_Square.Checked + "\t");
                sw.WriteLine("Triangle:   " + "\t" + rb_Triangle.Checked + "\t");
                sw.WriteLine("Sweep:      " + "\t" + tb_s_p1.Text + "\t" + "Points");
                sw.WriteLine("Duration:   " + "\t" + rb_Duration1.Checked + "\t");
                sw.WriteLine("Duration_h: " + "\t" + tb_d_h1.Text + "\t");
                sw.WriteLine("Duration_m: " + "\t" + tb_d_m1.Text + "\t");
                sw.WriteLine("Duration_s: " + "\t" + tb_d_s1.Text + "\t");
                sw.WriteLine("repeat:     " + "\t" + tb_times.Text + "\t" + "Times");

                sw.Flush();
                sw.Close();



            }
            catch (Exception ex)
            {
                serialPort1.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();

                MessageBox.Show(ex.Message);

            }
        }

        //DC Stop
        private void btn_stop1_Click(object sender, EventArgs e)
        {
            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x08 }, 0, 5);
            btn_start1.Enabled = true;
            btn_stop1.Enabled = false;

            FileStream fs = new FileStream(parameter3, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
            sw.WriteLine("\n");
            sw.Flush();
            sw.Close();

            cnt_UIR++;

            if (!File.Exists(pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt"))
            {

                StreamWriter sw1 = new StreamWriter(pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt");
                sw1.WriteLine("Time" + "\t" + "Voltag" + "\t" + "Current" + "\t" + "Impedence");
                sw1.Flush();
                sw1.Close();
            }
        }


        //Slider for choice of wave
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tb_s_p1.Text = trackBar1.Value.ToString();
        }


        //wave choose
        private void tb_s_p1_TextChanged(object sender, EventArgs e)
        {
            chart_v.Series[0].Points.Clear();
            double k = Convert.ToDouble(tb_s_p1.Text);
            double sv = Convert.ToDouble(tb_start.Text);
            double peak = Convert.ToDouble(tb_amp1.Text);
            int i;
            double y;
            double t;
            if (rb_Triangle.Checked)
            {
                for (i = 0; i < 2 * 100; i++)
                {
                    t = 1.0 / 100 * i;
                    y = (peak - sv) * 0.5 * (trianglewav(2 * Math.PI * (200 - 0.75 * k + 1) * t / 100, 0.5) + 1) + sv;
                    //printf("%f\t%f\n", t, y);
                    this.chart_v.Series[0].Points.AddXY(t, y);

                }
            }
            else
            {
                for (i = 0; i < 2 * 100; i++)
                {
                    t = 1.0 / 100 * i;
                    y = (peak - sv) * (Squarewav(2 * Math.PI * (200 - 0.75 * k + 1) * t / 100, 0.5)) + sv;
                    //printf("%f\t%f\n", t, y);
                    this.chart_v.Series[0].Points.AddXY(t, y);

                }
            }

        }

        //Model choose

        private void rb_Sweep1_CheckedChanged(object sender, EventArgs e)
        {
            tb_s_p1.Enabled = true;
            tb_d_h1.Enabled = false;
            tb_d_m1.Enabled = false;
            tb_d_s1.Enabled = false;
            tb_times.Enabled = false;
        }

        private void rb_Duration1_CheckedChanged(object sender, EventArgs e)
        {
            tb_s_p1.Enabled = false;
            tb_d_h1.Enabled = true;
            tb_d_m1.Enabled = true;
            tb_d_s1.Enabled = true;
            tb_times.Enabled = true;
        }


        //*********************************************//
        //****************Combination window**********//
        //*******************************************//

        //Combination send
        private void btn_comb_start_Click(object sender, EventArgs e)
        {
            btn_comb_start.Enabled = false;
            btn_stop2.Enabled = true;


            if (Temperature.Equals("℃"))
                Temperature = tempt.ToString() + "℃";
            else
            {
                Temperature = "37℃";
            }



            DialogResult dr = MessageBox.Show("Everything already?", "Combination Measurement", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {


                tb_temp.Text = Temperature;
                for (int CNT_T = 0; CNT_T < 10; CNT_T++)
                {
                    this.chart_temp.Series[0].Points.AddXY(CNT_T, tempt);
                }

                try
                {
                    //首先判断串口是否开启
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x04 }, 0, 5);

                        byte[] flag = new byte[2];
                        flag[0] = 00;
                        flag[1] = 01;


                        byte[] Fre2 = new byte[8];
                        for (int i = 0; i < 8; i++)
                        {
                            Fre2[i] = 0x00;

                        }
                        byte[] Start_send12 = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            Start_send12[i] = 0x00;
                        }

                        byte[] Start_send22 = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            Start_send22[i] = 0x00;
                        }

                        byte[] End_send12 = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            End_send12[i] = 0x00;
                        }

                        byte[] End_send22 = new byte[4];
                        for (int i = 0; i < 4; i++)
                        {
                            End_send22[i] = 0x00;
                        }

                        byte[] amp2 = new byte[] { 0x00, 0x00 };
                        byte[] points2 = new byte[] { 0x00, 0x00 };

                        try
                        {
                            serialPort1.Write(strToHexByte(cb_dor2.Text.Trim()), 0, 1);//odr

                            byte[] tmp2 = strToHexByte(tb_amp2.Text);
                            byte[] tmp = exchange(tmp2);
                            for (int i = 0; i < tmp.Length; i++)
                            {
                                amp2[1 - i] = tmp[i];
                            }

                            serialPort1.Write(amp2, 0, 2);//amp

                            serialPort1.Write(strToHexByte(cb_dft2.Text.Trim()), 0, 1);//dft


                            //频率

                            if (cb_freq_f2.Text.Equals("kHz"))
                            {
                                byte[] temp1 = strToHexByte((Convert.ToInt32(tb_Sweep_f2.Text) * 1000).ToString());
                                byte[] temp = exchange(temp1);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre2[3 - i] = temp[i];
                                }
                            }
                            else
                            {

                                byte[] temp2 = strToHexByte(tb_Sweep_f2.Text);
                                byte[] temp = exchange(temp2);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre2[3 - i] = temp[i];
                                }
                            }
                            if (cb_freq_t2.Text.Equals("kHz"))
                            {
                                byte[] temp1 = strToHexByte((Convert.ToInt32(tb_Sweep_t2.Text) * 1000).ToString());
                                byte[] temp = exchange(temp1);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre2[7 - i] = temp[i];
                                }
                            }
                            else
                            {
                                byte[] temp2 = strToHexByte(tb_Sweep_t2.Text);
                                byte[] temp = exchange(temp2);
                                for (int i = 0; i < temp.Length; i++)
                                {
                                    Fre2[7 - i] = temp[i];
                                }
                            }


                            serialPort1.Write(Fre2, 0, 8);

                            serialPort1.Write(flag, 0, 1);//log  暂时没用

                            serialPort1.Write(strToHexByte(cb_tia2.Text.Trim()), 0, 1);//rtia

                            serialPort1.Write(strToHexByte(tb_s_p2.Text.Trim()), 0, 1);//points

                            //repeat

                            //计算天数

                            string dt1 = System.DateTime.Now.ToString("yyyy/MM/dd");
                            DTP_Start.Text = dt1;
                            DateTime d1 = DateTime.Parse(DTP_Start2.Text);
                            DateTime d2 = DateTime.Parse(DTP_End2.Text);
                            System.TimeSpan ND = d2 - d1;
                            int ts1 = ND.Days;   //天数差
                            int ts2 = 0;
                            if ((ts1 > 0) && (ts1 < 8))
                            {
                                ts2 = ts1;
                            }

                            string ts = Convert.ToString(ts2);
                            cb_days2.Text = ts;

                            serialPort1.Write(strToHexByte(cb_days2.Text), 0, 1);

                            //开始时间

                            string start_time1 = dateTimePicker_f_3.Text;
                            string start_send1 = TimeDifference(start_time1);

                            byte[] temp_s1 = strToHexByte(start_send1);
                            byte[] temp_se1 = exchange(temp_s1);
                            for (int i = 0; i < temp_se1.Length; i++)
                            {
                                Start_send12[3 - i] = temp_se1[i];
                            }

                            serialPort1.Write(Start_send12, 0, 4);//开始时间1发送

                            string end_time1 = dateTimePicker_t_3.Text;
                            string end_send1 = TimeDifference(end_time1);

                            byte[] temp_e1 = strToHexByte(end_send1);
                            byte[] temp_ee1 = exchange(temp_e1);
                            for (int i = 0; i < temp_ee1.Length; i++)
                            {
                                End_send12[3 - i] = temp_ee1[i];
                            }

                            serialPort1.Write(End_send12, 0, 4);//结束时间1发送

                            serialPort1.Write(strToHexByte(tb_times_D3.Text.Trim()), 0, 1);//次数1/天发送
                            serialPort1.Write(strToHexByte(tb_times_T3.Text.Trim()), 0, 1);//次数1/次发送

                            if (checkBox_SEC2.Checked)
                            {
                                string start_time2 = dateTimePicker_f_4.Text;
                                string start_send2 = TimeDifference(start_time2);
                                byte[] temp_s2 = strToHexByte(start_send2);
                                byte[] temp_se2 = exchange(temp_s2);
                                for (int i = 0; i < temp_se2.Length; i++)
                                {
                                    Start_send22[3 - i] = temp_se2[i];
                                }

                                serialPort1.Write(Start_send22, 0, 4);//开始时间2发送

                                string end_time2 = dateTimePicker_t_4.Text;
                                string end_send2 = TimeDifference(end_time2);

                                byte[] temp_e2 = strToHexByte(end_send2);
                                byte[] temp_ee2 = exchange(temp_e2);
                                for (int i = 0; i < temp_ee1.Length; i++)
                                {
                                    End_send22[3 - i] = temp_ee2[i];
                                }

                                serialPort1.Write(End_send22, 0, 4);//结束时间2发送

                                serialPort1.Write(strToHexByte(tb_times_D4.Text.Trim()), 0, 1);//次数2/天发送
                                serialPort1.Write(strToHexByte(tb_times_T4.Text.Trim()), 0, 1);//次数2/次发送


                            }
                            else
                            {
                                serialPort1.Write(Start_send22, 0, 4);//开始时间2发送
                                serialPort1.Write(End_send22, 0, 4);//结束时间2发送
                                serialPort1.Write(strToHexByte(tb_times_D4.Text.Trim()), 0, 1);//次数2/天发送
                                serialPort1.Write(strToHexByte(tb_times_T4.Text.Trim()), 0, 1);//次数2/次发送
                            }

                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("Please fill in the parameters completely!");

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

                try
                {
                    //首先判断串口是否开启
                    if (serialPort1.IsOpen)
                    {


                        byte[] flag = new byte[2];
                        flag[0] = 0x00;
                        flag[1] = 0x01;


                        byte[] Zeros = new byte[20];
                        for (int i = 0; i < 20; i++)
                        {
                            Zeros[i] = 0x00;
                        }

                        byte[] amp4 = new byte[] { 0x00, 0x00 };
                        try
                        {

                            byte[] tmp2 = strToHexByte(tb_amp4.Text);
                            byte[] tmp = exchange(tmp2);
                            for (int i = 0; i < tmp.Length; i++)
                            {
                                amp4[1 - i] = tmp[i];
                            }

                            serialPort1.Write(amp4, 0, 2);//amp                                                          

                            serialPort1.Write(strToHexByte(tb_d_s2.Text), 0, 1);//duration

                            if (rb_temp_y.Checked)
                            {
                                serialPort1.Write(flag, 0, 1);
                            }
                            else
                            {
                                serialPort1.Write(flag, 1, 1);
                            }

                            serialPort1.Write(Zeros, 0, 20);
                        }

                        catch
                        {
                            MessageBox.Show("Please fill in the parameters completely!");
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

                FileStream fs = new FileStream(parameter4, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine("Start at:   " + "\t" + System.DateTime.Now + "\t");
                sw.WriteLine("Sweep from: " + "\t" + tb_Sweep_f.Text + "\t" + cb_freq_f.Text);
                sw.WriteLine("to:         " + "\t" + tb_Sweep_t.Text + "\t" + cb_freq_t.Text);
                sw.WriteLine("Amplitude:  " + "\t" + tb_amp2.Text + "\t" + "mV");
                sw.WriteLine("ODR:        " + "\t" + cb_dor2.Text + "\t" + "sps");
                sw.WriteLine("RTIA:       " + "\t" + cb_tia2.Text + "\t");
                sw.WriteLine("DFT:        " + "\t" + cb_dft2.Text + "\t");
                sw.WriteLine("Sweep:      " + "\t" + tb_s_p2.Text + "\t" + "Points");
                sw.WriteLine("Start_Datum:" + "\t" + DTP_Start2.Text + "\t");
                sw.WriteLine("End_Datum:  " + "\t" + DTP_End2.Text + "\t");
                sw.WriteLine("Repeat:     " + "\t" + cb_days2.Text + "\t" + "days");
                sw.WriteLine("From:       " + "\t" + dateTimePicker_f_3.Text + "\t");
                sw.WriteLine("to:         " + "\t" + dateTimePicker_t_3.Text + "\t");
                sw.WriteLine("repeat:     " + "\t" + tb_times_D3.Text + "\t" + "Times/day");
                sw.WriteLine("repeat:     " + "\t" + tb_times_T3.Text + "\t" + "Times/Cycle");
                sw.WriteLine("Second:    " + "\t" + checkBox_SEC2.Checked + "\t");
                sw.WriteLine("From:       " + "\t" + dateTimePicker_f_4.Text + "\t");
                sw.WriteLine("to:         " + "\t" + dateTimePicker_t_4.Text + "\t");
                sw.WriteLine("repeat:     " + "\t" + tb_times_D4.Text + "\t" + "Times/day");
                sw.WriteLine("repeat:     " + "\t" + tb_times_T4.Text + "\t" + "Times/Cycle");
                sw.WriteLine("Amplitude:  " + "\t" + tb_amp4.Text + "\t" + "mV");
                sw.WriteLine("Duration_s: " + "\t" + tb_d_s1.Text + "\t");
                sw.WriteLine("Tempe/yes:   " + "\t" + rb_temp_y.Checked + "\t");
                sw.WriteLine("Tempe/no:   " + "\t" + rb_temp_y.Checked + "\t");
                sw.Flush();
                sw.Close();
            }
            else

            {
                btn_comb_start.Enabled = true;

            }
        }

        //stop of Combination
        private void btn_stop2_Click_1(object sender, EventArgs e)
        {
            btn_stop2.Enabled = false;
            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0xAA, 0x07 }, 0, 5);
            btn_comb_start.Enabled = true;
            chart_temp.Series[0].Points.Clear();

            FileStream fs = new FileStream(parameter4, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
            sw.WriteLine("\n");
            sw.Flush();
            sw.Close();

            cnt_comb++;

            if (!File.Exists(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt"))
            {

                StreamWriter sw1 = new StreamWriter(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt");
                sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                sw1.Flush();
                sw1.Close();
            }

        }

        //second check in COMB
        private void checkBox_SEC2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox_SEC2.Checked)
            {
                dateTimePicker_f_4.Enabled = true;
                dateTimePicker_t_4.Enabled = true;
                tb_times_D4.Enabled = true;
                tb_times_T4.Enabled = true;
            }

            else
            {
                dateTimePicker_f_4.Enabled = false;
                dateTimePicker_t_4.Enabled = false;
                tb_times_D4.Enabled = false;
                tb_times_T4.Enabled = false;
            }
        }

        //*********************************************//
        //****************New Project*****************//
        //*******************************************//

        //Form 2(data send back)
        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChartClear();
            NewProject f2 = new NewProject();
            f2.ShowDialog();
            if (f2.DialogResult == DialogResult.OK)
            {
                parameter1 = f2.Parameter1;
                parameter21 = f2.Parameter21;
                parameter22 = f2.Parameter22;
                parameter3 = f2.Parameter3;
                parameter4 = f2.Parameter4;
                pathString1 = f2.pathString1;
                pathString2 = f2.pathString2;
                pathString3 = f2.pathString3;
                pathString4 = f2.pathString4;
                Single_m = f2.Single_m;
                Single_m1 = f2.Single_m1;
                Multiple_m = f2.Multiple_m;
                U_I_R = f2.U_I_R;
                ID_Num = f2.ID_Num;
                Combination_m = f2.Combination_M;

                enableButtons(true, true, true, true, false, false, false, true, false, false);
                btn_freq.Visible = true;
                btn_TD.Visible = true;
                btn_DC.Visible = true;
                btn_AC.Visible = true;
                btn_cfg.Visible = true;
                panel_switch.Visible = true;
                panel_load.Visible = false;
            }

        }

        //*********************************************//
        //****************load  Project***************//
        //*******************************************//

         //ID Struct
        public struct ID
        {
            public string ID_NUM;
            public string ID_NAME;
            public string ID_FILE;
            public string ID_DES;
        }

        //open Project
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {

                enableButtons(true, true, true, true, false, false, false, true, false, false);
                btn_freq.Visible = true;
                btn_TD.Visible = true;
                btn_DC.Visible = true;
                btn_AC.Visible = true;
                btn_cfg.Visible = true;
                panel_switch.Visible = true;
                panel_load.Visible = false;


                foldPath = dialog.SelectedPath;

                pathString1 = foldPath + "/FDA";
                pathString2 = foldPath + "/TD";
                pathString3 = foldPath + "/DC"; ;
                pathString4 = foldPath + "/Combination";

                parameter1 = pathString2 + "/Parameter.txt"; ;
                parameter21 = pathString1 + "/Parameter_s.txt";
                parameter22 = pathString1 + "/Parameter_m.txt";
                parameter3 = pathString3 + "/Parameter.txt";
                parameter4 = pathString4 + "/Parameter.txt";

                Single_m = pathString1 + "/0Single_Measurement.txt";
                Single_m1 = pathString2 + "/0Single_Measurement.txt";
                Multiple_m = pathString1 + "/0Multiple_Measurement.txt";
                U_I_R = pathString3 + "/0U_I_R_data.txt";
                Combination_m = pathString4 + "/0Combination_Measurement.txt";

                string filePathOnly = Path.GetDirectoryName(foldPath);
                string file = Path.GetFileName(filePathOnly);

                Directory.SetCurrentDirectory(Directory.GetParent(foldPath).FullName);

                String ID_path = Directory.GetCurrentDirectory() + "/ID_Information.txt";

                string filePathOnly1 = Path.GetDirectoryName(pathString1);
                string fold1 = Path.GetFileName(filePathOnly1);


                //ID match

                List<ID> points = new List<ID>();
                ID p;
                string[] lines = File.ReadAllLines(ID_path);
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

                    // 获取Y（第一列）        
                    p.ID_NUM = v[0];
                    // 获取Y（第二列）        
                    p.ID_NAME = v[1];
                    p.ID_FILE = v[2];
                    p.ID_DES = v[3];
                    points.Add(p);
                    if (fold1.Equals(p.ID_NAME))
                    {
                        ID_Num = p.ID_NUM;
                    }

                }

            }
        }

        //*********************************************//
        //****************Data Analyser***************//
        //*******************************************//
        
         //open Dateset
        private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btn_freq.Visible = false;
            btn_TD.Visible = false;
            btn_DC.Visible = false;
            btn_AC.Visible = false;
            btn_cfg.Visible = false;
            panel_switch.Visible = false;

            button1.Visible = false;
            SaveBode2.Visible = false;
            SaveNyq.Visible = false;
            SaveTD.Visible = false;
            button2.Visible = false;
            panel_switch.Visible = false;
            panel_load.Visible = true;
            enableButtons(false, false, false, false, false, false, false, false, false, false);
            newProjectToolStripMenuItem.Enabled = false;

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                cnt7++;
                foldPath = dialog.SelectedPath;

            }
        }

        public struct Point
        {
            public double X;
            public double Y;
            public double Z;
        }


        public struct Point_T
        {
            public double X;
            public double Y;
        }

        public struct Point_U
        {
            public double X;
            public double Y;
            public double Z;
            public double K;
        }



        private void btn_fre_load_Click(object sender, EventArgs e)
        {

            btn_fre_load.Visible = false;
            btn_s.Visible = true;
            btn_m.Visible = true;

        }

        //painting of single measurement in FDA

        private void btn_s_Click(object sender, EventArgs e)
        {

            string FDA = foldPath + "/FDA";
            string FDA_S = FDA + "/0Single_Measurement.txt";
            string FDA_M = FDA + "/0Multiple_Measurement.txt";
            string FDA_P1 = FDA + "/Parameter_s.txt";
            string[] lines = File.ReadAllLines(FDA_S);
            chart1.Series.Add((cnt7).ToString());//添加
            chart2.Series.Add((cnt7).ToString());//添加
            chart3.Series.Add((cnt7).ToString());//添加
            this.chart1.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart2.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart3.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            // 点列表集合
            List<Point> points = new List<Point>();
            // 让过第一行，从第二行开始处理
            if (lines.Length > 0)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point p;
                    // 获取Y（第一列）        
                    p.X = double.Parse(v[0]);
                    // 获取Y（第二列）        
                    p.Y = double.Parse(v[1]);
                    p.Z = double.Parse(v[2]);
                    points.Add(p);

                    int fre_int = (int)(p.X * 100);
                    p.X = fre_int / 100;

                    double real = p.Y * Math.Cos(p.Z * Math.PI / 180);
                    double img = p.Y * Math.Sin(-p.Z * Math.PI / 180);

                    int real_int = (int)(real * 100);
                    real = real_int / 100;

                    int img_int = (int)(img * 100);
                    img = img_int / 100;


                    this.chart1.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    this.chart2.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Z);
                    this.chart3.Series[(cnt7).ToString()].Points.AddXY(real, img);

                }
            }
        }


        //paiting of multiple measurement in FDA
        private void btn_m_Click(object sender, EventArgs e)
        {

            string FDA = foldPath + "/FDA";
            string FDA_S = FDA + "/0Single_Measurement.txt";
            string FDA_M = FDA + "/0Multiple_Measurement.txt";
            string FDA_P2 = FDA + "/Parameter_m.txt";
            string[] lines = File.ReadAllLines(FDA_M);
            chart1.Series.Add((cnt7).ToString());//添加
            chart2.Series.Add((cnt7).ToString());//添加
            chart3.Series.Add((cnt7).ToString());//添加
            this.chart1.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart2.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart3.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;

            // 点列表集合
            List<Point> points = new List<Point>();
            // 让过第一行，从第二行开始处理

            if (lines.Length > 0)
            { 
                    for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point p;
                    // 获取Y（第一列）        
                    p.X = double.Parse(v[0]);
                    // 获取Y（第二列）        
                    p.Y = double.Parse(v[1]);
                    p.Z = double.Parse(v[2]);
                    points.Add(p);

                    int fre_int = (int)(p.X * 100);
                    p.X = fre_int / 100;

                    double real = p.Y * Math.Cos(p.Z * Math.PI / 180);
                    double img = p.Y * Math.Sin(-p.Z * Math.PI / 180);

                    int real_int = (int)(real * 100);
                    real = real_int / 100;

                    int img_int = (int)(img * 100);
                    img = img_int / 100;

                    this.chart1.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    this.chart2.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Z);
                    this.chart3.Series[(cnt7).ToString()].Points.AddXY(real, img);

                }
            }
        }

        // //paiting of single measurement in TD
        private void btn_td_load_Click(object sender, EventArgs e)
        {

            btn_fre_load.Visible = true;
            btn_s.Visible = false;
            btn_m.Visible = false;

            string TD = foldPath + "/TD";
            string TD_S = TD + "/0Single_Measurement.txt";
            //string FDA_M = FDA + "/Multiple_Measurement.txt";
            string TD_p = TD + "/Parameter.txt";

            chart4.Series.Add((cnt7).ToString());//添加
            this.chart4.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            List<Point_T> points = new List<Point_T>();
            string[] lines = File.ReadAllLines(TD_S);
            if(lines.Length>0)
            { 
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point_T p;
                    // 获取Y（第一列）        
                    p.X = double.Parse(v[0]);
                    // 获取Y（第二列）        
                    p.Y = double.Parse(v[1]);
                    //p.Z = double.Parse(v[2]);
                    points.Add(p);
                    this.chart4.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    //this.chart4.ChartAreas[0].AxisX.IsLogarithmic = true;
                }
            }
        }

        //paiting in DC
        private void btn_dc_load_Click(object sender, EventArgs e)
        {
            btn_fre_load.Visible = true;
            btn_s.Visible = false;
            btn_m.Visible = false;

            string DC = foldPath + "/DC";
            string DC_U = DC + "/0U_I_R_data.txt";
            //string FDA_M = FDA + "/Multiple_Measurement.txt";
            string TD_p = TD + "/Parameter.txt";
            chart_u_i_r.Series.Add((cnt7).ToString());//添加
            this.chart_u_i_r.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            chart_u_i_r.Series.Add((cnt7 + 1).ToString());//添加
            this.chart_u_i_r.Series[(cnt7 + 1).ToString()].ChartType = SeriesChartType.Point;
            chart_u_i_r.Series.Add((cnt7 + 2).ToString());//添加
            this.chart_u_i_r.Series[(cnt7 + 2).ToString()].ChartType = SeriesChartType.Point;
            List<Point_U> points = new List<Point_U>();
            string[] lines = File.ReadAllLines(DC_U);
            if (lines.Length > 0)
            { 
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point_U p;
                    // 获取Y（第一列）        
                    p.X = double.Parse(v[0]);
                    // 获取Y（第二列）        
                    p.Y = double.Parse(v[1]);
                    p.Z = double.Parse(v[2]);
                    p.K = double.Parse(v[3]);
                    points.Add(p);
                    this.chart_u_i_r.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    this.chart_u_i_r.Series[(cnt7 + 1).ToString()].Points.AddXY(p.X, p.Z);
                    this.chart_u_i_r.Series[(cnt7 + 2).ToString()].Points.AddXY(p.X, p.K);
                    //this.chart_u_i_r.ChartAreas[0].AxisX.IsLogarithmic = true;
                }
            }
        }

        //paiting of Combination_Measurement in COMB
        private void btn_ac_load_Click(object sender, EventArgs e)
        {
            string COMB = foldPath + "/Combination";
            string Comb = COMB + "/0Combination_Measurement.txt";

            string[] lines = File.ReadAllLines(Comb);
            chart1.Series.Add((cnt7).ToString());//添加
            chart2.Series.Add((cnt7).ToString());//添加
            chart3.Series.Add((cnt7).ToString());//添加
            this.chart1.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart2.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart3.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            // 点列表集合
            List<Point> points = new List<Point>();
            // 让过第一行，从第二行开始处理
            if (lines.Length > 0)
            {

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point p;
                    // 获取Y（第一列）        
                    p.X = double.Parse(v[0]);
                    // 获取Y（第二列）        
                    p.Y = double.Parse(v[1]);
                    p.Z = double.Parse(v[2]);
                    points.Add(p);

                    int fre_int = (int)(p.X * 100);
                    p.X = fre_int / 100;

                    double real = p.Y * Math.Cos(p.Z * Math.PI / 180);
                    double img = p.Y * Math.Sin(-p.Z * Math.PI / 180);

                    int real_int = (int)(real * 100);
                    real = real_int / 100;

                    int img_int = (int)(img * 100);
                    img = img_int / 100;

                    this.chart1.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    this.chart2.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Z);
                    this.chart3.Series[(cnt7).ToString()].Points.AddXY(real, img);

                }
            }
        }



        //*********************************************//
        //****************未使用**********************//
        //*******************************************//
        private void chart5_Click(object sender, EventArgs e)
        {

        }

        private void panel_thd_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cb_times_SelectedIndexChanged(object sender, EventArgs e)
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

        private void gb_ac_Enter(object sender, EventArgs e)
        {

        }

        private void label35_Click(object sender, EventArgs e)
        {

        }

        private void label36_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click(object sender, EventArgs e)
        {

        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void panel_fst_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {

        }

        private void gb_Single_Enter(object sender, EventArgs e)
        {

        }

        private void chart_u_i_r_Click(object sender, EventArgs e)
        {

        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void gb_dc_Enter(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
           
        }       

        private void groupBox6_Enter(object sender, EventArgs e)
        {

        }

        private void tabControl2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void test_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SendArea_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void chart3_Click(object sender, EventArgs e)
        {

        }

        private void TimeDomin_Click(object sender, EventArgs e)
        {

        }

        private void tabPage5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void groupBox4_Enter(object sender, EventArgs e)
        {

        }

        private void ipaddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void gb_time_Enter(object sender, EventArgs e)
        {

        }

        private void lab_date_Click(object sender, EventArgs e)
        {

        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void tb_s_p_TextChanged(object sender, EventArgs e)
        {

        }

        private void Sweep_Points_Click(object sender, EventArgs e)
        {

        }

        private void cb_dft_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lab_dft_Click(object sender, EventArgs e)
        {

        }

        private void cb_tia_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lab_tia_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void cb_dor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lab_dor_Click(object sender, EventArgs e)
        {

        }

        private void lab_ampmV_Click(object sender, EventArgs e)
        {

        }

        private void tb_amp_TextChanged(object sender, EventArgs e)
        {

        }

        private void lab_amp_Click(object sender, EventArgs e)
        {

        }

        private void cb_freq_f_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_freq_t_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void tb_Sweep_t_TextChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void tb_Sweep_f_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_freq_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tb_freq_TextChanged(object sender, EventArgs e)
        {

        }

        private void gb_exp_Enter(object sender, EventArgs e)
        {

        }

        private void textBox1_test_TextChanged(object sender, EventArgs e)
        {

        }

        private void gb_mulitply_Enter(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void DTP_End_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DTP_Start_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label34_Click(object sender, EventArgs e)
        {

        }

        private void label37_Click_1(object sender, EventArgs e)
        {

        }

        private void label36_Click_1(object sender, EventArgs e)
        {

        }

        private void label35_Click_1(object sender, EventArgs e)
        {

        }

        private void panel_sec_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void tb_times_T2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_t_2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_f_2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tb_times_D2_TextChanged(object sender, EventArgs e)
        {

        }

        private void lab_second_Click(object sender, EventArgs e)
        {

        }

        private void panel_fst_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {

        }

        private void tb_times_T1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_t_1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_f_1_ValueChanged_1(object sender, EventArgs e)
        {

        }

        private void tb_times_D1_TextChanged(object sender, EventArgs e)
        {

        }

        private void lab_first_Click(object sender, EventArgs e)
        {

        }

        private void cb_days_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label27_Click(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void tb_d_s_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_d_m_TextChanged(object sender, EventArgs e)
        {

        }

        private void label25_Click(object sender, EventArgs e)
        {

        }

        private void tb_d_h_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox_UI_Enter(object sender, EventArgs e)
        {

        }

        private void tb_I_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_R_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_u_TextChanged(object sender, EventArgs e)
        {

        }

        private void label32_Click(object sender, EventArgs e)
        {

        }

        private void label31_Click(object sender, EventArgs e)
        {

        }

        private void gb1_Enter_1(object sender, EventArgs e)
        {

        }

        private void label39_Click(object sender, EventArgs e)
        {

        }

        private void tb_cyc_TextChanged(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void chart_v_Click(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void tb_start_TextChanged(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void rb_Square_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rb_Triangle_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tb_times_TextChanged(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void tb_d_s1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_d_m1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void tb_d_h1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {

        }

        private void tb_amp4_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_d_s2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_Sweep_f2_TextChanged(object sender, EventArgs e)
        {

        }

        private void groupBox5_Enter(object sender, EventArgs e)
        {

        }

        private void tb_amp2_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_s_p2_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_tia2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void hlepHToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void panel_load_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gb_comb_Enter(object sender, EventArgs e)
        {

        }

        private void gb_temperature_Enter(object sender, EventArgs e)
        {

        }

        private void label64_Click(object sender, EventArgs e)
        {

        }

        private void rb_temp_n_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rb_temp_y_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void gb_dc2_Enter(object sender, EventArgs e)
        {

        }

        private void label63_Click(object sender, EventArgs e)
        {

        }

        private void label62_Click(object sender, EventArgs e)
        {

        }

        private void label70_Click(object sender, EventArgs e)
        {

        }

        private void label71_Click(object sender, EventArgs e)
        {

        }

        private void gb_rep2_Enter(object sender, EventArgs e)
        {

        }

        private void label40_Click(object sender, EventArgs e)
        {

        }

        private void label50_Click(object sender, EventArgs e)
        {

        }

        private void DTP_End2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DTP_Start2_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label51_Click(object sender, EventArgs e)
        {

        }

        private void label52_Click(object sender, EventArgs e)
        {

        }

        private void label53_Click(object sender, EventArgs e)
        {

        }

        private void label54_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label55_Click(object sender, EventArgs e)
        {

        }

        private void tb_times_T4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label56_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_t_4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_f_4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tb_times_D4_TextChanged(object sender, EventArgs e)
        {

        }

        private void label57_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label58_Click(object sender, EventArgs e)
        {

        }

        private void label59_Click(object sender, EventArgs e)
        {

        }

        private void tb_times_T3_TextChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_t_3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker_f_3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void tb_times_D3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label60_Click(object sender, EventArgs e)
        {

        }

        private void cb_days2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label61_Click(object sender, EventArgs e)
        {

        }

        private void gb_ac2_Enter(object sender, EventArgs e)
        {

        }

        private void chart_temp_Click(object sender, EventArgs e)
        {

        }

        private void tb_temp_TextChanged(object sender, EventArgs e)
        {

        }

        private void label41_Click(object sender, EventArgs e)
        {

        }

        private void cb_dft2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label42_Click(object sender, EventArgs e)
        {

        }

        private void tb_Sweep_t2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label43_Click(object sender, EventArgs e)
        {

        }

        private void label44_Click(object sender, EventArgs e)
        {

        }

        private void cb_dor2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label45_Click(object sender, EventArgs e)
        {

        }

        private void label46_Click(object sender, EventArgs e)
        {

        }

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void cb_freq_f2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cb_freq_t2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label48_Click(object sender, EventArgs e)
        {

        }

        private void label49_Click(object sender, EventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
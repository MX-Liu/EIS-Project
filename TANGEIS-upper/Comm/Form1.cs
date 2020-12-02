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
using System.Runtime.InteropServices;





namespace Comm
{



    public partial class Form1 : Form
    {

        //*********************************************//
        //**************数据定义**********************//
        //*******************************************//

        // 界面标志位
        bool FDA = false;
        bool TD = false;
        bool ACM = false;
        bool DCM = false;

        //串口数据定义
        static bool PortIsOpen = false;
        private static byte[] result = new byte[1024];

        //图形数据定义
        private int time = 0;//TD 计数器
        private float vol = 0;//DC 电压值
        private float cur = 0;//DC 电流值
        private float fre = 1;//FDA 频率
        private float mag = 0;//FDA 幅值
        private float pha = 0;//FDA 相位
        private float X = 0;  //Form 大小的x值
        private float Y = 0;  //Form 大小的y值

        private Int32 Num_FDA1 = 0; //Num_FDA1和Num_FDA2 用于FDA画多曲线的比较计数器
        private Int32 Num_FDA2 = 0;
        private Int32 Num_COMB1 = 0;//Num_COMB1和Num_COMB2 用于COMB画多曲线的比较计数器
        private Int32 Num_COMB2 = 0;
        private Int64 cnt = 0; //TD计数器
        private Int64 cnt1 = 0;//FDA 幅度图存
        private Int64 cnt2 = 0;//FDA 相位图存
        private Int64 cnt3 = 0;//FDA Nqst图存
        private Int64 cnt4 = 0;//TD 图存
        private Int64 cnt5 = 0;//DC图存
        private Int64 cnt6 = 0;//FDA画多曲线计数器
        private Int64 cnt7 = 0;//Data Analyzer 画多曲线计数器
        private Int64 cnt8 = 0;//Comb幅度图存
        private Int64 cnt9 = 0;//Comb相位图存
        private Int64 cnt10 = 0;//Comb Nqst图存
        private Int64 cnt11 = 0;//Comb画多曲线计数器
        private Int64 cnt_hands_shake = 0;//串口握手信号计数器

        private bool Hands_shake = false; //握手标志位
        private bool Hands_shake1 = false; //握手标志位

        private Int64 cnt_comb = 0;//comb 文件命名计数器
        private Int64 cnt_FDA1 = 0;//FDA 单次测量命名计数器
        private Int64 cnt_FDA2 = 0;////FDA 多次测量命名计数器
        private Int64 cnt_TD = 0;//TD 文件命名计数器
        private Int64 cnt_UIR = 0;//UIR 文件命名计数器

        private Queue chart_x = new Queue();//  数据队列 用于无线传输
        private Queue chart_y = new Queue();
        private Queue data_queue = new Queue();



        string parameter1 = "";//TD 参数路径
        string parameter21 = "";//FDA 单次测量 参数路径
        string parameter22 = "";//FDA 多次测量 参数路径
        string parameter3 = "";//DC 参数路径
        string parameter4 = "";//Comb 参数路径
        string pathString1 = "";//FDA 路径
        string pathString2 = "";//TD 路径
        string pathString3 = "";//CD 路径
        string pathString4 = "";//COMB 路径
        string Single_m = "";//FDA 单次 数据存储 路径
        string Single_m1 = "";//TD 数据存储 路径
        string Multiple_m = "";//FDA 多次 数据存储 路径
        string U_I_R = "";//DC 数据存储 路径
        string foldPath = "";//工程路径
        string ID_Num = "";//工程ID
        string Combination_m = "";//COMB 数据存储 路径
        string Temperature = "℃";//温度单位
        string dft = "";
        string dft2 = "";
        string tia = "";
        string tia2 = "";

        Int32 tempt = 37;//温度初值




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
            BaudrateChoose.Text = "1000000";
            tb_Sweep_f.Text = "10";
            tb_Sweep_t.Text = "100";
            tb_times_D1.Text = "0";
            tb_times_D2.Text = "0";
            tb_times_T1.Text = "0";
            tb_times_T2.Text = "0";
            cb_days.Text = "0";

            //
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
            btn_freq.Enabled = true;
            btn_TD.Enabled = true;
            btn_DC.Enabled = true;
            btn_AC.Enabled = true;


            ipaddress.Text = "http://192.168.191.1";
            //ipaddress.Text ="http://" + IP_get();
            Port.Text = "8080";

            string[] PortNames = SerialPort.GetPortNames();    //获取本机串口名称，存入PortNames数组中
            BackToolStripMenuItem.Enabled = true;
            for (int i = 0; i < PortNames.Count(); i++)
            {
                COMChoose.Items.Add(PortNames[i]);   //将数组内容加载到comboBox控件中

            }

            cb_dft.Items.Clear();
            cb_dft.Items.Add("4");
            cb_dft.Items.Add("8");
            cb_dft.Items.Add("16");
            cb_dft.Items.Add("32");
            cb_dft.Items.Add("64");
            cb_dft.Items.Add("128");
            cb_dft.Items.Add("256");
            cb_dft.Items.Add("512");
            cb_dft.Items.Add("1024");
            cb_dft.Items.Add("2048");
            cb_dft.Items.Add("4096");
            cb_dft.Items.Add("8192");
            cb_dft.Items.Add("16384");

            cb_tia.Items.Clear();
            cb_tia.Items.Add("200");
            cb_tia.Items.Add("1K");
            cb_tia.Items.Add("5K");
            cb_tia.Items.Add("10K");
            cb_tia.Items.Add("20K");
            cb_tia.Items.Add("40K");
            cb_tia.Items.Add("80K");
            cb_tia.Items.Add("160K");
            cb_tia.Items.Add("OPEN");

            cb_dor.Items.Clear();
            cb_dor.Items.Add("0");
            cb_dor.Items.Add("1");
            cb_dor.Items.Add("2");
            cb_dor.Items.Add("3");
            cb_dor.Items.Add("4");
            cb_dor.Items.Add("5");
            cb_dor.Items.Add("6");
            cb_dor.Items.Add("7");
            cb_dor.Items.Add("8");
            cb_dor.Items.Add("9");
            cb_dor.Items.Add("10");


            tb_freq.Text = "1000";
            tb_Sweep_f.Text = "100";
            tb_Sweep_t.Text = "100000";
            tb_amp.Text = "607";
            cb_dor.Text = "4";
            cb_tia.Text = "5K";
            cb_dft.Text = "16384";
            tb_s_p.Text = "101";
            
            cb_dor2.Text = "4";
            cb_tia2.Text = "5K";
            cb_dft2.Text = "16384";
            //添加串口接收时间
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(Port_DataRecevied);

            //chart 初始化
            Init_Chart();

            //分别给chart添加鼠标滚轮缩放时间
            chart1.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            chart2.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            chart3.MouseWheel += new MouseEventHandler(chart_MouseWheelXY);
            chart4.MouseWheel += new MouseEventHandler(chart_MouseWheel);
            chart_u_i_r.MouseWheel += new MouseEventHandler(chart_MouseWheel);

            //分别给chart添加悬停读数
            chart1.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);
            chart2.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);
            chart3.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);
            chart4.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);
            chart_u_i_r.GetToolTipText += new EventHandler<ToolTipEventArgs>(chart_GetToolTipText);

            //窗体调整大小时引发事件  用于按比例修改Form内组件大小
            this.Resize += new EventHandler(modular_calEchoPhaseFromSignal1_Resize);

            X = this.Width;//获取窗体的宽度

            Y = this.Height;//获取窗体的高度

            setTag(this);//调用方法

            //添加窗口关闭事件
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);

        }




        //*********************************************//
        //**************自编函数**********************//
        //*******************************************//

        //鼠标悬停读数设置
        void chart_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                //分别显示x轴和y轴的数值，其中{1:F3},表示显示的是float类型，精确到小数点后3位。   
                e.Text = string.Format("{0:F3},{1:F3} ", dp.XValue, dp.YValues[0]);
            }

        }


        //控制控件随窗体大小变化而变化
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

            setControls(newx * X, newy * Y, btn_comb_start);//随窗体改变控件大小

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

        //时间转秒
        private string TimeToSec(string tmp1)
        {
            tmp1 = tmp1.Replace(":", "");

            int tmp2 = Convert.ToInt32(tmp1);
            int hh = tmp2 / 10000;
            int tmp3 = tmp2 % 10000;
            int mm = tmp3 / 100;
            int ss = tmp3 % 100;
            int sum = hh * 3600 + mm * 60 + ss;
            string Zeit = sum.ToString();
            return Zeit;
        }

        //计算时间差
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
            //test.AppendText(Convert.ToString(c[0]));
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
            //test.AppendText(hexString);
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;

        }

        //方波发生器
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

        //方波发生器
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

        //按钮清空
        private void ClearReceive_Click(object sender, EventArgs e)
        {
            ReceiveArea.Clear();
            this.chart1.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart2.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart4.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart_u_i_r.ChartAreas[0].AxisX.IsLogarithmic = false;
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart2.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart3.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart4.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart_u_i_r.Series)
            {
                series.Points.Clear();
            }
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
            chart_u_i_r.Series.Clear();
            Init_Chart();
            // chart5.Series[0].Points.Clear();

        }

        //函数清空
        private void ChartClear()
        {
            ReceiveArea.Clear();
            this.chart1.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart2.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart4.ChartAreas[0].AxisX.IsLogarithmic = false;
            this.chart_u_i_r.ChartAreas[0].AxisX.IsLogarithmic = false;
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart1.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart2.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart3.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart4.Series)
            {
                series.Points.Clear();
            }
            foreach (var series in chart_u_i_r.Series)
            {
                series.Points.Clear();
            }
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart3.Series.Clear();
            chart4.Series.Clear();
            chart_u_i_r.Series.Clear();
            // chart5.Series[0].Points.Clear();
            Init_Chart();

        }

        //发送位清空
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
                    btn_freq.Enabled = false;
                    btn_TD.Enabled = false;
                    btn_DC.Enabled = false;
                    btn_AC.Enabled = false;
                }
            }
            else
            {
                Hands_shake = false;
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
            for (int i = 0; i < 5; i++)
            {
                Zeros[i] = 0x00;
            }
            if (serialPort1.IsOpen)

            {
                //
                byte[] temp2 = strToHexByte(ID_Num);
                byte[] temp = exchange(temp2);
                for (int i = 0; i < temp.Length; i++)
                {
                    ID_N[3 - i] = temp[i];
                }

                //重复发送5次   延时10ms

                for(int h = 0; h < 3; h++)
                {
                    if (Hands_shake == true)
                    {
                        break;
                    }
                    else
                    {
                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x10, 0x00 }, 0, 5);//帧头
                        serialPort1.Write(ID_N, 0, 4);
                        serialPort1.Write(Zeros, 0, 5);
                        serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
                        Thread.Sleep(100);
                    }
                }
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

                    if (Hands_shake == true)//握手成功
                    {
                        
                        if (TD == true)//TD 数据接收
                        {

                            string strv = serialPort1.ReadLine();
                            ReceiveArea.AppendText(strv);
                            string[] strArry = strv.Split(',');
                            int length_v = strArry.Length;
                            time++;
                            string length2 = Convert.ToString(length_v);

                            //DC 画图
                            if (length_v == 1 && strArry[0] != "200000.00")
                            {

                                cnt++;
                                //TD  画图
                                this.chart4.Series[0].Points.AddXY(cnt, strv);
                                this.chart4.ChartAreas[0].AxisX.IsLogarithmic = true;

                                //添加到无线传输队列
                                data_queue.Enqueue(cnt + "," + strv + ":");

                                //写入文件
                                FileStream fs = new FileStream(pathString2 + "\\" + cnt_TD + "Single_Measurement1.txt", FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.WriteLine(cnt + "\t" + strv + "\t");
                                sw.Flush();
                                sw.Close();
                                fs.Close();
                            }


                        }
                        else if (DCM == true) // DC 数据接收
                        {

                            string stru = serialPort1.ReadLine();
                            ReceiveArea.AppendText(stru);
                            string[] strArry = stru.Split(',');
                            int length_u = strArry.Length;
                            time++;
                            string length1 = Convert.ToString(length_u);

                            //DC 画图
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

                                    //DC  添加到无线队列
                                    data_queue.Enqueue(time + "," + stru + ":");

                                    //DC 写入文件
                                    FileStream fs = new FileStream(pathString3 +"\\" +cnt_UIR+"U_I_R_data.txt", FileMode.Append);
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

                        //FDA 数据接收
                        else if (FDA == true)
                        {

                            string str = serialPort1.ReadLine();
                            ReceiveArea.AppendText(str);

                            string[] strArr = str.Split(',');
                            int length = strArr.Length;
                            //ReceiveArea.AppendText(length.ToString());

                            //FDA 画图
                            if (length == 4 && strArr[0] != "200000.00")
                            {
                                try
                                {
                                    fre = Convert.ToSingle(strArr[0]);
                                    mag = Convert.ToSingle(strArr[1]);
                                    pha = Convert.ToSingle(strArr[2]);

                                    //比较上一个数的序号
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

                                    //FDA 添加到无线队列
                                    data_queue.Enqueue(str + ":");

                                    if (rb_dur.Checked)//单次测量文件写入
                                    {
                                        FileStream fs = new FileStream(pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt", FileMode.Append);
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.WriteLine(strArr[0] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t");
                                        sw.Flush();
                                        sw.Close();
                                        fs.Close();
                                    }
                                    else if (rb_rep.Checked)//多次测量文件写入
                                    {
                                        FileStream fs = new FileStream(pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt", FileMode.Append);
                                        StreamWriter sw = new StreamWriter(fs);
                                        sw.WriteLine(strArr[0] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t" + Num_FDA1.ToString());
                                        sw.Flush();
                                        sw.Close();
                                        fs.Close();
                                    }
                            }
                                catch
                                {
                                    cnt6++;
                                    //MessageBox.Show("图表未工作");
                                    Console.WriteLine("error");

                                }
                            }
                        }

                        //Comb 数据接收
                        else if (ACM == true)
                        {
                            string strc = serialPort1.ReadLine();
                            ReceiveArea.AppendText(strc);

                            string[] strArr = strc.Split(',');
                            int length = strArr.Length;


                            //Comb 画图
                            if (length == 4 && strArr[0] != "200000.00")
                            {
                                try
                                {
                                    fre = Convert.ToSingle(strArr[0]);
                                    mag = Convert.ToSingle(strArr[1]);
                                    pha = Convert.ToSingle(strArr[2]);

                                    //Comb 数据与前一个序号作比较
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

                                    //Comb 添加到无线队列
                                    data_queue.Enqueue(strc + ":");

                                    //Comb 写入文件
                                    FileStream fs = new FileStream(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt", FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[0] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t" + cnt11.ToString());
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();

                                }
                                catch
                                {
                                    //MessageBox.Show("图表未工作");
                                    Console.WriteLine("error");

                                }
                            }
                        }
                        else
                        {
                            //其他数据
                            //string strvv = serialPort1.ReadExisting();
                            try
                            {
                                string strvv = serialPort1.ReadLine();
                                string[] strArr = strvv.Split(',');
                                int length = strArr.Length;


                                data_queue.Enqueue(strvv + ':');
                                ReceiveArea.AppendText(strvv);
                            }
                            catch
                            {
                                Hands_shake = false;
                                //MessageBox.Show("False COM!");
                                serialPort1.Close();
                                PortIsOpen = false;
                                OpenPortButton.Text = "Port Open";
                            }
                        }

                    }
                    else
                    {
                        //握手不成功  按钮无法使用
                        btn_freq.Enabled = false;
                        btn_TD.Enabled = false;
                        btn_DC.Enabled = false;
                        btn_AC.Enabled = false;

                        string strh = serialPort1.ReadLine();
                        ReceiveArea.AppendText(strh);

                        string[] strArr = strh.Split(',');
                        int length = strArr.Length;
                        if (length == 2 && strArr[0] != "200000.00")
                        {
                            //Int16 status = Convert.ToInt16(strArr[0]);
                            //Int16 endbit = Convert.ToInt16(strArr[1]);

                            //收到0，4时，正常工作
                            if ((strArr[0].Equals("0")) && (strArr[1].Equals("4")))
                            {

                                cnt_hands_shake++;                                
                            }
                            // 收到1/2，4时，正常工作
                            else if (((strArr[0].Equals("1")) || (strArr[0].Equals("2")) && (strArr[1].Equals("4"))))
                            {
                                MessageBox.Show("MCU is busy!");
                                serialPort1.Close();
                                PortIsOpen = false;
                                Hands_shake = false;
                            }
                            // 其他为错误串口
                            else
                            {
                                Hands_shake = false;
                                MessageBox.Show("False COM!");
                                serialPort1.Close();
                                PortIsOpen = false;
                                OpenPortButton.Text = "Port Open";
                            }
                            if (cnt_hands_shake>0)
                            {
                                Hands_shake = true;
                                MessageBox.Show("MCU is already!");
                                //显示功能按钮
                                btn_freq.Enabled = true;
                                btn_TD.Enabled = true;
                                btn_DC.Enabled = true;
                                btn_AC.Enabled = true;
                                cnt_hands_shake = 0;
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


        //鼠标滚轮改变图像大小，单X轴
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

        //鼠标滚轮改变图像大小，XY轴
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
            //定义图像轴的初始值
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart2.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart3.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart3.ChartAreas[0].AxisY.ScaleView.Size = 3000;
            chart4.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            chart1.Series.Add((0).ToString());//添加
            chart2.Series.Add((0).ToString());//添加
            chart3.Series.Add((0).ToString());//添加
            this.chart1.Series[(0).ToString()].ChartType = SeriesChartType.Point;
            this.chart2.Series[(0).ToString()].ChartType = SeriesChartType.Point;
            this.chart3.Series[(0).ToString()].ChartType = SeriesChartType.Point;
            chart_u_i_r.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            cnt6 = 0;//FDA线条数目初始化
            cnt11 = 0;//Comb线条数目初始化
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
            //socketIoManager(1, ID_Num + ":");
            Hands_shake1 = true;
       
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

        public static string IP_get()
        {
            {
                try
                {
                    string HostName = Dns.GetHostName(); //得到主机名
                    IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                    for (int i = 0; i < IpEntry.AddressList.Length; i++)
                    {
                        //从IP地址列表中筛选出IPv4类型的IP地址
                        //AddressFamily.InterNetwork表示此IP为IPv4,
                        //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                        if (IpEntry.AddressList[i].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return IpEntry.AddressList[i].ToString();
                        }
                    }
                    return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取本机IP出错:" + ex.Message);
                    return "";
                }
            }

        }

    //wifi定时器
    private void timer2_Tick(object sender, EventArgs e)
        {
            if (data_queue.Count != 0)
            {
                string data;
                data = (string)data_queue.Dequeue();
                try
                {
                    for (int i = 0; i < 7; i++)
                {
                        if (Hands_shake1 == true)
                        {
                            data += ID_Num + ":";
                            //Hands_shake1 = false;
                        }
                        else
                        {
                            data += (string)data_queue.Dequeue();
                        }
                    }
               
                    socketIoManager(1, data);//发送 数据队列
                }
                catch
                {

                }
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
            //标志位初始化
            FDA = true;
            TD = false;
            ACM = false;
            DCM = false;

            //界面初始化
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
            //标志位初始化
            FDA = false;
            TD = false;
            ACM = false;
            DCM = false;

            //界面初始化
            panel_switch.Height = btn_cfg.Height;
            panel_switch.Top = btn_cfg.Top;
            enableButtons(true, true, true, true, false, false, false, true, false, false);

        }

        // 时域
        private void btn_TD_Click_1(object sender, EventArgs e)
        {
            //标志位初始化
            FDA = false;
            TD = true;
            ACM = false;
            DCM = false;

            //界面初始化
            rb_dur.Enabled = true;
            rb_rep.Enabled = false;
            panel_switch.Height = btn_TD.Height;
            panel_switch.Top = btn_TD.Top;
            gb_ac.Text = "AC Control";
            enableButtons(false, true, false, false, true, false, true, false, false, false);
            rb_Frequncy.Checked = true;
            rb_Sweep.Checked = false;

        }

        // 交流+直流
        private void btn_AC_Click(object sender, EventArgs e)
        {
            //标志位初始化   
            FDA = false;
            TD = false;
            ACM = true;
            DCM = false;

            //界面初始化
            panel_switch.Height = btn_AC.Height;
            panel_switch.Top = btn_AC.Top;
            gb_ac.Text = "AC Control";
            enableButtons(false, true, false, false, false, false, false, false, false, true);
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

        }

        // 直流
        private void btn_DC_Click(object sender, EventArgs e)
        {
            //标志位初始化
            FDA = false;
            TD = false;
            DCM = true;
            ACM = false;

            //界面初始化
            panel_switch.Height = btn_DC.Height;
            panel_switch.Top = btn_DC.Top;
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

            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    if (rb_Frequncy.Checked)//TD选择

                    {
                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x05 }, 0, 5);//TD帧头
                    }
                    else if (rb_Sweep.Checked)//FDA选择
                    {
                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x01 }, 0, 5);//FDA帧头
                    }

                    //duration 时间初始化
                    int temp_h = 0;
                    int temp_m = 0;
                    int temp_s = 0;
                    int sum = 0;

                    //flag 定义
                    byte[] flag = new byte[2];
                    flag[0] = 00;
                    flag[1] = 01;
                    byte[] flag1 = new byte[2];
                    flag1[0] = 0x00;
                    flag1[1] = 6;

                    //各类较长的数组初始化
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



                        if (cb_dft.Text.Equals("4"))
                        {
                            dft = "0";
                        }
                        if (cb_dft.Text.Equals("8"))
                        {
                            dft = "1";
                        }
                        if (cb_dft.Text.Equals("16"))
                        {
                            dft = "2";
                        }
                        if (cb_dft.Text.Equals("32"))
                        {
                            dft = "3";
                        }
                        if (cb_dft.Text.Equals("64"))
                        {
                            dft = "4";
                        }
                        if (cb_dft.Text.Equals("128"))
                        {
                            dft = "5";
                        }
                        if (cb_dft.Text.Equals("256"))
                        {
                            dft = "6";
                        }
                        if (cb_dft.Text.Equals("512"))
                        {
                            dft = "7";
                        }
                        if (cb_dft.Text.Equals("1024"))
                        {
                            dft = "8";
                        }
                        if (cb_dft.Text.Equals("2048"))
                        {
                            dft = "9";
                        }
                        if (cb_dft.Text.Equals("4096"))
                        {
                            dft = "10";
                        }
                        if (cb_dft.Text.Equals("8192"))
                        {
                            dft = "11";
                        }
                        if (cb_dft.Text.Equals("16384"))
                        {
                            dft = "12";
                        }


                        serialPort1.Write(strToHexByte(dft), 0, 1);//dft

                        //TD
                        if (rb_Frequncy.Checked)
                        {
                            serialPort1.Write(flag, 0, 1);//
                            tb_Sweep_f.Enabled = false;
                            tb_Sweep_t.Enabled = false;
                            cb_freq_f.Enabled = false;
                            cb_freq_t.Enabled = false;
                            btn_freq.Enabled = false;
                            //btn_TD.Enabled = false;
                            btn_DC.Enabled = false;
                            btn_AC.Enabled = false;

                            //固定频率
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
                            //btn_freq.Enabled = false;
                            btn_TD.Enabled = false;
                            btn_DC.Enabled = false;
                            btn_AC.Enabled = false;

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

                        serialPort1.Write(Fre, 0, 12);//频率发送

                        serialPort1.Write(flag1, 0, 1);//log  暂时没用


                        //string tia = "";

                        if (cb_tia.Text.Equals("200"))
                        {
                            tia = "0";
                        }
                        if (cb_tia.Text.Equals("1K"))
                        {
                            tia = "1";
                        }
                        if (cb_tia.Text.Equals("5K"))
                        {
                            tia = "2";
                        }
                        if (cb_tia.Text.Equals("10K"))
                        {
                            tia = "3";
                        }
                        if (cb_tia.Text.Equals("20K"))
                        {
                            tia = "4";
                        }
                        if (cb_tia.Text.Equals("40K"))
                        {
                            tia = "5";
                        }
                        if (cb_tia.Text.Equals("80K"))
                        {
                            tia = "6";
                        }
                        if (cb_tia.Text.Equals("160K"))
                        {
                            tia = "7";
                        }
                        if (cb_tia.Text.Equals("OPEN"))
                        {
                            tia = "8";
                        }

                        serialPort1.Write(strToHexByte(tia), 0, 1);//rtia
                        //serialPort1.Write(strToHexByte(cb_tia.Text.Trim()), 0, 1);//rtia

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

                            cb_days.Text = ts;//天数显示

                            serialPort1.Write(strToHexByte(cb_days.Text), 0, 1);//天数 发送

                            //开始时间

                            string start_time1 = dateTimePicker_f_1.Text;//获取开始时间
                            string start_send1 = TimeDifference(start_time1);//计算与当前时间差

                            byte[] temp_s1 = strToHexByte(start_send1);
                            byte[] temp_se1 = exchange(temp_s1);
                            for (int i = 0; i < temp_se1.Length; i++)
                            {
                                Start_send1[3 - i] = temp_se1[i];
                            }

                            serialPort1.Write(Start_send1, 0, 4);//开始时间1发送

                            string end_time1 = dateTimePicker_t_1.Text;//获取结束时间
                            string end_send1 = TimeDifference(end_time1);//计算与当前时间差

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
                                string start_time2 = dateTimePicker_f_2.Text;//获取第二次开始时间
                                string start_send2 = TimeDifference(start_time2);//计算与当前时间差
                                byte[] temp_s2 = strToHexByte(start_send2);
                                byte[] temp_se2 = exchange(temp_s2);
                                for (int i = 0; i < temp_se2.Length; i++)
                                {
                                    Start_send2[3 - i] = temp_se2[i];
                                }

                                serialPort1.Write(Start_send2, 0, 4);//开始时间2发送

                                string end_time2 = dateTimePicker_t_2.Text;//获取第二次结束时间
                                string end_send2 = TimeDifference(end_time2);//计算与当前时间差

                                byte[] temp_e2 = strToHexByte(end_send2);
                                byte[] temp_ee2 = exchange(temp_e2);
                                for (int i = 0; i < temp_ee1.Length; i++)
                                {
                                    End_send2[3 - i] = temp_ee2[i];
                                }

                                serialPort1.Write(End_send2, 0, 4);//结束时间2发送

                                serialPort1.Write(strToHexByte(tb_times_D2.Text.Trim()), 0, 1);//次数2/天发送
                                serialPort1.Write(strToHexByte(tb_times_T2.Text.Trim()), 0, 1);//次数2/次发送
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

                            temp_h = Convert.ToInt16(tb_d_h.Text);//获取时
                            temp_m = Convert.ToInt16(tb_d_m.Text);//获取分
                            temp_s = Convert.ToInt16(tb_d_s.Text);//获取秒
                            sum = 3600 * temp_h + 60 * temp_m + temp_s;//总时间

                            byte[] temp2 = strToHexByte(Convert.ToString(sum));
                            byte[] temp = exchange(temp2);
                            for (int i = 0; i < temp.Length; i++)
                            {
                                Sum[3 - i] = temp[i];
                            }

                            serialPort1.Write(Sum, 0, 4);//发送总时间
                        }

                        serialPort1.Write(Zeros, 0, 12);//发送空闲位，0

                        serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾

                        if (rb_Sweep.Checked)
                        {
                            if (rb_dur.Checked)//单次测量参数写入
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
                            else //多次测量参数写入
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
                        else if (rb_Frequncy.Checked)//TD参数写入
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
                        btn_start.Enabled = true;
                        btn_stop.Enabled = false;

                    }
                }
                else
                {
                    btn_start.Enabled = true;
                    btn_stop.Enabled = false;
                    MessageBox.Show("Please open the serialport!");
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show("Please open the serialport!");
                serialPort1.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                btn_start.Enabled = true;
                btn_stop.Enabled = false;
               

            }
        }


        //FDA+TD stop
        private void btn_stop_Click(object sender, EventArgs e)
        {
            //界面初始化
            btn_freq.Enabled = true;
            btn_TD.Enabled = true;
            btn_DC.Enabled = true;
            btn_AC.Enabled = true ;
            BackToolStripMenuItem.Enabled = true;

            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x07, 0x09 }, 0, 5);//停止帧头
            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾

            btn_start.Enabled = true;
            btn_stop.Enabled = false;


            if (rb_Frequncy.Checked)//TD 停止时间写入
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
                if (rb_dur.Checked)//FDA 单次停止时间写入
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
                else//FDA 多次停止时间写入
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

        //TD操作框
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

        //天数选择判断
        private void DTP_End_ValueChanged(object sender, EventArgs e)
        {
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
            else
            {
                MessageBox.Show("The Days should between 1 and 7.");
            }
            string ts = Convert.ToString(ts2);
            //textBox1_test.Text = ts;
            cb_days.Text = ts;
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

        // 时间输入判断
        private void dateTimePicker_f_1_ValueChanged(object sender, EventArgs e)
        {
            String tmp1 = dateTimePicker_f_1.Text;
            string tmp3 = TimeDifference(tmp1);
        }

        //二次测量界面
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
            //部分按钮使能不可用
            btn_start1.Enabled = false;
            btn_stop1.Enabled = true;
            btn_freq.Enabled = false;
            btn_TD.Enabled = false;
            btn_AC.Enabled = false;


            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x20, 0x03 }, 0, 5);//帧头

                    //DC 持续时间输入
                    int temp_h1 = 0;
                    int temp_m1 = 0;
                    int temp_s1 = 0;
                    int sum1 = 0;

                    //数组初始化
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

                            serialPort1.Write(Sum1, 0, 4);//时间空值发送
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

                            serialPort1.Write(Sum1, 0, 4);//总时间发送
                            serialPort1.Write(strToHexByte(tb_times.Text.Trim()), 0, 1);//Times
                        }

                        serialPort1.Write(Zeros, 0, 13);//补零
                        serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
                    }

                    catch
                    {
                        MessageBox.Show("Please fill in the parameters completely!");
                        btn_start1.Enabled = true;
                        btn_stop1.Enabled = false;
                    }
                }
                else
                {
                    MessageBox.Show("Please open the serialport!");
                    btn_start1.Enabled = true;
                    btn_stop1.Enabled = false;
                }

                //DC 参数写入
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

                MessageBox.Show("Please open the serialport!");
                btn_start1.Enabled = true;
                btn_stop1.Enabled = false;
            }
        }

        //DC Stop
        private void btn_stop1_Click(object sender, EventArgs e)
        {
            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x07, 0x08 }, 0, 5);//DC 停止帧头
            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
            btn_start1.Enabled = true;
            btn_stop1.Enabled = false;
            BackToolStripMenuItem.Enabled = true;

            btn_freq.Enabled = true;
            btn_TD.Enabled = true;
            btn_DC.Enabled = true;
            btn_AC.Enabled = true;
            
            //DC 停止时间写入
            FileStream fs = new FileStream(parameter3, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
            sw.WriteLine("\n");
            sw.Flush();
            sw.Close();

            cnt_UIR++;

            //添加新的数据文件
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

            btn_freq.Enabled = false;
            btn_TD.Enabled = false;
            btn_DC.Enabled = false;
            //btn_AC.Enabled = false;

            btn_comb_start.Enabled = false;
            btn_stop2.Enabled = true;

            //温度显示判断
            if (Temperature.Equals("℃"))
                Temperature = tempt.ToString() + "℃";
            else
            {
                Temperature = "37℃";
            }


            //弹出框确认实验是否开始
            DialogResult dr = MessageBox.Show("Everything already?", "Combination Measurement", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                
                try
                {
                    //首先判断串口是否开启
                    if (serialPort1.IsOpen)
                    {
                        tb_temp.Text = Temperature;//显示温度
                        for (int CNT_T = 0; CNT_T < 10; CNT_T++)
                        {
                            this.chart_temp.Series[0].Points.AddXY(CNT_T, tempt);
                        }

                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x04 }, 0, 5);//Comb 帧头

                        //长数组定义
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

                        //AC 部分
                        try
                        {

                           
                            //dor2 = Convert.ToDouble();
                            //dor2 = Math.Log(dor2) / Math.Log(4);
                            serialPort1.Write(strToHexByte(cb_dor2.Text), 0, 1);//odr

                            byte[] tmp2 = strToHexByte(tb_amp2.Text);
                            byte[] tmp = exchange(tmp2);
                            for (int i = 0; i < tmp.Length; i++)
                            {
                                amp2[1 - i] = tmp[i];
                            }

                            serialPort1.Write(amp2, 0, 2);//amp


                            string dft2 = "";

                            if (cb_dft2.Text.Equals("4"))
                            {
                                dft2 = "0";
                            }
                            if (cb_dft2.Text.Equals("8"))
                            {
                                dft2 = "1";
                            }
                            if (cb_dft2.Text.Equals("16"))
                            {
                                dft2 = "2";
                            }
                            if (cb_dft2.Text.Equals("32"))
                            {
                                dft2 = "3";
                            }
                            if (cb_dft2.Text.Equals("64"))
                            {
                                dft2 = "4";
                            }
                            if (cb_dft2.Text.Equals("128"))
                            {
                                dft2 = "5";
                            }
                            if (cb_dft2.Text.Equals("256"))
                            {
                                dft2 = "6";
                            }
                            if (cb_dft2.Text.Equals("512"))
                            {
                                dft2 = "7";
                            }
                            if (cb_dft2.Text.Equals("1024"))
                            {
                                dft2 = "8";
                            }
                            if (cb_dft2.Text.Equals("2048"))
                            {
                                dft2 = "9";
                            }
                            if (cb_dft2.Text.Equals("4096"))
                            {
                                dft2 = "10";
                            }
                            if (cb_dft2.Text.Equals("8192"))
                            {
                                dft2 = "11";
                            }
                            if (cb_dft2.Text.Equals("16384"))
                            {
                                dft2 = "12";
                            }


                            serialPort1.Write(strToHexByte(dft2), 0, 1);//dft




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


                            serialPort1.Write(Fre2, 0, 8);//频率发送

                            serialPort1.Write(flag, 0, 1);//log  暂时没用

                            //string tia2 = "";

                            if (cb_tia2.Text.Equals("200"))
                            {
                                tia2 = "0";
                            }
                            if (cb_tia2.Text.Equals("1K"))
                            {
                                tia2 = "1";
                            }
                            if (cb_tia2.Text.Equals("5K"))
                            {
                                tia2 = "2";
                            }
                            if (cb_tia2.Text.Equals("10K"))
                            {
                                tia2 = "3";
                            }
                            if (cb_tia2.Text.Equals("20K"))
                            {
                                tia2 = "4";
                            }
                            if (cb_tia2.Text.Equals("40K"))
                            {
                                tia2 = "5";
                            }
                            if (cb_tia2.Text.Equals("80K"))
                            {
                                tia2 = "6";
                            }
                            if (cb_tia2.Text.Equals("160K"))
                            {
                                tia2 = "7";
                            }
                            if (cb_tia2.Text.Equals("OPEN"))
                            {
                                tia2 = "8";
                            }

                            serialPort1.Write(strToHexByte(tia2), 0, 1);//rtia

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
                            cb_days2.Text = ts;//显示天数

                            serialPort1.Write(strToHexByte(cb_days2.Text), 0, 1);//发送天书

                            //开始时间

                            string start_time1 = dateTimePicker_f_3.Text;//获取时间
                            string start_send1 = TimeDifference(start_time1);//计算开始时间差

                            byte[] temp_s1 = strToHexByte(start_send1);
                            byte[] temp_se1 = exchange(temp_s1);
                            for (int i = 0; i < temp_se1.Length; i++)
                            {
                                Start_send12[3 - i] = temp_se1[i];
                            }

                            serialPort1.Write(Start_send12, 0, 4);//开始时间1发送

                            string end_time1 = dateTimePicker_t_3.Text;//获取第一次截止时间
                            string end_send1 = TimeDifference(end_time1);//计算结束时间差

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
                                string start_time2 = dateTimePicker_f_4.Text;//获取第二次开始时间
                                string start_send2 = TimeDifference(start_time2);//计算第二次开始时间差
                                byte[] temp_s2 = strToHexByte(start_send2);
                                byte[] temp_se2 = exchange(temp_s2);
                                for (int i = 0; i < temp_se2.Length; i++)
                                {
                                    Start_send22[3 - i] = temp_se2[i];
                                }

                                serialPort1.Write(Start_send22, 0, 4);//开始时间2发送

                                string end_time2 = dateTimePicker_t_4.Text;//获取第二次结束时间
                                string end_send2 = TimeDifference(end_time2);//计算第二次结束时间差

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

                    else
                    {
                        //MessageBox.Show("Please open the serialport!");
                        //btn_comb_start.Enabled = true;
                        //btn_stop2.Enabled = false;
                    }
                }


                catch (Exception ex)
                {
                    serialPort1.Close();
                    //捕获到异常，创建一个新的对象，之前的不可以再用
                    serialPort1 = new System.IO.Ports.SerialPort();
                
                    MessageBox.Show("Plesase open the serialport!");

                }

                try
                {
                    //首先判断串口是否开启
                    if (serialPort1.IsOpen)
                    {

                        //数组定义
                        byte[] flag = new byte[2];
                        flag[0] = 0x00;
                        flag[1] = 0x01;


                        byte[] Zeros = new byte[20];
                        for (int i = 0; i < 20; i++)
                        {
                            Zeros[i] = 0x00;
                        }

                        byte[] amp4 = new byte[] { 0x00, 0x00 };

                        //DC部分
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

                            serialPort1.Write(Zeros, 0, 18);//补零
                            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
                        }

                        catch
                        {
                            MessageBox.Show("Please fill in the parameters completely!");
                            btn_comb_start.Enabled = true;
                            btn_stop2.Enabled = false;
                        }


                    }
                    else
                    {
                        MessageBox.Show("Please open the serialport!");
                        btn_comb_start.Enabled = true;
                        btn_stop2.Enabled = false;
                    }

                }
                catch (Exception ex)
                {
                    serialPort1.Close();
                    //捕获到异常，创建一个新的对象，之前的不可以再用
                    serialPort1 = new System.IO.Ports.SerialPort();

                    MessageBox.Show("Plesase open the serialport!");
                    btn_comb_start.Enabled = true;
                    btn_stop2.Enabled = false;

                }

                //Comb 参数写入
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
                btn_stop2.Enabled = false;

            }
        }

        //stop of Combination
        private void btn_stop2_Click_1(object sender, EventArgs e)
        {
            btn_freq.Enabled = true;
            btn_TD.Enabled = true;
            btn_DC.Enabled = true;
            btn_AC.Enabled = true;
            BackToolStripMenuItem.Enabled = true;
            btn_comb_start.Enabled = true;
            btn_stop2.Enabled = false;

            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x07, 0x07 }, 0, 5);//comb 停止帧头
            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
            btn_comb_start.Enabled = true;
            chart_temp.Series[0].Points.Clear();
            
            //Comb 停止时间写入
            FileStream fs = new FileStream(parameter4, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
            sw.WriteLine("\n");
            sw.Flush();
            sw.Close();

            cnt_comb++;

            //新建参数文件
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

        //Days Difference
        private void DTP_End2_ValueChanged(object sender, EventArgs e)
        {
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
            else
            {
                MessageBox.Show("The Days should between 1 and 7.");
            }

            string ts = Convert.ToString(ts2);
            cb_days2.Text = ts;
        }

        //*********************************************//
        //****************New Project*****************//
        //*******************************************//

        //Form 2(data send back)
        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ChartClear();
            newProjectToolStripMenuItem.Enabled = true;
            LoadProtoolStripMenuItem1.Enabled = false;
            DataAnalyserToolStripMenuItem.Enabled = false;

            NewProject f2 = new NewProject();
            f2.ShowDialog();     
            if (f2.DialogResult == DialogResult.OK)
            {
                //从新建文件窗口获取传输参数
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

                //新建文件后文件界面初始化
                enableButtons(true, true, true, true, false, false, false, true, false, false);
                btn_freq.Visible = true;
                btn_TD.Visible = true;
                btn_DC.Visible = true;
                btn_AC.Visible = true;
                btn_cfg.Visible = true;
                btn_freq.Enabled = false;
                btn_TD.Enabled = false;
                btn_DC.Enabled = false;
                btn_AC.Enabled = false;
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
            //ChartClear();
            newProjectToolStripMenuItem.Enabled = false;
            LoadProtoolStripMenuItem1.Enabled = true;
            DataAnalyserToolStripMenuItem.Enabled = false;

            //选择已存在工程
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //界面初始化
                enableButtons(true, true, true, true, false, false, false, true, false, false);
                btn_freq.Visible = true;
                btn_TD.Visible = true;
                btn_DC.Visible = true;
                btn_AC.Visible = true;
                btn_cfg.Visible = true;
                btn_freq.Enabled = false;
                btn_TD.Enabled = false;
                btn_DC.Enabled = false;
                btn_AC.Enabled = false;
                panel_switch.Visible = true;
                panel_load.Visible = false;

                //路径及参数赋值
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

                //在工程目录建立ID_Information.txt，保存工程信息
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
            //界面初始化  关闭New Project和Load Project功能
            ChartClear();
            newProjectToolStripMenuItem.Enabled = false;
            LoadProtoolStripMenuItem1.Enabled = false;
            DataAnalyserToolStripMenuItem.Enabled = true;
            BackToolStripMenuItem.Enabled = true;

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

            //选择已存在的工程文件
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择文件路径";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                cnt7++;//每打开一个工程文件  计数器加一
                foldPath = dialog.SelectedPath;

            }
        }

        //FDA 结构体
        public struct Point1
        {
            public double X;
            public double Y;
            public double Z;
        }

        //TD 结构体
        public struct Point_T
        {
            public double X;
            public double Y;
        }

        //DC 结构体
        public struct Point_U
        {
            public double X;
            public double Y;
            public double Z;
            public double K;
        }


        //FDA 选择进入
        private void btn_fre_load_Click(object sender, EventArgs e)
        {

            btn_fre_load.Visible = false;
            btn_s.Visible = true;
            btn_m.Visible = true;

        }

        //painting of single measurement in FDA

        private void btn_s_Click(object sender, EventArgs e)
        {
            ChartClear();
            cnt7 = 0;
            //路径添加
            string FDA = foldPath + "/FDA";
            string FDA_S = FDA + "/0Single_Measurement.txt";
            string FDA_M = FDA + "/0Multiple_Measurement.txt";
            string FDA_P1 = FDA + "/Parameter_s.txt";
            string[] lines = File.ReadAllLines(FDA_S);

            //线条添加
            chart1.Series.Add((cnt7).ToString());//添加
            chart2.Series.Add((cnt7).ToString());//添加
            chart3.Series.Add((cnt7).ToString());//添加
            this.chart1.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart2.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart3.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            // 点列表集合
            List<Point1> points = new List<Point1>();
            // 让过第一行，从第二行开始处理
            if (lines.Length > 0)
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point1 p;
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

                    //画图
                    this.chart1.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    this.chart2.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Z);
                    this.chart3.Series[(cnt7).ToString()].Points.AddXY(real, img);

                }
            }
        }


        //paiting of multiple measurement in FDA
        private void btn_m_Click(object sender, EventArgs e)
        {
            ChartClear();
            cnt7 = 0;
            //路径添加
            string FDA = foldPath + "/FDA";
            string FDA_S = FDA + "/0Single_Measurement.txt";
            string FDA_M = FDA + "/0Multiple_Measurement.txt";
            string FDA_P2 = FDA + "/Parameter_m.txt";
            string[] lines = File.ReadAllLines(FDA_M);
            //线条添加
            chart1.Series.Add((cnt7).ToString());//添加
            chart2.Series.Add((cnt7).ToString());//添加
            chart3.Series.Add((cnt7).ToString());//添加
            this.chart1.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart2.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart3.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;

            // 点列表集合
            List<Point1> points = new List<Point1>();
            // 让过第一行，从第二行开始处理

            if (lines.Length > 0)
            { 
                    for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point1 p;
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

                    //画图
                    this.chart1.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    this.chart2.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Z);
                    this.chart3.Series[(cnt7).ToString()].Points.AddXY(real, img);

                }
            }
        }

        // //paiting of single measurement in TD
        private void btn_td_load_Click(object sender, EventArgs e)
        {
            ChartClear();
            cnt7 = 0;
            
            btn_fre_load.Visible = true;
            btn_s.Visible = false;
            btn_m.Visible = false;

            //路径添加
            string TD = foldPath + "/TD";
            string TD_S = TD + "/0Single_Measurement.txt";
            //string FDA_M = FDA + "/Multiple_Measurement.txt";
            string TD_p = TD + "/Parameter.txt";

            //线条添加
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

                    //画图
                    this.chart4.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    //this.chart4.ChartAreas[0].AxisX.IsLogarithmic = true;
                }
            }
        }

        //paiting in DC
        private void btn_dc_load_Click(object sender, EventArgs e)
        {
            ChartClear();
            cnt7 = 0;
            btn_fre_load.Visible = true;
            btn_s.Visible = false;
            btn_m.Visible = false;

            //路径添加
            string DC = foldPath + "/DC";
            string DC_U = DC + "/0U_I_R_data.txt";
            //string FDA_M = FDA + "/Multiple_Measurement.txt";
            string TD_p = TD + "/Parameter.txt";

            //线条添加
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

                    //画图
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

            ChartClear();
            cnt7 = 0;
            //路径添加
            string COMB = foldPath + "/Combination";
            string Comb = COMB + "/0Combination_Measurement.txt";
            string[] lines = File.ReadAllLines(Comb);

            //线条添加
            chart1.Series.Add((cnt7).ToString());//添加
            chart2.Series.Add((cnt7).ToString());//添加
            chart3.Series.Add((cnt7).ToString());//添加
            this.chart1.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart2.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            this.chart3.Series[(cnt7).ToString()].ChartType = SeriesChartType.Point;
            // 点列表集合
            List<Point1> points = new List<Point1>();
            // 让过第一行，从第二行开始处理
            if (lines.Length > 0)
            {

                for (int i = 1; i < lines.Length; i++)
                {
                    string line = lines[i];
                    // 拆分行
                    string[] v = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    Point1 p;
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

                    //画图
                    this.chart1.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Y);
                    this.chart2.Series[(cnt7).ToString()].Points.AddXY(p.X, p.Z);
                    this.chart3.Series[(cnt7).ToString()].Points.AddXY(real, img);

                }
            }
        }


        //*********************************************//
        //****************Windows Close***************//
        //*******************************************//
        //shut down
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Data will be lost, are you sure to close the window?",
                    "Attention",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) != DialogResult.OK)
            {


               // e.Cancel = true;
                return;
            }
            System.Environment.Exit(0);
        }

        //back
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            newProjectToolStripMenuItem.Enabled = true;
            LoadProtoolStripMenuItem1.Enabled = true;
            DataAnalyserToolStripMenuItem.Enabled = true;
            BackToolStripMenuItem.Enabled = false;
            ChartClear();
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(
                    "Data will be lost, are you sure to close the window?",
                    "Attention",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) != DialogResult.OK)
            {

                
                e.Cancel = true;
                return;
            }
            System.Environment.Exit(0);

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

        private void btn_scan_Click(object sender, EventArgs e)
        {
            string[] PortNames = SerialPort.GetPortNames();    //获取本机串口名称，存入PortNames数组中
            BackToolStripMenuItem.Enabled = true;
            for (int i = 0; i < PortNames.Count(); i++)
                //for(int j = 0; j < PortNames.Count(); j++)
                {
                       
                        
                            COMChoose.Items.Add(PortNames[i]);   //将数组内容加载到comboBox控件中
                        
                }
        }

        private void btn_download_Click(object sender, EventArgs e)
        {
            string filePathOnly2 = Path.GetDirectoryName(pathString1);
            string fold2 = Path.GetFileName(filePathOnly2);

            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "//" + fold2 + "/FDA/Single measurement.txt"))//判断是否已存在文件
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(new Uri("http://192.168.191.1:8080/" + ID_Num + "//" + "FDA/Single measurement.txt"), Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "//" + fold2 + "/FDA/Single measurement1.txt");
            }
            if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "//" + fold2 + "/FDA/Single measurement.txt"))//判断是否已存在文件
            {
                WebClient wc = new WebClient();
                wc.DownloadFile(new Uri("http://192.168.191.1:8080/" + ID_Num + "//" + "FDA/Single measurement.txt"), Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "//" + fold2 + "/FDA/Single measurement1.txt");
            }
        }
    }
}
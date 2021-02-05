﻿using System;
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
using Microsoft.VisualBasic;
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
        private double fre = 1;//FDA 频率
        private double mag = 0;//FDA 幅值
        private double pha = 0;//FDA 相位
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
        private Int64 cnt6 = 0;//TD幅度图存
        private Int64 cnt7 = 0;//Data Analyzer 画多曲线计数器
        private Int64 cnt8 = 0;//Comb幅度图存
        private Int64 cnt9 = 0;//Comb相位图存
        private Int64 cnt10 = 0;//Comb Nqst图存
        private Int64 cnt11 = 0;//TD相位图存
        private Int64 cnt12 = 0;//TDNqst图存
        private Int64 cnt_hands_shake = 0;//串口握手信号计数器
        private Int64 cnt_hands_shake_wifi = 0;//wifi握手信号计数器

        private bool Hands_shake = true; //握手标志位
        private bool Hands_shake1 = false; //握手标志位
        private bool FDA_PLOT = true; //FDA多曲线标志位
        private bool COMB_PLOT = true; //COMB多曲线标志位
        private bool TD_PLOT = true; //COMB多曲线标志位

        private Int64 cnt_comb = -1;//comb 文件命名计数器
        private Int64 cnt_FDA1 = -1;//FDA 单次测量命名计数器
        private Int64 cnt_FDA2 = -1;////FDA 多次测量命名计数器
        private Int64 cnt_TD = -1;//TD 文件命名计数器
        private Int64 cnt_UIR = -1;//UIR 文件命名计数器
        private Int64 cnt_temp = 0;//UIR 文件命名计数器

        private Queue chart_x = new Queue();//  数据队列 用于无线传输
        private Queue chart_y = new Queue();
        private Queue data_queue = new Queue();
        private Queue ID_queue = new Queue();

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
<<<<<<< HEAD

=======
          
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            tb_Sweep_f.Text = "10";
            tb_Sweep_t.Text = "100";

            tb_times_T1.Text = "0";
            tb_times_T2.Text = "0";
<<<<<<< HEAD
            cb_days.Text = "0";
=======
            cb_days.Text = "0";            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
<<<<<<< HEAD

=======
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

           
            cb_freq_f2.Text = "Hz";
            cb_freq_t2.Text = "Hz";

            cb_days2.Text = "1";

            SaveFilePath.Visible = true;
            ChooseFile.Visible = true;
            CheckForIllegalCrossThreadCalls = false;
            panel_load.Visible = false;
            enableButtons(false, false, false, false, false, false, false, false, false, false);

        }

        //事件添加
        private void Form1_Load(object sender, EventArgs e)
        {
            FuncBtnInit("close");
<<<<<<< HEAD

            btn_download.Visible = false;

=======

            btn_download.Visible = false;

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            this.MinimumSize = new Size(800, 350);  //限制窗体的最小宽度为370，最小高度为240

            ipaddress.Text = "http://192.168.191.1";
            Port.Text = "8080";

            string[] PortNames = SerialPort.GetPortNames();    //获取本机串口名称，存入PortNames数组中
            BackToolStripMenuItem.Enabled = true;
            for (int i = 0; i < PortNames.Count(); i++)
            {
                COMChoose.Items.Add(PortNames[i]);   //将数组内容加载到comboBox控件中

            }

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
<<<<<<< HEAD
                a = Convert.ToSingle(mytag[2]) * newx;//左边距离
=======
               a = Convert.ToSingle(mytag[2]) * newx;//左边距离
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
            {
                s = fest_str(s, s.Length + 1);
            }
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

        //计算时间差(与系统时间作差)
        private string TimeDifference(DateTime Zeit)
        {
            string TD1 = "0";
            DateTime start_time1 = System.DateTime.Now;//获取系统时间
<<<<<<< HEAD
            TimeSpan ts = Zeit.Subtract(start_time1);//计算系统时间与当前时间差
=======
            TimeSpan ts = Zeit.Subtract(start_time1);//计算系统事件与当前时间差

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;

            if (TD < 0)
            {
                TD = TD + 86400;
            }

<<<<<<< HEAD
            TD1 = (TD >= 60) ? TD.ToString() : "0";
=======
            if (TD >= 60)
            {
                TD1 = TD.ToString();
            }
            else
            {
                TD1 = "0";
            }
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            return TD1;
        }

        //字符串转BCDbyte
        private byte[] strToBCDByte(string s)
<<<<<<< HEAD
        {
            Int32 BCD_Code = Convert.ToInt32(s) / 10 * 16 + Convert.ToInt32(s) % 10;
            string BCD = BCD_Code.ToString();
            byte[] result = strToHexByte(BCD);
=======
        {
            Int32 BCD_Code = Convert.ToInt32(s) / 10 * 16 + Convert.ToInt32(s) % 10;
            string BCD = BCD_Code.ToString();
            byte[] result = strToHexByte(BCD);
            return result;
        }


        //字符串转16进制字符串(不用)
        private string StringToHexString(string s)
        {
            char[] c = s.ToCharArray();
            string result = string.Empty;
            for (int i = 0; i < c.Length; i++)//逐字节变为16进制字符，以%隔开
            {
                result += Convert.ToString(Convert.ToInt32(c[i]) - 48);
            }
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            return result;
        }

        /// 字符串转16进制byte[]编码发送
        private byte[] strToHexByte(string Str)
        {
            string hexString = strToHex(Str);
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            //test.AppendText(hexString);
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }

        //三角波发生器
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

            y = (t < p) ? 2 * t / p - 1 : -2 * t / (2 * Math.PI - p) + (2 * Math.PI + p) / (2 * Math.PI - p);// 1上升沿，0下降沿

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
<<<<<<< HEAD

            y = (t < p) ? 1 : 0;// 1上升沿，0下降沿

            return y;
        }
        //时间校准
        private void datetimesend()
        {
            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x0F, 0x06 }, 0, 5);//帧头
                    DateTime realtime = System.DateTime.Now;
                    Int64 date = Convert.ToInt64(realtime.ToString("yy/MM/dd").Replace("/", ""));
                    Int64 time = Convert.ToInt64(realtime.ToString("HH:mm:ss").Replace(":", ""));

                    //时分秒计算
                    Int64 nowhours = time / 10000;
                    Int64 prepare_min = time % 10000;
                    Int64 nowmin = prepare_min / 100;
                    Int64 nowsec = prepare_min % 100;

                    serialPort1.Write(strToBCDByte(nowhours.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowmin.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowsec.ToString()), 0, 1);

                    string Weekday = (realtime.DayOfWeek).ToString();

                    string[] weekdays = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                    for (int i = 0; i < weekdays.Length; i++)
                    {
                        if (Weekday.Equals(weekdays[i]))
                        {
                            serialPort1.Write(new byte[] { Convert.ToByte(i) }, 0, 1);
                        }

                    }
                    //年月日计算
                    Int64 prepare_year = date % 1000000;
                    Int64 nowyear = prepare_year / 10000;
                    Int64 prepare_month = prepare_year % 10000;
                    Int64 nowmonth = prepare_month / 100;
                    Int64 nowday = prepare_month % 100;

                    serialPort1.Write(strToBCDByte(nowmonth.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowday.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowyear.ToString()), 0, 1);

                    serialPort1.Write(new byte[] { 0x00 }, 0, 1);
                    serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾

                }
                else
                {
                    MessageBox.Show("Please open the serialport at first");
                }
            }
            catch
=======
            if (t < p)
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            {
                MessageBox.Show("Please open the serialport at first");
            }
        }

        private string CombBoxItemSet(ComboBox sender)
        {
            string[] Array = new string[sender.Items.Count];
            string result = string.Empty;
            for (int i = 0; i < sender.Items.Count; i++)
            {
                Array[i] = sender.Items[i].ToString();
            }
            for (int i = 0; i < Array.Length; i++)
            {
                if (sender.Text.Equals(Array[i]))
                {
                    ReceiveArea.Text = i.ToString();
                    result = i.ToString();
                }
            }
            return result;
        }
        //时间校准
        private void datetimesend()
        {
            try
            {
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x0F, 0x06 }, 0, 5);//帧头
                    DateTime realtime = System.DateTime.Now;
                    Int64 date = Convert.ToInt64(realtime.ToString("yy/MM/dd").Replace("/", ""));
                    Int64 time = Convert.ToInt64(realtime.ToString("HH:mm:ss").Replace(":", ""));

<<<<<<< HEAD
        private void ButtonEnable(bool FDA_EN, bool TD_EN, bool DC_EN, bool Comb_EN, bool start_EN, bool DC_start_EN, bool comb_start_EN, bool stop_EN, bool DC_stop_EN, bool comb_stop_EN)
        {
            btn_freq.Enabled = FDA_EN;
            btn_TD.Enabled = TD_EN;
            btn_DC.Enabled = DC_EN;
            btn_AC.Enabled = Comb_EN;
            btn_start.Enabled = start_EN;
            btn_start1.Enabled = DC_start_EN;
            btn_comb_start.Enabled = comb_start_EN;
            btn_stop.Enabled = stop_EN;
            btn_stop1.Enabled = DC_stop_EN;
            btn_stop2.Enabled = comb_stop_EN;
        }

        private void FuncBtnInit(string judgment)
        {
            switch (judgment) {
                case "init" : ButtonEnable(true,  true,  true,  true,  true,  true,  true,  false, false, false); break;//初始化
                case "FDA"  : ButtonEnable(true,  false, false, false, false, false, false, true,  false, false); break;//FDA开始实验
                case "TD"   : ButtonEnable(false, true,  false, false, false, false, false, true,  false, false); break;//TD开始实验
                case "DC"   : ButtonEnable(false, false, true,  false, false, false, false, false, true,  false); break;//DC开始实验
                case "Comb" : ButtonEnable(false, false, false, true,  false, false, false, false, false, true) ; break;//Comb开始实验
                case "close": ButtonEnable(false, false, false, false, true,  true,  true,  false, false, false); break;//Comb开始实验
                default: break;
            }
        }

        //文件序号最大值查询
        private Int64 filemaxseach(string filepath, string filename)
        {
            char ID_file = '0';
            Int64 value = 0;

            DirectoryInfo dir = new DirectoryInfo(filepath);
            FileInfo[] finfo = dir.GetFiles(filename, SearchOption.AllDirectories);
            string fnames = string.Empty;
            try
            {
                ID_file = finfo[finfo.Length - 1].Name[0];
                value = Convert.ToInt64(ID_file) - 48;
                return value;
            }
            catch
            {
                return -1;
            }
        }

        private double MovTheFloatOfInt(double Input)
        {
            int value = (int)(Input * 100);
            Input = value / 100;
            return Input;
        }
        //求复数实部和虚部
        private Tuple<double, double, double, double, double> VirtualCalculate(double X, double Y, double Z)
        {
            X = MovTheFloatOfInt(X);

            double real = Y * Math.Cos(Z * Math.PI / 180);
            double img = Y * Math.Sin(-Z * Math.PI / 180);

            real = MovTheFloatOfInt(real);
            img = MovTheFloatOfInt(img);

            return Tuple.Create(X, Y, Z, real, img);
        }

        //天数控制在1-7
        private void DTP_E_ValueChanged(Control DTP_start, Control DTP_end, Control control)
        {
            string dt1 = System.DateTime.Now.ToString("yyyy/MM/dd");
            DTP_start.Text = dt1;
            DateTime d1 = DateTime.Parse(DTP_start.Text);
            DateTime d2 = DateTime.Parse(DTP_end.Text);
            System.TimeSpan ND = d2 - d1;
            int ts1 = ND.Days + 1;   //天数差
            int ts2 = 0;
            if ((ts1 > 0) && (ts1 < 8))
            {
                ts2 = ts1;
            }
            else
            {
                MessageBox.Show("The Days should between 1 and 7.");
                ts2 = 0;
            }
            string ts = Convert.ToString(ts2);
            control.Text = ts;
        }

        //开始  结束时间控制
        private void DTPWaring(Control DTP, DateTime referenz, bool flag, string msg)
        {
            DateTime control_time = DateTime.Parse(DTP.Text);
            DateTime referenz_time1 = referenz;//获取系统时间

            if (flag == true)
            {
                TimeSpan ts = control_time.Subtract(referenz_time1);//计算系统时间与当前时间差
                int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if (TD < 60)
                {
                    MessageBox.Show(msg);
                    DTP.Text = System.DateTime.Now.AddMinutes(2).ToString("HH:mm:ss");
                }
            }
            else
            {
                string tp = TimeDifference(control_time);//计算系统时间与结束时间差
                string ts = TimeDifference(referenz_time1);//计算系统时间与开始时间差
                if ((Convert.ToUInt64(tp) - Convert.ToUInt64(ts)) < 60)
                {
                    MessageBox.Show(msg);
                    DTP.Text = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }
=======
                    //时分秒计算
                    Int64 nowhours = time / 10000;
                    Int64 prepare_min = time % 10000;
                    Int64 nowmin = prepare_min / 100;
                    Int64 nowsec = prepare_min % 100;

                    serialPort1.Write(strToBCDByte(nowhours.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowmin.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowsec.ToString()), 0, 1);

                    string Weekday = (realtime.DayOfWeek).ToString();

                    string[] weekdays = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                    for (int i = 0; i < weekdays.Length; i++)
                    {
                        if (Weekday.Equals(weekdays[i]))
                        {
                            serialPort1.Write(new byte[] {Convert.ToByte(i) }, 0, 1);
                        }

                    }

                    //年月日计算
                    Int64 prepare_year = date % 1000000;
                    Int64 nowyear = prepare_year / 10000;
                    Int64 prepare_month = prepare_year % 10000;
                    Int64 nowmonth = prepare_month / 100;
                    Int64 nowday = prepare_month % 100;

                    serialPort1.Write(strToBCDByte(nowmonth.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowday.ToString()), 0, 1);
                    serialPort1.Write(strToBCDByte(nowyear.ToString()), 0, 1);

                    serialPort1.Write(new byte[] { 0x00 }, 0, 1);
                    serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾

                }
                else
                {
                    MessageBox.Show("Please open the serialport at first");
                }
            }
            catch
            {
                MessageBox.Show("Please open the serialport at first");
            }
        }

        private string CombBoxItemSet(ComboBox sender)
        {
            string[] Array = new string[sender.Items.Count];
            string result = string.Empty;
            for (int i = 0; i < sender.Items.Count; i++)
            {
                Array[i] = sender.Items[i].ToString();
            }
            for (int i = 0; i < Array.Length; i++)
            {
                if (sender.Text.Equals(Array[i]))
                {
                    ReceiveArea.Text = i.ToString();
                    result =  i.ToString();
                }              
            }
            return result;
        }

        private void ButtonEnable(bool FDA_EN,bool TD_EN, bool DC_EN, bool Comb_EN, bool start_EN, bool DC_start_EN, bool comb_start_EN,bool stop_EN, bool DC_stop_EN, bool comb_stop_EN ) 
        {
            btn_freq.Enabled = FDA_EN;
            btn_TD.Enabled = TD_EN;
            btn_DC.Enabled = DC_EN;
            btn_AC.Enabled = Comb_EN;
            btn_start.Enabled = start_EN;
            btn_start1.Enabled = DC_start_EN;
            btn_comb_start.Enabled = comb_start_EN;
            btn_stop.Enabled = stop_EN;
            btn_stop1.Enabled = DC_stop_EN;
            btn_stop2.Enabled = comb_stop_EN;
        }

        private void FuncBtnInit(string judgment)
        {
            switch (judgment) {
                case "init" : ButtonEnable(true,  true,  true,  true,  true,  true,  true,  false, false, false); break;//初始化
                case "FDA"  : ButtonEnable(true,  false, false, false, false, false, false, true,  false, false); break;//FDA开始实验
                case "TD"   : ButtonEnable(false, true,  false, false, false, false, false, true,  false, false); break;//TD开始实验
                case "DC"   : ButtonEnable(false, false, true,  false, false, false, false, false, true,  false); break;//DC开始实验
                case "Comb" : ButtonEnable(false, false, false, true,  false, false, false, false, false, true ); break;//Comb开始实验
                case "close": ButtonEnable(false, false, false, false, true,  true,  true,  false, false, false); break;//Comb开始实验
                default: break;
            }
        }


        //*********************************************//
        //**************界面清空**********************//
        //*******************************************//
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

        //按钮清空
        private void ClearReceive_Click(object sender, EventArgs e)
        {
            ChartClear();
        }
        //*********************************************//
        //**************界面清空**********************//
        //*******************************************//

        //按钮清空
        private void ClearReceive_Click(object sender, EventArgs e)
        {
            ChartClear();
        }

        //发送位清空
        private void ClearSendArea_Click(object sender, EventArgs e)
        {
            SendArea.Clear();
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
        }




        //*********************************************//
        //**************串口传输+画图*****************//
        //*******************************************//

        //手动添加串口
        private void btn_scan_Click(object sender, EventArgs e)
        {
            string[] PortNames = SerialPort.GetPortNames();    //获取本机串口名称，存入PortNames数组中
            BackToolStripMenuItem.Enabled = true;
            for (int i = 0; i < PortNames.Count(); i++)
            {
                COMChoose.Items.Add(PortNames[i]);   //将数组内容加载到comboBox控件中
            }
        }

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
                    FuncBtnInit("close");
                }
            }
            else
            {
                Hands_shake = false;
                serialPort1.Close();
<<<<<<< HEAD
=======
                //timer2.Enabled = false;
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
                byte[] temp2 = strToHexByte(ID_Num);
                byte[] temp = exchange(temp2);
                for (int i = 0; i < temp.Length; i++)
                {
                    ID_N[3 - i] = temp[i];
                }

                //重复发送5次   延时10ms

                for (int h = 0; h < 3; h++)
                {
                    if (Hands_shake == true)
                    {
                        //时间校准
                        datetimesend();
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

        //定义串口接收事件
        private void Port_DataRecevied(object sender, SerialDataReceivedEventArgs e)
        {
<<<<<<< HEAD

=======
          
        }

        //定义串口接收事件
        private void Port_DataRecevied(object sender, SerialDataReceivedEventArgs e)
        {
                    
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            if (Hands_shake == true)//握手成功
            {
                if (TD == true)//TD 数据接收
                {
                    string strv = serialPort1.ReadLine();
                    if (strv.Equals("stop"))
                    {
                        MessageBox.Show("The TD Experiment is alrasdy Stop");
                        FuncBtnInit("init");
                        TD_PLOT = true;

<<<<<<< HEAD
                        try {
                            string oldStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement.txt";

                            // 新文件名
                            string newStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement(Experiment Finished).txt";

                            // 改名方法
                            FileInfo fi = new FileInfo(oldStr);
                            fi.MoveTo(Path.Combine(newStr));
                        }
                        catch
                        {
                            //防止stop后继续给数据出错
                        }
                    }
                    string[] strArr = strv.Split(',');

=======

                        try {
                        string oldStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement.txt";

                        // 新文件名
                        string newStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement(Experiment Finished).txt";

                        // 改名方法
                        FileInfo fi = new FileInfo(oldStr);
                        fi.MoveTo(Path.Combine(newStr));
                    }
                        catch
                        {
                        //防止stop后继续给数据出错
                        }
                    }
                            
                    string[] strArr = strv.Split(',');
                            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                    int length_v = strArr.Length;
                    time++;

                    try
                    {
                        //TD 画图
                        if (length_v == 9 && strArr[0] == "AA" && strArr[1] == "FF" && strArr[2] == "FF" && strArr[3] == "AA" && strArr[4] == "5")
                        {

                            ReceiveArea.AppendText(cnt.ToString() + "," + strArr[5] + "," + strArr[6] + "," + strArr[7] + '\r' + '\n');
<<<<<<< HEAD
                            fre = Convert.ToDouble(strArr[5]);
                            mag = Convert.ToDouble(strArr[6]);
                            pha = Convert.ToDouble(strArr[7]);
=======
                            fre = Convert.ToSingle(strArr[5]);
                            mag = Convert.ToSingle(strArr[6]);
                            pha = Convert.ToSingle(strArr[7]);
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

                            if (TD_PLOT == true)
                            {
                                try
                                {
                                    chart1.Series.Add(strArr[8]);//添加
                                    chart2.Series.Add((strArr[8]));//添加
                                    chart3.Series.Add((strArr[8]));//添加
                                    chart4.Series.Add(("mag"));//添加
                                    chart4.Series.Add(("pha"));//添加
                                    this.chart1.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                    this.chart2.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                    this.chart3.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                    this.chart4.Series["mag"].ChartType = SeriesChartType.Point;
                                    this.chart4.Series["pha"].ChartType = SeriesChartType.Point;
                                    TD_PLOT = false;
                                }
                                catch
                                {
                                }
                            }
<<<<<<< HEAD

                            var result = VirtualCalculate(fre, mag, pha);
                            fre = result.Item1;
                            mag = result.Item2;
                            pha = result.Item3;
                            double real = result.Item4;
                            double img = result.Item5;
=======
                            int fre_int = (int)(fre * 100);
                            fre = fre_int / 100;

                            double real = mag * Math.Cos(pha * Math.PI / 180);
                            double img = mag * Math.Sin(-pha * Math.PI / 180);

                            int real_int = (int)(real * 100);
                            real = real_int / 100;

                            int img_int = (int)(img * 100);
                            img = img_int / 100;
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

                            this.chart1.Series[strArr[8]].Points.AddXY(fre, mag);
                            this.chart2.Series[strArr[8]].Points.AddXY(fre, pha);
                            this.chart3.Series[strArr[8]].Points.AddXY(real, img);

                            cnt++;
                            this.chart4.Series["mag"].Points.AddXY(cnt, Convert.ToSingle(strArr[6]));

                            this.chart4.Series["pha"].Points.AddXY(cnt, Convert.ToSingle(strArr[7]));

                            //添加到无线传输队列
                            data_queue.Enqueue(cnt + "," + strArr[6] + ":");

                            //写入文件
                            if (cnt_TD > -1)
                            {
                                FileStream fs = new FileStream(pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement.txt", FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.WriteLine(strArr[5] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t");
                                sw.Flush();
                                sw.Close();
                                fs.Close();
                            }
                        }
                    }
                    catch 
                    {
<<<<<<< HEAD

=======
                            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                    }
                }
                else if (DCM == true) // DC 数据接收
                {
<<<<<<< HEAD
                    string stru = serialPort1.ReadLine();
                    if (stru.Equals("stop"))
                    {
                        MessageBox.Show("The DCM Experiment is alrasdy Stop");
                        FuncBtnInit("init");
                        try
                        {
                            string oldStr = pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt";
                            // 新文件名
                            string newStr = pathString3 + "\\" + cnt_UIR + "U_I_R_data(Experiment Finished).txt";
                            // 改名方法
                            FileInfo fi = new FileInfo(oldStr);
                            fi.MoveTo(Path.Combine(newStr));
                        }
                        catch
                        {
                            //STOP后，仍给数据会出错
                        }
                    }

                    string[] strArry = stru.Split(',');

                    int length_u = strArry.Length;
                    time++;
                    string length1 = Convert.ToString(length_u);

                    //DC 画图
                    if (length_u == 7 && strArry[0] == "AA" && strArry[1] == "FF" && strArry[2] == "FF" && strArry[3] == "AA" && strArry[4] == "3")
                    {
                        try
                        {
                            ReceiveArea.AppendText(strArry[5] + "," + strArry[6] + '\r' + '\n');
                            vol = Convert.ToSingle(strArry[5]);
                            cur = Convert.ToSingle(strArry[6]);
                            try
                            {
                                chart_u_i_r.Series.Add("0");//添加
                                chart_u_i_r.Series.Add("1");//添加
                                chart_u_i_r.Series.Add("2");//添加
                                this.chart_u_i_r.Series["0"].ChartType = SeriesChartType.Point;
                                this.chart_u_i_r.Series["1"].ChartType = SeriesChartType.Point;
                                this.chart_u_i_r.Series["2"].ChartType = SeriesChartType.Point;
                            }
                            catch
                            {
                            }

                            double res = vol / cur;

                            this.chart_u_i_r.Series[0].Points.AddXY(time, vol);
                            this.chart_u_i_r.Series[1].Points.AddXY(time, cur);
                            this.chart_u_i_r.Series[2].Points.AddXY(time, res);

                            tb_u.Text = vol.ToString();
                            tb_I.Text = cur.ToString();
                            tb_R.Text = res.ToString();

                            //DC  添加到无线队列
                            data_queue.Enqueue(time + "," + strArry[5] + "," + strArry[6] + ":");

=======

                    string stru = serialPort1.ReadLine();


                    if (stru.Equals("stop"))
                    {
                        MessageBox.Show("The DCM Experiment is alrasdy Stop");

                        FuncBtnInit("init");

                        try
                        {
                            string oldStr = pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt";
                            // 新文件名
                            string newStr = pathString3 + "\\" + cnt_UIR + "U_I_R_data(Experiment Finished).txt";

                            // 改名方法
                            FileInfo fi = new FileInfo(oldStr);
                            fi.MoveTo(Path.Combine(newStr));
                        }
                        catch 
                        {
                        //STOP后，仍给数据会出错
                        }
                    }

                    string[] strArry = stru.Split(',');
                            
                    int length_u = strArry.Length;
                    time++;
                    string length1 = Convert.ToString(length_u);

                    //DC 画图
                    if (length_u == 7 && strArry[0] == "AA" && strArry[1] == "FF" && strArry[2] == "FF" && strArry[3] == "AA" && strArry[4] == "3")
                    {
                        try
                        {
                            ReceiveArea.AppendText(strArry[5] + "," + strArry[6] + '\r' + '\n');
                            vol = Convert.ToSingle(strArry[5]);
                            cur = Convert.ToSingle(strArry[6]);
                            try
                            {
                                chart_u_i_r.Series.Add("0");//添加
                                chart_u_i_r.Series.Add("1");//添加
                                chart_u_i_r.Series.Add("2");//添加
                                this.chart_u_i_r.Series["0"].ChartType = SeriesChartType.Point;
                                this.chart_u_i_r.Series["1"].ChartType = SeriesChartType.Point;
                                this.chart_u_i_r.Series["2"].ChartType = SeriesChartType.Point;
                            }
                            catch
                            {
                            }

                            double res = vol / cur;

                            this.chart_u_i_r.Series[0].Points.AddXY(time, vol);
                            this.chart_u_i_r.Series[1].Points.AddXY(time, cur);
                            this.chart_u_i_r.Series[2].Points.AddXY(time, res);

                            tb_u.Text = vol.ToString();
                            tb_I.Text = cur.ToString();
                            tb_R.Text = res.ToString();

                            //DC  添加到无线队列
                            data_queue.Enqueue(time + "," + strArry[5] + "," + strArry[6] + ":");

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                            //DC 写入文件
                            if (cnt_UIR > -1)
                            {
                                FileStream fs = new FileStream(pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt", FileMode.Append);
                                StreamWriter sw = new StreamWriter(fs);
                                sw.WriteLine(time.ToString() + "\t" + tb_u.Text + "\t" + tb_I.Text + "\t" + tb_R.Text + '\t');
                                sw.Flush();
                                sw.Close();
                                fs.Close();
                            }
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
                    if (str.Equals("stop"))
                    {
                        MessageBox.Show("The FDA Experiment is alrasdy Stop");
                        FuncBtnInit("init");
                        rb_dur.Enabled = true;
                        rb_rep.Enabled = true;
                        FDA_PLOT = true;

                        if (rb_dur.Checked)//单次测量文件写入
                        {
                            try
<<<<<<< HEAD
                            {
                                string oldStr = pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt";
                                // 新文件名
                                string newStr = pathString1 + "\\" + cnt_FDA1 + "Single_Measurement(Experiment Finished).txt";
                                // 改名方法
                                FileInfo fi = new FileInfo(oldStr);
                                fi.MoveTo(Path.Combine(newStr));
                            }
                            catch
                            {

=======
                            {
                                string oldStr = pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt";
                                // 新文件名
                                string newStr = pathString1 + "\\" + cnt_FDA1 + "Single_Measurement(Experiment Finished).txt";
                                // 改名方法
                                FileInfo fi = new FileInfo(oldStr);
                                fi.MoveTo(Path.Combine(newStr));                                      
                            }
                            catch
                            { 
                                    
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                            }
                        }
                        else if (rb_rep.Checked)
                        {
                            try
                            {
                                string oldStr = pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt";
                                // 新文件名
                                string newStr = pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement(Experiment Finished).txt";
                                // 改名方法
                                FileInfo fi = new FileInfo(oldStr);
                                fi.MoveTo(Path.Combine(newStr));
                            }
<<<<<<< HEAD
                            catch
                            {

                            }
                        }
                    }

                    string[] strArr = str.Split(',');
                    int length = strArr.Length;

                    //FDA 画图
                    if (length == 9 && strArr[0] == "AA" && strArr[1] == "FF" && strArr[2] == "FF" && strArr[3] == "AA" && strArr[4] == "1")
                    {
                        ReceiveArea.AppendText(strArr[5] + "," + strArr[6] + "," + strArr[7] + '\r' + '\n');
                        try
                        {
                            fre = Convert.ToDouble(strArr[5]);
                            mag = Convert.ToDouble(strArr[6]);
                            pha = Convert.ToDouble(strArr[7]);

                            //比较上一个数的序号
                            Num_FDA1 = Convert.ToInt32(strArr[8]);
                            if (Num_FDA1 > Num_FDA2)
                            {
                                FDA_PLOT = true;

                            }
                            Num_FDA2 = Convert.ToInt32(strArr[8]);

                            if (FDA_PLOT == true)
                            {
                                try
                                {
                                    chart1.Series.Add((strArr[8]));//添加
                                    chart2.Series.Add((strArr[8]));//添加
                                    chart3.Series.Add((strArr[8]));//添加
                                    this.chart1.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                    this.chart2.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                    this.chart3.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                    FDA_PLOT = false;
                                }
                                catch
                                {

                                }
                            }

                            var result = VirtualCalculate(fre, mag, pha);
                            fre = result.Item1;
                            mag = result.Item2;
                            pha = result.Item3;
                            double real = result.Item4;
                            double img = result.Item5;

                            this.chart1.Series[strArr[8]].Points.AddXY(fre, mag);
                            this.chart2.Series[strArr[8]].Points.AddXY(fre, pha);
                            this.chart3.Series[strArr[8]].Points.AddXY(real, img);

                            //FDA 添加到无线队列
                            data_queue.Enqueue(str + ":");
                            //string data = (string)data_queue.Dequeue();
                            //for (int i = 0; i < 15; i++)
                            //{
                            //     data += (string)data_queue.Dequeue();
                            //    if (i == 15)
                            //    {
                            //        i = 0;
                            //    }

                            //}
                            //socketIoManager(1, str + ":");//发送 数据队列

                            if (rb_dur.Checked)//单次测量文件写入
                            {
                                if (cnt_FDA1 > -1)
                                {
                                    FileStream fs = new FileStream(pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt", FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[5] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t");
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            else if (rb_rep.Checked)//多次测量文件写入
                            {
                                if (cnt_FDA2 > -1)
                                {
                                    FileStream fs = new FileStream(pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt", FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[5] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t" + strArr[8] + '\t');
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("error");
                        }
                    }
                }

                //Comb 数据接收
                else if (ACM == true)
                {
                    string strc = serialPort1.ReadLine();
                    //ReceiveArea.AppendText(strc);
                    if (strc.Equals("stop"))
                    {
                        MessageBox.Show("The Combination Experiment is alrasdy Stop");
                        FuncBtnInit("init");

                        string oldStr = pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt";

                        // 新文件名
                        string newStr = pathString4 + "\\" + cnt_comb + "Combination_Measurement(Experiment Finished).txt";
                        try
                        {
                            // 改名方法
                            FileInfo fi = new FileInfo(oldStr);
                            fi.MoveTo(Path.Combine(newStr));
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        string[] strArr = strc.Split(',');
                        int length = strArr.Length;

                        //Comb 画图
                        if (length == 9 && strArr[0] == "AA" && strArr[1] == "FF" && strArr[2] == "FF" && strArr[3] == "AA" && strArr[4] == "4")
                        {
                            ReceiveArea.AppendText(strArr[5] + "," + strArr[6] + "," + strArr[7] + "," + strArr[8] + '\r' + '\n');
                            try
                            {
                                fre = Convert.ToDouble(strArr[5]);
                                mag = Convert.ToDouble(strArr[6]);
                                pha = Convert.ToDouble(strArr[7]);

                                //Comb 数据与前一个序号作比较
                                Num_COMB1 = Convert.ToInt32(strArr[8]);
                                if (Num_COMB1 > Num_COMB2)
                                {
                                    COMB_PLOT = true;
                                }

                                Num_COMB2 = Convert.ToInt32(strArr[8]);

                                if (COMB_PLOT == true)
                                {
=======
                            catch 
                            {
                                    
                            }
                        }
                    }

                    string[] strArr = str.Split(',');                           
                    int length = strArr.Length;

                    //FDA 画图
                    if (length == 9 && strArr[0] == "AA" && strArr[1] == "FF" && strArr[2] == "FF" && strArr[3] == "AA" && strArr[4] == "1")
                    {
                        ReceiveArea.AppendText(strArr[5] + "," + strArr[6] + "," + strArr[7]+ '\r' + '\n');
                        try
                        {
                            fre = Convert.ToSingle(strArr[5]);
                            mag = Convert.ToSingle(strArr[6]);
                            pha = Convert.ToSingle(strArr[7]);

                        //比较上一个数的序号
                        Num_FDA1 = Convert.ToInt32(strArr[8]);
                        if (Num_FDA1 > Num_FDA2)
                        {                                  
                            FDA_PLOT = true;

                        }
                        Num_FDA2 = Convert.ToInt32(strArr[8]);

                        if (FDA_PLOT == true)
                        {
                            try
                            {
                                chart1.Series.Add((strArr[8]));//添加
                                chart2.Series.Add((strArr[8]));//添加
                                chart3.Series.Add((strArr[8]));//添加
                                this.chart1.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                this.chart2.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                this.chart3.Series[strArr[8]].ChartType = SeriesChartType.Point;
                                FDA_PLOT = false;
                            }
                            catch 
                            { 
                                        
                            }
                        }
                            int fre_int = (int)(fre * 100);
                            fre = fre_int / 100;

                            double real = mag * Math.Cos(pha * Math.PI / 180);
                            double img = mag * Math.Sin(-pha * Math.PI / 180);

                            int real_int = (int)(real * 100);
                            real = real_int / 100;

                            int img_int = (int)(img * 100);
                            img = img_int / 100;

                            this.chart1.Series[strArr[8]].Points.AddXY(fre, mag);
                            this.chart2.Series[strArr[8]].Points.AddXY(fre, pha);
                            this.chart3.Series[strArr[8]].Points.AddXY(real, img);

                            //FDA 添加到无线队列
                            data_queue.Enqueue(str + ":");
                            //string data = (string)data_queue.Dequeue();
                            //for (int i = 0; i < 15; i++)
                            //{
                            //     data += (string)data_queue.Dequeue();
                            //    if (i == 15)
                            //    {
                            //        i = 0;
                            //    }
                                       
                            //}
                            //socketIoManager(1, str + ":");//发送 数据队列

                            if (rb_dur.Checked)//单次测量文件写入
                            {
                                if(cnt_FDA1 > -1)
                                { 
                                    FileStream fs = new FileStream(pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt", FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[5] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t");
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            else if (rb_rep.Checked)//多次测量文件写入
                            {
                                if (cnt_FDA2 > -1)
                                {
                                    FileStream fs = new FileStream(pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt", FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[5] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t" + strArr[8] + '\t');
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                        }
                        catch
                        {
                            Console.WriteLine("error");
                        }
                    }
                }

                //Comb 数据接收
                else if (ACM == true)
                {
                    string strc = serialPort1.ReadLine();
                    //ReceiveArea.AppendText(strc);
                    if (strc.Equals("stop"))
                    {
                        MessageBox.Show("The Combination Experiment is alrasdy Stop");
                        FuncBtnInit("init");

                        string oldStr = pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt";

                        // 新文件名
                        string newStr = pathString4 + "\\" + cnt_comb + "Combination_Measurement(Experiment Finished).txt";
                        try
                        {
                            // 改名方法
                            FileInfo fi = new FileInfo(oldStr);
                            fi.MoveTo(Path.Combine(newStr));
                            //cnt_comb ++;
                        }
                        catch 
                        {
                                
                        }
                    }
                    else
                    {
                        string[] strArr = strc.Split(',');                               
                        int length = strArr.Length;

                        //Comb 画图
                        if (length == 9 && strArr[0] == "AA" && strArr[1] == "FF" && strArr[2] == "FF" && strArr[3] == "AA" && strArr[4] == "4")
                        {
                            ReceiveArea.AppendText(strArr[5] + "," + strArr[6] + "," + strArr[7] + "," + strArr[8] + '\r' + '\n');
                            try
                            {
                                fre = Convert.ToSingle(strArr[5]);
                                mag = Convert.ToSingle(strArr[6]);
                                pha = Convert.ToSingle(strArr[7]);

                                //Comb 数据与前一个序号作比较
                                Num_COMB1 = Convert.ToInt32(strArr[8]);
                                if (Num_COMB1 > Num_COMB2)
                                {
                                    COMB_PLOT = true;
                                }

                                Num_COMB2 = Convert.ToInt32(strArr[8]);

                                if (COMB_PLOT == true)
                                {
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                                    try
                                    {
                                        chart1.Series.Add((strArr[8]));//添加
                                        chart2.Series.Add((strArr[8]));//添加
                                        chart3.Series.Add((strArr[8]));//添加
                                        this.chart1.Series[(strArr[8])].ChartType = SeriesChartType.Point;
                                        this.chart2.Series[(strArr[8])].ChartType = SeriesChartType.Point;
                                        this.chart3.Series[(strArr[8])].ChartType = SeriesChartType.Point;
                                        COMB_PLOT = false;
                                    }
<<<<<<< HEAD
                                    catch
                                    {

                                    }
                                }

                                var result = VirtualCalculate(fre, mag, pha);
                                fre = result.Item1;
                                mag = result.Item2;
                                pha = result.Item3;
                                double real = result.Item4;
                                double img = result.Item5;
=======
                                    catch 
                                    {
                                            
                                    }
                                }
                                int fre_int = (int)(fre * 100);
                                fre = fre_int / 100;

                                double real = mag * Math.Cos(pha * Math.PI / 180);
                                double img = mag * Math.Sin(-pha * Math.PI / 180);

                                int real_int = (int)(real * 100);
                                real = real_int / 100;

                                int img_int = (int)(img * 100);
                                img = img_int / 100;
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

                                this.chart1.Series[(strArr[8])].Points.AddXY(fre, mag);
                                this.chart2.Series[(strArr[8])].Points.AddXY(fre, pha);
                                this.chart3.Series[(strArr[8])].Points.AddXY(real, img);

                                //Comb 添加到无线队列
                                data_queue.Enqueue(strc + ":");

                                //Comb 写入文件
                                if (cnt_comb > -1)
                                {
                                    FileStream fs = new FileStream(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt", FileMode.Append);
                                    StreamWriter sw = new StreamWriter(fs);
                                    sw.WriteLine(strArr[5] + "\t" + mag.ToString() + "\t" + pha.ToString() + "\t" + strArr[8] + '\t');
                                    sw.Flush();
                                    sw.Close();
                                    fs.Close();
                                }
                            }
                            catch
                            {
                                //MessageBox.Show("图表未工作");
                                Console.WriteLine("error");
                            }
                        }
                        else if (length == 6 && strArr[0] == "AA" && strArr[1] == "FF" && strArr[2] == "FF" && strArr[3] == "AA" && strArr[4] == "2")
                        {
                            try
                            {
                                cnt_temp++;
                                this.chart_temp.Series[0].Points.AddXY(cnt_temp, Convert.ToSingle(strArr[5]));
                                tb_temp.Text = strArr[5] + "℃";
                                data_queue.Enqueue(strArr[5] + "℃" + ':');
<<<<<<< HEAD
                                ReceiveArea.AppendText(strArr[5] + "℃" + '\r' + '\n');
                            }
                            catch
                            {
=======
                                ReceiveArea.AppendText(strArr[5] + "℃" +'\r' + '\n');
                            }
                            catch
                            {
                                //MessageBox.Show("图表未工作");
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                                Console.WriteLine("error");
                            }
                        }
                    }
                }
                else
                {
                    //其他数据                           
                    try
                    {
                        string strvv = serialPort1.ReadExisting();
                        string[] strArr = strvv.Split(',');
                        int length = strArr.Length;
                        data_queue.Enqueue(strvv + ':');
                        ReceiveArea.AppendText(strvv);
                    }
                    catch
                    {
                        Hands_shake = false;
                        serialPort1.Close();
                        PortIsOpen = false;
                        OpenPortButton.Text = "Port Open";
                    }
                }
            }
            else
            {
                //握手不成功  按钮无法使用
                FuncBtnInit("close");

                string strh = serialPort1.ReadLine();
                ReceiveArea.AppendText(strh);

                string[] strArr = strh.Split(',');
                int length = strArr.Length;
                if (length == 2 && strArr[0] != "200000.00")
                {
                    //收到0，4时，正常工作
                    if ((strArr[0].Equals("0")) && (strArr[1].Equals("4")))
                    {
<<<<<<< HEAD
                        cnt_hands_shake++;
=======

                        cnt_hands_shake++;                                
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
<<<<<<< HEAD
                    if (cnt_hands_shake > 0)
=======
                    if (cnt_hands_shake>0)
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                    {
                        Hands_shake = true;
                        MessageBox.Show("MCU is already!");
                        //显示功能按钮
                        FuncBtnInit("init");
                        cnt_hands_shake = 0;
                    }
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
            chart3.ChartAreas[0].AxisX.ScaleView.Size = 16000;
            chart3.ChartAreas[0].AxisY.ScaleView.Size = 16000;
            chart4.ChartAreas[0].AxisX.ScaleView.Size = 3000;

            chart_u_i_r.ChartAreas[0].AxisX.ScaleView.Size = 3000;
            FDA_PLOT = true;
            TD_PLOT = true;
            COMB_PLOT = true;
<<<<<<< HEAD
=======
            //cnt11 = 0;//Comb线条数目初始化
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
            Hands_shake1 = true;
            string data = null;
            socketIoManager(0, data);
            socketIoManager(1, ID_Num + ":");
<<<<<<< HEAD

=======
                 
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
                //timer2.Enabled = true;
                socket.On(Socket.EVENT_CONNECT, () =>
                {
                    MessageBox.Show("connect server successfully");
                    btn_download.Visible = true;
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

        //从服务器下载文件
        private void btn_download_Click(object sender, EventArgs e)
        {
            string filePathOnly2 = Path.GetDirectoryName(pathString1);
            string fold2 = Path.GetFileName(filePathOnly2);

<<<<<<< HEAD
            WebClient wc = new WebClient();
            string path_server = "http://192.168.191.1:8080/" + ID_Num + "//";
            string path_Project = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "//" + fold2;
            wc.DownloadFile(new Uri(path_server + "FDA/Single measurement.txt"), path_Project + "/FDA/Single_Measurement.txt");
            wc.DownloadFile(new Uri(path_server + "FDA/Multiple measurement.txt"), path_Project + "/FDA/Multiple_Measurement.txt");
            wc.DownloadFile(new Uri(path_server + "TD/Single measurement.txt"), path_Project + "/TD/Single_Measurement.txt");
            wc.DownloadFile(new Uri(path_server + "DC/U_I_R_Data.txt"), path_Project + "/DC/U_I_R_data.txt");
            wc.DownloadFile(new Uri(path_server + "Combination/Combination_Measurement.txt"), path_Project + "/Combination/Combination_Measurement.txt");

=======
            //if (!File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "//" + fold2 + "/FDA/Single measurement.txt"))//判断是否已存在文件
            {
                WebClient wc = new WebClient();
                string path_server = "http://192.168.191.1:8080/" + ID_Num + "//";
                string path_Project = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "//" + fold2;
                wc.DownloadFile(new Uri(path_server + "FDA/Single measurement.txt"), path_Project + "/FDA/Single_Measurement.txt");
                wc.DownloadFile(new Uri(path_server + "FDA/Multiple measurement.txt"), path_Project + "/FDA/Multiple_Measurement.txt");
                wc.DownloadFile(new Uri(path_server + "TD/Single measurement.txt"), path_Project + "/TD/Single_Measurement.txt");
                wc.DownloadFile(new Uri(path_server + "DC/U_I_R_Data.txt"), path_Project + "/DC/U_I_R_data.txt");
                wc.DownloadFile(new Uri(path_server + "Combination/Combination_Measurement.txt"), path_Project + "/Combination/Combination_Measurement.txt");
            }
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
        }
        //获取本机IPv4
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
<<<<<<< HEAD
                string data = ID_Num + ":";
=======
                string data= ID_Num + ":";
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                //data = (string)data_queue.Dequeue();
                try
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (Hands_shake1 == false)
                        {
                            data += (string)data_queue.Dequeue();
                        }
                        else
                        {
                            data += ID_Num + ":";
                            cnt_hands_shake_wifi++;
                            if (cnt_hands_shake_wifi > 10)
                            {
                                Hands_shake1 = false;
                                cnt_hands_shake_wifi = 0;
                            }
                        }
<<<<<<< HEAD
                    }
                    // socketIoManager(1, data);//发送 数据队列
=======
                    }              
                   // socketIoManager(1, data);//发送 数据队列
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                }
                catch
                {

                }
            }
<<<<<<< HEAD
        }
=======
    }
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

        //*********************************************//
        //**************文件存储**********************//
        //*******************************************//

        private void PicSave(string filepath, Chart chart, Int64 counter)
        {
            chart.SaveImage(filepath, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
            MessageBox.Show("you have stored this picture");
            counter++;
        }

        //bode图幅值保存
        private void button1_Click_2(object sender, EventArgs e)//button1 bode图幅值save
        {
            if (FDA == true)
            {
                string bode_amp = pathString1 + "/bode_amp" + cnt1 + ".png";
                PicSave(bode_amp, chart1, cnt1);
            }
            else if (ACM == true)
            {
                string bode_amp = pathString4 + "/bode_amp" + cnt8 + ".png";
                PicSave(bode_amp, chart1, cnt8);
            }
            else if (TD == true)
            {
                string bode_amp = pathString2 + "/bode_amp" + cnt6 + ".png";
                PicSave(bode_amp, chart1, cnt6);
            }
            else if (TD == true)
            {
                string bode_amp = pathString2 + "/bode_amp" + cnt6 + ".png";
                this.chart1.SaveImage(bode_amp, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt6++;
            }
        }

        //bode图相位保存
        private void SaveBode2_Click(object sender, EventArgs e)
        {
            if (FDA == true)
            {
                string bode_pha = pathString1 + "/bode_pha" + cnt2 + ".png";
                PicSave(bode_pha, chart2, cnt2);
            }
            else if (ACM == true)
            {
                string bode_pha = pathString4 + "/bode_pha" + cnt9 + ".png";
                PicSave(bode_pha, chart2, cnt9);
            }
            else if (TD == true)
            {
                string bode_pha = pathString2 + "/bode_pha" + cnt11 + ".png";
                PicSave(bode_pha, chart2, cnt11);
            }
            else if (TD == true)
            {
                string bode_pha = pathString2 + "/bode_pha" + cnt11 + ".png";
                this.chart2.SaveImage(bode_pha, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt11++;
            }

        }

        //Nyquist图保存
        private void SaveNyq_Click(object sender, EventArgs e)
        {
            if (FDA == true)
            {
                string Nyqst = pathString1 + "/Nyqst" + cnt3 + ".png";
                PicSave(Nyqst, chart3, cnt3);
            }
            else if (ACM == true)
            {
                string Nyqst = pathString4 + "/Nyqst" + cnt10 + ".png";
                PicSave(Nyqst, chart3, cnt10);
            }
            else if (TD == true)
            {
                string Nyqst = pathString2 + "/Nyqst" + cnt12 + ".png";
                PicSave(Nyqst, chart3, cnt12);
            }
            else if (TD == true)
            {
                string Nyqst = pathString2 + "/Nyqst" + cnt12 + ".png";
                this.chart3.SaveImage(Nyqst, System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png);
                MessageBox.Show("you have stored this picture");
                cnt12++;
            }
        }

        //时域曲线保存
        private void SaveTD_Click(object sender, EventArgs e)
        {
            string TD_Wave = pathString2 + "/TD_Wave" + cnt4 + ".png";
            PicSave(TD_Wave, chart4, cnt4);
        }

        //U_I_R曲线保存
        private void button2_Click_1(object sender, EventArgs e)
        {
            string U_I_R_Wave = pathString3 + "/U_I_R_Wave" + cnt5 + ".png";
            PicSave(U_I_R_Wave, chart_u_i_r, cnt5);
        }


        //*********************************************//
        //**************界面切换**********************//
        //*******************************************//

<<<<<<< HEAD
=======
      
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
        private void flag_enabled(bool FDA_f, bool TD_f, bool DCM_f, bool Comb_f)
        {
            FDA = FDA_f;
            TD = TD_f;
            DCM = DCM_f;
            ACM = Comb_f;
        }

        private void FunctionSelect(string select)
        {
            switch (select)
            {
<<<<<<< HEAD
                case "FDA" : flag_enabled(true,  false, false, false); break;
                case "TD"  : flag_enabled(false, true,  false, false); break;
                case "DCM" : flag_enabled(false, false, true , false); break;
                case "Comb": flag_enabled(false, false, false, true ); break;
                case "Init": flag_enabled(false, false, false, false); break;
=======
                case "":; break;
            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            }
        }

        // 频域
        private void btn_freq_Click(object sender, EventArgs e)
        {
            //标志位初始化
<<<<<<< HEAD
            FunctionSelect("FDA");
            FDA_PLOT = true;
            rb_dur.Enabled = true;
            rb_rep.Enabled = true;
=======
            FDA = true;
            TD = false;
            ACM = false;
            DCM = false;

            FDA_PLOT = true;

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            ChartClear();
            //界面初始化
            rb_rep.Enabled = true;
            tb_s_p.Enabled = true;
            panel_switch.Height = btn_freq.Height;
            panel_switch.Top = btn_freq.Top;
            enableButtons(false, true, false, false, true, false, true, false, false, false);
            rb_Frequncy.Checked = false;
            rb_Sweep.Checked = true;

        }

        // 初始化
        private void btn_cfg_Click_1(object sender, EventArgs e)
        {
            //标志位初始化
<<<<<<< HEAD
            FunctionSelect("Init");
=======
            FDA = false;
            TD = false;
            ACM = false;
            DCM = false;
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            COMB_PLOT = true;

            ChartClear();
            //界面初始化
            panel_switch.Height = btn_cfg.Height;
            panel_switch.Top = btn_cfg.Top;
            enableButtons(false, true, true, true, false, false, false, false, false, false);

        }

        // 时域
        private void btn_TD_Click_1(object sender, EventArgs e)
        {
            //标志位初始化
<<<<<<< HEAD
            FunctionSelect("TD");
=======
            FDA = false;
            TD = true;
            ACM = false;
            DCM = false;
            TD_PLOT = true;

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            ChartClear();
            tb_s_p.Text = "1";
            tb_s_p.Enabled = false;
            //界面初始化
            rb_dur.Checked = true;
            rb_rep.Enabled = false;
<<<<<<< HEAD

=======
            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
            FunctionSelect("Comb");

            ChartClear();
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
<<<<<<< HEAD

=======
            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            //是否进行第二阶段
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
            FunctionSelect("DCM");

            ChartClear();
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
            enableButtons(false, true, false, false, false, true, false, false, true, false);
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

            rb_dur.Enabled = false;
            rb_rep.Enabled = false;
<<<<<<< HEAD

            //duration 时间初始化
            int temp_h = 0;
            int temp_m = 0;
            int temp_s = 0;
            int sum = 0;

            //各类较长的数组初始化
            byte[] SendBuff = new byte[64];
            for (int i = 0; i < 64; i++)
            {
                SendBuff[i] = 0x00;
            }

            byte[] StartBit = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 };
            try
            {
=======
            try
            {

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                //socketIoManager(1, data);//发送 数据队列
                //首先判断串口是否开启
                if (serialPort1.IsOpen)
                {
                    if (rb_Frequncy.Checked)//TD选择
                    {
<<<<<<< HEAD
                        StartBit = new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x05 };//TD帧头
                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x05 }, 0, 5);
=======
                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x05 }, 0, 5);//TD帧头
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

                    }
                    else if (rb_Sweep.Checked)//FDA选择
                    {
                        StartBit = new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x01 };//FDA帧头
                        serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x40, 0x01 }, 0, 5);
                    }

                    for (int i = 0; i < StartBit.Length; i++)
                    {
                        SendBuff[i] = StartBit[i];
                    }


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

                        dft = CombBoxItemSet(cb_dft);
                        serialPort1.Write(strToHexByte(dft), 0, 1);//dft


                        //TD
                        if (rb_Frequncy.Checked)
                        {
                            serialPort1.Write(flag, 0, 1);//
                            tb_Sweep_f.Enabled = false;
                            tb_Sweep_t.Enabled = false;
                            cb_freq_f.Enabled = false;
                            cb_freq_t.Enabled = false;
                            FuncBtnInit("TD");

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
                            FuncBtnInit("FDA");

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

                        tia = CombBoxItemSet(cb_tia);
                        serialPort1.Write(strToHexByte(tia), 0, 1);//rtia
                        serialPort1.Write(strToHexByte(tb_s_p.Text.Trim()), 0, 1);//points

                        //repeat
                        if (rb_rep.Checked)
                        {
                            serialPort1.Write(strToHexByte(cb_days.Text), 0, 1);//天数 发送

                            //开始时间

                            DateTime start_time1 = DateTime.Parse(dateTimePicker_f_1.Text);//获取开始时间
                            string start_send1 = TimeDifference(start_time1);//计算与当前时间差

                            byte[] temp_s1 = strToHexByte(start_send1);
                            byte[] temp_se1 = exchange(temp_s1);
                            for (int i = 0; i < temp_se1.Length; i++)
                            {
                                Start_send1[3 - i] = temp_se1[i];
                            }
                            serialPort1.Write(Start_send1, 0, 4);//开始时间1发送
                            DateTime end_time1 = DateTime.Parse(dateTimePicker_t_1.Text);//获取结束时间
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
                                DateTime start_time2 = DateTime.Parse(dateTimePicker_f_2.Text);//获取第二次开始时间
                                string start_send2 = TimeDifference(start_time2);//计算与当前时间差
                                byte[] temp_s2 = strToHexByte(start_send2);
                                byte[] temp_se2 = exchange(temp_s2);
                                for (int i = 0; i < temp_se2.Length; i++)
                                {
                                    Start_send2[3 - i] = temp_se2[i];
                                }

                                serialPort1.Write(Start_send2, 0, 4);//开始时间2发送

                                DateTime end_time2 = DateTime.Parse(dateTimePicker_t_2.Text);//获取第二次结束时间
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
                                cnt_FDA1++;

                                if (!File.Exists(pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt"))
                                {

                                    StreamWriter sw1 = new StreamWriter(pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt");
                                    sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                                    sw1.Flush();
                                    sw1.Close();
                                }
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
                                cnt_FDA2++;
                                if (!File.Exists(pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt"))
                                {
                                    StreamWriter sw1 = new StreamWriter(pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt");
                                    sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                                    sw1.Flush();
                                    sw1.Close();
                                }

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
                            cnt_TD++;
                            if (!File.Exists(pathString2 + "\\" + cnt_TD + "Single_Measurement.txt"))
                            {

                                StreamWriter sw1 = new StreamWriter(pathString2 + "\\" + cnt_TD + "Single_Measurement.txt");
                                sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                                sw1.Flush();
                                sw1.Close();
                            }

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
                        FuncBtnInit("init");
<<<<<<< HEAD
=======

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                    }
                }
                else
                {
                    FuncBtnInit("init");
                    MessageBox.Show("Please open the serialport!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please open the serialport!");
                serialPort1.Close();
                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                FuncBtnInit("init");
            }
        }

        //FDA+TD stop
        private void btn_stop_Click(object sender, EventArgs e)
        {
            //界面初始化
            FuncBtnInit("init");

            FDA_PLOT = true;
            TD_PLOT = true;
            BackToolStripMenuItem.Enabled = true;

            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x07, 0x09 }, 0, 5);//停止帧头
            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾

<<<<<<< HEAD

=======
            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

            if (rb_Frequncy.Checked)//TD 停止时间写入
            {
                
                string oldStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement.txt";

                // 新文件名
                string newStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement(Manual Finished).txt";

                // 改名方法
                FileInfo fi = new FileInfo(oldStr);
                fi.MoveTo(Path.Combine(newStr));

                string oldStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement.txt";

                // 新文件名
                string newStr = pathString2 + "\\" + cnt_TD.ToString() + "Single_Measurement(Manual Finished).txt";

                // 改名方法
                FileInfo fi = new FileInfo(oldStr);
                fi.MoveTo(Path.Combine(newStr));

                FileStream fs = new FileStream(parameter1, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
                sw.WriteLine("\n");
                sw.Flush();
<<<<<<< HEAD
                sw.Close();
=======
                sw.Close();               
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            }
            else
            {
                rb_dur.Enabled = true;
                rb_rep.Enabled = true;

                if (rb_dur.Checked)//FDA 单次停止时间写入
                {

<<<<<<< HEAD

=======
                    
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                    string oldStr = pathString1 + "\\" + cnt_FDA1 + "Single_Measurement.txt";
                    // 新文件名
                    string newStr = pathString1 + "\\" + cnt_FDA1 + "Single_Measurement(Manual Finished).txt";
                    // 改名方法

                    if (!Directory.Exists(newStr))
                    {
                        FileInfo fi = new FileInfo(oldStr);
                        fi.MoveTo(Path.Combine(newStr));
                    }

                    FileStream fs = new FileStream(parameter21, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
                    sw.WriteLine("\n");
                    sw.Flush();
                    sw.Close();

<<<<<<< HEAD

=======
                    
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

                }
                else//FDA 多次停止时间写入
                {
                    string oldStr = pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement.txt";
                    // 新文件名
                    string newStr = pathString1 + "\\" + cnt_FDA2 + "Multiple_Measurement(Manual Finished).txt";
                    // 改名方法
                    FileInfo fi = new FileInfo(oldStr);
                    fi.MoveTo(Path.Combine(newStr));

                    FileStream fs = new FileStream(parameter22, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs);
                    sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
                    sw.WriteLine("\n");
                    sw.Flush();
<<<<<<< HEAD
                    sw.Close();
=======
                    sw.Close();                    
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
            tb_freq.Visible = true;
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


        //每小时最多只能测量一次
        private void tb_times_D1_TextChanged(object sender, EventArgs e)
        {
<<<<<<< HEAD
            tb_times_D_TextChanged(dateTimePicker_f_1, dateTimePicker_t_1, tb_times_D1);
=======
            DateTime start_time1 = DateTime.Parse(dateTimePicker_f_1.Text);//获取开始时间
            DateTime end_time1 = DateTime.Parse(dateTimePicker_t_1.Text);//获取结束时间

            TimeSpan ts = end_time1.Subtract(start_time1);

            int different = (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds) / 3600;
            int repeat_times = Convert.ToInt32(tb_times_D1.Text);

            if (repeat_times > (different + 1))
            {
                MessageBox.Show("The repeat times should be not more than 1 time/hour!");
                tb_times_D1.Text = "0";
            }

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
        }

        private void tb_times_D2_TextChanged(object sender, EventArgs e)
        {
<<<<<<< HEAD
            tb_times_D_TextChanged(dateTimePicker_f_2, dateTimePicker_t_2, tb_times_D2);
=======
            DateTime start_time1 = DateTime.Parse(dateTimePicker_f_2.Text);//获取开始时间
            DateTime end_time1 = DateTime.Parse(dateTimePicker_t_2.Text);//获取结束时间

            TimeSpan ts = end_time1.Subtract(start_time1);

            int different = (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds) / 3600;
            int repeat_times = Convert.ToInt32(tb_times_D2.Text);

            if (repeat_times > (different + 1))
            {
                MessageBox.Show("The repeat times should be not more than 1 time/hour!");
                tb_times_D2.Text = "0";
            }
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
        }

        //天数选择判断
        private void DTP_End_ValueChanged(object sender, EventArgs e)
        {
<<<<<<< HEAD
            DTP_E_ValueChanged(DTP_Start, DTP_End, cb_days);
=======
            string dt1 = System.DateTime.Now.ToString("yyyy/MM/dd");
            DTP_Start.Text = dt1;
            DateTime d1 = DateTime.Parse(DTP_Start.Text);
            DateTime d2 = DateTime.Parse(DTP_End.Text);
            System.TimeSpan ND = d2 - d1;
            int ts1 = ND.Days + 1;   //天数差
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
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
<<<<<<< HEAD
            gb_Single.Visible = false; 
            cb_days.Text = (rb_rep.Checked == true) ?"1": "0";
=======
            gb_Single.Visible = false;
            if (rb_rep.Checked == true)
            {
                cb_days.Text = "1";
            }
            else
            {
                cb_days.Text = "0";
            }
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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

        //二次测量界面
        private void checkBox_SEC_CheckedChanged_1(object sender, EventArgs e)
        {
            panel_sec.Visible = (checkBox_SEC.Checked) ? true: false;
        }

        //*********************************************//
        //****************DC窗口**********************//
        //*******************************************//

        // DC send
        private void btn_start1_Click(object sender, EventArgs e)
        {
            FDA = false;
            TD = false;
            DCM = true;
            ACM = false;
            //部分按钮使能不可用
            FuncBtnInit("DC");

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
                        FuncBtnInit("init");
                    }
                }
                else
                {
                    MessageBox.Show("Please open the serialport!");
                    FuncBtnInit("init");
                }
                cnt_UIR++;

                //添加新的数据文件
                if (!File.Exists(pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt"))
                {
                    StreamWriter sw1 = new StreamWriter(pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt");
                    sw1.WriteLine("Time" + "\t" + "Voltag" + "\t" + "Current" + "\t" + "Impedence");
                    sw1.Flush();
                    sw1.Close();
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
                FuncBtnInit("init");
            }
        }

        //DC Stop
        private void btn_stop1_Click(object sender, EventArgs e)
        {
            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x07, 0x08 }, 0, 5);//DC 停止帧头
            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
            BackToolStripMenuItem.Enabled = true;

            FuncBtnInit("init");

            string oldStr = pathString3 + "\\" + cnt_UIR + "U_I_R_data.txt";

            // 新文件名
            string newStr = pathString3 + "\\" + cnt_UIR + "U_I_R_data(Manual Finished).txt";

            // 改名方法
            FileInfo fi = new FileInfo(oldStr);
            fi.MoveTo(Path.Combine(newStr));

            //DC 停止时间写入
            FileStream fs = new FileStream(parameter3, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
            sw.WriteLine("\n");
            sw.Flush();
<<<<<<< HEAD
            sw.Close();
=======
            sw.Close();          
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
<<<<<<< HEAD
=======
            FDA = false;
            TD = false;
            DCM = false;
            ACM = true;

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            FuncBtnInit("Comb");
            chart_temp.Series[0].Points.Clear();
            //btn_AC.Enabled = false;

            btn_comb_start.Enabled = false;
            btn_stop2.Enabled = true;

            //温度显示判断

<<<<<<< HEAD
            Temperature = (Temperature.Equals("℃") )? tempt.ToString() + "℃": "37℃";

            //弹出框确认实验是否开始
            DialogResult dr = MessageBox.Show("Everything already?", "Combination Measurement", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
=======
            //弹出框确认实验是否开始
            DialogResult dr = MessageBox.Show("Everything already?", "Combination Measurement", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {                
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
                            serialPort1.Write(strToHexByte(cb_dor2.Text), 0, 1);//odr

                            byte[] tmp2 = strToHexByte(tb_amp2.Text);
                            byte[] tmp = exchange(tmp2);
                            for (int i = 0; i < tmp.Length; i++)
                            {
                                amp2[1 - i] = tmp[i];
                            }
                            serialPort1.Write(amp2, 0, 2);//amp
<<<<<<< HEAD
                            dft2 = CombBoxItemSet(cb_dft2);
=======
                            dft2 = CombBoxItemSet(cb_dft2);                           
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                            serialPort1.Write(strToHexByte(dft2), 0, 1);//dft
                            serialPort1.Write(new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00 }, 0, 5);//中部补零

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

<<<<<<< HEAD
                            tia2 = CombBoxItemSet(cb_tia2);
=======
                            tia2 = CombBoxItemSet(cb_tia2);                          
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                            serialPort1.Write(strToHexByte(tia2), 0, 1);//rtia
                            serialPort1.Write(strToHexByte(tb_s_p2.Text.Trim()), 0, 1);//points

                            //repeat

                            serialPort1.Write(strToHexByte(cb_days2.Text), 0, 1);//发送天数

                            //开始时间

                            DateTime start_time1 = DateTime.Parse(dateTimePicker_f_3.Text);//获取时间
                            string start_send1 = TimeDifference(start_time1);//计算开始时间差

                            byte[] temp_s1 = strToHexByte(start_send1);
                            byte[] temp_se1 = exchange(temp_s1);
                            for (int i = 0; i < temp_se1.Length; i++)
                            {
                                Start_send12[3 - i] = temp_se1[i];
                            }

                            serialPort1.Write(Start_send12, 0, 4);//开始时间1发送

                            DateTime end_time1 = DateTime.Parse(dateTimePicker_t_3.Text);//获取第一次截止时间
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
                                DateTime start_time2 = DateTime.Parse(dateTimePicker_f_4.Text);//获取第二次开始时间
                                string start_send2 = TimeDifference(start_time2);//计算第二次开始时间差
                                byte[] temp_s2 = strToHexByte(start_send2);
                                byte[] temp_se2 = exchange(temp_s2);
                                for (int i = 0; i < temp_se2.Length; i++)
                                {
                                    Start_send22[3 - i] = temp_se2[i];
                                }

                                serialPort1.Write(Start_send22, 0, 4);//开始时间2发送

                                DateTime end_time2 = DateTime.Parse(dateTimePicker_t_4.Text);//获取第二次结束时间
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
<<<<<<< HEAD
                            serialPort1.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4);//结束时间2发送
=======
                            serialPort1.Write(new byte[] { 0x00,0x00,0x00,0x00}, 0, 4);//结束时间2发送
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                        }
                        catch (Exception ee)
                        {
                            MessageBox.Show("Please fill in the parameters completely!");
                            FuncBtnInit("init");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please open the serialport!");
                        FuncBtnInit("init");
                    }
                }
                catch (Exception ex)
                {
                    serialPort1.Close();
                    //捕获到异常，创建一个新的对象，之前的不可以再用
                    serialPort1 = new System.IO.Ports.SerialPort();
<<<<<<< HEAD
=======
                
                    MessageBox.Show("Plesase open the serialport!");
                    FuncBtnInit("init");
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

                    MessageBox.Show("Plesase open the serialport!");
                    FuncBtnInit("init");
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
                                serialPort1.Write(flag, 1, 1);
                            }
                            else
                            {
                                serialPort1.Write(flag, 0, 1);
                            }
                            serialPort1.Write(Zeros, 0, 8);//补零
                            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
                        }
                        catch
                        {
                            MessageBox.Show("Please fill in the parameters completely!");
                            FuncBtnInit("init");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please open the serialport!");
                        FuncBtnInit("init");
                    }
                }
                catch (Exception ex)
                {
                    serialPort1.Close();
                    //捕获到异常，创建一个新的对象，之前的不可以再用
                    serialPort1 = new System.IO.Ports.SerialPort();
                    MessageBox.Show("Plesase open the serialport!");
                    FuncBtnInit("init");
<<<<<<< HEAD
                }

                cnt_comb++;

                //新建参数文件
                if (!File.Exists(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt"))
                {
                    StreamWriter sw1 = new StreamWriter(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt");
                    sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha" + "\t" + "ID");
                    sw1.Flush();
                    sw1.Close();
=======
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                }

                cnt_comb++;

                //新建参数文件
                if (!File.Exists(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt"))
                {                  
                    StreamWriter sw1 = new StreamWriter(pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt");
                    sw1.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha" + "\t" + "ID");
                    sw1.Flush();
                    sw1.Close(); 
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
                FuncBtnInit("init");
            }
        }

        //stop of Combination
        private void btn_stop2_Click_1(object sender, EventArgs e)
        {
            FuncBtnInit("init");
            BackToolStripMenuItem.Enabled = true;


            serialPort1.Write(new byte[] { 0xAA, 0xFF, 0xFF, 0x07, 0x07 }, 0, 5);//comb 停止帧头
            serialPort1.Write(new byte[] { 0x0d, 0x0a }, 0, 2);//帧尾
            btn_comb_start.Enabled = true;
<<<<<<< HEAD

=======
            
            
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            //Comb 停止时间写入
            FileStream fs = new FileStream(parameter4, FileMode.Append);
            StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("Stop at:   " + "\t" + System.DateTime.Now + "\t");
            sw.WriteLine("\n");
            sw.Flush();
            sw.Close();

            string oldStr = pathString4 + "\\" + cnt_comb + "Combination_Measurement.txt";

            // 新文件名
            string newStr = pathString4 + "\\" + cnt_comb + "Combination_Measurement(Manual Finished).txt";

            // 改名方法
            FileInfo fi = new FileInfo(oldStr);
            fi.MoveTo(Path.Combine(newStr));

        }

<<<<<<< HEAD
        private void tb_times_D_TextChanged(Control DTP1, Control DTP2, Control tb_times_D)
        {
            DateTime start_time1 = DateTime.Parse(DTP1.Text);//获取开始时间
            DateTime end_time1 = DateTime.Parse(DTP2.Text);//获取结束时间

            TimeSpan ts = end_time1.Subtract(start_time1);

            int different = (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds) / 3600;
            int repeat_times = Convert.ToInt32(tb_times_D3.Text);

            if (repeat_times > (different + 1))
            {
                MessageBox.Show("The repeat times should be not more than 1 time/hour!");
                tb_times_D.Text = "0";
            }
        }

        private void tb_times_D3_TextChanged(object sender, EventArgs e)
        {
            tb_times_D_TextChanged(dateTimePicker_f_3, dateTimePicker_t_3, tb_times_D3);
        }

        private void tb_times_D4_TextChanged(object sender, EventArgs e)
        {
            tb_times_D_TextChanged(dateTimePicker_f_4, dateTimePicker_t_4, tb_times_D4);
=======
        private void tb_times_D3_TextChanged(object sender, EventArgs e)
        {
            DateTime start_time1 = DateTime.Parse(dateTimePicker_f_3.Text);//获取开始时间
            DateTime end_time1 = DateTime.Parse(dateTimePicker_t_3.Text);//获取结束时间

            TimeSpan ts = end_time1.Subtract(start_time1);
            
            int different = (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds) / 3600;
            int repeat_times = Convert.ToInt32(tb_times_D3.Text);

            if (repeat_times > (different + 1))
            {
                MessageBox.Show("The repeat times should be not more than 1 time/hour!");
                tb_times_D3.Text = "0";
            }
        }

        private void tb_times_D4_TextChanged(object sender, EventArgs e)
        {
            DateTime start_time1 = DateTime.Parse(dateTimePicker_f_4.Text);//获取开始时间
            DateTime end_time1 = DateTime.Parse(dateTimePicker_t_4.Text);//获取结束时间

            TimeSpan ts = end_time1.Subtract(start_time1);

            int different = (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds) / 3600;
            int repeat_times = Convert.ToInt32(tb_times_D4.Text);

            if (repeat_times > (different + 1))
            {
                MessageBox.Show("The repeat times should be not more than 1 time/hour!");
                tb_times_D4.Text = "0";
            }
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
<<<<<<< HEAD
            DTP_E_ValueChanged(DTP_Start2, DTP_End2, cb_days2);
=======
            string dt1 = System.DateTime.Now.ToString("yyyy/MM/dd");
            DTP_Start.Text = dt1;
            DateTime d1 = DateTime.Parse(DTP_Start2.Text);
            DateTime d2 = DateTime.Parse(DTP_End2.Text);
            System.TimeSpan ND = d2 - d1;
            int ts1 = ND.Days + 1;   //天数差
            int ts2 = 0;
            if ((ts1 > 0) && (ts1 < 8))
            {
                ts2 = ts1;
            }
            else
            {
                MessageBox.Show("The Days should between 1 and 7.");
                ts2 = 0;
            }
            string ts = Convert.ToString(ts2);
            cb_days2.Text = ts;
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
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
                FuncBtnInit("close");
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
                FuncBtnInit("init");
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

                string ID_path = Directory.GetCurrentDirectory() + "/ID_Information.txt";
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

                //FDA文件序号最大值查询            
                string FDA_path = foldPath + "/FDA";
                string TD_path = foldPath + "/TD";
                string DC_path = foldPath + "/DC";
                string COMB_path = foldPath + "/Combination";
                cnt_FDA1 = filemaxseach(FDA_path, "*Single_Measurement" + "*Finished).txt");
                cnt_FDA2 = filemaxseach(FDA_path, "*Multiple_Measurement" + "*Finished).txt");
<<<<<<< HEAD
=======
                //MessageBox.Show(cnt_FDA2.ToString());                
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                cnt_TD = filemaxseach(TD_path, "*Single_Measurement" + "*Finished).txt");
                cnt_UIR = filemaxseach(DC_path, "*U_I_R_data" + "*Finished).txt");
                cnt_comb = filemaxseach(COMB_path, "*Combination_Measurement" + "*Finished).txt");
            }
        }

        private Int64 filemaxseach(string filepath, string filename)
        {
            char ID_file = '0';
            Int64 value = 0;

           // string FDA = foldPath + "/FDA";
            DirectoryInfo dir = new DirectoryInfo(filepath);
            FileInfo[] finfo = dir.GetFiles(filename, SearchOption.AllDirectories);
            string fnames = string.Empty;
            try
            {
                ID_file = finfo[finfo.Length - 1].Name[0];
                value = Convert.ToInt64(ID_file) - 48;
                return value;
            }
            catch
            {
                return -1;
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
            public int k;
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
            string[] filedir = Directory.GetFiles(FDA, "*Single_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string[] FDA_S = filedir;
            char ID_file = '0';
<<<<<<< HEAD

            DirectoryInfo dir = new DirectoryInfo(FDA);
            FileInfo[] finfo = dir.GetFiles("*Single_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string fnames = string.Empty;
            for (int j = 0; j < finfo.Length; j++)
            {
                FDA_S[j] = filedir[j];
                ID_file = finfo[j].Name[0];

                string[] lines = File.ReadAllLines(FDA_S[j]);

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
                        p.k = 0;
                        points.Add(p);

                        try
                        {
                            //线条添加
                            chart1.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart2.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart3.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                        }
                        catch
                        {
                        }

                        var result = VirtualCalculate(p.X, p.Y, p.Z);
                        p.X = result.Item1;
                        p.Y = result.Item2;
                        p.Z = result.Item3;
                        double real = result.Item4;
                        double img = result.Item5;

=======

            DirectoryInfo dir = new DirectoryInfo(FDA);
            FileInfo[] finfo = dir.GetFiles("*Single_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string fnames = string.Empty;
            for (int j = 0; j < finfo.Length; j++)
            {
                FDA_S[j] = filedir[j];
                ID_file = finfo[j].Name[0];

                string[] lines = File.ReadAllLines(FDA_S[j]);

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
                        p.k = 0;
                        points.Add(p);

                        try
                        {
                            //线条添加
                            chart1.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart2.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart3.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                        }
                        catch
                        {
                        }
                        int fre_int = (int)(p.X * 100);
                        p.X = fre_int / 100;

                        double real = p.Y * Math.Cos(p.Z * Math.PI / 180);
                        double img = p.Y * Math.Sin(-p.Z * Math.PI / 180);

                        int real_int = (int)(real * 100);
                        real = real_int / 100;

                        int img_int = (int)(img * 100);
                        img = img_int / 100;

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                        //画图
                        this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Y);
                        this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Z);
                        this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(real, img);
                    }
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
            string[] filedir = Directory.GetFiles(FDA, "*Multiple_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string[] FDA_M = filedir;
            char ID_file = '0';

            DirectoryInfo dir = new DirectoryInfo(FDA);
            FileInfo[] finfo = dir.GetFiles("*Multiple_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string fnames = string.Empty;
            for (int j = 0; j < finfo.Length; j++)
            {
<<<<<<< HEAD

                FDA_M[j] = filedir[j];
                ID_file = finfo[j].Name[0];

                string[] lines = File.ReadAllLines(FDA_M[j]);

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
                        p.k = int.Parse(v[3]);
                        points.Add(p);

                        try
                        {
                            //线条添加
                            chart1.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart2.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart3.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                        }
                        catch
                        {
                        }
                        var result = VirtualCalculate(p.X, p.Y, p.Z);
                        p.X = result.Item1;
                        p.Y = result.Item2;
                        p.Z = result.Item3;
                        double real = result.Item4;
                        double img  = result.Item5;

=======

                FDA_M[j] = filedir[j];
                ID_file = finfo[j].Name[0];

                string[] lines = File.ReadAllLines(FDA_M[j]);

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
                        p.k = int.Parse(v[3]);
                        points.Add(p);

                        try
                        {
                            //线条添加
                            chart1.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart2.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart3.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                        }
                        catch
                        {
                        }

                        int fre_int = (int)(p.X * 100);
                        p.X = fre_int / 100;

                        double real = p.Y * Math.Cos(p.Z * Math.PI / 180);
                        double img = p.Y * Math.Sin(-p.Z * Math.PI / 180);

                        int real_int = (int)(real * 100);
                        real = real_int / 100;

                        int img_int = (int)(img * 100);
                        img = img_int / 100;

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                        //画图
                        this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Y);
                        this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Z);
                        this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(real, img);

                    }
                }
            }
        }

        //paiting of single measurement in TD
        private void btn_td_load_Click(object sender, EventArgs e)
        {
            ChartClear();
            cnt7 = 0;
            
            btn_fre_load.Visible = true;
            btn_s.Visible = false;
            btn_m.Visible = false;

            //路径添加
            string TD = foldPath + "/TD";
<<<<<<< HEAD
=======
            //string TD_S = TD + "/0Single_Measurement.txt";
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
            string[] filedir = Directory.GetFiles(TD, "*Single_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string[] TD_S = filedir;
            char ID_file = '0';

            DirectoryInfo dir = new DirectoryInfo(TD);
            FileInfo[] finfo = dir.GetFiles("*Single_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string fnames = string.Empty; 

            for (int j = 0; j < finfo.Length; j++)
            {
                TD_S[j] = filedir[j];
                ID_file = finfo[j].Name[0];

                string[] lines = File.ReadAllLines(TD_S[j]);

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
                        p.k = 0;
                        points.Add(p);

                        try
                        {
                            //线条添加
                            chart1.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart2.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart3.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart4.Series.Add("Mag" + "(" + ID_file + ")");//添加
                            chart4.Series.Add("Pha" + "(" + ID_file + ")");//添加
                            this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart4.Series["Mag" + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart4.Series["Pha" + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;                           
                        }
                        catch
                        {
                        }
<<<<<<< HEAD

                        var result = VirtualCalculate(p.X, p.Y, p.Z);
                        p.X = result.Item1;
                        p.Y = result.Item2;
                        p.Z = result.Item3;
                        double real = result.Item4;
                        double img = result.Item5;
=======

                        int fre_int = (int)(p.X * 100);
                        p.X = fre_int / 100;

                        double real = p.Y * Math.Cos(p.Z * Math.PI / 180);
                        double img = p.Y * Math.Sin(-p.Z * Math.PI / 180);

                        int real_int = (int)(real * 100);
                        real = real_int / 100;

                        int img_int = (int)(img * 100);
                        img = img_int / 100;
>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c

                        //画图
                        this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Y);
                        this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Z);
                        this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(real, img);
                        this.chart4.Series["Mag" + "(" + ID_file + ")"].Points.AddXY(i, p.Y);
                        this.chart4.Series["Pha" + "(" + ID_file + ")"].Points.AddXY(i, p.Z);
                    }
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
            string[] filedir = Directory.GetFiles(DC, "*U_I_R_data(Experiment Finished).txt", SearchOption.AllDirectories);
            string[] DC_U = filedir;

            char ID_file = '0';

            DirectoryInfo dir = new DirectoryInfo(DC);
            FileInfo[] finfo = dir.GetFiles("*U_I_R_data(Experiment Finished).txt", SearchOption.AllDirectories);
            string fnames = string.Empty;

            for (int j = 0; j < finfo.Length; j++)
            {

                DC_U[j] = filedir[j];
                ID_file = finfo[j].Name[0];

                List<Point_U> points = new List<Point_U>();
                string[] lines = File.ReadAllLines(DC_U[j]);
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

                        try
                        {
                            chart_u_i_r.Series.Add("vol" + "(" + ID_file + ")" + (cnt7).ToString());//添加
                            this.chart_u_i_r.Series["vol" + "(" + ID_file + ")" + (cnt7).ToString()].ChartType = SeriesChartType.Point;
                            chart_u_i_r.Series.Add("cur" + "(" + ID_file + ")" + (cnt7).ToString());//添加
                            this.chart_u_i_r.Series["cur" + "(" + ID_file + ")" + (cnt7).ToString()].ChartType = SeriesChartType.Point;
                            chart_u_i_r.Series.Add("res" + "(" + ID_file + ")" + (cnt7).ToString());//添加
                            this.chart_u_i_r.Series["res" + "(" + ID_file + ")" + (cnt7).ToString()].ChartType = SeriesChartType.Point;
                        }
                        catch
                        {

                        }
                        //画图
                        this.chart_u_i_r.Series["vol" + "(" + ID_file + ")" + (cnt7).ToString()].Points.AddXY(p.X, p.Y);
                        this.chart_u_i_r.Series["cur" + "(" + ID_file + ")" + (cnt7).ToString()].Points.AddXY(p.X, p.Z);
                        this.chart_u_i_r.Series["res" + "(" + ID_file + ")" + (cnt7).ToString()].Points.AddXY(p.X, p.K);
                    }
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
            string[] filedir = Directory.GetFiles(COMB, "*Combination_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string[] Comb = filedir;
            char ID_file = '0';

            DirectoryInfo dir = new DirectoryInfo(COMB);
            FileInfo[] finfo = dir.GetFiles("*Combination_Measurement(Experiment Finished).txt", SearchOption.AllDirectories);
            string fnames = string.Empty;
            for (int j = 0; j < finfo.Length; j++)
            {               
                Comb[j] = filedir[j];
                ID_file = finfo[j].Name[0];
                string[] lines = File.ReadAllLines(Comb[j]);

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
                        p.k = int.Parse(v[3]);
                        points.Add(p);
<<<<<<< HEAD

                        try
                        {
                            //线条添加
                            chart1.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart2.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart3.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                        }
                        catch
                        {
                        }
                        var result = VirtualCalculate(p.X, p.Z, p.Z);
                        p.X = result.Item1;
                        p.Y = result.Item2;
                        p.Z = result.Item3;
                        double real = result.Item4;
                        double img = result.Item5;

=======

                        try
                        {
                            //线条添加
                            chart1.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart2.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            chart3.Series.Add((p.k).ToString() + "(" + ID_file + ")");//添加
                            this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                            this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].ChartType = SeriesChartType.Point;
                        }
                        catch
                        {
                        }

                        int fre_int = (int)(p.X * 100);
                        p.X = fre_int / 100;

                        double real = p.Y * Math.Cos(p.Z * Math.PI / 180);
                        double img = p.Y * Math.Sin(-p.Z * Math.PI / 180);

                        int real_int = (int)(real * 100);
                        real = real_int / 100;

                        int img_int = (int)(img * 100);
                        img = img_int / 100;

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
                        //画图
                        this.chart1.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Y);
                        this.chart2.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(p.X, p.Z);
                        this.chart3.Series[(p.k).ToString() + "(" + ID_file + ")"].Points.AddXY(real, img);

                    }
                }
            }          
        }

        //*********************************************//
        //****************Windows Close***************//
        //*******************************************//
        //exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(
                    "Data will be lost, are you sure to close the window?",
                    "Attention",
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) != DialogResult.OK)
            {

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

        //shut down
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

        private void lab_date_Click(object sender, EventArgs e)
        {
            datetimesend();
        }

        private void dateTimePicker_t_2_ValueChanged(object sender, EventArgs e)
        {
            if ((FDA == true) && (checkBox_SEC.Checked))
            {
                DTPWaring(dateTimePicker_t_2, DateTime.Parse(dateTimePicker_f_2.Text), false, "End time should be 1 min later than start time at least");
            }
        }

        private void dateTimePicker_f_2_ValueChanged(object sender, EventArgs e)
        {
            if ((FDA == true) && (checkBox_SEC.Checked))
            {
                DTPWaring(dateTimePicker_f_2, DateTime.Parse(dateTimePicker_t_1.Text), false, "The second start time should be 1 min later than the first end time at least");
            }       
        }        

        private void dateTimePicker_t_1_ValueChanged(object sender, EventArgs e)
        {
            if (FDA == true)
            {
                DTPWaring(dateTimePicker_t_1, DateTime.Parse(dateTimePicker_f_1.Text), false, "End time should be 1 min later than start time at least");
            }
        }

        private void dateTimePicker_f_1_ValueChanged_1(object sender, EventArgs e)
        {
            if ((FDA == true) )
            {
                DTPWaring(dateTimePicker_f_1, System.DateTime.Now, true, "Start time should be 1 min later");
            }
        }
        private void dateTimePicker_t_4_ValueChanged(object sender, EventArgs e)
        {
            if((ACM == true)&&(checkBox_SEC2.Checked))
            {
                DTPWaring(dateTimePicker_t_4, DateTime.Parse(dateTimePicker_f_4.Text), false, "End time should be 1 min later than start time at least");
            }
        }

        private void dateTimePicker_f_4_ValueChanged(object sender, EventArgs e)
        {
            if ((ACM == true) && (checkBox_SEC2.Checked))
            {
                DTPWaring(dateTimePicker_f_4, DateTime.Parse(dateTimePicker_t_3.Text), false, "The second start time should be 1 min later than the first end time at least");
            }
        }

        private void dateTimePicker_t_3_ValueChanged(object sender, EventArgs e)
        {
            if(ACM==true)
            {
                DTPWaring(dateTimePicker_t_3, DateTime.Parse(dateTimePicker_f_3.Text), false, "End time should be 1 min later than start time at least");
            }
        }

        private void dateTimePicker_f_3_ValueChanged(object sender, EventArgs e)
        {
<<<<<<< HEAD
            if (ACM == true)
            {
                DTPWaring(dateTimePicker_f_3, System.DateTime.Now, true, "Start time should be 1 min later");
            }
=======

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
            datetimesend();
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
            if ((FDA == true) && (checkBox_SEC.Checked))
            {
                DateTime end_time = DateTime.Parse(dateTimePicker_t_2.Text);
                DateTime start_time = DateTime.Parse(dateTimePicker_f_2.Text);//获取开始时间
                string tp = TimeDifference(end_time);//计算系统事件与结束时间差
                string ts = TimeDifference(start_time);//计算结束与开始时间差
                                                       // int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if ((Convert.ToUInt64(tp) - Convert.ToUInt64(ts)) < 60)
                {
                    MessageBox.Show("End time should be 1 min later than start time at least");
                    dateTimePicker_t_2.Text = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }
        }

        private void dateTimePicker_f_2_ValueChanged(object sender, EventArgs e)
        {
            if ((FDA == true) && (checkBox_SEC.Checked))
            {
                DateTime end_time = DateTime.Parse(dateTimePicker_f_2.Text);
                DateTime start_time = DateTime.Parse(dateTimePicker_t_1.Text);//获取开始时间
                string tp = TimeDifference(end_time);//计算系统事件与结束时间差
                string ts = TimeDifference(start_time);//计算结束与开始时间差
                                                       // int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if ((Convert.ToUInt64(tp) - Convert.ToUInt64(ts)) < 60)
                {
                    MessageBox.Show("The second start time should be 1 min later than the first end time at least");
                    dateTimePicker_t_3.Text = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }       
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
            if (ACM == true)
            {
                DateTime end_time = DateTime.Parse(dateTimePicker_t_1.Text);
                DateTime start_time = DateTime.Parse(dateTimePicker_f_1.Text);//获取开始时间
                string tp = TimeDifference(end_time);//计算系统事件与结束时间差
                string ts = TimeDifference(start_time);//计算结束与开始时间差
                                                       // int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if ((Convert.ToUInt64(tp) - Convert.ToUInt64(ts)) < 60)
                {
                    MessageBox.Show("End time should be 1 min later than start time at least");
                    dateTimePicker_t_1.Text = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }
        }

        private void dateTimePicker_f_1_ValueChanged_1(object sender, EventArgs e)
        {
            if ((FDA == true) )
            {
                bool limit_start_time = false;
                DateTime start_time = DateTime.Parse(dateTimePicker_f_1.Text);
                DateTime system_time1 = System.DateTime.Now;//获取系统时间
                TimeSpan ts = start_time.Subtract(system_time1);//计算系统事件与当前时间差
                int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if (TD < 60)
                {
                    MessageBox.Show("Start time should be 1 min later");
                    dateTimePicker_f_1.Text = System.DateTime.Now.AddMinutes(2).ToString("HH:mm:ss");
                }
            }
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
            if((ACM == true)&&(checkBox_SEC2.Checked))
            {
                DateTime end_time = DateTime.Parse(dateTimePicker_t_4.Text);
                DateTime start_time = DateTime.Parse(dateTimePicker_f_4.Text);//获取开始时间
                string tp = TimeDifference(end_time);//计算系统事件与结束时间差
                string ts = TimeDifference(start_time);//计算结束与开始时间差
                                                       // int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if ((Convert.ToUInt64(tp) - Convert.ToUInt64(ts)) < 60)
                {
                    MessageBox.Show("End time should be 1 min later than start time at least");
                    dateTimePicker_t_4.Text = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }
        }

        private void dateTimePicker_f_4_ValueChanged(object sender, EventArgs e)
        {
            if ((ACM == true) && (checkBox_SEC2.Checked))
            {
                DateTime end_time = DateTime.Parse(dateTimePicker_f_4.Text);
                DateTime start_time = DateTime.Parse(dateTimePicker_t_3.Text);//获取开始时间
                string tp = TimeDifference(end_time);//计算系统事件与结束时间差
                string ts = TimeDifference(start_time);//计算结束与开始时间差
                                                       // int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if ((Convert.ToUInt64(tp) - Convert.ToUInt64(ts)) < 60)
                {
                    MessageBox.Show("The second start time should be 1 min later than the first end time at least");
                    dateTimePicker_f_4.Text = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }
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
            if(ACM==true)
            { 
                DateTime end_time = DateTime.Parse(dateTimePicker_t_3.Text);
                DateTime start_time = DateTime.Parse(dateTimePicker_f_3.Text);//获取开始时间
                string tp = TimeDifference(end_time);//计算系统事件与结束时间差
                string ts = TimeDifference(start_time);//计算结束与开始时间差
               // int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if ((Convert.ToUInt64(tp)- Convert.ToUInt64(ts)) < 60)
                {
                    MessageBox.Show("End time should be 1 min later than start time at least");
                    dateTimePicker_t_3.Text = System.DateTime.Now.ToString("HH:mm:ss");
                }
            }
        }

        private void dateTimePicker_f_3_ValueChanged(object sender, EventArgs e)
        {
            if (ACM == true)
            {
                DateTime start_time = DateTime.Parse(dateTimePicker_f_3.Text);
                DateTime system_time1 = System.DateTime.Now;//获取系统时间
                TimeSpan ts = start_time.Subtract(system_time1);//计算系统事件与当前时间差
                int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;
                if (TD < 60)
                {
                    MessageBox.Show("Start time should be 1 min later");
                    dateTimePicker_f_3.Text = System.DateTime.Now.AddMinutes(2).ToString("HH:mm:ss");
                }
            }
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

        private void cb_days2_TextChanged(object sender, EventArgs e)
        {

>>>>>>> 44e73a06f50329945b55d3fc094efd7342cf1c4c
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class UnitTestClass
    {
        /// <summary>
        /// 获取三角形类型.
        /// </summary>
        /// <param name="sideArr">三角形三边长度数组.</param>
        /// <returns>返回三角形类型名称.</returns>
        public static string GetTriangle(string[] sideArr)
        {
            string result = string.Empty;
            int a = int.Parse(sideArr[0]);
            int b = int.Parse(sideArr[1]);
            int c = int.Parse(sideArr[2]);
            if (a + b > c && a + c > b && b + c > a)
            {
                if (a == b && a == c)
                {
                    result = "等边三角形";
                }

              else  if (a == b || a == c || b == c)
                {
                    result = "等腰三角形";
                }
                else
                {
                    result = "一般三角形";
                }
            }
            else
            {
                result = "不构成三角形";
            }
            return result;
        }

        //字符串定长
        public static string fest_str(string s, int n)
        {
            string feststr;
            feststr = Convert.ToInt32(s).ToString().PadLeft(n, '0');
            return feststr;
        }

        //字符串转16进制
        public static string strToHex(string s)
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
        public static byte[] exchange(byte[] array)
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
        public static string TimeToSec(string tmp1)
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
        public static string TimeDifference(DateTime Zeit)
        {
            string TD1 = "0";
            DateTime start_time1 = System.DateTime.Now;//获取系统时间
            TimeSpan ts = Zeit.Subtract(start_time1);//计算系统时间与当前时间差
            int TD = ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds;

            if (TD < 0)
            {
                TD = TD + 86400;
            }
            TD1 = (TD >= 60) ? TD.ToString() : "0";
            return TD1;
        }

        //字符串转BCDbyte
        public static byte[] strToBCDByte(string s)
        {
            Int32 BCD_Code = Convert.ToInt32(s) / 10 * 16 + Convert.ToInt32(s) % 10;
            string BCD = BCD_Code.ToString();
            byte[] result = strToHexByte(BCD);
            return result;
        }

        /// 字符串转16进制byte[]编码发送
        public static byte[] strToHexByte(string Str)
        {
            string hexString = strToHex(Str);
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString = "0" + hexString;
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2).Trim(), 16);
            return returnBytes;
        }

        //三角波发生器
        public static double trianglewav(double t, double p)

        {
            //三角波函数，p：0到1占空比
            //trianglewav(t,p)的周期为2pi
            //返回值在-1到+1间
            double y = 0 ;
            //将t归化于0到2pi区间
            if (t >= 0)
                t = (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            else
                t = 2 * Math.PI + (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            //检查占空比参数范围是否合法
            if (p < 0 | p > 1)
            {
                //MessageBox.Show("fun trianglewav error: p<0 or p>1\n");
            }
            else
            {
                p = p * 2 * Math.PI;
                //当p为0或2*pi时的近似处理
                p = (p - 2 * Math.PI == 0) ? 2 * Math.PI - 1e-10 : p;
                p = (p == 0) ? 1e-10 : p;
                y = (t < p) ? 2 * t / p - 1 : -2 * t / (2 * Math.PI - p) + (2 * Math.PI + p) / (2 * Math.PI - p);// 1上升沿，0下降沿
            }
            return y;
        }

        //方波发生器
        public static double Squarewav(double t, double p)

        {
            //三角波函数，p：0到1占空比
            //trianglewav(t,p)的周期为2pi
            //返回值在-1到+1间
            double y = 0;
            //将t归化于0到2pi区间
            if (t >= 0)
                t = (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            else
                t = 2 * Math.PI + (t / (2 * Math.PI) - (int)(t / (2 * Math.PI))) * 2 * Math.PI;
            //检查占空比参数范围是否合法
            if (p < 0 | p > 1)
            {
                //MessageBox.Show("fun trianglewav error: p<0 or p>1\n");
            }
            else
            { 
                p = p * 2 * Math.PI;
                //当p为0或2*pi时的近似处理
                p = (p - 2 * Math.PI == 0) ? 2 * Math.PI - 1e-10 : p;
                p = (p == 0) ? 1e-10 : p;
                y = (t < p) ? 1 : 0;// 1上升沿，0下降沿
            }
            return y;
        }


        public static int Main()
        {

            string input = "0212541";
            string output = "";
            double sendout = Squarewav(9,5);
            double sendout1 = trianglewav(9, 5);
            byte[] buff = strToBCDByte(input);
            for (int i = 0; i < buff.Length; i++)
            {
                output += buff[i];
            }
            Console.Write(sendout);
            Console.Write(sendout1);
            Console.ReadKey();
            return 0;
        }
    }
}

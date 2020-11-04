using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Comm
{
    public partial class NewProject : Form
    {

        //定义变量
        string path = "";                                        //文件路径
        string filename = "";                                    //文件夹名




        public NewProject()
        {
            InitializeComponent();
        }

        public string name1;
        public string Name1
        {
            get { return this.name1; }
        }


        public string save;
        public string Save
        {
            get { return this.save; }
        }

        public string desp;
        public string Desp
        {
            get { return this.desp; }
        }

        public string Parameter1;
        public string parameter1
        {
            get { return this.Parameter1; }
        }

        public string Parameter21;
        public string parameter21
        {
            get { return this.Parameter21; }
        }

        public string Parameter22;
        public string parameter22
        {
            get { return this.Parameter22; }
        }

        public string Parameter3;
        public string parameter3
        {
            get { return this.Parameter3; }
        }


        public string Parameter4;
        public string parameter4
        {
            get { return this.Parameter4; }
        }

        public string pathString1;
        public string PathString1
        {
            get { return this.pathString1; }
        }

        public string pathString2;
        public string PathString2
        {
            get { return this.pathString2; }
        }

        public string pathString3;
        public string PathString3
        {
            get { return this.pathString3; }
        }

        public string pathString4;
        public string PathString4
        {
            get { return this.pathString4; }
        }

        public string Single_m;
        public string single_m
        {
            get { return this.Single_m; }
        }

        public string Multiple_m;
        public string multiple_m
        {
            get { return this.Multiple_m; }
        }

        public string Single_m1;
        public string single_m1
        {
            get { return this.Single_m1; }
        }

        public string U_I_R;
        public string u_i_r
        {
            get { return this.U_I_R; }
        }

        public string Combination_M;
        public string combination_M
        {
            get { return this.Combination_M; }
        }

        public string ID_Num;
        public string Id_num
        {
            get { return this.ID_Num; }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            name1 = this.tb_name.Text;
            save = this.tb_save.Text;
            desp = this.tb_descrip.Text;
            this.DialogResult = DialogResult.OK;

            filename = path + "\\"+  this.tb_name.Text ;    //组合路径
            Directory.CreateDirectory(filename);              //创建文件夹


            //*********************************************//
            //*******************FDA**********************//
            //*******************************************//

            string filename1 = "FDA";
            pathString1 = System.IO.Path.Combine(filename, filename1);
            if (!Directory.Exists(pathString1))
            {
                Directory.CreateDirectory(pathString1);
            }
 


            Parameter21 = pathString1 + "/Parameter_s.txt";
            if (!File.Exists(Parameter21))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Parameter21);
                sw.WriteLine("ProjectName:" + tb_name.Text + "\n" + "Save as:" + tb_save.Text + "\n" + "Description:" + tb_descrip.Text);
                sw.Flush();
                sw.Close();
            }
            Parameter22 = pathString1 + "/Parameter_m.txt";
            if (!File.Exists(Parameter22))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Parameter22);
                sw.WriteLine("ProjectName:" + tb_name.Text + "\n" + "Save as:" + tb_save.Text + "\n" + "Description:" + tb_descrip.Text);
                sw.Flush();
                sw.Close();
            }


            Single_m = pathString1 + "/0Single_Measurement.txt";
            if (!File.Exists(Single_m))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Single_m);
                sw.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                sw.Flush();
                sw.Close();
            }


            Multiple_m = pathString1 + "/0Multiple_Measurement.txt";
            if (!File.Exists(Multiple_m))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Multiple_m);
                sw.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                sw.Flush();
                sw.Close();
            }
 




            //*********************************************//
            //*******************TD***********************//
            //*******************************************//

            string filename2 = "TD";
            pathString2 = System.IO.Path.Combine(filename, filename2);
            if (!Directory.Exists(pathString2))
            {
                Directory.CreateDirectory(pathString2);
            }

            Parameter1 = pathString2 + "/Parameter.txt";
            if (!File.Exists(Parameter1))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Parameter1);
                sw.WriteLine("ProjectName:" + tb_name.Text + "\n" + "Save as:" + tb_save.Text + "\n" + "Description:" + tb_descrip.Text);
                sw.Flush();
                sw.Close();
            }

            Single_m1 = pathString2 + "/0Single_Measurement.txt";
            if (!File.Exists(Single_m1))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Single_m1);
                sw.WriteLine("Time" +  "\t" + "Impendence" );
                sw.Flush();
                sw.Close();
            }




            //*********************************************//
            //*******************DC***********************//
            //*******************************************//


            string filename3 = "DC";
            pathString3 = System.IO.Path.Combine(filename, filename3);
            if (!Directory.Exists(pathString3))
            {
                Directory.CreateDirectory(pathString3);
            }

            Parameter3 = pathString3 + "/Parameter.txt";
            if (!File.Exists(Parameter3))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Parameter3);
                sw.WriteLine("ProjectName:" + tb_name.Text + "\n" + "Save as:" + tb_save.Text + "\n" + "Description:" + tb_descrip.Text);
                sw.Flush();
                sw.Close();
            } 

            U_I_R = pathString3 + "/0U_I_R_data.txt";
            if (!File.Exists(U_I_R))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(U_I_R);
                sw.WriteLine("Time" + "\t" + "Voltage" + "\t" + "Current" + "\t" + "Resistance");
                sw.Flush();
                sw.Close();
            }



            //*********************************************//
            //*******************Combination***********************//
            //*******************************************//

            string filename4 = "Combination";
            pathString4 = System.IO.Path.Combine(filename, filename4);
            if (!Directory.Exists(pathString4))
            {
                Directory.CreateDirectory(pathString4);
            }

            Parameter4 = pathString4 + "/Parameter.txt";
            if (!File.Exists(Parameter4))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Parameter4);
                sw.WriteLine("ProjectName:" + tb_name.Text + "\n" + "Save as:" + tb_save.Text + "\n" + "Description:" + tb_descrip.Text);
                sw.Flush();
                sw.Close();
            }

            Combination_M = pathString4 + "/0Combination_Measurement.txt";
            if (!File.Exists(Combination_M))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Combination_M);
                sw.WriteLine("Fre" + "\t" + "Mag" + "\t" + "Pha");
                sw.Flush();
                sw.Close();
            }




            string Read_path = filename + "/ReadME.txt";
            if (!File.Exists(Read_path))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(Read_path);
                sw.WriteLine("ProjectName:" +  tb_name.Text + "\n" + "Save as:" + tb_save.Text + "\n" + "Description:" + tb_descrip.Text);
                sw.Flush();
                sw.Close();
            }






            
            Random rd = new Random();

            int i = 0;
            for (i = 0; i < 8; i++)
            {
                ID_Num += rd.Next(0, 10).ToString();
            }

            Directory.SetCurrentDirectory(Directory.GetParent(filename).FullName);

            String ID_path = Directory.GetCurrentDirectory() + "/ID_Information.txt"; ;


            if (!File.Exists(ID_path))
            {
                //FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite);

                StreamWriter sw = new StreamWriter(ID_path);


                sw.WriteLine("ID Number" + "\t" + "ProjectName" + "\t" + "Save as:" + "\t" + "Description");
                sw.WriteLine(ID_Num + "\t" + tb_name.Text + "\t" + tb_save.Text + "\t" + tb_descrip.Text);

                sw.Flush();
                sw.Close();
            }
            else
            {
                FileStream fs = new FileStream(ID_path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.WriteLine(ID_Num + "\t" + tb_name.Text + "\t" + tb_save.Text + "\t" + tb_descrip.Text);
                sw.Flush();
                sw.Close();
                fs.Close();

            }

        }

        private void btn_SaveFile_Click(object sender, EventArgs e)
        {
            DialogResult result = fdb.ShowDialog();
            if (result == DialogResult.OK)
            {
                path = fdb.SelectedPath;  //获取文件路径
                tb_save.Text = path;                         //.......
            }
        }
    }
}

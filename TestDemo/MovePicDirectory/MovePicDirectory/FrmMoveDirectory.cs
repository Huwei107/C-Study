using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
using System.IO;

namespace MovePicDirectory
{
    public partial class FrmMoveDirectory : Form
    {
        private string dir = string.Empty;
        PicEntity entity = new PicEntity();
        string filepath = "D:\\FTP\\localuser\\";
        int count = 0;
        private static object ojbLock = new object();
        private static bool blRunFlag = false;

        public FrmMoveDirectory()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            blRunFlag = true;
            this.txtinfo.ReadOnly = true;
            //ExceuteMovePic();
            //timer = new System.Timers.Timer(10000);
            //timer.Elapsed += new System.Timers.ElapsedEventHandler(ExceuteMovePic_Time);//timer定时事件绑定Update方法
            //timer.AutoReset = true;//设置一直循环调用；若设置timer.AutoReset = false;只调用一次绑定方法
            //timer.Start();//开启定时器事件或者写成timer.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (blRunFlag)
            {
                ExceuteMovePic();
            }
        }

        private void ExceuteMovePic_Time(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (blRunFlag)
            {
                ExceuteMovePic();
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            this.btnStart.Enabled = true;
            this.btnStop.Enabled = false;
            this.txtDir.ReadOnly = false;
            this.txtNum.ReadOnly = false;
            this.txtTime.ReadOnly = false;
            this.btnSelectDir.Enabled = true;
            this.txtinfo.Text = "";
            //timer.Stop();
            timer1.Enabled = false;
        }

        private void ExceuteMovePic()
        {
            lock (ojbLock)
            {
                blRunFlag = false;
            };
            this.btnStart.Enabled = false;
            this.btnStop.Enabled = true;
            this.txtDir.ReadOnly = true;
            this.txtNum.ReadOnly = true;
            this.txtTime.ReadOnly = true;
            this.btnSelectDir.Enabled = false;
            if (string.IsNullOrEmpty(txtDir.Text.Trim()))
            {
                MessageBox.Show("请先输入要移动的文件目录！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtTime.Text.Trim()))
            {
                MessageBox.Show("请输入时间差！", "提示");
                return;
            }
            if (string.IsNullOrEmpty(txtNum.Text.Trim()))
            {
                MessageBox.Show("请输入每次执行的图片数量！", "提示");
                return;
            }
            if (!Common.DataValidate.IsInteger(txtTime.Text.Trim()))
            {
                MessageBox.Show("请输入数字！", "提示");
                return;
            }
            if (!Common.DataValidate.IsInteger(txtNum.Text.Trim()))
            {
                MessageBox.Show("请输入数字！", "提示");
                return;
            }
            string nowDate = DateTime.Now.ToString("yyyy-MM-dd");
            DataSet ds = entity.MovePic(nowDate, txtTime.Text.Trim(), txtNum.Text.Trim());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                string msgReturn = MoveDirectory(ds, dir);
                if (!string.IsNullOrEmpty(msgReturn))
                {
                    txtinfo.Text = msgReturn;
                }
            }
            else
            {
                txtinfo.Text = string.Format("数据库里没有找到合适的图片！");
            }
            lock (ojbLock)
            {
                blRunFlag = true;
            };
        }

        private string MoveDirectory(DataSet sourceDir, string targetDir)
        {
            string msg = string.Empty;
            if (!Directory.Exists(targetDir))
            {
                Directory.CreateDirectory(targetDir);//创建目标文件夹
            }
            try
            {
                foreach (DataRow drDir in sourceDir.Tables[0].Rows)
                {
                    string type = IsType(drDir["TYPE"].ToString()) + "\\";//源分类文件夹
                    string wholePath = filepath + type + drDir["Path"].ToString();
                    string targetPath = targetDir + "\\" + drDir["TYPE"].ToString() + "\\" + Path.GetDirectoryName(drDir["Path"].ToString());
                    if (!Directory.Exists(targetPath))
                    {
                        Directory.CreateDirectory(targetPath);
                    }
                    string targetPic = targetDir + "\\" + drDir["TYPE"].ToString() + "\\" + drDir["Path"].ToString();
                    if (File.Exists(targetPic))
                    {//判断目标文件是否已经存在此图片
                        return msg;
                    }
                    FileInfo file = new FileInfo(wholePath);
                    file.CopyTo(targetPic, true);
                    msg = entity.ModifySourcePic(targetPic, drDir["LOT_NUMBER"].ToString(), drDir["TYPE"].ToString());
                    txtinfo.Text = string.Format("已拷贝了{0}张图片, 正在执行...", ++count);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return msg;
        }

        private void btnSelectDir_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dilog = new FolderBrowserDialog();
            dilog.Description = "请选择文件夹";
            if (dilog.ShowDialog() == DialogResult.OK)
            {
                txtDir.Text = dilog.SelectedPath;
                dir = dilog.SelectedPath;
            }
        }

        private string IsType(string type)
        {
            if (type.Equals("CBoxImagePath"))
                return "cbox";
            if (type.Equals("ELImagePath"))
                return "hel";
            if (type.Equals("GJWGImagePath"))
                return "gjwg";
            if (type.Equals("IVDiagramImagePath"))
                return "ivpic";
            if (type.Equals("MELImagePath") || type.Equals("MWELImagePath"))
                return "cyhel";
            if (type.Equals("QELImagePath"))
                return "qel";
            return "没有数据";
        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ThreadDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(3000);
            MessageBox.Show("素材做好了", "提示");
            Thread.Sleep(5000);
            MessageBox.Show("荤菜做好了", "提示");
        }

        /// <summary>
        /// 利用Thread进行多线程开发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(() => {
                Thread.Sleep(3000);
                MessageBox.Show("素菜做好了", "提示");
                Thread.Sleep(5000);
                MessageBox.Show("荤菜做好了", "提示");
            });
            t.Start();
        }

        /// <summary>
        /// 利用Task进行多线程开发，底层也是Thread，但提供很多线程的管理，比如线程池，所以性能更好，管理更方便
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            Task.Run(() => {
                Thread.Sleep(3000);
                MessageBox.Show("素菜做好了", "提示");
                Thread.Sleep(5000);
                MessageBox.Show("荤菜做好了", "提示");
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                Thread.Sleep(3000);
                MessageBox.Show("素菜做好了", "提示");
            });
            Task.Run(() =>
            {
                Thread.Sleep(5000);
                MessageBox.Show("荤菜做好了", "提示");
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //await Task.Run(() =>
            //{
            //    Thread.Sleep(3000);
            //    MessageBox.Show("素菜做好了", "提示");
            //    Thread.Sleep(5000);
            //    MessageBox.Show("荤菜做好了", "提示");
            //});
            //MessageBox.Show("菜都做好了，大家快来吃饭！", "提示");

            List<Task> ts = new List<Task>();
            ts.Add(Task.Run(() =>
            {
                Thread.Sleep(3000);
                MessageBox.Show("素菜做好了", "提示");
            }));
            ts.Add(Task.Run(() =>
            {
                Thread.Sleep(5000);
                MessageBox.Show("荤菜做好了", "提示");
            }));

            Task.WhenAll(ts).ContinueWith(t =>
            {
                MessageBox.Show("菜都做好了，大家快来吃饭！", "提示");
            });
            
        }
    }
}

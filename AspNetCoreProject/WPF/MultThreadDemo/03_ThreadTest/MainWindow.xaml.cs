using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _03_ThreadTest
{
  
    /// <summary>
    /// 线程特点：在具有多个cpu的计算机中，可以并行执行
    /// Thread类：表示托管线程，每个Thread对象都代表一个托管线程，每个托管线程都会对应一个函数
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 执行任务1
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int a = 0;
            Thread objThread = new Thread(delegate ()
            {
                for (int i = 0; i < 20; i++)
                {
                    a += i;
                    Console.WriteLine((a) + "    ");
                    Thread.Sleep(500);
                }
            });
            objThread.IsBackground = true;//设置为后台线程
            objThread.Start();
        }

        /// <summary>
        /// 执行任务2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }


    }
}

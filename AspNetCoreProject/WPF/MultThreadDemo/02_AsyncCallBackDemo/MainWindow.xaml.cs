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

namespace _02_AsyncCallBackDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.MyCalculator = ExcuteTask;//初始化
            //发布任务
            for (int i = 0; i < 11; i++)
            {
                //开始异步执行
                this.MyCalculator.BeginInvoke(10 * i, 1000 * i, MyCallBack, i);
                //最后一个参数i，给回调函数字段AsyncState赋值，如果数据很多可以定义成类或结构
            }
        }

        private void MyCallBack(IAsyncResult asyncResult)
        {
            int res = this.MyCalculator.EndInvoke(asyncResult);
            //显示异步调用结果
            //asyncResult.AsyncState用来封装回调函数自定义参数，object类型
            Console.WriteLine($"第{asyncResult.AsyncState}个计算结果为{res}");
        }

        /// <summary>
        /// 根据委托定义一个方法：返回一个数的平方
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        private int ExcuteTask(int num, int ms)
        {
            Thread.Sleep(ms);
            return num * num;
        }

        private Func<int, int, int> MyCalculator;
    }
}

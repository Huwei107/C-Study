using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace 委托事件
{
    /// <summary>
    /// SonWin.xaml 的交互逻辑
    /// </summary>
    public partial class SonWin : Window
    {
        //先定义委托
        public delegate void del();     //public delegate 返回值类型 del(参数)

        public delegate int del1(int i, string str);

        //public del Dele;//委托类型属性    接收主窗口传过来的方法

        //系统委托/预置委托     Action、Func
        public event Action _Dele;

        public Func<int, int, string> Dele1;//第一个是返回值，第二开始是参数，如果方法没有参数就没有第二个

        //事件：特殊的委托 
        //委托、事件的区别：(安全性)
        //1.使用位置不同，事件只能在本类内部触发，委托不管在本类型内部还是外部都可以调用
        //2.使用符号不同，在类外部，事件只能用“+=”和“-=”去订阅/取消订阅，如果是委托还可以用“=”

        public SonWin(Action Dele)
        {
            InitializeComponent();
            _Dele = Dele;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_Dele != null)
            {
                _Dele();
            }
        }
    }
}

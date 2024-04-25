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
        public Action Dele;



        public SonWin()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Dele != null)
            {
                Dele();
            }
        }
    }
}

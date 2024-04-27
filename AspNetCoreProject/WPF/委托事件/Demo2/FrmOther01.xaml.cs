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

namespace Demo2
{
    /// <summary>
    /// FrmOther01.xaml 的交互逻辑
    /// </summary>
    public partial class FrmOther01 : Window
    {
        public FrmOther01()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 接收委托传递的信息
        /// </summary>
        /// <param name="counter"></param>
        public void Receiver(string counter)
        {
            this.lbCount.Text = counter;
        }
    }
}

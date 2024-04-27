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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Demo2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FrmOther01 frmOther01 = new FrmOther01();
            FrmOther02 frmOther02 = new FrmOther02();
            FrmOther03 frmOther03 = new FrmOther03();

            //将委托变量和具体方法关联
            this.ShowCounter += frmOther01.Receiver;
            this.ShowCounter += frmOther02.Receiver;
            this.ShowCounter += frmOther03.Receiver;
            frmOther01.Show();
            frmOther02.Show();
            frmOther03.Show();

        }

        //声明委托
        public Action<string> ShowCounter;

        private int count = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            count++;
            //同时对三个从窗体发消息
            this.ShowCounter?.Invoke(count.ToString());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.ShowCounter?.Invoke("0");
        }
    }

    
}

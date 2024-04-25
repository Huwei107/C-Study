using OAManage.Model;
using OAManage.ViewModels;
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

namespace OAManage
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //设置数据上下文（视图模型）
            accountViewModel = new AccountViewModel();
            this.DataContext = accountViewModel;
        }

        //问题：界面上修改了，后台能接收，后台改了，界面不知道（界面没有收到通知）
        private AccountViewModel accountViewModel;
    }
}

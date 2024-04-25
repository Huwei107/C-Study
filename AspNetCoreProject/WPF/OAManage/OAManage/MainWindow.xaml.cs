using OAManage.Model;
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

            //设置数据上下文
            accountModel = new AccountModel();
            this.DataContext = accountModel;
        }
        /// <summary>
        /// 账号
        /// </summary> 
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd { get; set; }
        //问题：界面上修改了，后台能接收，后台改了，界面不知道（界面没有收到通知）
        private AccountModel accountModel;

        /// <summary>
        /// 登录，单击执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Login(object sender, RoutedEventArgs e)
        {
            //前后耦合大
            //string account = txtAccount.Text;
            //string password = txtPassword.Password;
            if(accountModel.Account == "admin" && accountModel.Pwd == "123")
            {
                MessageBox.Show("登录成功！");
            }
            else
            {
                MessageBox.Show("登录失败！");
                //清空账号密码
                accountModel.Pwd = string.Empty;//后台清空
            }
        }
    }
}

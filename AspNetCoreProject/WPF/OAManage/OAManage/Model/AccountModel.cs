using OAManage.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OAManage.Model
{
    /// <summary>
    /// 绑定登录模型
    /// </summary>
    public class AccountModel : INotifyPropertyChanged //属性改变的通知接口
    {
        /// <summary>
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 账号
        /// </summary>
        private string _Account { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get { return _Account; }
            set
            {
                _Account = value;
                //通知
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Account"));
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Account"));
                }
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        private string _Pwd;
        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get { return _Pwd; }
            set
            {
                _Pwd = value;
                //通知
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Pwd"));
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Pwd"));
                }
            }
        }



        /// <summary>
        /// 登录
        /// </summary>
        private void Login()
        {
            if (this.Account == "admin" && this.Pwd == "123")
            {
                MessageBox.Show("登录成功！");
            }
            else
            {
                MessageBox.Show("登录失败！");
                //清空账号密码
                this.Pwd = string.Empty;//后台清空
            }
        }

        /// <summary>
        /// 命令属性
        /// 实体把登录这个方法委托给命令，前端只能绑定命令
        /// 委托把方法作为参数
        /// </summary>
        public ICommand LoginCommand
        {
            get { return new DoCommand(Login); }
        }
    }
}

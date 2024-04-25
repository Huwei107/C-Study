using OAManage.Command;
using OAManage.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace OAManage.ViewModels
{
    public class AccountViewModel : INotifyPropertyChanged  //属性改变的通知接口
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private AccountModel _accountModel = new AccountModel();

        /// <summary>
        /// 账号
        /// </summary>
        public string Account
        {
            get
            {
                return _accountModel.Account;
            }
            set
            {
                _accountModel.Account = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Account"));
                }
            }
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string Pwd
        {
            get
            {
                return _accountModel.Pwd;
            }
            set
            {
                _accountModel.Pwd = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Pwd"));
                }
            }
        }

        /// <summary>
        /// 用户属性，开放给视图V绑定
        /// </summary>
        //public AccountModel AccountModel
        //{
        //    get
        //    {
        //        return _accountModel;
        //    }
        //    set
        //    {
        //        _accountModel = value;
        //        if (PropertyChanged != null)
        //        {
        //            PropertyChanged(this, new PropertyChangedEventArgs("AccountModel"));
        //        }
        //    }
        //}

        /// <summary>
        /// 登录
        /// </summary>
        private void Login()
        {
            if (Account == "admin" && Pwd == "123")
            {
                MessageBox.Show("登录成功！");
            }
            else
            {
                MessageBox.Show("登录失败！");
                //清空账号密码
                Account = string.Empty;
                Pwd = string.Empty;
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

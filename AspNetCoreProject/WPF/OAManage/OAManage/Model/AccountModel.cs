using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OAManage.Model
{
    public class AccountModel : INotifyPropertyChanged //属性改变的通知接口
    {
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
        /// 属性改变事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
    }
}

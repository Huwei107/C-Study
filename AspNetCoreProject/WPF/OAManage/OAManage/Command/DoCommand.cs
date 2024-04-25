using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OAManage.Command
{
    public class DoCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// 要执行的事 
        /// </summary>
        private Action _excute;//委托

        /// <summary>
        /// 构造函数
        /// </summary>
        public DoCommand(Action excute)//程序一执行就初始化了这个命令
        {
            _excute = excute;
        }

        /// <summary>
        /// 能否执行
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object? parameter)
        {//假设一直能执行
            return true;
        }

        /// <summary>
        /// 执行的事情
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object? parameter)
        {
            if (_excute != null)
            {
                _excute();
            }
        }
    }
}

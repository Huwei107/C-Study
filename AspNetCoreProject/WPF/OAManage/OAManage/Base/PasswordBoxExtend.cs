using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace OAManage.Base
{
    /// <summary>
    /// 自定义PasswordBox属性
    /// </summary>
    public class PasswordBoxExtend
    {

        /// <summary>
        /// MyPwd：自定义属性名
        /// 附加属性(特殊的依赖属性)
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetMyPwd(DependencyObject obj)
        {
            return (string)obj.GetValue(MyPwdProperty);
        }

        /// <summary>
        /// MyPwd属性赋值
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="value"></param>
        public static void SetMyPwd(DependencyObject obj, string value)
        {
            obj.SetValue(MyPwdProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPwdProperty =
            DependencyProperty.RegisterAttached("MyPwd", typeof(string), typeof(PasswordBoxExtend)
                ,new PropertyMetadata("",OnMyPwdChanged));// new PropertyMetadata(0)：默认值，可不加        

        /// <summary>
        /// MyPwd -> Password 当MyPwd属性发生改变，给Password属性赋值
        /// </summary>
        /// <param name="d"></param>
        /// <param name="e"></param>
        private static void OnMyPwdChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;
            if (passwordBox != null)
            {
                passwordBox.Password = e.NewValue.ToString();
            }
        }
    }
}

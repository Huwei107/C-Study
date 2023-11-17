//-----------------------------------------------------------------------
// <copyright company="工品一号" file="NumberHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 18:11:36
//  功能描述:   数字操作集合(常用操作数字方法)
//  历史版本:
//          2017-2-13 18:11:36 刘少林 创建NumberHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FX.MainForms
{
    /// <summary>
    /// 数字操作集合(常用操作数字方法)
    /// </summary>
    public class HelperVerificationCode
    {
        /// <summary>
        /// 生成随机的字符串
        /// </summary>
        public static string CreateRandomCode(int codeCount)
        {
            string allChar = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,W,X,Y,Z";
            //将allChar这个字符串用逗号“，”进行分割 ；分割后的字符串数组放在 allCharArray[] 中
            string[] allCharArray = allChar.Split(',');
            string randomCode = "";
            int temp = -1;
            Random rand = new Random();
            //字符串长度为 codeCount，比如：5 位 codeCount =5； 循环 5遍，每次生成一个数字，作为数组的下标
            for (int i = 0; i < codeCount; i++)
            {
                if (temp != -1)
                {
                    /*  Random(参数) 中的参数名为：“种子值” 作用：
                        计算机的“随机数”并非真的“随机数”，而是伪随机数——也就是通过一系列算法，从一个起始数字开始按照一定规则算出来的。 
                        你可以做个测试：把你的"new Random(x)"里面的x设置成一个固定数字，例如1，多次运行程序打印随机数，你会发现每次运行打印出来的数字都是一样的。
                        Random(i * temp * ((int)DateTime.Now.Ticks)) 使用指定的“种子值”初始化 Random 类的新实例。
                    */
                    //种子值 == i=当前循环次数 * temp=上次循环生成的随机数 * 参数((int)DateTime.Now.Ticks)=获取表示此实例的日期和时间的刻度数；
                    rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
                }
                //返回一个小于所指定最大值‘35’非负数的随机数
                int t = rand.Next(35);
                if (temp == t)
                {
                    return CreateRandomCode(codeCount);
                }
                temp = t;
                randomCode += allCharArray[t];
            }
            return randomCode;
        }
    }
}

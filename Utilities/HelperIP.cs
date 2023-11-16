//-----------------------------------------------------------------------
// <copyright company="工品一号" file="IPHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 16:57:03
//  功能描述:   IP(IPv4格式)地址扩展操作集
//  历史版本:
//          2017-2-13 16:57:03 刘少林 创建IPHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Management;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace FX.MainForms
{
    /// <summary>
    /// IP(IPv4格式)地址扩展操作集
    /// </summary>
    public class HelperIP
    {
        /// <summary>
        /// 获取IP地址
        /// </summary>
        /// <remarks>暂时只用于wins操作系统,暂时只使用win窗体程序,WEB程序暂未测试!</remarks>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            string stringIp = string.Empty;
            var managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
            foreach (var o in managementObjectCollection)
            {
                var managementObject = (ManagementObject)o;
                if ((bool)managementObject["IPEnabled"] == true)
                {
                    string[] ipAddresses = (string[])managementObject["IPAddress"];
                    if (ipAddresses.Length > 0)
                    {
                        stringIp = ipAddresses[0];
                    }
                }
            }
            return stringIp;
        }

        /// <summary>
        ///  得到主机名  
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return  Dns.GetHostName();
        }

        /// <summary>
        /// 获取MAC地址
        /// </summary>
        /// <remarks>暂时只用于wins操作系统,暂时只使用win窗体程序,WEB程序暂未测试!</remarks>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            string stringMac = string.Empty;
            var managementClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
            foreach (var o in managementObjectCollection)
            {
                var managementObject = (ManagementObject)o;
                if ((bool)managementObject["IPEnabled"] == true)
                {
                    stringMac += managementObject["MACAddress"].ToString();
                }
            }
            return stringMac;
        }

        /// <summary>
        /// 判断是否是ip地址(IPv4格式)格式 0.0.0.0
        /// </summary>
        /// <param name="ip">待判断的ip地址(IPv4格式)</param>
        /// <returns>布尔值,true为ip地址(IPv4格式),false非ip地址(非IPv4格式)</returns>
        public static bool IsIPv4(string ip)
        {
            Regex ipReg = new Regex(RegexFormat.IPv4RegexFormat);
            ip = ip.Trim();
            if (ipReg.IsMatch(ip))
            {
                string[] s = ip.Split('.');
                foreach (string g in s)
                {
                    if (HelperConvert.StrToInt(g, 256) > 255)
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }//end method

        /// <summary>
        /// 将ip(IPv4格式)转成长整型
        /// </summary>
        /// <param name="ip">待转换ip地址</param>
        /// <returns>转换后的长整型数值</returns>
        public static long IPv4StrToLong(string ip)
        {
            if (IsIPv4(ip))
            {
                string[] _iparr = ip.Split('.');
                long r0 = HelperConvert.StrToLong(_iparr[0].ToString(), 0L) * 16777216L;
                long r1 = HelperConvert.StrToLong(_iparr[1].ToString(), 0L) * 65536L;
                long r2 = HelperConvert.StrToLong(_iparr[2].ToString(), 0L) * 256L;
                long r3 = HelperConvert.StrToLong(_iparr[3].ToString(), 0L);
                return r0 + r1 + r2 + r3;
            }
            else
            {
                return 0;
            }
        }//end method

        /// <summary>
        /// 将ip由整型转为字符串型
        /// </summary>
        /// <param name="ip">整型ip数据</param>
        /// <returns>字符串型的ip地址</returns>
        public static string LongToIPv4Str(long ip)
        {
            long tempIPAddress;
            //将目标整形数字intIPAddress转换为IP地址字符串
            //-1062731518 192.168.1.2 
            //-1062731517 192.168.1.3 
            if (ip >= 0)
            {
                tempIPAddress = ip;
            }
            else
            {
                tempIPAddress = ip + 1;
            }
            long s1 = tempIPAddress / 256 / 256 / 256;
            long s21 = s1 * 256 * 256 * 256;
            long s2 = (tempIPAddress - s21) / 256 / 256;
            long s31 = s2 * 256 * 256 + s21;
            long s3 = (tempIPAddress - s31) / 256;
            long s4 = tempIPAddress - s3 * 256 - s31;
            if (ip < 0)
            {
                s1 = 255 + s1;
                s2 = 255 + s2;
                s3 = 255 + s3;
                s4 = 255 + s4;
            }
            string strIPAddress = s1.ToString() + "." + s2.ToString() + "." + s3.ToString() + "." + s4.ToString();
            return strIPAddress;
        }//end method


        /// <summary>
        /// 取本主机ip地址
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIp()
        {
            ///获取本地的IP地址
            string AddressIP = string.Empty;
            foreach (IPAddress _IPAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (_IPAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    AddressIP = _IPAddress.ToString();
                }
            }
            return AddressIP;
        }
    }//end class
}//end namespace

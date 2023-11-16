//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperMachineInfo.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-14 8:42:17
//  功能描述:   获取windows系统硬件信息
//  历史版本:
//          2017-2-14 8:42:17 刘少林 创建HelperMachineInfo类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
namespace FX.MainForms
{
    /// <summary>
    /// 描述操作系统构成单元的对象类型枚举
    /// </summary>
    /// <remarks>
    /// WMI，是Windows 2K/XP管理系统的核心；对于其他的Win32操作系统，WMI是一个有用的插件。WMI以CIMOM为基础，
    /// CIMOM即公共信息模型对象管理器（Common Information Model Object Manager），是一个描述操作系统构成单元的对象数据库，
    /// 为MMC和脚本程序提供了一个访问操作系统构成单元的公共接口。有了WMI，
    /// 工具软件和脚本程序访问操作系统的不同部分时不需要使用不同的API
    /// </remarks>
    internal enum WmiType
    {
        Win32_Processor,
        Win32_PerfFormattedData_PerfOS_Memory,
        Win32_PhysicalMemory,
        Win32_NetworkAdapterConfiguration,
        Win32_LogicalDisk
    }

    /// <summary>
    /// 获取硬盘号和CPU号
    /// </summary>
    public class HelperMachineInfo
    {
        static Dictionary<string, ManagementObjectCollection> WmiDict = new Dictionary<string, ManagementObjectCollection>();

        /// <summary>
        /// 静态构造函数
        /// </summary>
        static HelperMachineInfo()
        {
            var names = Enum.GetNames(typeof(WmiType));
            foreach (string name in names)
            {
                WmiDict.Add(name, new ManagementObjectSearcher("SELECT * FROM " + name).Get());
            }
        }

        /// <summary>
        /// 获取硬盘号码
        /// </summary>
        /// <returns></returns>
        public static string GetHardDiskNumber()
        {
            var query = WmiDict[WmiType.Win32_LogicalDisk.ToString()];
            //var collection = query.Get();

            string result = string.Empty;
            foreach (var obj in query)
            {
                result = obj["VolumeSerialNumber"].ToString();
                break;
            }

            return result;
        }

        /// <summary>
        /// 获取CPU号码
        /// </summary>
        /// <returns></returns>
        public static string GetCPUNumber()
        {
            var query = WmiDict[WmiType.Win32_Processor.ToString()];
            //var collection = query.Get();

            string result = string.Empty;
            foreach (var obj in query)
            {
                result = obj["Processorid"].ToString();
                break;
            }

            return result;
        }

        /// <summary>
        /// 获取内存编号
        /// </summary>
        /// <returns></returns>
        public static string GetMemoryNumber()
        {
            var query = WmiDict[WmiType.Win32_PhysicalMemory.ToString()];
            //var collection = query.Get();

            string result = string.Empty;
            foreach (var obj in query)
            {
                result = obj["PartNumber"].ToString();
                break;
            }
            return result;
        }

        /// <summary>
        /// 获取硬盘信息
        /// </summary>
        /// <returns></returns>
        public static string HardDiskInfo()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            StringBuilder sr = new StringBuilder();
            foreach (DriveInfo drive in drives)
            {
                if (drive.IsReady)
                {
                    var val1 = (double)drive.TotalSize / 1024 / 1024;
                    var val2 = (double)drive.TotalFreeSpace / 1024 / 1024;
                    sr.AppendFormat("{0}:{2}/{3}MB/{4}MB/{1}%可用;",
                        drive.Name,
                        string.Format("{0:F2}", val2 / val1 * 100),
                        drive.DriveFormat,
                        (long)val1,
                        (long)val2);
                }
            }
            return sr.ToString();
        }

        /// <summary>
        /// 获取操作系统信息
        /// </summary>
        /// <returns></returns>
        public static string OSInfo()
        {
            StringBuilder sr = new StringBuilder();
            sr.AppendFormat("机器名:{0};操作系统:{1};系统文件夹:{2};语言:{3};.NET:{4};当前目录:{5};当前用户:{6};",
                Environment.MachineName,
                Environment.OSVersion,
                Environment.SystemDirectory,
                CultureInfo.InstalledUICulture.EnglishName,
                Environment.Version,
                Environment.CurrentDirectory,
                Environment.UserName);
            return sr.ToString();
        }

        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        public static string MemoryInfo()
        {
            StringBuilder sr = new StringBuilder();
            long capacity = 0;
            var query = WmiDict[WmiType.Win32_PhysicalMemory.ToString()];
            int index = 1;
            foreach (var obj in query)
            {
                sr.Append("内存" + index + "频率:" + obj["ConfiguredClockSpeed"] + ";");
                capacity += Convert.ToInt64(obj["Capacity"]);
                index++;
            }
            sr.Append("总物理内存:");
            sr.Append(capacity / 1024 / 1024 + "MB;");

            query = WmiDict[WmiType.Win32_PerfFormattedData_PerfOS_Memory.ToString()];
            sr.Append("总可用内存:");
            long available = 0;
            foreach (var obj in query)
            {
                available += Convert.ToInt64(obj.Properties["AvailableMBytes"].Value);
            }
            sr.Append(available + "MB;");
            sr.AppendFormat("{0:F2}%可用; ", (double)available / (capacity / 1024 / 1024) * 100);

            return sr.ToString();
        }

        /// <summary>
        /// 获取CPU信息
        /// </summary>
        /// <returns></returns>
        public static string CpuInfo()
        {
            StringBuilder sr = new StringBuilder();

            var query = WmiDict[WmiType.Win32_Processor.ToString()];
            foreach (var obj in query)
            {
                sr.Append("厂商:" + obj["Manufacturer"] + ";");
                sr.Append("产品名称:" + obj["Name"] + ";");
                sr.Append("最大频率:" + obj["MaxClockSpeed"] + ";");
                sr.Append("当前频率:" + obj["CurrentClockSpeed"] + ";");
            }
            return sr.ToString();
        }
    }//end class
}//end namespace

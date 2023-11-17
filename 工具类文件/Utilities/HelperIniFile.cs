//-----------------------------------------------------------------------
// <copyright company="工品一号" file="IniFileHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-02-06 16:09:47
//  功能描述:   ini文件操作类
//  历史版本:
//          2017-02-06 16:09:47 刘少林 创建IniFileHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// ini文件操作类
    /// </summary>
    public class HelperIniFile
    {
        /// <summary>
        /// 批量获取ini配置键值对
        /// </summary>
        /// <param name="configPath">配置文件</param>
        /// <param name="section">节点块名称</param>
        /// <param name="keys">键值数组</param>
        /// <returns>键值对字典</returns>
        public static IDictionary<string, string> GetInitDictionary(string configPath, string section, params string[] keys)
        {
            if (keys == null)
            {
                return new Dictionary<string, string>(0);
            }
            IDictionary<string, string> dictionary = new Dictionary<string, string>(keys.Length);
            if (!File.Exists(configPath))
            {
                return new Dictionary<string, string>(0);
            }
            foreach (var key in keys)
            {
                dictionary.Add(key, GetPrivateProfileString(section, key, configPath));
            }
            return dictionary;
        }

        /// <summary>
        /// 获取ini文件节点配置值
        /// </summary>
        /// <param name="section">所属节点模块</param>
        /// <param name="key"> 配置值名称</param>
        /// <param name="filePath">ini文件路径</param>
        /// <returns></returns>
        public static string GetPrivateProfileString(string section, string key, string filePath)
        {
            var retVal = new StringBuilder();
            GetPrivateProfileString(section, key, "@_-1_@", retVal, 1000, filePath);
            if (retVal.ToString() != "@_-1_@")
            {
                return retVal.ToString();
            }
            return null;
        }

        #region 写Ini文件
        /// <summary>
        /// 写入ini文件节点配置值
        /// </summary>
        /// <param name="Section">所属节点模块</param>
        /// <param name="Key">配置值名称</param>
        /// <param name="Value">配置值</param>
        /// <param name="iniFilePath">文件路径</param>
        /// <returns></returns>
        public static bool WriteIniData(string Section, string[] Key, string[] Value, string iniFilePath)
        {
            if (File.Exists(iniFilePath))
            {
                long OpStation = 0;
                if (Key != null && Key.Length > 0)
                {
                    for (int i = 0; i < Key.Length; i++)
                    {
                        OpStation += WritePrivateProfileString(Section, Key[i].ToString(), Value[i].ToString(), iniFilePath);
                    }
                }
                if (OpStation == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 删除指定的 节
        /// </summary>
        /// <param name="section"></param>
        public static bool DeleteSection(string section, string filePath)
        {
            var result = WritePrivateProfileString(section, null, null, filePath);
            if (result != 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 读取ini配置参数值
        /// </summary>
        /// <param name="section">参数块名称</param>
        /// <param name="key">参数名称</param>
        /// <param name="def">默认值(没有值的时候，使用此默认值)</param>
        /// <param name="retVal">收INI文件中的值的CString对象</param>
        /// <param name="size">接收缓冲区的大小</param>
        /// <param name="filePath">ini文件所在路径</param>
        /// <returns></returns>
        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        /// <summary>
        /// 写入配置参数值到ini文件
        /// </summary>
        /// <param name="section">参数块名称</param>
        /// <param name="key">参数名称</param>
        /// <param name="val">参数值</param>
        /// <param name="filePath">ini文件所在路径</param>
        /// <returns></returns>
        /// <remarks>如文件不存在，则自动生成ini文件</remarks>
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
    }
}

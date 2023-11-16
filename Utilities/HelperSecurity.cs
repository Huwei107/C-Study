//-----------------------------------------------------------------------
// <copyright company="工品一号" file="SecurityHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-02-07 13:37:27
//  功能描述:   安全相关操作帮助类
//  历史版本:
//          2017-02-07 13:37:27 刘少林 创建SecurityHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace FX.MainForms
{
    /// <summary>
    /// 安全相关操作帮助类
    /// </summary>
    /// <remarks>密码加密，解密，MD5加密等</remarks>
    public class HelperSecurity
    {
        /// </summary>
        ///  DES加密字符串
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns>
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            byte[] rgbKey = Encoding.ASCII.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            dCSP.Key = rgbKey;
            dCSP.IV = rgbIV;
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary>
        /// 解密DES字符串
        /// </summary>
        /// <param name="decryptString">解密字符串</param>
        /// <param name="decryptKey">密钥</param>
        /// <returns></returns>
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            byte[] rgbKey = Encoding.ASCII.GetBytes(decryptKey.Substring(0, 8));
            byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            DCSP.Key = rgbKey;
            DCSP.IV = rgbIV;
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }

        /// <summary>
        /// 加密单字符
        /// </summary>
        /// <param name="Source"></param>
        /// <returns></returns>
        public static string Encrypt(string Source)
        {
            byte[] bts = Encoding.ASCII.GetBytes(Source);
            for (int i = 0; i < bts.Length; i++)
            {
                bts[i]++;
            }
            return Convert.ToBase64String(bts);
        }

        /// <summary>
        /// 小写金额转大写金额
        /// </summary>
        /// <param name="x">待转换数字参数</param>
        /// <returns>大写金额字符串</returns>
        public static string ConvertToChinese(double x)
        {
            string s = x.ToString("#L#E#D#C#K#E#D#C#J#E#D#C#I#E#D#C#H#E#D#C#G#E#D#C#F#E#D#C#.0B0A");
            string d = Regex.Replace(s, @"((?<=-|^)[^1-9]*)|((?'z'0)[0A-E]*((?=[1-9])|(?'-z'(?=[F-L\.]|$))))|((?'b'[F-L])(?'z'0)[0A-L]*((?=[1-9])|(?'-z'(?=[\.]|$))))", "${b}${z}");
            return Regex.Replace(d, ".", m => "负元空零壹贰叁肆伍陆柒捌玖空空空空空空空分角拾佰仟萬億兆京垓秭穰"[m.Value[0] - '-'].ToString());
        }

        /// <summary>
        /// 32位md5加密
        /// </summary>
        /// <param name="text">待加密明文</param>
        /// <param name="isLower">是否小写，默认小写</param>
        /// <returns>密文</returns>
        public static string MD5(string text, bool isLower = true)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(text);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = string.Empty;
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return isLower ? ret.PadLeft(32, '0').ToLower() : ret.PadLeft(32, '0').ToUpper();
        }
    }
}

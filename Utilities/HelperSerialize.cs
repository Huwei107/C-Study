//-----------------------------------------------------------------------
// <copyright company="工品一号" file="SerializeHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-14 8:42:17
//  功能描述:   序列化操作常用方法集
//  历史版本:
//          2017-2-14 8:42:17 刘少林 创建SerializeHelper类
// </copyright>
//-----------------------------------------------------------------------

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
namespace FX.MainForms
{
    /// <summary>
    /// 序列化操作常用方法集
    /// </summary>
    public class HelperSerialize
    {
        ///<summary>Convert a string of hex digits (ex: E4 CA B2) to a byte array. </summary>  
        ///<param name="s">The string containing the hex digits (with or without spaces).</param>  
        ///<returns>Returns an array of bytes.</returns>  
        private static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace("   ", " ");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }


        /// <summary>
        /// 获取字节码数组对应二进制字符串
        /// </summary>
        /// <param name="bytes">字节码数组</param>
        /// <returns>二进制字符串</returns>
        public static string GetBinaryFromBytes(byte[] bytes)
        {
            return GetStringFromBytes(bytes, NumberBaseTypes.Binary);
        }

        /// <summary>
        /// 获取字节码数组对应二进制字符串
        /// </summary>
        /// <param name="bytes">字节码数组</param>
        /// <returns>二进制字符串</returns>
        public static string GetHexadecimalFromBytes(byte[] bytes)
        {
            return GetStringFromBytes(bytes, NumberBaseTypes.Hexadecimal, '0');
        }

        public static byte[] HexStringToString(string hs)
        {
            //以%分割字符串，并去掉空字符
            string[] chars = hs.Split(new char[] { '%' }, StringSplitOptions.RemoveEmptyEntries);
            byte[] b = new byte[chars.Length];
            //逐个字符变为16进制字节数据
            for (int i = 0; i < chars.Length; i++)
            {
                b[i] = Convert.ToByte(hs[i].ToString(), 16);
            }
            return b;
        }

        /// <summary>
        /// 获得由字节数组转换而来的字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="toBase">数值进制枚举</param>
        /// <param name="padding">转换中是否填充字符,
        /// 比如字节值0转换为16进制，还是显示0，为了后期可逆方便，需要填充0，
        /// 比如十六进制最多两位，最大值ff,最小值00，填充字符后增加占用存储空间</param>
        /// <param name="split">每个字节分隔字符，默认不进行分隔，
        /// 不分隔字符串将无法可逆转换来源字节,可以减少占用的存储空间</param>
        /// <returns>字符串</returns>
        public static string GetStringFromBytes(byte[] bytes, NumberBaseTypes toBase, char padding = char.MinValue, char split = char.MinValue)
        {
            StringBuilder binaries = new StringBuilder(256);
            for (int i = 0; i < bytes.Length; i++)
            {
                if (split != char.MinValue)
                {
                    if (padding != char.MinValue)
                    {
                        binaries.AppendFormat("{0}{1}", split, System.Convert.ToString(bytes[i], (int)toBase).PadLeft(2, padding));
                    }
                    else
                    {
                        binaries.AppendFormat("{0}{1}", split, System.Convert.ToString(bytes[i], (int)toBase));
                    }
                }
                else
                {
                    if (padding != char.MinValue)
                    {
                        binaries.Append(System.Convert.ToString(bytes[i], (int)toBase).PadLeft(2, padding));
                    }
                    else
                    {
                        binaries.Append(System.Convert.ToString(bytes[i], (int)toBase));
                    }
                }
            }
            return binaries.ToString();
        }

        public enum NumberBaseTypes
        {
            /// <summary>
            /// 二进制
            /// </summary>
            [Description("二进制")]
            Binary = 2,

            /// <summary>
            /// 八进制
            /// </summary>
            [Description("八进制")]
            Octonary = 8,

            /// <summary>
            /// 十进制
            /// </summary>
            [Description("十进制")]
            Decimalism = 10,

            /// <summary>
            /// 十六进制
            /// </summary>
            [Description("十六进制")]
            Hexadecimal = 16,
        }

        /// <summary>
        /// 获取对象转换的字节码
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>字节码数组</returns>
        public static byte[] GetObjectBytes(object obj)
        {
            if (null == obj)
            {
                return new byte[0];
            }
            var format = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                format.Serialize(stream, obj);
                return stream.GetBuffer();
            }
        }

        /// <summary>
        /// 获取对象转换的字节码
        /// </summary>
        /// <param name="obj">实体对象</param>
        /// <returns>字节码数组</returns>
        public static T GetBytesObject<T>(byte[] bytes) where T : class
        {
            var format = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                return format.Deserialize(stream) as T;
            }
        }
    }//end class
}//end namespace

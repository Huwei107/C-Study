//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ConvertHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-14 8:36:18
//  功能描述:   类型转换帮助类
//  历史版本:
//          2017-2-14 8:36:18 刘少林 创建ConvertHelper类
//          2022-01-17        刘少林 修改StrToInt(string num, int defaultValue, bool isRound)方法
//                                   内部本来使用int.TryParse，但是遇到1.00的时候会显示不正确，不会变成整型1，因此将
//                                   将int.TryParse修改位double.TryParse，解决这种小数点字符串转换为整型问题                                     
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// 类型转换帮助类
    /// </summary>
    public class HelperConvert
    {
        const string DateArgsFormatExceptionDesc = "时间格式参数不正确";
        /// <summary>
        /// 将字符串时间转换为日期类型结果
        /// </summary>
        /// <param name="date">字符串时间</param>
        /// <returns>时间格式对象</returns>
        /// <remarks>参数传递不</remarks>
        public static DateTime StrToDate(string date)
        {
            DateTime result = new DateTime(1, 1, 1);
            if (DateTime.TryParse(date, out result))
            {
                return result;
            }
            else
            {
                throw new FormatException(DateArgsFormatExceptionDesc);
            }
        }

        /// <summary>
        /// 数字字符串转型双精度数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <returns>单精度数值</returns>
        public static float StrToSingle(string num)
        {
            return StrToSingle(num, 0f);
        }//end method

        /// <summary>
        /// 数字字符串转型双精度数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>单精度数值</returns>
        public static float StrToSingle(string num, float defaultValue)
        {
            if (num != null)
            {
                float d;
                if (HelperNumber.IsNumberFormat(num))
                {
                    if (float.TryParse(num, out d))
                    {
                        return d;
                    }
                }
            }
            return defaultValue;
        }//end method



        /// <summary>
        /// 数字字符串转型Int32整型数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <returns>转型后整数</returns>
        public static int StrToInt(string num)
        {
            return StrToInt(num, 0);
        }//end method

        /// <summary>
        ///  数字对象转型Int32整型数值(参数为null值时，返回0)
        /// </summary>
        /// <param name="num">数字对象</param>
        /// <returns>转型后整数</returns>
        public static int StrToInt(object num)
        {
            if (num != null)
            {
                return StrToInt(num.ToString());
            }
            else
            {
                return 0;
            }
        }//end method

        /// <summary>
        /// 数字字符串转型Int32整型数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转型整数</returns>
        public static int StrToInt(string num, int defaultValue)
        {
            return StrToInt(num, defaultValue, false);
        }//end method

        /// <summary>
        /// 数字字符串转型byte数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <returns>转型后整数</returns>
        public static byte StrToByte(string num)
        {
            return StrToByte(num, 0);
        }//end method

        /// <summary>
        /// 数字字符串转型byte数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>byte数值</returns>
        public static byte StrToByte(string num, byte defaultValue)
        {
            return StrToByte(num, defaultValue, false);
        }//end method

        /// <summary>
        /// 数字字符串转型byte数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isRound">布尔值,true则1.5转型后将获得2,否则1.5转型后将获得1</param>
        /// <returns>byte数值</returns>
        public static byte StrToByte(string num, byte defaultValue, bool isRound)
        {
            if (num != null)
            {
                if (HelperNumber.IsNumberFormat(num))
                {
                    byte d;
                    if (byte.TryParse(num, out d))
                    {
                        return d;
                    }
                }
            }
            return defaultValue;
        }//end method

        /// <summary>
        /// 数字字符串转型Int32整型数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isRound">布尔值,true则1.5转型后将获得2,否则1.5转型后将获得1</param>
        /// <returns>转型整数</returns>
        public static int StrToInt(string num, int defaultValue, bool isRound)
        {
            if (num != null)
            {
                if (HelperNumber.IsNumberFormat(num))
                {
                    int d;
                    double de;
                    if (double.TryParse(num, out de))
                    {
                        unchecked
                        {
                            d = (int)de;
                        }
                        return d;
                    }
                }
            }
            return defaultValue;

        }//end method

        /// <summary>
        /// 数字字符串转型Int64整型数值(此方法用于转型大型数据的时候有BUG，位数在12位之上)
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <returns>转型Int64整数</returns>
        public static long StrToLong(string num)
        {
            return StrToLong(num, 0L);
        }//end method

        /// <summary>
        /// 数字字符串转型Int64整型数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>转型Int64整数</returns>
        public static long StrToLong(string num, long defaultValue)
        {
            return StrToLong(num, defaultValue, false);
        }//end method

        /// <summary>
        /// 数字字符串转型Int64整型数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="isRound">布尔值,true则1.5转型后将获得2,否则1.5转型后将获得1</param>
        /// <returns>转型Int64整数</returns>
        public static long StrToLong(string num, long defaultValue, bool isRound)
        {
            if (num != null)
            {
                if (HelperNumber.IsNumberFormat(num))
                {
                    long d;
                    if (long.TryParse(num, out d))
                    {
                        return d;
                    }
                }
            }
            return defaultValue;


        }//end method

        /// <summary>
        /// 数字字符串转型双精度数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <returns>双精度数值</returns>
        public static double StrToDouble(string num)
        {
            return StrToDouble(num, 0d);
        }//end method

        /// <summary>
        /// 数字字符串转型双精度数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>双精度数值</returns>
        public static double StrToDouble(string num, double defaultValue)
        {
            if (num != null)
            {
                double d;
                if (HelperNumber.IsNumberFormat(num))
                {
                    if (double.TryParse(num, out d))
                    {
                        return d;
                    }
                }
            }
            return defaultValue;
        }//end method

        /// <summary>
        /// 数字字符串转型双精度数值
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="count">保留多少位小数位数</param>
        /// <returns>双精度数值</returns>
        public static double StrToDouble(string num, double defaultValue, int count)
        {
            if (num != null)
            {
                double d;
                if (HelperNumber.IsNumberFormat(num))
                {
                    if (double.TryParse(num, out d))
                    {
                        return Math.Round(d, count);
                    }
                }
            }
            return defaultValue;

        }//end method

        /// <summary>
        /// 数字字符串转型货币单位
        /// </summary>
        /// <param name="num">待转型字符串</param>
        /// <param name="defaultValue">返回的默认值</param>
        /// <returns>货币类型数值</returns>
        public static decimal StrToDecimal(string num, decimal defaultValue)
        {
            if (num != null)
            {
                decimal result;
                if (HelperNumber.IsNumberFormat(num))
                {
                    if (decimal.TryParse(num, out result))
                    {
                        return result;
                    }
                }
            }
            return defaultValue;
        }
    }

    /// <summary>
    /// 【Datable转List】
    /// </summary>
    public class ConvertToHelper<T> where T : new()
    {
        public static List<T> ConvertToList(DataTable dt)
        {
            // 定义集合   
            List<T> ts = new List<T>();
            // 获得此模型的类型   
            Type type = typeof(T);
            //定义一个临时变量   
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行   
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性   
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性   
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量   
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                        //取值   
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                        {
                            var tt = value.GetType();
                            if (tt == typeof(System.Decimal))
                            {
                                value = value.ToString().ToFloat();
                            }
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中   
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 转换数组
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> ConvertToList2(DataTable dt)
        {
            // 定义集合   
            List<T> ts = new List<T>();
            // 获得此模型的类型   
            Type type = typeof(T);
            //定义一个临时变量   
            string tempName = string.Empty;
            //遍历DataTable中所有的数据行   
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                // 获得此模型的公共属性   
                PropertyInfo[] propertys = t.GetType().GetProperties();
                //遍历该对象的所有属性   
                foreach (PropertyInfo pi in propertys)
                {
                    tempName = pi.Name;//将属性名称赋值给临时变量   
                    //检查DataTable是否包含此列（列名==对象的属性名）     
                    if (dt.Columns.Contains(tempName))
                    {
                        // 判断此属性是否有Setter   
                        if (!pi.CanWrite) continue;//该属性不可写，直接跳出   
                        //取值   
                        object value = dr[tempName];
                        //如果非空，则赋给对象的属性   
                        if (value != DBNull.Value)
                        {
                            var tt = value.GetType();
                            pi.SetValue(t, value, null);
                        }
                    }
                }
                //对象添加到泛型集合中   
                ts.Add(t);
            }
            return ts;
        }

        /// <summary>
        /// 转换表格
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(string tableName=null, List<T> collection = null)
        {
            var tb = new DataTable(string.IsNullOrEmpty(tableName) ? typeof(T).Name : tableName);
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                Type colType = prop.PropertyType;
                if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    colType = colType.GetGenericArguments()[0];
                }
                tb.Columns.Add(prop.Name, colType);
            }
            if (collection == null) return tb;
            foreach (var i in collection)
            {
                var row = tb.NewRow();
                foreach (var pi in props)
                {
                    object obj = pi.GetValue(i, null) == null ? DBNull.Value : pi.GetValue(i, null); //pi.GetValue(collection.ElementAt(i), null);
                    row[pi.Name] = obj;
                }
                tb.Rows.Add(row);
            }
            return tb;
        }

    }


}


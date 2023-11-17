//-----------------------------------------------------------------------
// <copyright company="工品一号" file="EnumHelper.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-02-05 13:23:21
//  功能描述:   日志帮助类
//  历史版本:
//          2017-02-05 13:23:21 刘少林 创建EnumHelper类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;

namespace FX.MainForms
{
    /// <summary>
    /// 枚举操作帮助类
    /// </summary>
    public class HelperEnum
    {
        private HelperEnum() { }
        #region 获取枚举的描述信息

        /// <summary>
        /// 获取枚举的描述信息
        /// </summary>
        /// <param name="e">枚举值</param>
        /// <returns>描述信息</returns>
        public static string GetEnumDesc(Enum e)
        {
            var enumInfo = e.GetType().GetField(e.ToString());
            var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            return enumAttributes.Length > 0 ? enumAttributes[0].Description : e.ToString();
        }//end method

        public static Dictionary<string,string> GetEnumDescs(Type type)
        {
            Dictionary<string,string> descs = new Dictionary<string,string>();
            foreach (var enumInfo in type.GetFields())
            {
                var enumAttributes = (DescriptionAttribute[])enumInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if(enumAttributes.Length > 0)
                {
                    descs.Add(enumInfo.Name, enumAttributes[0].Description);
                }
                
            }
            return descs;
        }
        #endregion

        #region 获取枚举的描述信息

        /// <summary>
        /// 获取枚举的描述信息，根据传入的枚举字符串
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumStrValue">枚举字符串</param>
        /// <returns>描述信息</returns>
        public static string GetEnumDesc(System.Type enumType, string enumStrValue)
        {
            string result = "";
            foreach (var enumObject in Enum.GetValues(enumType))
            {
                var e = (Enum)enumObject;
                if (e.ToString() == enumStrValue)
                {
                    result = GetEnumDesc(e);
                    break;
                }
            }
            return result;
        }//end method

        /// <summary>
        /// 根据枚举类型和枚举值对应整形数值获取对应的枚举描述
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumIntValue">枚举值对应的枚举整型值</param>
        /// <returns>对应枚举值的描述</returns>
        public static string GetEnumDescByInt(System.Type enumType, int enumIntValue)
        {
            string result = "";
            foreach (object enumObject in Enum.GetValues(enumType))
            {
                int tmp = (int)Enum.Parse(enumType, enumObject.ToString());
                if (tmp == enumIntValue)
                {
                    result = GetEnumDesc((Enum)enumObject);
                    break;
                }
            }
            return result;
        }//end method

        #endregion

        #region 根据枚举值名称获取对应的整数值
        /// <summary>
        /// 根据枚举值名称获取对应的整数值,名称不区分大小写,如果找不到相应的值则返回0
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumStrValue">枚举字符串</param>
        /// <returns>对应的整数值</returns>
        public static T GetIntValueByName<T>(System.Type enumType, string enumStrValue)
        {
            try
            {
                object value = Enum.Parse(enumType, enumStrValue, true);
                if (Enum.IsDefined(enumType, value))
                {
                    return (T)value;
                }
            }
            catch (ArgumentException)
            {
                return default(T);
            }
            return default(T);
        }//end method
        #endregion

        #region 根据枚举字符串,获取对应的枚举值
        /// <summary>
        /// 根据枚举字符串,获取对应的枚举值
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumStrValue">枚举字符串</param>
        /// <returns>枚举字符串对应枚举</returns>
        public static Enum GetEnumByEnumString(Type enumType, string enumStrValue)
        {
            if (string.IsNullOrEmpty(enumStrValue))
            {
                return null;
            }
            string enumStr = enumStrValue.Trim();
            if (enumStr.Length == 0)
            {
                return null;
            }
            object v = Enum.Parse(enumType, enumStr, true);
            if (Enum.IsDefined(enumType, v))
            {
                return (Enum)v;
            }
            return null;
        }//end method
        #endregion


        #region 将含有描述信息的枚举绑定到列表控件中

        /// <summary>
        /// 将含有描述信息的枚举绑定到列表控件中
        /// </summary>
        /// <param name="comboBox">待绑定控件</param>
        /// <param name="enumType">枚举类型</param>
        /// <remarks>combox的值和名称都显示枚举描述</remarks>
        public static void BindDesEnumToComboBox(ComboBox comboBox, System.Type enumType)
        {
            var array = Enum.GetValues(enumType);
            foreach (object enumValue in array)
            {
                if (enumValue != null)
                {
                    var e = (Enum)enumValue;
                    comboBox.Items.Add(GetEnumDesc(e));
                }
            }

        }//end method

        /// <summary>
        /// 将含有描述信息的枚举绑定到列表控件中
        /// </summary>
        /// <param name="comboBox">待绑定控件</param>
        /// <param name="enumType">枚举类型</param>
        /// <remarks>combox的值和名称都显示枚举描述</remarks>
        public static void BindDesAndValueEnumToComboBox(ComboBox comboBox, System.Type enumType)
        {
            var fields = enumType.GetType().GetFields();
            var dt = new System.Data.DataTable();
            dt.Columns.Add(new System.Data.DataColumn("Key", typeof(string)));
            dt.Columns.Add(new System.Data.DataColumn("Value", typeof(int)));
            foreach (var item in Enum.GetValues(enumType))
            {
                var row = dt.NewRow();
                var array = item.GetType().GetField(item.ToString()).GetCustomAttributes(typeof(DescriptionAttribute),false);
                if (array != null && array.Length > 0)
                {
                    row["Key"]= ((DescriptionAttribute)array[0]).Description;
                    //comboBox.Items.Add(new { Key = ((DescriptionAttribute)array[0]).Description, Value = item.GetHashCode() });
                }
                else
                {
                    row["Key"] = item.ToString();
                    //comboBox.Items.Add(new { Key = item.ToString(), Value = item.GetHashCode() });
                }
                row["Value"] = item.GetHashCode();
                dt.Rows.Add(row);
            }
            comboBox.DataSource = dt;
            comboBox.ValueMember = "Value";
            comboBox.DisplayMember = "Key";
        }

        /// <summary>
        /// 将含有描述信息的枚举绑定到列表控件中
        /// </summary>
        /// <param name="comboBox">待绑定控件</param>
        /// <param name="enumType">枚举类型</param>
        /// <remarks>combox的值和名称都显示枚举描述</remarks>
        public static void BindEnumStringToComboBox(ComboBox comboBox, System.Type enumType)
        {
            var array = Enum.GetValues(enumType);
            foreach (object enumValue in array)
            {
                if (enumValue != null)
                {
                    comboBox.Items.Add(enumValue.ToString());
                }
            }

        }//end method


        /// <summary>
        /// 将含有描述信息的枚举绑定到列表控件中
        /// </summary>
        /// <param name="comboBox">待绑定控件</param>
        /// <param name="enumType">枚举类型</param>
        /// <remarks>combox的值和名称都显示枚举描述</remarks>
        public static void BindDesEnumToListBox(ListBox listBox, System.Type enumType)
        {
            var array = Enum.GetValues(enumType);
            foreach (object enumValue in array)
            {
                if (enumValue != null)
                {
                    var e = (Enum)enumValue;
                    listBox.Items.Add(GetEnumDesc(e));
                }
            }

        }//end method
        #endregion
    }
}

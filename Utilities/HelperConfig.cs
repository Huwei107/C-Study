using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
namespace FX.MainForms
{
    public class HelperConfig
    {
        private readonly static object Locker = new object();


       

        ///// <summary>
        ///// 修改App.Config的值
        ///// </summary>
        ///// <param name="path">全路径</param>
        ///// <param name="key">config key值</param>
        ///// <param name="value">修改后的value</param>
        public static void SetConfigValue(string path, string key, string value)
        {
            SetConfigValue(path, key, value, 1000);
        }

        /// <summary>
        /// 根据key获取value值
        /// </summary>
        /// <param name="path">config全路径</param>
        /// <param name="appKey">config key值</param>
        /// <returns>value</returns>
        public static string GetConfigValue(string path, string appKey)
        {
            return GetConfigValue(path, appKey, "value");
        }

        ///// <summary>
        ///// 根据key获取value值
        ///// </summary>
        ///// <param name="path">config全路径</param>
        ///// <param name="appKey">config key值</param>
        ///// <returns>value</returns>
        public static string GetConfigTop(string path, string appKey)
        {
            return GetConfigValue(path, appKey, "top");
        }

        /// <summary>
        /// 修改App.Config的值
        /// </summary>
        /// <param name="path">全路径</param>
        /// <param name="key">config key值</param>
        /// <param name="value">修改后的value</param>
        public static void SetConfigValue(string path, string key, string value, int top = 1000)
        //public static void SetConfigValue(string path, string key, string value)
        {
            lock (Locker)
            {
                var xDoc = new XmlDocument();
                xDoc.Load(path);
                var xNode = xDoc.SelectSingleNode("//appSettings");
                if (xNode != null)
                {
                    var xElem1 = (XmlElement)xNode.SelectSingleNode("//add [@key='" + key + "']");
                    if (xElem1 != null)
                    {
                        xElem1.SetAttribute("value", value);
                        xElem1.SetAttribute("top", top.ToString());
                    }
                    else
                    {
                        XmlElement xElem2 = xDoc.CreateElement("add");
                        xElem2.SetAttribute("key", key);
                        xElem2.SetAttribute("value", value);
                        xElem2.SetAttribute("top", top.ToString());
                        xNode.AppendChild(xElem2);
                    }
                }
                xDoc.Save(path);
            }
        }
       

        /// <summary>
        /// 根据key获取value值
        /// </summary>
        /// <param name="path">config全路径</param>
        /// <param name="appKey">config key值</param>
        /// <returns>value</returns>
        public static string GetConfigValue(string path, string appKey, string keyname)
        {
            var xmlDocument = new XmlDocument();
            XmlElement xmlElement = null;
            if (!File.Exists(path))
            {
                HelperLog.Write(path + "===,path路径不存在");
                return string.Empty;
            }
            try
            {
                xmlDocument.Load(path);
                var xmlNode = xmlDocument.SelectSingleNode("//appSettings");
                if (xmlNode != null)
                    xmlElement = (XmlElement)xmlNode.SelectSingleNode("//add[@key=\"" + appKey + "\"]");
                var result = xmlElement != null ? xmlElement.GetAttribute(keyname) : string.Empty;
                return result;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取conf.config配置文件节点值字典
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="keys">配置节点键值</param>
        /// <remarks>根据key获取value值
        /// 配置文件如下
        /// <!--<configuration>
        /// <appSettings>
        /// 远程服务器地址,放置服务端更新文件和客户端更新文件的地址,由开发人员自动上传更新
        /// <add key="Url" value="http://58.211.82.131:90/" />
        /// -->
        /// 以上配置文件格式即可使用此方法获取
        /// </remarks>
        /// <returns>配置值字典</returns>
        public static IDictionary<string, string> GetConfigValueDictionary(string configPath, params string[] keys)
        {
            if (keys == null)
            {
                return new Dictionary<string, string>(0);
            }
            IDictionary<string, string> dictionary = new Dictionary<string, string>(keys.Length);
            //UpDate ServerUpDate Url FilePath SupplierCode Version ServerCode
            var xmlDocument = new XmlDocument();
            string prefixPath = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.LastIndexOf("\\"));
            if (File.Exists(configPath))
            {
                xmlDocument.Load(configPath);
            }
            else
            {
                return new Dictionary<string, string>(0);
            }
            var xmlNode = xmlDocument.SelectSingleNode("//appSettings");
            if (xmlNode != null)
                foreach (var key in keys)
                {
                    XmlElement xmlElement = (XmlElement)xmlNode.SelectSingleNode("//add[@key=\"" + key + "\"]");
                    if (xmlElement != null)
                    {
                        dictionary.Add(key, xmlElement.GetAttribute("value"));
                    }
                }

            return dictionary;
        }
    }
}

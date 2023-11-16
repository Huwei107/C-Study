
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperJson">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   唐亮
//  创建时间:   2018-12-11 13:35:16
//  功能描述:   【请输入类描述】
//  历史版本:
//          2018-12-11 13:35:16 唐亮 创建HelperJson类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using FX.ORM.Websharp.ORM.Base;
using FX.ORM.Websharp.ORM.Service;
using FX.MainForms;
using System.Web;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace FX.MainForms
{
    /// <summary>
    /// 【HelperJson帮助】
    /// </summary>
    public class HelperJson
    {

        /// <summary>
        /// app_key
        /// </summary>
        public static string APP_KEY = HelperJson.GetXml("APP_KEY");
        /// <summary>
        /// app_secret
        /// </summary>
        public static string APP_SECRET = HelperJson.GetXml("APP_SECRET");

        private static string url = GetXml("JavaUrl") + "?";

        #region 添加接口发送表
        public static string AddSysApiSend(string task_number, string task_type, string url, string param, string data, string send_time, string Remark)
        {
            using (PersistenceManager pm = PersistenceManagerFactory.Instance().CreatePersistenceManager())
            {
                //string sql = @"INSERT INTO sys_api_send (task_number, task_type, url, param, data, send_time,Remark,cs) VALUES (@task_number, @task_type, @url, @param, @data, @send_time,@Remark,0)  SELECT @@IDENTITY";
                //SqlParameter[] sp = { 
                //                  new SqlParameter("@task_number",task_number),
                //                  new SqlParameter("@task_type",task_type),
                //                  new SqlParameter("@url",url),
                //                  new SqlParameter("@param",param),
                //                  new SqlParameter("@data",data),
                //                  new SqlParameter("@send_time",send_time),
                //                  new SqlParameter("@Remark",Remark)
                //                  };
                //object o = DBHelper.GetSingle(sql, sp);
                //return o == null ? "" : o.ToString();
                string sql = @"INSERT INTO sys_api_send (task_number, task_type, url, param, data, send_time,Remark,cs) VALUES (@task_number, @task_type, @url, @param, @data, @send_time,@Remark,0)";
                SqlParameter[] sp = { 
                              new SqlParameter("@task_number",task_number),
                              new SqlParameter("@task_type",task_type),
                              new SqlParameter("@url",url),
                              new SqlParameter("@param",param),
                              new SqlParameter("@data",data),
                              new SqlParameter("@send_time",send_time),
                              new SqlParameter("@Remark",Remark)
                              };
                pm.ExecuteNonQuery(sql, sp);
                DataTable dt = pm.ExecuteDataSet("select id from sys_api_send where task_number='" + task_number + "';").Tables[0];
                if (dt != null && dt.Rows[0][0] != null)
                {
                    return dt.Rows[0][0].ToString();
                }
                else
                {
                    return "";
                }
            }
        }
        #endregion

        #region 修改返回状态结果和业务状态结果
        public static void UpdateSysApiSend(string id, string network_status, string response_time, string response_code, string response_message)
        {
            PersistenceManager pm = PersistenceManagerFactory.Instance().CreatePersistenceManager();
            string sql = @"UPDATE sys_api_send SET network_status = @network_status,response_time = @response_time, response_code = @response_code,response_message = @response_message,cs=1 WHERE id = @id";
            SqlParameter[] sp = { 
                        new SqlParameter("@id",id),
                        new SqlParameter("@network_status",network_status),
                        new SqlParameter("@response_time",response_time),
                        new SqlParameter("@response_code",response_code),
                        new SqlParameter("@response_message",response_message)
                        };
            pm.ExecuteNonQuery(sql, sp);
        }
        #endregion


        /// <summary>
        /// 获取xml
        /// </summary>
        /// <param name="name">标签名</param>
        /// <returns></returns>
        public static string GetXml(string name)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AppDomain.CurrentDomain.BaseDirectory + "/config.xml");//xmlPath为xml文件路径
            XmlNode xmlNode = xmlDoc.SelectSingleNode("/data/" + name);
            return xmlNode.InnerText;
        }

        public static ResultState Send(string type, string needResult, string url, object obj, out string id
            , out string network_status, out string response_time)
        {
            network_status = response_time = "";
            string data = HttpUtility.UrlEncode(SerializeObject(obj));
            string number = GetMaxTaskNumber(type);
            string sign = GetSign(type, number, needResult, data);
            string paras = "appKey=" + APP_KEY + "&needResult=" + needResult + "&sign=" + sign + "&type=" + type + "&number=" + number;
            //添加自己数据库
            id = AddSysApiSend(number, type, url, paras, HttpUtility.UrlDecode(data), DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //LogHelper.Write(id);
            //url += paras + "&data=" + HttpUtility.UrlEncode(data); //完整url
            //string b = Request(url, "", out network_status, out response_time);

            paras += "&data=" + HttpUtility.UrlEncode(data);
            string b = string.Empty;
            try
            {
                b = Request(url, paras, out network_status, out response_time);
            }
            catch (Exception ex)
            {
               // LogHelper.Write(ex, "Request方法发生了内部错误!" + paras);
                var str = ex.Message;
            }
            //LogHelper.Write("成功发送了请求，请求相应内容=" + b);
            ResultState rs = DeserializeJsonToObject<ResultState>(b);
            //修改数据库
            UpdateSysApiSend(id, network_status, string.IsNullOrEmpty(response_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : response_time, rs.resultCode, rs.resultMsg);
            return rs;
        }
        /// <summary>
        /// 调用接口 
        /// N001同步供应商信息 Supplier 
        /// N002同步库存信息 ProStock 
        /// N003同步取货信息 TakeGoods
        /// N004回写运单号码 SendNumber
        /// N005同步退货信息 SellReturn
        /// N006库存占用信息 StockUsing
        /// N007同步出货通知单作废状态 DeliveryRequisitionState
        /// </summary>
        /// <param name="type"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static ResultState API_Request(string type, object obj)
        {
            ResultState rs = new ResultState();
            string id = string.Empty, network_status = string.Empty, response_time = string.Empty;
            string needResult = "0"; //默认不要返回数据
            try
            {

                rs = Send(type, needResult, url, obj, out id, out network_status, out response_time);

            }
            catch (Exception ex)
            {
               // LogHelper.Write(ex, "Send方法发生了内部错误!");
                rs.resultCode = "1";
                rs.resultMsg = ex.Message.Length > 500 ? ex.Message.Substring(0, 500) : ex.Message;
                UpdateSysApiSend(id, network_status, string.IsNullOrEmpty(response_time) ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") : response_time, rs.resultCode, rs.resultMsg);
                //WriteTxt(AppDomain.CurrentDomain.BaseDirectory + "/log.txt", DateTime.Now + ex.Message + "\r\n\r\n");
            }
            return rs;
        }

        #region 获取任务号 T20160408N0010001
        public static string GetMaxTaskNumber(string type)
        {
            PersistenceManager pm = PersistenceManagerFactory.Instance().CreatePersistenceManager();
            //string header = "T" + DateTime.Now.ToString("yyyyMMdd") + type;
            //string sql = "SELECT max(task_number) FROM sys_api_send WHERE task_number LIKE '" + header + "%'";
            //object obj = DBHelper.GetSingle(sql);
            //if (obj == null)
            //{
            //    return header + "0001";
            //}
            //if (obj.ToString().Contains("-"))
            //{
            //    obj = obj.ToString().Split('-')[0];
            //}
            //int num = int.Parse(obj.ToString().Substring(13));
            //num++;
            //return header + num.ToString().PadLeft(4, '0');        

            string code = string.Empty;
            SqlParameter[] p = new SqlParameter[3];
            p[0] = new SqlParameter("@tablename", "dbo.sys_api_send");
            p[1] = new SqlParameter("@CodeName", type);
            p[2] = new SqlParameter("@Res", SqlDbType.NVarChar, 50);
            p[2].Direction = ParameterDirection.Output;
            pm.RunProcedure("dbo.GetCode_API", p);
            code = p[2].Value.ToString();
            return code;

        }
        //获取签名
        public static string GetSign(string type, string number, string needResult, string data)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("appKey", APP_KEY);
            dic.Add("type", type);
            dic.Add("number", number);
            dic.Add("needResult", needResult);
            dic.Add("data", data);//编码过
            return GetSign(dic);
        }

        #endregion
        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public static string GetSign(Dictionary<string, string> dic)
        {
            string sign = "";
            //参数名ASCII码从小到大排序（字典序）
            //var q = from d in dic
            //        orderby d.Key ascending
            //        select d;
            string temp = APP_SECRET;
            List<KeyValuePair<string, string>> q = new List<KeyValuePair<string, string>>(dic);
            q.Sort(delegate(KeyValuePair<string, string> s1, KeyValuePair<string, string> s2)
            {
                return s1.Key.CompareTo(s2.Key);
            });
            foreach (KeyValuePair<string, string> i in q)
            {
                if (i.Key != "sign" && i.Key != "" && i.Value != "")
                {
                    temp += i.Key + i.Value;
                }
            }
            temp += APP_SECRET;
            sign = EncodeMD5(temp, 32).ToUpper();
            return sign;
        }
        /// <summary>
        /// md5加密小写
        /// </summary>
        /// <param name="str">加密字符串</param>
        /// <param name="code">16位还是32位</param>
        /// <returns></returns>
        public static string EncodeMD5(string str, int code = 32)
        {
            if (code == 16) //16位MD5加密（取32位加密的9~25字符） 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            else//32位加密 
            {
                return System.Web.Security.FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
        }
        #region json helper
        /// <summary>
        /// request请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string Request(string url, string paras, out string network_status, out string response_time)
        {
            string strResult = "";
            byte[] data = Encoding.UTF8.GetBytes(paras.Trim());
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;
            Stream stream = myHttpWebRequest.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            HttpWebResponse response = myHttpWebRequest.GetResponse() as HttpWebResponse;
            network_status = response.StatusCode.ToString();
            response_time = response.LastModified.ToString("yyyy-MM-dd HH:mm:ss");
            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), Encoding.UTF8);
            strResult = sr.ReadToEnd();
            return strResult;

        }
        /// <summary>
        /// request请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="paras">参数</param>
        /// <returns></returns>
        public static string Request(string url, string paras)
        {
            string strResult = "";
            byte[] data = Encoding.UTF8.GetBytes(paras.Trim());
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            myHttpWebRequest.Method = "POST";
            myHttpWebRequest.ContentType = "application/x-www-form-urlencoded";
            myHttpWebRequest.ContentLength = data.Length;
            Stream stream = myHttpWebRequest.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            System.IO.StreamReader sr = new System.IO.StreamReader(myHttpWebRequest.GetResponse().GetResponseStream(), Encoding.UTF8);
            strResult = sr.ReadToEnd();
            return strResult;

        }

        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            string json = JsonConvert.SerializeObject(o);
            return json;
        }

        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }

        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }

        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
        #endregion

        #region 用户发送写入txt，用于记录。
        /// <summary>
        /// 用户发送写入txt，用于记录。
        /// </summary>
        /// <returns></returns>
        public static bool WriteTxt(string path, string str)
        {
            try
            {
                FileStream fs = new FileStream(path, FileMode.Append);
                StreamWriter sw = new StreamWriter(fs);
                sw.Write(str);
                sw.Flush();
                sw.Close();
                fs.Close();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}

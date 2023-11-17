using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace FX.MainForms
{
    public class HelperWebApi
    {
        /// <summary>  
        /// 调用api返回json  
        /// </summary>  
        /// <param name="url">api地址</param>  
        /// <param name="jsonstr">接收参数</param>  
        /// <param name="type">类型</param>  
        /// <returns></returns>  
        public static string HttpApi(string url, string jsonstr, string type)
        {
            Encoding encoding = Encoding.UTF8;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);//webrequest请求api地址  
            request.Accept = "text/html,application/xhtml+xml,*/*";
            request.ContentType = "application/json";
            request.Method = type.ToUpper().ToString();//get或者post
            if (type.ToUpper().ToString().Contains("POST"))
            {
                byte[] buffer = encoding.GetBytes(jsonstr);
                request.ContentLength = buffer.Length;
                request.GetRequestStream().Write(buffer, 0, buffer.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }

           
        }

        /// <summary>
        /// 如果是以流的方式提交表单数据的时候不能使用get方法，必须用post方法.
        /// </summary>
        /// <param name="postUrl"></param>
        /// <param name="postDatas"></param>
        /// <returns></returns>
        public static string HttpApiPost(string postUrl, string postDatas)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(postUrl); //--需要封装的参数
            request.CookieContainer = new CookieContainer();
            CookieContainer cookie = request.CookieContainer;//如果用不到Cookie，删去即可 
            //以下是发送的http头
            request.Referer = "";
            request.Accept = "Accept:text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers["Accept-Language"] = "zh-CN,zh;q=0.";
            request.Headers["Accept-Charset"] = "GBK,utf-8;q=0.7,*;q=0.3";
            request.UserAgent = "User-Agent:Mozilla/5.0 (Windows NT 5.1) AppleWebKit/535.1 (KHTML, like Gecko) Chrome/14.0.835.202 Safari/535.1";
            request.KeepAlive = true;
            //上面的http头看情况而定，但是下面俩必须加 
            request.ContentType = "application/x-www-form-urlencoded";
            Encoding encoding = Encoding.UTF8;//根据网站的编码自定义
            request.Method = "get";  //--需要将get改为post才可行
            string postDataStr;
            Stream requestStream = request.GetRequestStream();
            if (postDatas != "")
            {
                postDataStr = postDatas;//--需要封装,形式如“arg=arg1&arg2=arg2”
                byte[] postData = encoding.GetBytes(postDataStr);//postDataStr即为发送的数据，
                request.ContentLength = postData.Length;
                requestStream.Write(postData, 0, postData.Length);
            }

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();


            StreamReader streamReader = new StreamReader(responseStream, encoding);
            string retString = streamReader.ReadToEnd();

            streamReader.Close();
            responseStream.Close();
            return retString;
        }
    }
}

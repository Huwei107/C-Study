//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ResponseService.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/8/9 10:35:51 
//  功能描述:   
//  历史版本:
//          2023/8/9 10:35:51 王健 创建ResponseService类
// </copyright>
//-----------------------------------------------------------------------
using FX.MainForms.Common.Context;
using FX.MainForms.Common.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace FX.MainForms.Common.Response
{
    /// <summary>
    /// http请求逻辑类
    /// </summary>
    public class ResponseService
    {
        #region GET
        /// <summary>
        /// 发起GET同步请求
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <returns></returns>
        public static DataResponseBase<T> Get<T>(string subfix)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpResponseMessage response = client.GetAsync(requestUri).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex, $"Uri:{requestUri},err");
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// 发起GET异步请求
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <returns></returns>
        public static async Task<DataResponseBase<T>> GetAsync<T>(string subfix)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    //读取远程的资源.执行Get操作(跨域访问)
                    var task = await client.GetAsync(requestUri);
                    var json = await task.Content.ReadAsStringAsync();
                    HelperLog.Write($"Uri:{requestUri};Result:{json}");
                    if (task.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex, $"Uri:{requestUri},err");
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// Get请求带body请求体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subfix"></param>
        /// <param name="bodyParam"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static DataResponseBase<T> Get<T>(string subfix, object bodyParam)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(subfix),
                        Content = new StringContent(JsonConvert.SerializeObject(bodyParam), Encoding.UTF8),
                    };
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpResponseMessage response = client.SendAsync(request).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex, $"Uri:{requestUri},err");
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// Get异步请求带body请求体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subfix"></param>
        /// <param name="bodyParam"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static async Task<DataResponseBase<T>> GetAsync<T>(string subfix, object bodyParam)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Get,
                        RequestUri = new Uri(subfix),
                        Content = new StringContent(JsonConvert.SerializeObject(bodyParam), Encoding.UTF8),
                    };
                    client.DefaultRequestHeaders.Add("ContentType", "application/json");
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    var task = await client.SendAsync(request);
                    var json = await task.Content.ReadAsStringAsync();
                    HelperLog.Write($"Uri:{requestUri};Result:{json}");
                    if (task.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex, $"Uri:{requestUri},err");
            }
            return new DataResponseBase<T>();
        }
        #endregion GET


        #region POST
        /// <summary>
        /// 发起POST同步请求 application/x-www-form-urlencoded
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static DataResponseBase<T> Post<T>(string subfix, Dictionary<string, string> param)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent postContent = new FormUrlEncodedContent(param);
                    //读取远程的资源.执行Get操作(跨域访问)
                    HttpResponseMessage response = client.PostAsync(requestUri, postContent).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// 发起POST异步请求 application/x-www-form-urlencoded
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <param name="param">请求参数</param>
        /// <returns></returns>
        public static async Task<DataResponseBase<T>> PostAsync<T>(string subfix, Dictionary<string, string> param)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent postContent = new FormUrlEncodedContent(param);
                    //读取远程的资源.执行Get操作(跨域访问)
                    HttpResponseMessage response = await client.PostAsync(requestUri, postContent);
                    var json = await response.Content.ReadAsStringAsync();
                    HelperLog.Write($"StatusCode:{response.StatusCode},Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// 发起POST同步请求 application/json
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static DataResponseBase<T> Post<T>(string subfix, object param)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent postContent = param == null ? null : new StringContent(JsonConvert.SerializeObject(param));
                    postContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    //读取远程的资源.执行Get操作(跨域访问)
                    HttpResponseMessage response = client.PostAsync(requestUri, postContent).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    HelperLog.Write($"StatusCode:{response.StatusCode},Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// 发起POST异步请求 application/json
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static async Task<DataResponseBase<T>> PostAsync<T>(string subfix, object param)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent postContent = param == null ? null : new StringContent(JsonConvert.SerializeObject(param));
                    postContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await client.PostAsync(requestUri, postContent);

                    var json = await response.Content.ReadAsStringAsync();
                    HelperLog.Write($"StatusCode:{response.StatusCode}, Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }



                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }


        /// <summary>
        /// 发起POST同步请求 multipart/form-data(上传文件)
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix"></param>
        /// <param name="file"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static DataResponseBase<T> Post<T>(string subfix, string fileName, string paramName = "file")
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    using (var multipartFormDataContent = new MultipartFormDataContent())
                    {
                        var ba = new ByteArrayContent(File.ReadAllBytes(fileName));
                        var context = GetContentType(Path.GetExtension(fileName));
                        if (string.IsNullOrEmpty(context)) throw new Exception("暂不支持上传该文件格式");
                        ba.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(context);
                        multipartFormDataContent.Add(ba, paramName, Path.GetFileName(fileName));

                        //读取远程的资源.执行Get操作(跨域访问)
                        HttpResponseMessage response = client.PostAsync(requestUri, multipartFormDataContent).Result;
                        var json = response.Content.ReadAsStringAsync().Result;
                        HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                        }
                        else
                        {
                            var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                            DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                            return data;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// 发起POST异步请求 multipart/form-data(上传文件)
        /// </summary>
        /// <typeparam name="T">接口泛型</typeparam>
        /// <param name="subfix"></param>
        /// <param name="file"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<DataResponseBase<T>> PostAsync<T>(string subfix, string fileName, string paramName = "file")
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    using (var multipartFormDataContent = new MultipartFormDataContent())
                    {
                        var ba = new ByteArrayContent(File.ReadAllBytes(fileName));
                        var context = GetContentType(Path.GetExtension(fileName));
                        if (string.IsNullOrEmpty(context)) throw new Exception("暂不支持上传该文件格式");
                        ba.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(context);
                        multipartFormDataContent.Add(ba, paramName, Path.GetFileName(fileName));

                        //读取远程的资源.执行Get操作(跨域访问)
                        HttpResponseMessage response = await client.PostAsync(requestUri, multipartFormDataContent);
                        var json = await response.Content.ReadAsStringAsync();
                        HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                        }
                        else
                        {
                            var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                            DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                            return data;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }

        #endregion POST


        #region PUT
        /// <summary>
        /// 发起PUT同步请求
        /// </summary>
        /// <typeparam name="T">接口泛型<typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static DataResponseBase<T> Put<T>(string subfix, object param)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent content = param == null ? null : new StringContent(JsonConvert.SerializeObject(param));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = client.PutAsync(requestUri, content).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }

                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }

        /// <summary>
        /// 发起PUT异步请求
        /// </summary>
        /// <typeparam name="T">接口泛型<typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <param name="param">请求参数</param>
        /// <param name="headers">请求头</param>
        /// <returns></returns>
        public static async Task<DataResponseBase<T>> PutAsync<T>(string subfix, object param = null)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent content = param == null ? null : new StringContent(JsonConvert.SerializeObject(param));
                    if (content != null) content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await client.PutAsync(requestUri, content);
                    var json = await response.Content.ReadAsStringAsync();
                    HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }

                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }
        #endregion PUT


        #region DELETE
        /// <summary>
        /// 发起DELETE同步请求
        /// </summary>
        /// <typeparam name="T">接口泛型<typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <returns>JsonResult返回结果对象</returns>
        public static DataResponseBase<T> Delete<T>(string subfix, object param = null)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent content = param == null ? null : new StringContent(JsonConvert.SerializeObject(param));
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = client.DeleteAsync(requestUri).Result;
                    var json = response.Content.ReadAsStringAsync().Result;
                    HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }

                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }


        /// <summary>
        /// 发起DELETE异步请求
        /// </summary>
        /// <typeparam name="T">接口泛型<typeparam>
        /// <param name="subfix">api后缀具体链接</param>
        /// <returns>JsonResult返回结果对象</returns>
        public static async Task<DataResponseBase<T>> DeleteAsync<T>(string subfix, object param = null)
        {
            var requestUri = EnvironmentContext.Instance.Url + subfix;
            try
            {
                //客户端对象的创建与初始化
                using (HttpClient client = new HttpClient())
                {
                    if (EnvironmentContext.Instance.Header != null)
                    {
                        foreach (var header in EnvironmentContext.Instance.Header)
                            client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                    HttpContent content = param == null ? null : new StringContent(JsonConvert.SerializeObject(param));
                    if (content != null) content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                    HttpResponseMessage response = await client.DeleteAsync(requestUri);
                    var json = await response.Content.ReadAsStringAsync();
                    HelperLog.Write($"StatusCode: {response.StatusCode} ,Uri:{requestUri};Result:{json}");
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return JsonConvert.DeserializeObject<DataResponseBase<T>>(json);
                    }
                    else
                    {
                        var result = JsonConvert.DeserializeObject<DataResponseBase<object>>(json);
                        DataResponseBase<T> data = new DataResponseBase<T>() { Code = result.Code, Message = result.Message, Success = result.Success };
                        return data;
                    }

                }
            }
            catch (Exception ex)
            {
                HelperLog.Write($"Uri:{requestUri},err", ex);
            }
            return new DataResponseBase<T>();
        }
        #endregion DELETE

        /// <summary>
        /// 移除多余的URL参数请求,避免参数过程（经过反射机制，将可能导致默认数据传递，从而使一些URL参数过度长）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string SplicingUrl<T>(T t)
        {
            System.Reflection.PropertyInfo[] propertys = typeof(T).GetProperties();
            string querystr = string.Empty;
            string paramvalues;
            foreach (System.Reflection.PropertyInfo property in propertys)
            {
                var obj = property.GetValue(t, null);
                paramvalues = obj != null ? obj.ToString() : string.Empty;
                //移除多余的URL参数请求,避免参数过程（经过反射机制，将可能导致默认数据传递，从而使一些URL参数过度长）
                if (!string.IsNullOrEmpty(paramvalues))
                {
                    if (!property.PropertyType.Name.Equals("DateTime"))
                    {
                        querystr += property.Name + "=" + System.Web.HttpUtility.UrlEncode(paramvalues) + "&";
                    }
                    else
                    {
                        DateTime time;
                        if (DateTime.TryParse(paramvalues, out time) && DateTime.MinValue != time)
                        {
                            querystr += property.Name + "=" + System.Web.HttpUtility.UrlEncode(paramvalues) + "&";
                        }
                    }
                }
            }
            //从foreach循环中移除字符累加操作!
            if (querystr.Length > 0)
            {
                return "?" + querystr.TrimEnd('&');
            }
            return querystr;
        }

        #region 获取文件的 ContentType 标头值
        /// <summary>
        /// 获取文件的 ContentType 标头值
        /// </summary>
        /// <param name="extension"></param>
        /// <returns></returns>
        private static string GetContentType(string extension)
        {
            var context = string.Empty;
            switch (extension)
            {
                case ".png":
                    context = "image/png";
                    break;
                case ".jpeg":
                case ".jpg":
                case ".jpe":
                    context = "image/jpeg";
                    break;
                case ".doc":
                case ".dot":
                    context = "application/msword";
                    break;
                case ".docx":
                    context = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case ".xlsx":
                    context = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case ".xls":
                case ".xla":
                    context = "application/msexcel";
                    break;
                default:
                    break;

            }
            return context;
        }
        #endregion 获取文件的 ContentType 标头值
    }
}

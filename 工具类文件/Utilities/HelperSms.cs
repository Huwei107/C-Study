using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using FX.MainForms.Public.Models;
using System.Data;
using System.Drawing;
using Newtonsoft.Json;
using FX.MainForms.Public.Models.JsonEntity;
using FX.MainForms.Public.Enums;

namespace FX.MainForms
{
    public class HelperSms
    {      
        //阿里云服务器地址
        private static string Endpoint = string.Empty;
        // 密匙ID
        private static string AccessKeyId = string.Empty;
        // 密匙
        private static string AccessKeySecret = string.Empty;
        // 短信签名
        private static string SignName = string.Empty;
        // 模板编码
        private static string TemplateCode = string.Empty;
        /// <summary>
        /// 获取SMS信息结构
        /// </summary>
        /// <returns></returns>
        public static SMSInfoModule GetSMSModule()
        {
            SMSInfoModule info = new SMSInfoModule();

            DataTable table = ExtendDictionary.GetDictionaryByCode(DataDictionaryModuleIndex.ERPSMSUploadPathConfig);
            if (table != null &&
                table.Rows.Count > 0)
            {
                string smsInfo = table.Rows[0]["SGDDictValue"].ToString();
                if (!string.IsNullOrEmpty(smsInfo) && smsInfo.Trim().Length > 0)
                {
                    string[] ftpArray = smsInfo.Split('@');
                    if (ftpArray.Length == 4)
                    {
                        //短信发送需要端口，账号，密码
                        info.Url = ftpArray[0];
                        info.Account = ftpArray[1];
                        info.Password = ftpArray[2].DecryptDES();
                    }
                }
            }
            return info;
        }

        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="moblie">手机号</param>
        /// <param name="context">发送内容</param>
        /// <returns></returns>
        public static string SMSSend(string moblie, string context)
        {
            SMSInfoModule smsInfo = new SMSInfoModule();
            string encode = "utf8";
            try
            {
                smsInfo = GetSMSModule();
                string encodeType = "base64";
                string pswdMd5 = SMSmd5(smsInfo.Password);
                Encoding unicode = Encoding.Unicode;
                Encoding utf8 = Encoding.GetEncoding("UTF-8");
                byte[] unicodeBytes = unicode.GetBytes(context);
                byte[] utf8Bytes = Encoding.Convert(unicode, utf8, unicodeBytes);
                string contentToSend = Convert.ToBase64String(utf8Bytes);
                IDictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("uid", smsInfo.Account);
                parameters.Add("password", pswdMd5);
                parameters.Add("mobile", moblie);
                parameters.Add("encode", encode);
                parameters.Add("content", contentToSend);
                parameters.Add("encodeType", encodeType);
                HttpWebResponse res = HelperHttp.CreatePostHttpResponse(smsInfo.Url, parameters);
                return HelperHttp.GetResponseString(res);
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// 获取短信平台接口地址
        /// </summary>
        /// <returns></returns>
        public static string[] GetSendUrl()
        {
            string[] url = { };
            DataTable dt = ExtendDictionary.GetDictionaryByCode(DataDictionaryModuleIndex.ERPSMSUploadPathConfig);
            if (dt != null && dt.Rows.Count > 0)
            {
                string info = dt.Rows[0]["SGDDictValue"].ToString();
                if (!string.IsNullOrEmpty(info) && info.Trim().Length > 0)
                {
                    url = info.Split('@');
                }
            }
            return url;
        }

        /// <summary>
        /// 企信通发送短信
        /// </summary>
        /// <param name="moblie">手机号</param>
        /// <param name="context">发送内容</param>
        /// <returns></returns>
        public static string QXTSend(string moblie, string context)
        {
            try
            {
                string url = string.Empty;
                string[] info = GetSendUrl();
                Encoding unicode = Encoding.Unicode;
                Encoding utf8 = Encoding.GetEncoding("UTF-8");
                byte[] unicodeBytes = unicode.GetBytes(context);
                byte[] utf8Bytes = Encoding.Convert(unicode, utf8, unicodeBytes);
                string contentToSend = Convert.ToBase64String(utf8Bytes);
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                //发送短信示例http://web.186117.cn/sms.aspx?action=send&userid=企业ID&account=账号&password=密码&mobile=13708989179,13212345678&content=内容&sendTime=&checkcontent=1
                if (info != null && info.Length > 3)
                {
                    url = info[0];
                    //userid:企业ID
                    parameters.Add("userid", info[1]);
                    //account:发送用户帐号,用户帐号，(建议使用英文或数字)
                    parameters.Add("account", info[2]);
                    //password:发送帐号密码,用户账号对应的密码
                    parameters.Add("password", info[3]);
                    //mobile:全部被叫号码,发信发送的目的号码.多个号码之间用半角逗号隔开            
                    parameters.Add("mobile", moblie);
                    //content:发送内容	短信的内容
                    parameters.Add("content", context);
                    //sendTime:定时发送时间	为空表示立即发送，定时发送格式2010-10-24 09:08:10
                    //action:发送任务命令	设置为固定的:send
                    parameters.Add("action", "send");
                    //checkcontent:是否检查内容 当设置为1时表示需要检查，默认0为不检查
                    parameters.Add("checkcontent", "1");
                    return KdApi.sendPost(url, parameters);
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return string.Empty;
            }

        }

        /// <summary>
        /// 短信发送加密（专用）
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static String SMSmd5(String s)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
            bytes = md5.ComputeHash(bytes);
            md5.Clear();

            string ret = "";
            for (int i = 0; i < bytes.Length; i++)
            {
                ret += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
            }

            return ret.PadLeft(32, '0').ToUpper();
        }


        private static void GetAliyuncsServersProfile(TemplateTypes templateTypes)
        {
            DataTable dataTable = SQL.GetDataTable("SELECT SGDName,SGDCode,SGDDictValue FROM Sys_GlobalDict WHERE SGDParentCode='AlicloudSMSServiceConfiguration'");
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                DataRow[] dr = dataTable.Select("SGDCode='" + DataDictionaryModuleIndex.ALIYUNCSENDPOINT + "'");
                if (dr != null && dr.Length > 0)
                {
                    Endpoint = dr[0]["SGDDictValue"].ToString();
                }

                dr = dataTable.Select("SGDCode='" + DataDictionaryModuleIndex.ACCESSKEYID + "'");
                if (dr != null && dr.Length > 0)
                {
                    AccessKeyId = dr[0]["SGDDictValue"].ToString();
                }


                dr = dataTable.Select("SGDCode='" + DataDictionaryModuleIndex.ACCESSKEYSECRET + "'");
                if (dr != null && dr.Length > 0)
                {
                    AccessKeySecret = dr[0]["SGDDictValue"].ToString();
                }
                dr = dataTable.Select("SGDCode='" + DataDictionaryModuleIndex.SIGNNAME + "'");
                if (dr != null && dr.Length > 0)
                {
                    SignName = dr[0]["SGDDictValue"].ToString();
                }
                if (templateTypes == TemplateTypes.送货单模板)
                {
                    dr = dataTable.Select("SGDCode='" + DataDictionaryModuleIndex.DELIVERYNOTETEMPLATECODE + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        TemplateCode = dr[0]["SGDDictValue"].ToString();
                    }
                }
                else if (templateTypes == TemplateTypes.验证码模板)
                {
                    dr = dataTable.Select("SGDCode='" + DataDictionaryModuleIndex.VERIFICATIONTEMPLATECODE + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        TemplateCode = dr[0]["SGDDictValue"].ToString();
                    }
                }
                else if (templateTypes == TemplateTypes.客户祝福模板)
                {
                    dr = dataTable.Select("SGDCode='" + DataDictionaryModuleIndex.CUSTOMERBLESSINGTEMPLATECODE + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        TemplateCode = dr[0]["SGDDictValue"].ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 阿里云发送短信
        /// </summary>
        /// <param name="mobile">手机号码</param>
        /// <param name="templateTypes">模板类型</param>
        /// <param name="param">模板参数</param>
        /// <returns></returns>
        public static string SendSms(string mobile, TemplateTypes templateTypes, object param)
        {
            GetAliyuncsServersProfile(templateTypes);
            string templateParam = HelperJson.SerializeObject(param);
            // UrlEncode的特殊转换
            Func<string, string> specialUrlEncode = (string temp) =>
            {
                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < temp.Length; i++)
                {
                    string t = temp[i].ToString();
                    string k = System.Web.HttpUtility.UrlEncode(t, Encoding.UTF8);
                    if (t == k)
                    {
                        stringBuilder.Append(t);
                    }
                    else
                    {
                        stringBuilder.Append(k.ToUpper());
                    }
                }

                return stringBuilder.ToString().Replace("+", "%20").Replace("*", "%2A").Replace("%7E", "~");
            };

            string nowDate = SQL.GetTime().ToUniversalTime().ToString("yyyy-MM-dd'T'HH:mm:ss'Z'");//GTM时间
            System.Collections.Generic.Dictionary<string, string> keyValues = new System.Collections.Generic.Dictionary<string, string>();//声明一个字典

            #region 系统参数                                        
            keyValues.Add("SignatureMethod", "HMAC-SHA1");
            keyValues.Add("SignatureNonce", Guid.NewGuid().ToString()); // ==========
            keyValues.Add("AccessKeyId", AccessKeyId);
            keyValues.Add("SignatureVersion", "1.0");
            keyValues.Add("Timestamp", nowDate);
            keyValues.Add("Format", "Json");//可换成xml
            #endregion

            #region 业务api参数
            keyValues.Add("Action", "SendSms");
            keyValues.Add("Version", "2017-05-25");
            keyValues.Add("RegionId", "cn-hangzhou");
            keyValues.Add("PhoneNumbers", mobile);
            keyValues.Add("SignName", SignName);
            keyValues.Add("TemplateParam", templateParam);
            keyValues.Add("TemplateCode", TemplateCode);
            keyValues.Add("OutId", string.Empty);
            #endregion

            //去除签名关键字Signature key
            if (keyValues.ContainsKey("Signature"))
            {
                keyValues.Remove("Signature");
            }

            // 参数key排序
            System.Collections.Generic.Dictionary<string, string> ascDic = keyValues.OrderBy(o => o.Key).ToDictionary(o => o.Key, p => p.Value.ToString());

            #region 构造待签名的字符串
            StringBuilder builder = new StringBuilder();
            foreach (var item in ascDic)
            {
                if (item.Key == "SignName")
                {
                }
                else
                {
                    builder.Append("&").Append(specialUrlEncode(item.Key)).Append("=").Append(specialUrlEncode(item.Value));
                }
                if (item.Key == "RegionId")
                {
                    builder.Append("&").Append(specialUrlEncode("SignName")).Append("=").Append(specialUrlEncode(keyValues["SignName"]));
                }
            }
            string sorteQueryString = builder.ToString().Substring(1);
            #endregion

            StringBuilder stringToSign = new StringBuilder();
            stringToSign.Append("GET").Append("&");
            stringToSign.Append(specialUrlEncode("/")).Append("&");
            stringToSign.Append(specialUrlEncode(sorteQueryString));

            #region macsha加密
            var hmacsha1 = new System.Security.Cryptography.HMACSHA1(Encoding.UTF8.GetBytes(AccessKeySecret + "&"));
            var dataBuffer = Encoding.UTF8.GetBytes(stringToSign.ToString());
            var hashBytes = hmacsha1.ComputeHash(dataBuffer);
            string stringbyte = BitConverter.ToString(hashBytes, 0).Replace("-", string.Empty).ToLower();
            string hexString = stringbyte.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            string Sign = Convert.ToBase64String(bytes);
            #endregion

            // 签名特殊URL编码
            string signture = specialUrlEncode(Sign);

            // 格式化GET请求的URL
            string url = string.Format("http://{0}/?Signature={1}{2}", Endpoint, signture, builder);

            #region 请求发送短信
            string strRet = string.Empty;
            if (url == null || url.Trim().ToString() == "")
            {
                return strRet;
            }
            string targeturl = url.Trim().ToString();
            try
            {
                HttpWebRequest hr = (HttpWebRequest)WebRequest.Create(targeturl);
                hr.UserAgent = "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)";
                hr.Method = "GET";
                hr.Timeout = 30 * 60 * 1000;
                WebResponse hs = hr.GetResponse();
                Stream sr = hs.GetResponseStream();
                StreamReader ser = new StreamReader(sr, Encoding.UTF8);

                strRet = MessageHandle(ser.ReadToEnd());
            }
            catch (Exception ex)
            {
                strRet = "短信发送失败！" + ex.Message;
            }
            #endregion

            return strRet;
        }
        /// <summary>
        /// 消息处理机制
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string MessageHandle(string str)
        {
            MessageModel message = JsonConvert.DeserializeObject<MessageModel>(str);
            string result = "";
            switch (message.Code)
            {
                case "OK":
                    result = "短信发送成功！";
                    break;
                case "isp.RAM_PERMISSION_DENY":
                    result = "RAM权限DENY";
                    break;
                case "isv.OUT_OF_SERVICE":
                    result = "业务停机";
                    break;
                case "isv.PRODUCT_UN_SUBSCRIPT":
                    result = "未开通云通信产品的阿里云客户";
                    break;
                case "isv.PRODUCT_UNSUBSCRIBE":
                    result = "产品未开通";
                    break;
                case "isv.ACCOUNT_NOT_EXISTS":
                    result = "账户不存在";
                    break;
                case "isv.ACCOUNT_ABNORMAL":
                    result = "账户异常	";
                    break;
                case "isv.SMS_TEMPLATE_ILLEGAL":
                    result = "短信模板不合法";
                    break;
                case "isv.SMS_SIGNATURE_ILLEGAL":
                    result = "短信签名不合法";
                    break;
                case "isv.INVALID_PARAMETERS":
                    result = "参数异常";
                    break;
                case "isv.MOBILE_NUMBER_ILLEGAL":
                    result = "非法手机号";
                    break;
                case "isv.MOBILE_COUNT_OVER_LIMIT":
                    result = "手机号码数量超过限制";
                    break;
                case "isv.TEMPLATE_MISSING_PARAMETERS":
                    result = "模板缺少变量";
                    break;
                case "isv.BUSINESS_LIMIT_CONTROL":
                    result = "业务限流";
                    break;
                case "isv.INVALID_JSON_PARAM":
                    result = "JSON参数不合法，只接受字符串值";
                    break;
                case "isv.PARAM_LENGTH_LIMIT":
                    result = "参数超出长度限制";
                    break;
                case "isv.PARAM_NOT_SUPPORT_URL":
                    result = "不支持URL";
                    break;
                case "isv.AMOUNT_NOT_ENOUGH":
                    result = "账户余额不足";
                    break;
                case "isv.TEMPLATE_PARAMS_ILLEGAL":
                    result = "模板变量里包含非法关键字";
                    break;
            }
            return result;
        }
    }
}


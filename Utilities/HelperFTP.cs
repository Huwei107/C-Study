//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperFTP.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   刘少林
//  创建时间:   2017-2-13 16:57:03
//  功能描述:   自定义FTP类
//  历史版本:
//          2017-2-13 16:57:03 刘少林 创建HelperFTP类
// </copyright>
//-----------------------------------------------------------------------
using System;
using FX.Entity;
using System.Data;
using System.Net;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;

namespace FX.MainForms
{
    /// <summary>
    /// 自定义FTP类
    /// </summary>
    public class HelperFTP
    {
        /// <summary>
        /// 获取FTP信息结构
        /// </summary>
        /// <returns></returns>
        public static FtpInfoModule GetFTPModule()
        {
            FtpInfoModule info = new FtpInfoModule();

            DataTable table = ExtendDictionary.GetDictionaryByCode(DataDictionaryModuleIndex.ERPFTPUploadImgPathConfig);
            if (table != null &&
                table.Rows.Count > 0)
            {
                string ftpInfo = table.Rows[0][Sys_GlobalDict.SGDDictValue_ColumnName].ToString();
                if (!string.IsNullOrEmpty(ftpInfo) && ftpInfo.Trim().Length > 0)
                {
                    string[] ftpArray = ftpInfo.Split('@');
                    if (ftpArray.Length == 3)
                    {
                        //ftp必须要有账号和密码还有FTP路径
                        info.Url = ftpArray[0];
                        info.Account = ftpArray[1];
                        info.Password = ftpArray[2].DecryptDES();
                    }
                }
            }
            return info;
        }

        /// <summary>
        /// ftp方式上传 
        /// </summary>
        public static string UploadFtp(string filePath, string partialPath, FTPFolderTypes type)
        {
            //FTP上传下载账号
            string ftpUserAccount = string.Empty;
            //FTP上传下载账号对应密码
            string ftpPassword = string.Empty;

            string ftpServerPath = string.Empty;
            FtpInfoModule ftp = new FtpInfoModule();
            try
            {
                ftp = GetFTPModule();
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
            }
            if (string.IsNullOrEmpty(ftp.Account) ||
                string.IsNullOrEmpty(ftp.Url) ||
                string.IsNullOrEmpty(ftp.Password))
            {
                //FTP服务器配置信息为空，则无法上传FTP服务器
                HelperLog.Write("FTP服务器配置信息为空，则无法上传FTP服务器");
                return string.Empty;
            }
            else
            {
                ftpUserAccount = ftp.Account;
                ftpPassword = ftp.Password;
            }
            if (!File.Exists(filePath))
            {
                //上传本地文件不存在
                HelperLog.Write("上传本地文件不存在");
                return string.Empty;
            }
            if (string.IsNullOrEmpty(partialPath))
            {
                //FTP服务器文件名称路径不能为空
                HelperLog.Write("FTP服务器文件名称路径不能为空");
                return string.Empty;
            }
            else
            {
                //移除部分路径的前缀，统一处理，避免有的没有传，有的有传
                partialPath = partialPath.TrimStart('/');
                ftpServerPath = ftp.Url.TrimEnd('/') + "/" + type.ToString() + "/" + partialPath;
                FtpMakeDirectory(ftp.Url.TrimEnd('/') + "/" + type.ToString());
                string yy = ftp.Url.TrimEnd('/') + "/" + type.ToString();
                string fi = ftpServerPath.Substring(0, ftpServerPath.LastIndexOf('/'));
                FtpMakeDirectory(ftpServerPath.Substring(0, ftpServerPath.LastIndexOf('/')));
            }
            //FtpMakeDirectory(ftpServerPath.Substring(0, ftpServerPath.LastIndexOf('/')));
            FileInfo fileInf = new FileInfo(filePath);
            FtpWebRequest reqFTP;
            // Create FtpWebRequest object from the Uri provided 
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpServerPath));
            try
            {
                // Provide the WebPermission Credintials 
                reqFTP.Credentials = new NetworkCredential(ftpUserAccount, ftpPassword);
                //reqFTP.
                // By default KeepAlive is true, where the control connection is not closed 
                // after a command is executed. 
                reqFTP.KeepAlive = false;

                // Specify the command to be executed. 
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;


                // Specify the data transfer type. 
                reqFTP.UseBinary = true;

                // Notify the server about the size of the uploaded file 
                reqFTP.ContentLength = fileInf.Length;

                // The buffer size is set to 2kb 
                int buffLength = 2048;
                byte[] buff = new byte[buffLength];
                int contentLen;

                // Opens a file stream (System.IO.FileStream) to read the file to be uploaded 
                //FileStream fs = fileInf.OpenRead(); 
                FileStream fs = fileInf.Open(FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                // Stream to which the file to be upload is written 
                Stream strm = reqFTP.GetRequestStream();

                // Read from the file stream 2kb at a time 
                contentLen = fs.Read(buff, 0, buffLength);

                // Till Stream content ends 
                while (contentLen != 0)
                {
                    // Write Content from the file stream to the FTP Upload Stream 
                    strm.Write(buff, 0, contentLen);
                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // Close the file stream and the Request Stream 
                strm.Close();
                fs.Close();
                //DownloadFtp(ftpServerPath);
                return ftpServerPath;
            }
            catch (Exception ex)
            {
                reqFTP.Abort();
                HelperLog.Write(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// ftp方式下载 
        /// </summary>
        /// <param name="ftpUrl">ftp路径</param>
        /// <returns></returns>
        public static MemoryStream DownloadFtp(string ftpUrl)
        {
            if (string.IsNullOrEmpty(ftpUrl) || ftpUrl.Length <= 2)
            {
                HelperLog.Write("ftpUrl参数传递不合法!");
                return null;
            }
            MemoryStream ms = null;
            FtpWebRequest reqFTP;

            //FTP上传下载账号
            string ftpUserAccount = string.Empty;
            //FTP上传下载账号对应密码
            string ftpPassword = string.Empty;

            string ftpServerPath = string.Empty;
            FtpInfoModule ftp = null;
            try
            {
                ftp = GetFTPModule();
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
            }
            if (ftp == null || string.IsNullOrEmpty(ftp.Account) ||
                string.IsNullOrEmpty(ftp.Url) ||
                string.IsNullOrEmpty(ftp.Password))
            {
                //FTP服务器配置信息为空，则无法上传FTP服务器
                HelperLog.Write("FTP服务器配置信息为空，则无法上传FTP服务器");
                return null;
            }
            else
            {
                ftpUserAccount = ftp.Account;
                ftpPassword = ftp.Password;
            }

            //filePath = < <The full path where the file is to be created.>>, 
            //fileName = < <Name of the file to be created(Need not be the name of the file on FTP server).>> 
            //FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);
            string imgTfpUrl = ftp.Url + ftpUrl.Substring(2, ftpUrl.Length - 2);
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(imgTfpUrl));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = true;
                reqFTP.KeepAlive = false;
                reqFTP.Credentials = new NetworkCredential(ftpUserAccount, ftpPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                ms = new MemoryStream();
                while (readCount > 0)
                {
                    ms.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                response.Close();
                return ms;
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
                return null;
            }
        }

        /// <summary>
        /// 下载图片
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static Image DownloadImage(string Url)
        {
            if (string.IsNullOrEmpty(Url) || Url.Length <= 2)
            {
                HelperLog.Write("Url参数传递不合法!");
                return null;
            }
            try
            {
                System.Net.WebRequest webreq = System.Net.WebRequest.Create(Url);
                webreq.Timeout = 2000;
                System.Net.WebResponse webres = webreq.GetResponse();
                Image image = null;
                using (System.IO.Stream stream = webres.GetResponseStream())
                {
                    image = Image.FromStream(stream);
                }
                return image;
            }
            catch(Exception ex)
            {
                HelperLog.Write(ex);
                return null;
            }
        }

        /// <summary>
        /// 根据ftp地址ftpUrl，将文件下载到本地地址dirPath
        /// </summary>
        /// <param name="ftpUrl">ftp地址</param>
        /// <param name="dirPath">本地地址</param>
        public static void DownLoad(string ftpUrl, string dirPath)
        {
            /*首先从配置文件读取ftp的登录信息*/
            //string TempFolderPath = "DownLoad"; //指定文件夹存储
            string FtpUserName = string.Empty;
            string FtpPassWord = string.Empty;
            FtpInfoModule ftp = null;
            try
            {
                ftp = GetFTPModule();
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
            }
            if (ftp == null || string.IsNullOrEmpty(ftp.Account) ||
                string.IsNullOrEmpty(ftp.Url) ||
                string.IsNullOrEmpty(ftp.Password))
            {
                //FTP服务器配置信息为空，则无法上传FTP服务器
                HelperLog.Write("FTP服务器配置信息为空，则无法上传FTP服务器");
            }
            else
            {
                FtpUserName = ftp.Account;
                FtpPassWord = ftp.Password;
            }
            string FtpPath = ftp.Url + ftpUrl.Substring(2, ftpUrl.Length - 2);
            Uri uri = new Uri(FtpPath);
            string FileName = dirPath + Path.DirectorySeparatorChar.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(uri.LocalPath);
            //指定文件夹存储
            //string FileName = Path.GetFullPath(TempFolderPath) + Path.DirectorySeparatorChar.ToString() + Path.GetFileName(uri.LocalPath);

            //创建一个文件流
            FileStream fs = null;
            Stream responseStream = null;
            try
            {
                //创建一个与FTP服务器联系的FtpWebRequest对象
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(uri);
                //设置请求的方法是FTP文件下载
                request.Method = WebRequestMethods.Ftp.DownloadFile;

                //连接登录FTP服务器
                request.Credentials = new NetworkCredential(FtpUserName, FtpPassWord);

                //获取一个请求响应对象
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                //获取请求的响应流
                responseStream = response.GetResponseStream();

                //判断本地文件是否存在，如果存在，删除本地文件
                if (File.Exists(FileName))
                {
                    //删除这个文件
                    File.Delete(FileName);
                }
                //创建本地文件
                fs = File.Create(FileName);

                if (fs != null)
                {
                    int buffer_count = 65536;
                    byte[] buffer = new byte[buffer_count];
                    int size = 0;
                    while ((size = responseStream.Read(buffer, 0, buffer_count)) > 0)
                    {
                        fs.Write(buffer, 0, size);
                    }
                    fs.Flush();
                    fs.Close();
                    responseStream.Close();
                }
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
            }
            finally
            {
                if (fs != null)
                    fs.Close();
                if (responseStream != null)
                    responseStream.Close();
            }
        }

        /// <summary>
        /// 根据ftp服务器相对地址通过浏览器浏览图纸
        /// </summary>
        /// <param name="ftpUrl"></param>
        public static void LookUpPicture(string ftpUrl)
        {
            /*首先从配置文件读取ftp的登录信息*/
            string FtpUserName = string.Empty;
            string FtpPassWord = string.Empty;
            FtpInfoModule ftp = null;
            try
            {
                ftp = GetFTPModule();
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
            }
            if (ftp == null || string.IsNullOrEmpty(ftp.Account) ||
                string.IsNullOrEmpty(ftp.Url) ||
                string.IsNullOrEmpty(ftp.Password))
            {
                //FTP服务器配置信息为空，则无法上传FTP服务器
                HelperLog.Write("FTP服务器配置信息为空，则无法上传FTP服务器");
            }
            else
            {
                FtpUserName = ftp.Account;
                FtpPassWord = ftp.Password;
            }
            System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
            string FtpPath = ftp.Url.Insert(6, asciiToStr(asciiEncoding.GetBytes(ftp.Account)) + ":" + asciiToStr(asciiEncoding.GetBytes(ftp.Password)) + "@") + ftpUrl.Substring(2, ftpUrl.Length - 2);
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            System.Diagnostics.Process.Start("explorer.exe", FtpPath);
            ShellExecute(0, @"open", FtpPath, null, null, (int)ShowWindowCommands.SW_NORMAL);
        }

        private static string asciiToStr(byte[] buf)
        {
            string str = string.Empty;
            for (int i = 0; i < buf.Length; i++)
            {
                str += "%" + Convert.ToString(buf[i].ToString().ToInt(), 16);
            }
            return str;
        }

        /// <summary>
        /// 使用ShellExecute，从浏览器打开文件，可以解决被360拦截问题
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lpOperation"></param>
        /// <param name="lpFile"></param>
        /// <param name="lpParameters"></param>
        /// <param name="lpDirectory"></param>
        /// <param name="nShowCmd"></param>
        /// <returns></returns>
        [DllImport("shell32.dll")]
        public extern static IntPtr ShellExecute(int hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, int nShowCmd);
        public enum ShowWindowCommands : int
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 10
        }


        public static Boolean FtpMakeDirectory(string ftpServerIP)
        {
            //FTP上传下载账号
            string ftpUserAccount = string.Empty;
            //FTP上传下载账号对应密码
            string ftpPassword = string.Empty;

            string ftpServerPath = string.Empty;
            FtpInfoModule ftp = new FtpInfoModule();
            try
            {
                ftp = GetFTPModule();
            }
            catch (Exception ex)
            {
                HelperLog.Write(ex);
            }
            if (string.IsNullOrEmpty(ftp.Account) ||
                string.IsNullOrEmpty(ftp.Url) ||
                string.IsNullOrEmpty(ftp.Password))
            {
                //FTP服务器配置信息为空，则无法上传FTP服务器
                HelperLog.Write("FTP服务器配置信息为空，则无法上传FTP服务器");
                return false;
            }
            else
            {
                ftpUserAccount = ftp.Account;
                ftpPassword = ftp.Password;
            }
            FtpWebRequest req = (FtpWebRequest)WebRequest.Create(ftpServerIP);
            req.Credentials = new NetworkCredential(ftpUserAccount, ftpPassword);
            req.Method = WebRequestMethods.Ftp.MakeDirectory;
            try
            {
                FtpWebResponse response = (FtpWebResponse)req.GetResponse();
                response.Close();
            }
            catch (Exception)
            {
                req.Abort();
                return false;
            }
            req.Abort();
            return true;
        }


        /// <summary>  
        /// 文件存在检查  
        /// </summary>  
        /// <param name="ftpPath"></param>  
        /// <param name="ftpName"></param>  
        /// <returns></returns>  
        public static bool fileCheckExist(string ftpPath, string ftpName, string ftpUserID, string ftpPassword)
        {
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            WebResponse webResponse = null;
            StreamReader reader = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpWebRequest.KeepAlive = false;
                webResponse = ftpWebRequest.GetResponse();
                reader = new StreamReader(webResponse.GetResponseStream());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line == ftpName)
                    {
                        success = true;
                        break;
                    }
                    line = reader.ReadLine();
                }
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (webResponse != null)
                {
                    webResponse.Close();
                }
            }
            return success;
        }


        /// <summary>  
        /// 重命名  
        /// </summary>  
        /// <param name="ftpPath">ftp文件路径</param>  
        /// <param name="currentFilename"></param>  
        /// <param name="newFilename"></param>  
        public static bool fileRename(string ftpPath, string newFileName, string ftpUserID, string ftpPassword)
        {
            bool success = false;
            FtpWebRequest ftpWebRequest = null;
            FtpWebResponse ftpWebResponse = null;
            Stream ftpResponseStream = null;
            try
            {
                ftpWebRequest = (FtpWebRequest)FtpWebRequest.Create(new Uri(ftpPath));
                ftpWebRequest.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
                ftpWebRequest.UseBinary = true;
                ftpWebRequest.Method = WebRequestMethods.Ftp.Rename;
                ftpWebRequest.RenameTo = newFileName;
                ftpWebResponse = (FtpWebResponse)ftpWebRequest.GetResponse();
                ftpResponseStream = ftpWebResponse.GetResponseStream();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            finally
            {
                if (ftpResponseStream != null)
                {
                    ftpResponseStream.Close();
                }
                if (ftpWebResponse != null)
                {
                    ftpWebResponse.Close();
                }
            }
            return success;
        }

    }//end class
}//end namespace

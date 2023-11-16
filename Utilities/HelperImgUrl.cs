
//-----------------------------------------------------------------------
// <copyright company="工品一号" file="HelperImgUrl">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   唐亮
//  创建时间:   2018-11-14 13:10:24
//  功能描述:   【请输入类描述】
//  历史版本:
//          2018-11-14 13:10:24 唐亮 创建HelperImgUrl类
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using FX.Entity;

namespace FX.MainForms
{
    /// <summary>
    /// 【请输入类描述】
    /// </summary>
  public  class HelperImgUrl
    {
        /// <summary>
        /// 获取产品图片全路径地址
        /// </summary>
        /// <param name="dbProductImagePartUrl">数据库产品图片部分路径</param>
        /// <returns>产品图片全路径地址</returns>
      public Image GetProductImageUrl(string dbProductImagePartUrl)
      {
          Image image = null;
          var globalInfo = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName, DataDictionaryModuleIndex.ImgUploadPath);
          if (globalInfo != null)
          {
              if (string.IsNullOrEmpty(globalInfo.SGDDictValue))
              {
                  if (string.IsNullOrEmpty(dbProductImagePartUrl))
                  {
                      return null;
                  }
                  else {
                      var fullImageUrl = string.Empty;
                      var prefixPartUrl = globalInfo.SGDDictValue.Trim();
                      int handleLength = prefixPartUrl.Length - 1;
                      if (prefixPartUrl.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase) == handleLength)
                      {
                          //移除最后的反斜杠
                          prefixPartUrl = prefixPartUrl.Substring(0, handleLength);
                      }
                      if (prefixPartUrl.IndexOf(":", StringComparison.CurrentCultureIgnoreCase) < 0)
                      {
                         // LogHelper.Write("图片地址没有配置端口，系统将会出现异常问题【程序临时配置了端口:98】");
                          //没默认端口则添加默认端口
                          prefixPartUrl = ":98";
                      }
                      dbProductImagePartUrl = dbProductImagePartUrl.Replace("\\", "/");
                      if (dbProductImagePartUrl.IndexOf("/", StringComparison.CurrentCultureIgnoreCase) != 0)
                      {
                          //数据库部分图片路径前缀没有反斜杠，则 补充上去
                          dbProductImagePartUrl = "/" + dbProductImagePartUrl;
                      }
                      fullImageUrl = prefixPartUrl + dbProductImagePartUrl;
                      MemoryStream buf = getImageByte(fullImageUrl);
                      image = Image.FromStream(buf, true);
                  }
              }
              else
              {

                  if (globalInfo.SGDBinaryContent != null && globalInfo.SGDBinaryContent.Length > 0)
                  {
                      using (MemoryStream buf = new MemoryStream(globalInfo.SGDBinaryContent))
                      {
                          image = Image.FromStream(buf, true);
                      }
                  }
                  else
                  {
                      return null;
                  }
              }
          }
          else
          {

              return null;
          }
         return image;
      }

      #region 返回图片的字节流byte[]
      /// <summary>
      /// 返回图片的字节流byte[]
      /// </summary>
      /// <param name="imagePath"></param>
      /// <param name="webClient"></param>
      /// <returns></returns>
      public static MemoryStream getImageByte(string imagePath)
      {
           MemoryStream ms =  null;
          try
          {
              WebRequest request = WebRequest.Create(imagePath);
              WebResponse response = request.GetResponse();
              Stream s = response.GetResponseStream();
              byte[] data = new byte[1024];
              int length = 0;
              
              while ((length = s.Read(data, 0, data.Length)) > 0)
              {
                  ms.Write(data, 0, length);
              }
              ms.Seek(0, SeekOrigin.Begin);
          }
          catch (Exception ee)
          {
                HelperLog.Write(ee);
            }
          return ms;
      }


      /// <summary>
      /// 获取产品图片全路径地址
      /// </summary>
      /// <param name="dbProductImagePartUrl">数据库产品图片部分路径</param>
      /// <returns>产品图片全路径地址</returns>
      public static string GetProductImageUrlStatic(string dbProductImagePartUrl)
      {
          string path = Process.GetCurrentProcess().MainModule.FileName.Replace(".vshost", "") + ".config";
          string defaultImageUrl = HelperConfig.GetConfigValue(path, "DefaultDrawing");
          if (string.IsNullOrEmpty(defaultImageUrl))
          {
              //没有配置默认图片
              defaultImageUrl = "ErroDefaultImg";
          }
          if (string.IsNullOrEmpty(dbProductImagePartUrl))
          {
              return defaultImageUrl;
          }
          string fullImageUrl;
          string prefixPartUrl = string.Empty;
          //默认外网,方便外部网络可访问本ERP图片地址
          if (string.IsNullOrEmpty(prefixPartUrl) || prefixPartUrl.IndexOf("http://", StringComparison.CurrentCultureIgnoreCase) < 0)
          {
              //获取图片服务器路径(不包含产品部分路径)
              var dictAddr = SQL.GetEntity<Sys_GlobalDict>(Sys_GlobalDict.SGDCode_ColumnName,DataDictionaryModuleIndex.ImgUploadPath);

              if (dictAddr != null && dictAddr.SGDDictValue!=string.Empty)
              {
                  prefixPartUrl = dictAddr.SGDDictValue;
              }               
            
          }
          if (string.IsNullOrEmpty(prefixPartUrl) || prefixPartUrl.IndexOf("http://", StringComparison.CurrentCultureIgnoreCase) < 0)
          {
              //本地址默认FAST内部访问无图纸路径
              fullImageUrl = "ErrorDefaultServerUrl";
          }
          else
          {
              prefixPartUrl = prefixPartUrl.Trim();
              int handleLength = prefixPartUrl.Length - 1;
              if (prefixPartUrl.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase) == handleLength)
              {
                  //移除最后的反斜杠
                  prefixPartUrl = prefixPartUrl.Substring(0, handleLength);
              }
              if (prefixPartUrl.IndexOf(":", StringComparison.CurrentCultureIgnoreCase) < 0)
              {
                //  LogHelper.Write("图片地址没有配置端口，系统将会出现异常问题【程序临时配置了端口:98】");
                  //没默认端口则添加默认端口
                  prefixPartUrl = ":98";
              }
              dbProductImagePartUrl = dbProductImagePartUrl.Replace("\\", "/");
              if (dbProductImagePartUrl.IndexOf("/", StringComparison.CurrentCultureIgnoreCase) != 0)
              {
                  //数据库部分图片路径前缀没有反斜杠，则 补充上去
                  dbProductImagePartUrl = "/" + dbProductImagePartUrl;
              }
              fullImageUrl = prefixPartUrl + dbProductImagePartUrl;
          }
          if (fullImageUrl.IndexOf("/image", StringComparison.CurrentCultureIgnoreCase) > 0)
          {
              fullImageUrl = fullImageUrl.Replace("/image", string.Empty);
          }
          return fullImageUrl;
      }


      #endregion



    }
}

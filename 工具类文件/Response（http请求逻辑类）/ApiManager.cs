//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ApiManager.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   王健
//  创建时间:   2023/8/9 11:17:27 
//  功能描述:   
//  历史版本:
//          2023/8/9 11:17:27 王健 创建ApiManager类
// </copyright>
//-----------------------------------------------------------------------
using FX.MainForms.Common.Context;
using FX.MainForms.Common.Models.Bos;
using FX.MainForms.Common.Models.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FX.MainForms.Common.Response
{
    public class ApiManager
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        /// <param name="userParam"></param>
        /// <returns></returns>
        public static async Task<(LoginBo user, string msg)> LoginUserAsync(LoginParam userParam)
        {
            var result = await ResponseService.PostAsync<LoginBo>(
                ApiMappering.Login, userParam);
            if (!(result?.Success ?? false))
            {
                return (null, result?.Message ?? "系统异常");
            }
            EnvironmentContext.Instance.User = result.Data;
            if (EnvironmentContext.Instance.Header == null) EnvironmentContext.Instance.Header = new Dictionary<string, string>();
            EnvironmentContext.Instance.Header.Clear();
            EnvironmentContext.Instance.Header.Add("Authorization", result.Data.TokenType + result.Data.AccessToken);
            return (result.Data, "");
        }
    }
}

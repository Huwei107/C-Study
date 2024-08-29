//-----------------------------------------------------------------------
// <copyright company="工品一号" file="ApiControllerBase.cs">
//  Copyright (c)  V1.0.0.0  
//  创建作者:   胡威
//  创建时间:   2022/11/1 14:59:52
//  功能描述:   
//  历史版本:
//          2022/11/1 14:59:52 胡威 创建ApiControllerBase类
// </copyright>
//-----------------------------------------------------------------------
using Microsoft.AspNetCore.Mvc;

namespace BCVP.Net8.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    [Route("v1/[controller]")]
    public class ApiControllerBase : ControllerBase
    {
    }
}

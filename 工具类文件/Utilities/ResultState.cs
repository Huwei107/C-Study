using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FX.MainForms
{
    /// <summary>
    /// 返回结果
    /// </summary>
    [Serializable]
    public class ResultState
    {
        /// <summary>
        /// 结果代码，0为正常，其它值参见错误代码一览表
        /// </summary>
        public string resultCode { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>
        public string resultMsg { get; set; }

        /// <summary>
        /// 处理结果，当needResult=1时，通过此字段返回json结果
        /// </summary>
        public object resultData { get; set; }

        /// <summary>
        /// 处理消息  成功失败等·
        /// </summary>
        public string debugResult { get; set; }

    }
}

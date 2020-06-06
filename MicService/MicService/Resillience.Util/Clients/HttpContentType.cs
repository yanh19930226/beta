﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Resillience.Util.Clients
{
    /// <summary>
    /// Http 内容类型
    /// </summary>
    public enum HttpContentType
    {
        /// <summary>
        /// Form格式：application/x-www-form-urlencoded
        /// </summary>
        [Description("application/x-www-form-urlencoded")]
        FormUrlEncoded,
        /// <summary>
        /// JSON格式：application/json
        /// </summary>
        [Description("application/json")]
        Json,
        /// <summary>
        /// 表单文件上传：multipart/form-data
        /// </summary>
        [Description("multipart/form-data")]
        FormData,
        /// <summary>
        /// XML格式：text/xml
        /// </summary>
        [Description("text/xml")]
        Xml
    }
}

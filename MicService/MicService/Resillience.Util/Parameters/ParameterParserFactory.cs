﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util.Parameters
{
    /// <summary>
    /// 参数解析器工厂
    /// </summary>
    public class ParameterParserFactory : IParameterParserFactory
    {
        /// <summary>
        /// 创建参数解析器
        /// </summary>
        /// <param name="type">参数解析器类型</param>
        /// <returns></returns>
        public IParameterParser CreateParameterParser(ParameterParserType type)
        {
            switch (type)
            {
                case ParameterParserType.Url:
                    return new UrlParameterParser();
                case ParameterParserType.Json:
                    return new JsonParameterParser();
                case ParameterParserType.Jsonp:
                    return new JsonpParameterParser();
                default:
                    throw new NotImplementedException();
            }
        }
    }
}

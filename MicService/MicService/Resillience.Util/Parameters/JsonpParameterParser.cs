﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.Util.Parameters
{
    /// <summary>
    /// Jsonp参数解析器
    /// </summary>
    public class JsonpParameterParser : ParameterParserBase, IParameterParser
    {
        /// <summary>
        /// 加载数据
        /// </summary>
        /// <param name="data">数据</param>
        public override void LoadData(string data)
        {
            if (data.IsNullOrEmpty())
            {
                return;
            }

            data = data.Trim().TrimEnd(';');
            var json = Regexs.GetValue(data, @"^\w+\((\s+\{[^()]+\}\s+)\)$", "$1");
            if (json.IsNullOrEmpty())
            {
                return;
            }

            var jObject = JObject.Parse(json);
            foreach (var token in jObject.Children())
            {
                AddNodes(token);
            }
        }

        /// <summary>
        /// 添加节点
        /// </summary>
        /// <param name="token">token节点</param>
        private void AddNodes(JToken token)
        {
            if (!(token is JProperty item))
            {
                return;
            }

            foreach (var value in item.Value)
            {
                AddNodes(value);
            }

            Add(item.Name, item.Value.SafeString());
        }
    }
}

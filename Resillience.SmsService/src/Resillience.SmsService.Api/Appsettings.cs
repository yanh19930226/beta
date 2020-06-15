using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api
{
    public class Appsettings
    {

        public AliConfig Ali { get; set; }
        public AliConfig Tencent { get; set; }


        public class AliConfig
        {
            public string AccessKeyId { get; set; }
            public string AccessSecret { get; set; }
        }
        public class TencentConfig
        {
            public bool IsDev { get; set; }
            public string AppId { get; set; }
            public string AppSecret { get; set; }
        }
    }
}

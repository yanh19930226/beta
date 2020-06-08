using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience.SmsService.AliSms.SDK
{
    public enum Endpoint
    {
        Send,
        Receive1
    }
    public enum ProtocolType
    {
        HTTP,
        HTTPS
    }
    public enum FormatType
    {
        XML,
        JSON,
        RAW
    }
    public enum MethodType
    {
        GET,
        PUT,
        POST,
        DELETE,
        HEAD,
        OPTIONS
    }
}

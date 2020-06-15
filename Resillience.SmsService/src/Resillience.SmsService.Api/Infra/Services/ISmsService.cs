using Resillience.Util.ResillienceResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Infra.Services
{
    public interface ISmsService
    {
        Task<dynamic> SendMessage(string PhoneNumbers, string SignName, string TemplateCode,string TemplateParam);
    }
}

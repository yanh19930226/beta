using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resilience.Zeus.Infra.Data.Map;
using Resillience.SmsService.Api.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Resillience.SmsService.Api.Infra.Data.Map
{
    public class SmsMessageConfig : ZeusEntityTypeConfiguration<SmsMessage>
    {
        public override void Config(EntityTypeBuilder<SmsMessage> builder)
        {
        }
    }
}

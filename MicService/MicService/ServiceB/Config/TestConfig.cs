using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resilience.Zeus.Infra.Data.Map;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Config
{
    public class TestConfig : ZeusEntityTypeConfiguration<TestModel>
    {
        public override void Config(EntityTypeBuilder<TestModel> builder)
        {
            
        }
    }
}

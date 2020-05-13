using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resilience.Zeus.Infra.Data.Map;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Config
{
    public class TagConfig : ZeusEntityTypeConfiguration<Tag>
    {
        public override void Config(EntityTypeBuilder<Tag> builder)
        {
            builder.HasMany(p => p.PostTags)
                  .WithOne(p => p.Tag)
                  .HasForeignKey(p => p.TagId);
        }
    }
}

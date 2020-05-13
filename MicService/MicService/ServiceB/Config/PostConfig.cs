using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resilience.Zeus.Infra.Data.Map;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Config
{
    public class PostConfig : ZeusEntityTypeConfiguration<Post>
    {
        public override void Config(EntityTypeBuilder<Post> builder)
        {
            builder.HasMany(p => p.PostTags)
                  .WithOne(p => p.Post)
                  .HasForeignKey(p => p.PostId);
        }
    }
}

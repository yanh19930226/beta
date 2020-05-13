using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Resilience.Zeus.Infra.Data.Map;
using ServiceB.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceB.Config
{
    public class BlogConfig : ZeusEntityTypeConfiguration<Blog>
    {
        public override void Config(EntityTypeBuilder<Blog> builder)
        {
            builder.HasMany(p => p.Posts)
                   .WithOne(p => p.Blog)
                   .HasForeignKey(p => p.BlogId);
        }
    }
}

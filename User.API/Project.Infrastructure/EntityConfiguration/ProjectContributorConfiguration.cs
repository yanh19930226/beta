using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectContributorConfiguration : IEntityTypeConfiguration<ProjectContributor>
    {
        public void Configure(EntityTypeBuilder<ProjectContributor> builder)
        {
        }
    }
}

﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Project.Infrastructure.EntityConfiguration
{
    public class ProjectPropertyConfiguration : IEntityTypeConfiguration<ProjectProperty>
    {
        public void Configure(EntityTypeBuilder<ProjectProperty> builder)
        {
        }
    }
}

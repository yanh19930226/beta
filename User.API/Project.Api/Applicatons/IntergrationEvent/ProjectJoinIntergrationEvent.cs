﻿using Project.Domain.AggregatesModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project.Api.Applicatons.IntergrationEvent
{
    public class ProjectJoinIntergrationEvent
    {
        public ProjectContributor Contributor { get; set; }
    }
}

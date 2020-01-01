﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ResilienceEventBus.Abstractions
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}

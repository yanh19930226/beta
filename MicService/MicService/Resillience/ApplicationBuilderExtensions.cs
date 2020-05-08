using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using System;
using System.Collections.Generic;
using System.Text;

namespace Resillience
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseResillience(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentNullException("app");
            }
            return app;
        }
    }
}

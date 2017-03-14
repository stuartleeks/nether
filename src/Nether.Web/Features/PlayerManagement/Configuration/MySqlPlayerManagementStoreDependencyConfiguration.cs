// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Nether.Common.DependencyInjection;
using Nether.Data.EntityFramework.PlayerManagement;
using Nether.Data.MySql.PlayerManagement;
using Nether.Data.PlayerManagement;

namespace Nether.Web.Features.PlayerManagement.Configuration
{
    public class MySqlPlayerManagementStoreDependencyConfiguration : DependencyConfiguration, IDependencyInitializer<IPlayerManagementStore>
    {
        protected override void OnConfigureServices(DependencyConfigurationContext context)
        {
            // configure store and dependencies
            context.Services.AddSingleton(context.ScopedConfiguration.Get<MySqlPlayerManagementContextOptions>());

            context.Services.AddTransient<PlayerManagementContextBase, MySqlPlayerManagementContext>();
            context.Services.AddTransient<IPlayerManagementStore, EntityFrameworkPlayerManagementStore>();

            // configure type to perform migrations
            context.Services.AddSingleton<IDependencyInitializer<IPlayerManagementStore>>(this);
        }

        public IApplicationBuilder Use(IApplicationBuilder app)
        {
            var logger = app.ApplicationServices.GetRequiredService<ILogger<MySqlPlayerManagementStoreDependencyConfiguration>>();
            logger.LogInformation("Run Migrations for MySqlPlayerManagementContext");
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = (MySqlPlayerManagementContext)serviceScope.ServiceProvider.GetRequiredService<PlayerManagementContextBase>();
                context.Database.Migrate();
            }

            return app;
        }
    }
}

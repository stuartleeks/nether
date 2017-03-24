// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

using Nether.Common.DependencyInjection;
using Nether.Data.Leaderboard;
using Nether.Data.EntityFramework.Leaderboard;
using Nether.Data.InMemory.Leaderboard;
using Nether.Data.Sql.Leaderboard;
using Nether.Integration.Analytics;
using Nether.Integration.Default.Analytics;
using Nether.Web.Features.Leaderboard.Configuration;
using Nether.Web.Utilities;
using Microsoft.AspNetCore.Hosting;
using Nether.Data.EntityFramework.Leaderboard;
using Nether.Data.InMemory.Leaderboard;
using Nether.Data.MySql.Leaderboard;

namespace Nether.Web.Features.Leaderboard
{
    public static class LeaderboardServiceExtensions
    {
        private static Dictionary<string, Type> s_wellKnownStoreTypes = new Dictionary<string, Type>
            {
                {"in-memory", typeof(InMemoryLeaderboardStoreDependencyConfiguration) },
                {"sql", typeof(SqlLeaderboardStoreDependencyConfiguration) },
                {"mysql", typeof(MySqlLeaderboardStoreDependencyConfiguration) },
            };

        private static Dictionary<string, Type> s_wellKnownAnalyticsIntegrationTypes = new Dictionary<string, Type>
            {
                {"null", typeof(NullAnalyticsIntegrationClientDependencyConfiguration) },
                {"http", typeof(HttpAnalyticsIntegrationClientDependencyConfiguration) },
                {"eventhub", typeof(EventHubAnalyticsIntegrationClientDependencyConfiguration) },
            };

        public static IServiceCollection AddLeaderboardServices(
            this IServiceCollection services,
            IConfiguration configuration,
            ILogger logger,
            NetherServiceSwitchSettings serviceSwitches,
            IHostingEnvironment hostingEnvironment
            )
        {
            bool enabled = configuration.GetValue<bool>("Leaderboard:Enabled");
            if (!enabled)
            {
                logger.LogInformation("Leaderboard service not enabled");
                return services;
            }
            logger.LogInformation("Configuring Leaderboard service");
            serviceSwitches.AddServiceSwitch("Leaderboard", true);

            services.AddServiceFromConfiguration("Leaderboard:Store", s_wellKnownStoreTypes, configuration, logger, hostingEnvironment);

            AddLeaderboardProvider(services, configuration);

            services.AddServiceFromConfiguration("Leaderboard:AnalyticsIntegrationClient", s_wellKnownAnalyticsIntegrationTypes, configuration, logger, hostingEnvironment);

            return services;
        }

        private static void AddLeaderboardProvider(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILeaderboardProvider, ConfigurationLeaderboardProvider>();
        }
    }
}
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.


using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

using Nether.Common.DependencyInjection;
using Nether.Data.PlayerManagement;
using Nether.Data.EntityFramework.PlayerManagement;
using Nether.Data.InMemory.PlayerManagement;
using Nether.Data.MongoDB.PlayerManagement;
using Nether.Data.Sql.PlayerManagement;
using Nether.Web.Features.PlayerManagement.Configuration;
using Nether.Web.Utilities;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Nether.Data.EntityFramework.PlayerManagement;
using Nether.Data.InMemory.PlayerManagement;
using Nether.Data.MySql.PlayerManagement;

namespace Nether.Web.Features.PlayerManagement
{
    public static class PlayerManagementServiceExtensions
    {
        private static Dictionary<string, Type> s_wellKnownStoreTypes = new Dictionary<string, Type>
            {
                {"in-memory", typeof(InMemoryPlayerManagementStoreDependencyConfiguration) },
                {"sql", typeof(SqlPlayerManagementStoreDependencyConfiguration) },
                {"mongo", typeof(SqlPlayerManagementStoreDependencyConfiguration) },
                {"mysql", typeof(MySqlPlayerManagementStoreDependencyConfiguration) },
            };

        public static IServiceCollection AddPlayerManagementServices(
            this IServiceCollection services,
            IConfiguration configuration,
            ILogger logger,
            NetherServiceSwitchSettings serviceSwitches,
            IHostingEnvironment hostingEnvironment
            )
        {
            bool enabled = configuration.GetValue<bool>("PlayerManagement:Enabled");
            if (!enabled)
            {
                logger.LogInformation("PlayerManagement service not enabled");
                return services;
            }
            logger.LogInformation("Configuring PlayerManagement service");
            serviceSwitches.AddServiceSwitch("PlayerManagement", true);

            services.AddServiceFromConfiguration("PlayerManagement:Store", s_wellKnownStoreTypes, configuration, logger, hostingEnvironment);

            return services;
        }
    }
}

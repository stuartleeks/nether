﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Nether.Data.EntityFramework.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nether.Data.MySql.PlayerManagement
{
    /// <summary>
    /// Class added to enable creating EF Migrations
    /// See https://docs.microsoft.com/en-us/ef/core/api/microsoft.entityframeworkcore.infrastructure.idbcontextfactory-1
    /// </summary>
    public class SqlPlayerManagementContextFactory : IDbContextFactory<MySqlPlayerManagementContext>
    {
        public MySqlPlayerManagementContext Create(DbContextFactoryOptions options)
        {
            var loggerFactory = new LoggerFactory();
            loggerFactory.AddConsole();
            var logger = loggerFactory.CreateLogger<SqlPlayerManagementContextFactory>();


            var configuration = ConfigurationHelper.GetConfiguration(logger, options.ContentRootPath, options.EnvironmentName);

            var connectionString = configuration["PlayerManagement:Store:properties:ConnectionString"];
            logger.LogInformation("Using connection string: {0}", connectionString);

            return new MySqlPlayerManagementContext(
                loggerFactory,
                new MySqlPlayerManagementContextOptions
                {
                    ConnectionString = connectionString
                });
        }
    }
}

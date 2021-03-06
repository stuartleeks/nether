﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using Nether.Data.PlayerManagement;
using System.ComponentModel.DataAnnotations;

namespace Nether.Web.Features.PlayerManagement.Models.GroupAdmin
{
    public class GroupPutRequestModel
    {
        /// <summary>
        /// Group custom type
        /// </summary>
        public string CustomType { get; set; }

        /// <summary>
        /// Group description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Initial gamertag list of members of this group, not required and can be changed later.
        /// </summary>
        public List<string> Members { get; set; }
    }
}

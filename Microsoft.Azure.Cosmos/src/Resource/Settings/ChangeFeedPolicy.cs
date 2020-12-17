﻿//------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//------------------------------------------------------------

namespace Microsoft.Azure.Cosmos
{
    using System;
    using Microsoft.Azure.Documents;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the change feed policy configuration for a container in the Azure Cosmos DB service.
    /// </summary> 
    /// <example>
    /// The example below creates a new container with a custom change feed policy.
    /// <code language="c#">
    /// <![CDATA[
    ///     ContainerProperties containerProperties = new ContainerProperties("MyCollection", "/country");
    ///     containerProperties.ChangeFeedPolicy.RetentionDuration = TimeSpan.FromMinutes(5);
    ///     
    ///     CosmosContainerResponse containerCreateResponse = await client.GetDatabase("dbName").CreateContainerAsync(containerProperties, 5000);
    ///     ContainerProperties createdContainerProperties = containerCreateResponse.Container;
    /// ]]>
    /// </code>
    /// </example>
    /// <seealso cref="ContainerProperties"/>
#if PREVIEW
    public
#else
    internal
#endif
    sealed class ChangeFeedPolicy
    {
        [JsonProperty(PropertyName = Constants.Properties.LogRetentionDuration, NullValueHandling = NullValueHandling.Ignore)]
        private int? retentionDurationInMinutes;

        /// <summary>
        /// Gets or sets a value that indicates for how long operation logs have to be retained.
        /// </summary>
        /// <remarks>
        /// Minimum granularity supported is minutes.
        /// </remarks>
        /// <value>
        /// Value is in TimeSpan.
        /// </value>
        [JsonIgnore]
        public TimeSpan RetentionDuration 
        {
            get
            {
                if (!this.retentionDurationInMinutes.HasValue)
                {
                    return TimeSpan.Zero;
                }

                return TimeSpan.FromMinutes(this.retentionDurationInMinutes.Value);
            }
            set
            {
                if (value.Seconds > 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(this.RetentionDuration), "Retention duration's minimum granularity is minutes.");
                }

                this.retentionDurationInMinutes = (int)value.TotalMinutes;
            }
        }
    }
}
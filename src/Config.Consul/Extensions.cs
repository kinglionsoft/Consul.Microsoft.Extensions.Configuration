﻿using System;
using Consul;

namespace Microsoft.Extensions.Configuration.Consul
{
    public static class Extensions
    {
        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from Consul.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder configurationBuilder)
        {
            return configurationBuilder.Add(new ConsulConfigurationSource());
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from Consul
        /// with a specified prefix.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="prefix">The prefix that Consul keys must start with. The prefix will be removed from the keys.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder configurationBuilder, 
            string prefix)
        {
            return configurationBuilder.Add(new ConsulConfigurationSource()
            {
                Prefix = prefix
            });
        }

        /// <summary>
        /// Adds an <see cref="IConfigurationProvider"/> that reads configuration values from Consul
        /// with a specified prefix.
        /// </summary>
        /// <param name="configurationBuilder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configure">Configure extra options, such as Prefixes and Consul Consistency level</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder configurationBuilder, 
            Action<ConsulConfigurationSource> configure)
        {
            var source = new ConsulConfigurationSource();
            configure(source);

            return configurationBuilder.Add(source);
        }

        public static IConfigurationBuilder AddConsul(this IConfigurationBuilder configurationBuilder,
            ConsulConfigurationSource source)
        {
            return configurationBuilder.Add(source);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Multitenant.Business.Interfaces;
using Multitenant.Common;


namespace Multitenant.Business.Classes
{
    /// <summary>
    /// Create a new options instance with configuration applied
    /// </summary>
    /// <typeparam name="TOptions"></typeparam>
    /// <typeparam name="T"></typeparam>
    internal class TenantOptionsFactory<TOptions, T> : IOptionsFactory<TOptions>
        where TOptions : class, new()
        where T : Tenant
    {

        private readonly IEnumerable<IConfigureOptions<TOptions>> _setups;
        private readonly IEnumerable<IPostConfigureOptions<TOptions>> _postConfigures;
        private readonly Action<TOptions, T> _tenantConfig;
        private readonly ITenantAccessor<T> _tenantAccessor;

        public TenantOptionsFactory(
            IEnumerable<IConfigureOptions<TOptions>> setups,
            IEnumerable<IPostConfigureOptions<TOptions>> postConfigures, Action<TOptions, T> tenantConfig,
            ITenantAccessor<T> tenantAccessor)
        {
            _setups = setups;
            _postConfigures = postConfigures;
            _tenantAccessor = tenantAccessor;
            _tenantConfig = tenantConfig;
        }

        /// <summary>
        /// Create a new options instance
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public TOptions Create(string name)
        {
            var options = new TOptions();

            //Apply options setup configuration
            foreach (var setup in _setups)
            {
                if (setup is IConfigureNamedOptions<TOptions> namedSetup)
                {
                    namedSetup.Configure(name, options);
                }
                else
                {
                    setup.Configure(options);
                }
            }

            //Apply tenant specifc configuration (to both named and non-named options)
            if (_tenantAccessor.Tenant != null)
                _tenantConfig(options, _tenantAccessor.Tenant);

            //Apply post configuration
            foreach (var postConfig in _postConfigures)
            {
                postConfig.PostConfigure(name, options);
            }

            return options;
        }
    }
}
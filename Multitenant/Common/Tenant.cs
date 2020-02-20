using System.Collections.Generic;

namespace Multitenant.Common
{
    public class Tenant
    {
        /// <summary>
        /// The tenant Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// The tenant identifier
        /// </summary>
        public string Identifier { get; set; }

        public string DatabaseName { get; set; }

        /// <summary>
        /// Tenant items
        /// </summary>
        public Dictionary<string, object> Items { get; private set; } = new Dictionary<string, object>();
    }
}
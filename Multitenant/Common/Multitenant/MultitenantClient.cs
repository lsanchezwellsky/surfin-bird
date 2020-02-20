using System;
using System.ComponentModel.DataAnnotations;

namespace Multitenant.Common.Multitenant
{
    public class MultitenantClient
    {
        [Key]
        public int ClientId { get; set; }
        public string ClientKey { get; set; }
        public string ConnectionString { get; set; }
    }
}

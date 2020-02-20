using System;
using Multitenant.Business.Interfaces;
using Multitenant.Common;


namespace Multitenant.Business.Classes
{
    public class TenantProvider :ITenantProvider
    {
        public Tenant getCurrentTenant()
        {
            throw new NotImplementedException();
        }
    }
}

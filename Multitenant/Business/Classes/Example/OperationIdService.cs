using System;
using Multitenant.Common;


namespace Multitenant.Business.Classes.Example
{
    public class OperationIdService
    {
        public readonly Guid Id;
        public readonly Tenant TenantInfo;

        public OperationIdService(Tenant tenant)
        {
            Id = Guid.NewGuid();
            TenantInfo = tenant;
        }
    }
}

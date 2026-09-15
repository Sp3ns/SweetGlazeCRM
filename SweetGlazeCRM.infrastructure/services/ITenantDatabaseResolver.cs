using System;
using System.Collections.Generic;
using System.Text;

namespace SweetGlazeCRM.infrastructure.services
{
    public interface ITenantDatabaseResolver
    {
        Task<TenantDatabaseInfo> GetDatabaseInfoAsync(int companyId);
    }
}
using SweetGlazeCRM.infrastructure.data;

namespace SweetGlazeCRM.infrastructure.services
{
    public interface ITenantDbContextFactory
    {
        Task<TenantCRMDbContext> CreateAsync(int companyId);
    }
}
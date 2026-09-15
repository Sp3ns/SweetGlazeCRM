using Microsoft.EntityFrameworkCore;
using SweetGlazeCRM.infrastructure.data;

namespace SweetGlazeCRM.infrastructure.services
{
    public class TenantDatabaseResolver : ITenantDatabaseResolver
    {
        private readonly MasterCRMDbContext _masterDb;

        public TenantDatabaseResolver(MasterCRMDbContext masterDb)
        {
            _masterDb = masterDb;
        }

        public async Task<TenantDatabaseInfo> GetDatabaseInfoAsync(int companyId)
        {
            var database = await _masterDb.CompanyDatabases
                .AsNoTracking()
                .FirstOrDefaultAsync(x =>
                    x.CompanyId == companyId &&
                    x.IsActive);

            if (database == null)
            {
                throw new InvalidOperationException(
                    $"No active database found for company {companyId}.");
            }

            return new TenantDatabaseInfo
            {
                ServerName = database.ServerName,
                DatabaseName = database.DatabaseName,
                CredentialKey = database.CredentialKey
            };
        }
    }
}
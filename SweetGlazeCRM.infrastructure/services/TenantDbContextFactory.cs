using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SweetGlazeCRM.infrastructure.data;

namespace SweetGlazeCRM.infrastructure.services
{
    public class TenantDbContextFactory : ITenantDbContextFactory
    {
        private readonly ITenantDatabaseResolver _resolver;
        private readonly IConfiguration _configuration;

        public TenantDbContextFactory(
            ITenantDatabaseResolver resolver,
            IConfiguration configuration)
        {
            _resolver = resolver;
            _configuration = configuration;
        }

        public async Task<TenantCRMDbContext> CreateAsync(int companyId)
        {
            var databaseInfo =
                await _resolver.GetDatabaseInfoAsync(companyId);

            var userId = _configuration[
                $"TenantCredentials:{databaseInfo.CredentialKey}:UserId"];

            var password = _configuration[
                $"TenantCredentials:{databaseInfo.CredentialKey}:Password"];

            if (string.IsNullOrWhiteSpace(userId) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    $"Credentials not found for key '{databaseInfo.CredentialKey}'.");
            }

            var connectionString =
                $"Server={databaseInfo.ServerName.Trim()};" +
                $"Database={databaseInfo.DatabaseName.Trim()};" +
                $"User Id={userId.Trim()};" +
                $"Password={password};" +
                $"Encrypt=True;" +
                $"TrustServerCertificate=True;" +
                $"MultipleActiveResultSets=True;";

            var options = new DbContextOptionsBuilder<TenantCRMDbContext>()
                .UseSqlServer(connectionString)
                .Options;

            return new TenantCRMDbContext(options);
        }
    }
}
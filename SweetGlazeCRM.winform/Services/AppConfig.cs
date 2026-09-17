namespace SweetGlazeCRM.winform.Services
{
    /// <summary>
    /// Central place for API connection settings.
    /// Update BaseUrl / TenantId here if your API address or tenant ever changes -
    /// nothing else in the WinForms project should hardcode these values.
    /// </summary>
    public static class AppConfig
    {
        public const string BaseUrl = "https://localhost:7070";
        public const int TenantId = 4;
    }
}

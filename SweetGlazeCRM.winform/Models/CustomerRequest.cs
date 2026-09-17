namespace SweetGlazeCRM.winform.Models
{
    /// <summary>
    /// The shape sent to the API for Create (POST) and Update (PUT).
    /// </summary>
    public class CustomerRequest
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

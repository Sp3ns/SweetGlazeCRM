using System.Net.Http.Json;
using SweetGlazeCRM.winform.Models;

namespace SweetGlazeCRM.winform.Services
{
    public class CustomerApiService
    {
        private readonly HttpClient _httpClient;

        private const string BaseUrl = "https://localhost:7070";

        private const int TenantId = 4;

        public CustomerApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };

            _httpClient.Timeout = TimeSpan.FromSeconds(15);
        }

        private string CustomerUrl =>
            $"/tenant/{TenantId}/customers";

        public async Task<List<Customer>> GetCustomersAsync()
        {
            var customers =
                await _httpClient.GetFromJsonAsync<List<Customer>>(
                    CustomerUrl);

            return customers ?? new List<Customer>();
        }

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            var response =
                await _httpClient.GetAsync(
                    $"{CustomerUrl}/{id}");

            if (response.StatusCode ==
                System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return await response.Content
                .ReadFromJsonAsync<Customer>();
        }

        public async Task<Customer> AddCustomerAsync(
            Customer customer)
        {
            var response =
                await _httpClient.PostAsJsonAsync(
                    CustomerUrl,
                    customer);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content
                    .ReadFromJsonAsync<Customer>();

            return result ?? customer;
        }

        public async Task<Customer> UpdateCustomerAsync(
            int id,
            Customer customer)
        {
            var response =
                await _httpClient.PutAsJsonAsync(
                    $"{CustomerUrl}/{id}",
                    customer);

            response.EnsureSuccessStatusCode();

            var result =
                await response.Content
                    .ReadFromJsonAsync<Customer>();

            return result ?? customer;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            var response =
                await _httpClient.DeleteAsync(
                    $"{CustomerUrl}/{id}");

            response.EnsureSuccessStatusCode();
        }
    }
}
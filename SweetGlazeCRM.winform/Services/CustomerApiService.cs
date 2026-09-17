using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using SweetGlazeCRM.winform.Models;

namespace SweetGlazeCRM.winform.Services
{
    /// <summary>
    /// Handles all HTTP communication with the existing Customer API.
    /// This is the ONLY class in the WinForms project that talks to the network -
    /// forms and controls should never build HttpClient calls themselves.
    /// Endpoints used (already implemented on the API, not modified here):
    ///   GET    /tenant/{tenantId}/customers
    ///   GET    /tenant/{tenantId}/customers/{id}
    ///   POST   /tenant/{tenantId}/customers
    ///   PUT    /tenant/{tenantId}/customers/{id}
    ///   DELETE /tenant/{tenantId}/customers/{id}
    /// </summary>
    public class CustomerApiService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        public CustomerApiService()
        {
            // Bypasses certificate validation so the WinForms client can call the
            // API over the ASP.NET Core HTTPS *development* certificate on localhost.
            // Safe for this lab exam; remove/replace before any real deployment.
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (msg, cert, chain, errors) => true
            };

            _http = new HttpClient(handler)
            {
                BaseAddress = new Uri(AppConfig.BaseUrl),
                Timeout = TimeSpan.FromSeconds(15)
            };
        }

        private string CustomersEndpoint => $"/tenant/{AppConfig.TenantId}/customers";

        public async Task<List<Customer>> GetCustomersAsync()
        {
            using var response = await _http.GetAsync(CustomersEndpoint);
            await EnsureSuccessAsync(response);
            var result = await response.Content.ReadFromJsonAsync<List<Customer>>(_jsonOptions);
            return result ?? new List<Customer>();
        }

        public async Task<Customer> GetCustomerAsync(int id)
        {
            using var response = await _http.GetAsync($"{CustomersEndpoint}/{id}");
            await EnsureSuccessAsync(response);
            var result = await response.Content.ReadFromJsonAsync<Customer>(_jsonOptions);
            return result!;
        }

        public async Task<Customer> AddCustomerAsync(CustomerRequest request)
        {
            using var response = await _http.PostAsJsonAsync(CustomersEndpoint, request, _jsonOptions);
            await EnsureSuccessAsync(response);
            var result = await response.Content.ReadFromJsonAsync<Customer>(_jsonOptions);
            return result!;
        }

        public async Task<Customer> UpdateCustomerAsync(int id, CustomerRequest request)
        {
            using var response = await _http.PutAsJsonAsync($"{CustomersEndpoint}/{id}", request, _jsonOptions);
            await EnsureSuccessAsync(response);

            // Some APIs return 204 No Content on update. Fall back to a synthesized
            // Customer from the request so the caller always gets a usable result.
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                return new Customer
                {
                    Id = id,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    PhoneNumber = request.PhoneNumber,
                    Address = request.Address,
                    Notes = request.Notes,
                    IsActive = request.IsActive
                };
            }

            var result = await response.Content.ReadFromJsonAsync<Customer>(_jsonOptions);
            return result!;
        }

        public async Task DeleteCustomerAsync(int id)
        {
            using var response = await _http.DeleteAsync($"{CustomersEndpoint}/{id}");
            await EnsureSuccessAsync(response);
        }

        private static async Task EnsureSuccessAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            string friendlyMessage = $"The server returned an error ({(int)response.StatusCode}).";

            try
            {
                var body = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    using var doc = JsonDocument.Parse(body);
                    if (doc.RootElement.TryGetProperty("message", out var messageProp))
                    {
                        friendlyMessage = messageProp.GetString() ?? friendlyMessage;
                    }
                }
            }
            catch
            {
                // Body wasn't JSON or had no "message" property - keep the generic message above.
            }

            throw new ApiException(friendlyMessage, response.StatusCode);
        }
    }
}

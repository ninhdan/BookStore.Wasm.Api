using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.DtoUser;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;

namespace BookStoreBlazorWasm.Services
{
    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;

        public AddressService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<AddressDto> AddAddress(AddressDto address)
        {
            var response = await _httpClient.PostAsJsonAsync("api/address", address);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<AddressDto>();
            }
            else
            {
                
                return null;
            }
        }

        public async Task<int> UpdateOldAddresses(Guid userId)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/address/{userId}", userId);
            if (response.IsSuccessStatusCode)
            {
                var updatedAddressesCount = await response.Content.ReadAsStringAsync();
                return int.Parse(updatedAddressesCount);
            }
            else
            {

                return -1;
            }
        }

        public async Task<UserDto> GetUserByIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"api/users/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Error getting user with ID {userId}: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            var user = JsonSerializer.Deserialize<UserDto>(content);

            return user;
        }

    }
}


using BookStoreBlazorWasm.Services.Interfaces;
using BookStoreView.Models.Dtos.ShoppingCart;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Text;


namespace BookStoreBlazorWasm.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly HttpClient httpClient;

        public ShoppingCartService(HttpClient httpClient) {

                this.httpClient = httpClient;
        }

        public event Action<int> OnShoppingCartChanged;

        public async Task<CartItemDto> AddItem(CartItemToAddDto cartItemToAddDto, Guid userId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<CartItemToAddDto>($"api/ShoppingCart?userId={userId}", cartItemToAddDto);
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return default(CartItemDto);
                    }
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {response.StatusCode} Message -- {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }




        public async Task<CartItemDto> DeleteItem(Guid id)
        {
            try
            {
                var response = await httpClient.DeleteAsync($"api/ShoppingCart/{id}");
                if(response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return default(CartItemDto);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<List<CartItemDto>> GetItems(Guid customerId)
        {
            try
            {
                var reponse = await httpClient.GetAsync($"api/ShoppingCart/{customerId}/GetItems");
                if(reponse.IsSuccessStatusCode)
                {
                    if(reponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<CartItemDto>().ToList();
                    }
                    return await reponse.Content.ReadFromJsonAsync<List<CartItemDto>>();
                }
                else
                {
                    var message = await reponse.Content.ReadAsStringAsync();
                    throw new Exception($"Http status: {reponse.StatusCode} Message -- {message}");
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        public void RaiseEventOnShoppingCartChanged(int totalQty)
        {
            if (OnShoppingCartChanged != null)
            {
                OnShoppingCartChanged.Invoke(totalQty);

            }
        }

        public async Task<CartItemDto> UpdateQuantity(CartIemQuanlityUpdateDto cartIemQuanlityUpdateDto)
        {
            try
            {
                var jsonRequest = JsonConvert.SerializeObject(cartIemQuanlityUpdateDto);

                var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json-patch+json");

                var response = await httpClient.PatchAsync($"api/ShoppingCart/{cartIemQuanlityUpdateDto.CartItemId}",content);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<CartItemDto>();
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
           
        }





    }
}

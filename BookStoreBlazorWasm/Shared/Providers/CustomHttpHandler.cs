using Blazored.LocalStorage;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using BookStoreView.Models.Dtos.DtoUser;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace BookStoreBlazorWasm.Shared.Providers
{
    public class CustomHttpHandler : DelegatingHandler
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        public CustomHttpHandler(ILocalStorageService localStorageService, HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider)
        {
            this._localStorageService = localStorageService;
            this._httpClient = httpClient;
            this._authStateProvider = authenticationStateProvider;
        }


        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.RequestUri.AbsolutePath.ToLower().Contains("login") ||
               request.RequestUri.AbsolutePath.ToLower().Contains("registration") ||
               request.RequestUri.AbsolutePath.ToLower().Contains("renew-tokens"))
            {
                return await base.SendAsync(request, cancellationToken);
            }

            var jwtToken = await _localStorageService.GetItemAsync<string>("jwt-access-token");

            if (!string.IsNullOrEmpty(jwtToken))
            {
                request.Headers.Add("Authorization", $"bearer {jwtToken}");
            }

            var originalResponse = await base.SendAsync(request, cancellationToken);
            if (originalResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                return await InvokeRefreshCall(request, originalResponse, jwtToken, cancellationToken);
            }
            return originalResponse;
        }

        private async Task<HttpResponseMessage> InvokeRefreshCall(HttpRequestMessage originalRequest,
           HttpResponseMessage originalResponse,
           string expiredJwtToken,
           CancellationToken cancellationToken)
        {
            var refreshToken = await _localStorageService.GetItemAsync<string>("refresh-token");

            var userCliams = Utility.ParseClaimsFromJwt(expiredJwtToken);

            var renewTokenRequest = new RenewTokenDto();
            renewTokenRequest.UserId = userCliams.ToList().Where(_ => _.Type.ToLower() == "sub").Select(_ => Guid.Parse(_.Value)).FirstOrDefault();
            renewTokenRequest.RefreshToken = refreshToken;

            var jsonPayload = JsonSerializer.Serialize(renewTokenRequest);
            var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var refreshTokenresponse = await _httpClient.PostAsync("/api/User/renew-tokens", requestContent);

            if (refreshTokenresponse.StatusCode == HttpStatusCode.OK)
            {
                var regeneratedTokenResponse = await refreshTokenresponse.Content.ReadFromJsonAsync<JWTTokenDto>();
                await _localStorageService.SetItemAsync<string>("jwt-access-token", regeneratedTokenResponse.AccessToken);
                await _localStorageService.SetItemAsync<string>("refresh-token", regeneratedTokenResponse.RefreshToken);
                (_authStateProvider as CustomAuthProvider).NotifyAuthState();

                originalRequest.Headers.Remove("Authorization");

                originalRequest.Headers.Add("Authorization", $"bearer {regeneratedTokenResponse.AccessToken}");

                return await base.SendAsync(originalRequest, cancellationToken);
            }
            return originalResponse;
        }


    }
}

using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using SharedClassLibrary.GenericModels;
using System.Security.Claims;
namespace BlazorWebAssemblyApp.Authentication
{
    public class CustomAuthenticationStateProvider(ILocalStorageService _localStorageService) : AuthenticationStateProvider
    {
        //- ASP.NET Core uses the ClaimsPrincipal class to represent the signed-in (currently authenticated ) user.
        //- the sign-in process obtains an IdentityUser object from the store and uses the data that 
        // it contains to create a ClaimsPrincipal object that can be used by ASP.NET Core.
        //- Evaluating the user’s credentials and creating the ClaimsPrincipal object are the responsibilities of 
        // the sign-in manager class, SignInManager<IdentityUser> which is configured as a service when Identity is configured.
        //The ClaimsPrincipal class also provides access to all the identities associated with the user and the complete set of claims.
        //(User) This property returns the ClaimsPrincipal object for the request that requires authorization.
        //The ClaimsPrincipal class defines the Identities property, which returns a sequence of ClaimsIdentity objects, representing the user’s identities.
        //The ClaimsIdentity class defines the Claims property, which returns a sequence of Claim objects

        private ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string stringToken = await _localStorageService.GetItemAsStringAsync("token");
                if(string.IsNullOrEmpty(stringToken)) return await Task.FromResult(new AuthenticationState(anonymous));

                var claim = Generics.GetClaimsFromToken(stringToken);
                var claimsPrincipal = Generics.SetClaimPrincipal(claim);
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        public async Task UpdateAuthenticationState(string? token)
        {
            ClaimsPrincipal claimsPrincipal = new();

            if (!string.IsNullOrWhiteSpace(token))
            {
                var userSession = Generics.GetClaimsFromToken(token);
                claimsPrincipal = Generics.SetClaimPrincipal(userSession);
                await _localStorageService.SetItemAsStringAsync("token", token);
            }
            else
            {
                claimsPrincipal = anonymous;
                await _localStorageService.RemoveItemAsync("token");
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}

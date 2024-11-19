using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Authorization;

namespace LostPet.Services
{
    [ExcludeFromCodeCoverage]
    public class UserIdentityProcessor(AuthenticationStateProvider authenticationStateAsync)
    {
        private readonly AuthenticationStateProvider _authenticationStateAsync = authenticationStateAsync;

        public async Task<string?> GetCurrentUserId()
        {
            var authstate = await this._authenticationStateAsync.GetAuthenticationStateAsync();

            if (authstate == null)
            {
                return null;
            }

            var user = authstate.User;

            return user.FindFirst(u => u.Type.Contains("nameidentifier"))?.Value;
        }
    }
}

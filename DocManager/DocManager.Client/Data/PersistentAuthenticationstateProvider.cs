using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Shared.Dto;
using System.Security.Claims;

namespace DocManager.Client.Data
{
    public class PersistentAuthenticationStateProvider : AuthenticationStateProvider
    {
        private static readonly Task<AuthenticationState> DefaultUnauthenticatedTask =
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

        private readonly Task<AuthenticationState> _authenticationStateTask = DefaultUnauthenticatedTask;

        public PersistentAuthenticationStateProvider(PersistentComponentState state)
        {
            if (!state.TryTakeFromJson(nameof(UserInfo), out UserInfo? userInfo) || userInfo is null)
            {
                return;
            }
            List<Claim> claims =
            [
                new Claim(ClaimTypes.NameIdentifier, userInfo.Login),
                new Claim(ClaimTypes.Role, userInfo.Role),
                new Claim("Name", userInfo.Name),
                new Claim("Surname", userInfo.Surname),
            ];

            _authenticationStateTask = Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims,
                    authenticationType: nameof(PersistentAuthenticationStateProvider)))));
        }
        public void Logout()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new())));
        }
        public override Task<AuthenticationState> GetAuthenticationStateAsync() => _authenticationStateTask;
    }
}

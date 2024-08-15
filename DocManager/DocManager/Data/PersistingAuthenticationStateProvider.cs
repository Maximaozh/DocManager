using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Shared.Dto;
using System.Diagnostics;
using System.Security.Claims;

namespace DocManager.Data
{
    // Используется на стороне сервера для обработки состояния
    public class PersistingAuthenticationStateProvider : ServerAuthenticationStateProvider, IDisposable
    {
        private Task<AuthenticationState>? _authenticationStateTask;
        private readonly PersistentComponentState _state;
        private readonly PersistingComponentStateSubscription _subscription;
        private readonly IdentityOptions _options;
        public PersistingAuthenticationStateProvider(
          PersistentComponentState persistentComponentState,
          IOptions<IdentityOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
            _state = persistentComponentState;
            AuthenticationStateChanged += OnAuthenticationStateChanged;
            _subscription = _state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
        }

        private async Task OnPersistingAsync()
        {
            if (_authenticationStateTask is null)
            {
                throw new UnreachableException($"Состояние аутентификации не установлено в{nameof(OnPersistingAsync)}().");
            }

            AuthenticationState authenticationState = await _authenticationStateTask;
            ClaimsPrincipal principal = authenticationState.User;


            if (principal.Identity?.IsAuthenticated == true)
            {
                string? id = principal.FindFirst("UserId")?.Value;
                string? login = principal.FindFirst(_options.ClaimsIdentity.UserIdClaimType)?.Value;
                string? role = principal.FindFirst(_options.ClaimsIdentity.RoleClaimType)?.Value;
                string? name = principal.FindFirst("Name")?.Value;
                string? surname = principal.FindFirst("Surname")?.Value;

                if (id != null && login != null && role != null)
                {
                    _state.PersistAsJson(nameof(UserInfo), new UserInfo
                    {
                        Id = id,
                        Login = login,
                        Role = role,
                        Name = name,
                        Surname = surname
                    });
                }

            }
        }

        private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
        {
            _authenticationStateTask = authenticationStateTask;
        }

        public void Dispose()
        {
            _authenticationStateTask?.Dispose();
            AuthenticationStateChanged -= OnAuthenticationStateChanged;
            _subscription.Dispose();
        }
    }
}

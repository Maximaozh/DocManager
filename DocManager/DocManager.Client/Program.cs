using Blazored.LocalStorage;
using DocManager.Client.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

WebAssemblyHostBuilder builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5197/") });
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// ��������� ��� ��������� ��������� �����������
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();

// � ���������� ����������� ��������� ������� ������ �������� ������� �����
builder.Services.AddSingleton<AuthenticationStateProvider, PersistentAuthenticationStateProvider>();

await builder.Build().RunAsync();

using ApplicationDB;
using Blazored.LocalStorage;
using DocManager.Client.Pages;
using DocManager.Components;
using Microsoft.EntityFrameworkCore;
using DocManager.Services;
using MudBlazor.Services;
using Shared.Dto;
using DocManager.Endpoints;
using Shared;
using Data.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddDbContext<AppContextDB>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Myconnection")));

builder.Services.AddScoped<IUserRepositoriy, UserRepositoriy>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.MapUserEndpoints();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DocManager.Client._Imports).Assembly);

app.Run();

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
using DocManager.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddHttpClient();
builder.Services.AddBlazoredLocalStorage();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();
ApiExtensions.AddApiAuthentication(builder.Services, builder.Configuration);

builder.Services.AddDbContext<AppContextDB>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Myconnection")));

builder.Services.AddScoped<IUserRepositoriy, UserRepositoriy>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

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

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DocManager.Client._Imports).Assembly);

app.AddMappedEndpoitns();


app.Run();

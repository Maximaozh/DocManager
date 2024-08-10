using Blazored.LocalStorage;
using DocManager.Client.Pages;
using DocManager.Components;
using DocManager.Services;
using MudBlazor.Services;
using Shared.Dto;

var builder = WebApplication.CreateBuilder(args);

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddHttpClient();
builder.Services.AddSingleton<LoginService>();
builder.Services.AddBlazoredLocalStorage();

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



app.MapPost("/LoginResult", (UserLogin user) =>
{
    var loginService = app.Services.GetService<LoginService>();
    var result = loginService.AuthenticateUser(user);

    if (result is not null)
        return Results.Json(result);
    else return Results.BadRequest(new { details = "Ошибка при проверке данных. Возможно вы ввели неправильный логин или пароль" });
});

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DocManager.Client._Imports).Assembly);

app.Run();

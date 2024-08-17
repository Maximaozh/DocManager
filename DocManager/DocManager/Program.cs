using DocManager.Components;
using DocManager.Extensions;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

ApiExtensions.AddAuth(builder);
ApiExtensions.AddDB(builder);
ApiExtensions.AddData(builder);
ApiExtensions.AddUnspecified(builder);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

// Не думаю что стоит выделять доп методы ещё и для app
// В теории нам должно хватить лишь endpoints
ApiExtensions.AddMappedEndpoitns(app);

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(DocManager.Client._Imports).Assembly);

app.UseHttpsRedirection();
app.Run();

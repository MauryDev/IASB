using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Web.Components;
using Web.Database;
using Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<FileService>();
builder.Services.AddSingleton<InformativoService>();
builder.Services.AddSingleton<ProvaiVedeService>();
builder.Services.AddSingleton<YoutubeService>();
builder.Services.AddSingleton<WindowMessageService>();
var connectionString = builder.Configuration.GetConnectionString("IASB_DB");
builder.Services.AddDbContextFactory<IASBContext>(options => options.UseSqlite(connectionString));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllers();
builder.Services.AddHttpClient();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
app.MapControllers();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

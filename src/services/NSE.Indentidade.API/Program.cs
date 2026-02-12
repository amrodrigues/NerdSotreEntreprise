using EasyNetQ;
using Microsoft.Extensions.Configuration;
using NSE.Indentidade.API.Configuration;
using NSE.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

// Configurações de Serviços (Antigo ConfigureServices)
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Registra o Bus como Singleton (uma única conexão para toda a aplicação)
builder.Services.AddSingleton<IBus>(sp =>
    RabbitHutch.CreateBus("host=localhost:5672;username=guest;password=guest",
        register => register.EnableNewtonsoftJson()));
// Nossos métodos de extensão
builder.Services.AddIdentityConfiguration(builder.Configuration);
builder.Services.AddSwaggerConfiguration();

var app = builder.Build();

// Configurações do Pipeline (Antigo Configure)
app.UseSwaggerConfiguration();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthConfiguration();

app.MapRazorPages();
app.MapControllers();

app.Run();
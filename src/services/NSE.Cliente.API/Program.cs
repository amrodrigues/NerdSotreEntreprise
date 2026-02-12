using EasyNetQ;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NSE.Clientes.API.Configuration;
using NSE.Clientes.API.Data;
using NSE.WebAPI.Core.Identidade;

var builder = WebApplication.CreateBuilder(args);

// 1. Essencial para API e Swagger
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// 2. Configuração do Banco
builder.Services.AddDbContext<ClientesContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 3. Onde deve estar o builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
builder.Services.RegisterServices();

builder.Services.AddSwaggerConfiguration();

builder.Services.AddJwtConfiguration(builder.Configuration);

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddSingleton<IBus>(sp =>
    RabbitHutch.CreateBus("host=localhost:5672;username=guest;password=guest",
        register => register.EnableNewtonsoftJson()));

var app = builder.Build();

// Configurações do Pipeline
app.UseSwaggerConfiguration();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Melhor para ver erros em desenvolvimento
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthConfiguration();
// 4. Mapeia os endpoints da API
app.MapControllers();
app.MapRazorPages();

app.Run();
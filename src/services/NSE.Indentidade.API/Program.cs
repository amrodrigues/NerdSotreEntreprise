using NSE.Indentidade.API.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configurações de Serviços (Antigo ConfigureServices)
builder.Services.AddControllers();
builder.Services.AddRazorPages();

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

// IMPORTANTE: Authentication SEMPRE vem antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
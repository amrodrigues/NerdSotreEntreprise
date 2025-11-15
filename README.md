# 🔐 NSE.Identidade.API (NerdStore Enterprise - API de Identidade)

Este projeto é uma API RESTful responsável pela gestão de usuários e autenticação (Registro e Login) da plataforma NerdStore Enterprise.

Utiliza ASP.NET Core Identity e Entity Framework Core para persistência de dados.

## 🚀 Tecnologias e Dependências Principais

* **Framework:** ASP.NET Core (.NET 8.0+)
* **Banco de Dados:** SQL Server
* **ORM:** Entity Framework Core
* **Autenticação/Autorização:** ASP.NET Core Identity
* **Documentação:** Swagger/OpenAPI (Swashbuckle)

## ⚙️ Configuração do Projeto

### 1. Database Connection

O projeto utiliza uma conexão com o SQL Server. Certifique-se de que a *connection string* esteja configurada corretamente no seu `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=NSE.Identidade;Trusted_Connection=True;MultipleActiveResultSets=true"
}
```

### 2. Migrações e Inicialização do Banco
Execute os comandos a seguir no Console do Gerenciador de Pacotes ou via CLI para aplicar as migrações e criar o banco de dados e tabelas do Identity:

```json
dotnet ef database update --context ApplicationDbContext
```

### 3. Middleware e Roteamento (Program.cs)
A configuração do roteamento e do Swagger foram ajustadas para garantir o mapeamento correto dos Controllers de API e a visualização da documentação.

Pontos de Configuração Críticos:

Garantir a ordem correta dos middlewares de autenticação/autorização.

Inclusão do app.MapControllers() para habilitar o roteamento da API.

```json
// Exemplo de ordem corrigida no Program.cs
 
app.MapControllers(); // Essencial para o funcionamento das rotas de API
 ```

# 📚 Documentação da API (Swagger UI)
A API está documentada usando o Swagger/OpenAPI.

Acesso
Para visualizar todos os endpoints e modelos de dados, acesse a interface do Swagger UI:

https://localhost:7261/swagger

 

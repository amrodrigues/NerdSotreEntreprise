# 🔐 NSE.Identidade.API (NerdStore Enterprise - API de Identidade)

Esta API é o Serviço de Identidade da plataforma NerdStore Enterprise. Ela centraliza a autenticação, autorização e gestão de usuários, emitindo tokens JWT (JSON Web Tokens) para permitir a comunicação segura entre os demais microserviços.

## 🚀 Tecnologias e Dependências Principais

* **Framework:** ASP.NET Core (.NET 8.0+)
* **Banco de Dados:** SQL Server
* **ORM:** Entity Framework Core
* **Autenticação/Autorização:** ASP.NET Core Identity
* **Documentação:** Swagger UI (OpenAPI)
* **DSegurança:**  Autenticação via JWT com criptografia HMAC-SHA256

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


# 🏗️ Arquitetura e Organização

O projeto segue os princípios de Clean Code e separação de responsabilidades. Recentemente, a Program.cs foi refatorada para utilizar Métodos de Extensão, movendo as configurações complexas para a pasta /Configuration:

* **IdentityConfig.cs:** Gerencia a configuração do banco de dados, políticas do Identity e parâmetros do JWT.

* **SwaggerConfig.cs:** Centraliza a documentação e versionamento da API.

* **MainController.cs:** Classe base para controllers, fornecendo notificações de erro e respostas padronizadas.

 

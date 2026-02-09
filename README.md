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

1. Crie as migrações:
```bash
 dotnet ef migrations add InitialCreate 
```
2. Aplique as migrações para criar o banco de dados:
```bash
 dotnet ef database update  
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

---------------------------------------------------------------------------------------------------------------------------------------------------------------------

# NSE.WebApp.MVC - NerdStore Enterprise

Este projeto é a camada de interface (Front-end MVC) da arquitetura **NerdStore Enterprise**. Ele gerencia a experiência do usuário, integrando-se aos microserviços de backend para operações de catálogo, carrinho e identidade.

## 🔐 Autenticação e Gestão de Identidade

A autenticação é baseada em um modelo híbrido que converte tokens de API em persistência local:

1. **Troca de Token**: O usuário envia credenciais para a API de Identidade e recebe um **JWT**.
2. **Claims Transformation**: O `IdentidadeController` lê o token, extrai as Claims (Email, Sub, etc.) e as mapeia para o esquema de autenticação do ASP.NET.
3. **Persistência**: Os dados são salvos em um **Cookie de Autenticação** seguro.



### Componentes Principais:
- **`IdentidadeController`**: Gerencia Login, Registro e Logout.
- **`RealizarLogin`**: Método que utiliza `SignInAsync` para carimbar o passaporte do usuário no navegador.
- **`ObterTokenFormatado`**: Utiliza `JwtSecurityTokenHandler` para processar a string do JWT vinda do microserviço.

---

## 🛡️ Tratamento Global de Erros e Resiliência

O projeto implementa uma estratégia de "fail-fast" e amigável ao usuário para lidar com falhas de rede ou de negócio.

### 1. Exception Middleware
Intercepta exceções customizadas (`CustomHttpRequestException`) disparadas pelos Services:
- **401 (Unauthorized)**: Redireciona para o `/login`.
- **404/403/500**: Redireciona para rotas de erro amigáveis no `HomeController`.



### 2. MainController
Classe base para todos os controllers que centraliza a lógica de validação:
- **`ResponsePossuiErros`**: Verifica o objeto `ResponseResult` da API e injeta as mensagens de erro diretamente no `ModelState` para exibição em tela via `ValidationSummary`.

---

## ⚙️ Configuração do Pipeline (Program.cs)

A ordem dos middlewares é fundamental para que a segurança e o tratamento de erros funcionem corretamente:

```csharp
// 1. Páginas de erro amigáveis
app.UseExceptionHandler("/erro/500");
app.UseStatusCodePagesWithRedirects("/erro/{0}");

// 2. Middleware Customizado de Exceção (Executa antes do Routing)
app.UseMiddleware<ExceptionMiddleware>();

// 3. Roteamento e Segurança
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```

## 📡 Integração com Services 
A comunicação com as APIs de backend segue padrões de alta resiliência:

* Desserialização JSON: Configurada com PropertyNameCaseInsensitive = true para suportar diferentes padrões de nomenclatura (camelCase vs PascalCase).

* Mapeamento de Dados: Uso de [JsonPropertyName] em Models como UsuarioRespostaLogin para garantir que campos como sub (do JWT) sejam mapeados corretamente para a propriedade Id.

### 🛡️ Segurança e Filtros de Autorização (ACL)

O projeto utiliza um sistema de **Access Control List (ACL)** baseado em Claims, permitindo um controle granular sobre o que cada usuário pode acessar ou executar.

#### Componentes de Segurança:
- **`ClaimsCustomAuthorize`**: Atributo derivado de `TypeFilterAttribute`. Ele é utilizado para decorar Actions ou Controllers, exigindo uma Claim específica (ex: `[ClaimsCustomAuthorize("Catalogo", "Ler")]`).
- **`RequisitoClaimFilter`**: Um filtro de autorização que implementa `IAuthorizationFilter`. Ele intercepta a requisição antes de chegar à Action e valida:
    1. Se o usuário está autenticado (**401 Unauthorized**).
    2. Se o usuário possui a permissão necessária (**403 Forbidden**).
- **`CustomAuthorization`**: Classe estática utilitária que executa a lógica de comparação entre as Claims presentes no `HttpContext` e os requisitos da rota.



#### Exemplo de Uso no Controller:
```csharp
[ClaimsCustomAuthorize("Catalogo", "Editar")]
public async Task<IActionResult> AtualizarProduto(Guid id) 
{ 
    // Somente usuários com a claim 'Catalogo' e valor 'Editar' entram aqui
}
```

# 🛒 NSE.Clientes.API - Microserviço de Gestão de Clientes

Este microserviço faz parte do ecossistema **NerdStore Enterprise (NSE)**. Ele é responsável por todo o ciclo de vida do cliente, desde o registro inicial até a manutenção de endereços, utilizando práticas avançadas de **DDD (Domain Driven Design)** e **CQRS**.


### 📐 Desenho da Solução
O microserviço foi desenhado para ser totalmente desacoplado e resiliente. Abaixo está o fluxo de processamento:

Fluxo de Registro de Cliente:

* **Entrada:** A Controller recebe um comando via POST.

* **Mediação:** O IMediatorHandler encaminha o RegistrarClienteCommand para o Handler responsável.

* **Regra de Negócio:** O ClienteCommandHandler valida o estado da entidade e dos Value Objects (CPF e E-mail).

* **Persistência:** O EF Core salva os dados. Durante o Commit, o ChangeTracker captura eventos de domínio.

* **Notificação:** Se tudo ocorrer bem, o evento ClienteRegistradoEvent é publicado para o sistema.

---

 

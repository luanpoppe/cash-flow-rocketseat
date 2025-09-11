# CashFlow — API de Gestão de Despesas

Projeto backend em .NET que expõe uma API REST para gestão de despesas (CRUD). Este README documenta a arquitetura, principais funcionalidades, endpoints, validações, persistência e instruções para rodar e testar o projeto.

---

## Sumário

- Visão geral
- Estrutura do projeto
- Tecnologias e bibliotecas
- Arquitetura e padrões
- Endpoints (rotas, exemplos)
- DTOs e modelos
- Validações e mensagens
- Tratamento de erros
- Persistência e repositórios
- Injeção de dependências e AutoMapper
- Middleware e filtros
- Como rodar localmente
- Testes
- Recomendações e próximos passos

---

## Visão geral

CashFlow é uma API projetada para gerenciar despesas pessoais e empresariais de forma robusta e extensível. O sistema fornece funcionalidades completas de criação, leitura, atualização e exclusão (CRUD) de registros de despesa, incluindo validação de entrada, tratamento padronizado de erros, e suporte a diferentes meios de pagamento. A implementação prioriza separação de responsabilidades e manutenabilidade: a camada API expõe os endpoints REST, a camada Application encapsula os casos de uso (use cases) e validações, a camada Domain contém entidades e contratos (interfaces de repositório e Unit of Work), e a camada Infrastructure realiza a persistência com Entity Framework Core.

O projeto foi desenvolvido com foco em:

- Clareza arquitetural: cada camada tem responsabilidades bem definidas, facilitando extensões e testes.
- Testabilidade: validações e regras de negócio são isoladas em use cases e validators (FluentValidation), permitindo cobertura por testes unitários.
- Observabilidade e documentação: integração com Swagger/OpenAPI para facilitar exploração dos endpoints em desenvolvimento.
- Internacionalização básica: middleware de cultura interpreta o header Accept-Language para ajustar cultura de execução.
- Boas práticas de persistência: uso de padrões Repository e UnitOfWork com EF Core para centralizar operações de dados e controlar o commit das transações.

## Essa organização torna o CashFlow adequado para ser evoluído (ex.: adicionar autenticação, paginação, filtros avançados, métricas) e integrado em pipelines de CI/CD. Em ambientes de produção, recomenda-se complementar com migrações controladas, configuração segura de secrets e políticas de monitoramento.

## Estrutura do projeto (resumo)

- src/CashFlow.API — Controllers, middleware, filtros, Program.cs
- src/CashFlow.Application — Casos de uso (use cases), validators, configuração do AutoMapper
- src/CashFlow.Communication — DTOs de request e response
- src/CashFlow.Domain — Entidades, enums, interfaces de repositório e UnitOfWork
- src/CashFlow.Infrastructure — EF Core DbContext, repositórios, UnitOfWork, extensão de DI
- src/CashFlow.Exception — Exceções customizadas e recursos de mensagens
- tests/ — testes unitários (ex.: validadores)

---

## Tecnologias e bibliotecas

- .NET / ASP.NET Core Web API
- Entity Framework Core (Provider: MySQL)
- AutoMapper
- FluentValidation
- Swagger / OpenAPI
- MySQL
- Padrões: Repository, Unit of Work, Clean Architecture-like separation

---

## Arquitetura e padrões

- Camadas claras: API → Application (use cases) → Domain → Infrastructure
- Use cases encapsulam lógica de aplicação (Register, GetAll, GetById, Update, Delete)
- Repositórios especializados (interfaces separadas para leitura/escrita/update)
- UnitOfWork chama SaveChangesAsync para confirmar transações
- AutoMapper para conversão DTO ↔ Entidade
- Validação de requests com FluentValidation (ExpenseValidator)

---

## Endpoints da API

Base route: `/api/expenses`

1. Registrar despesa

- POST /api/expenses
- Body: `RequestExpenseJson`
- 201 Created: `ResponseRegisterExpenseJson`
- 400 Bad Request: `ResponseErrorJson` (validação)
- Exemplo:
  curl -X POST "https://localhost:5001/api/expenses" -H "Content-Type: application/json" -d '{
  "title": "Aluguel",
  "description": "Aluguel do apartamento",
  "date": "2025-09-01T00:00:00Z",
  "amount": 1200.00,
  "paymentType": "ElectronicTransfer"
  }'

Resposta (201):
{
"title": "Aluguel"
}

2. Listar todas as despesas

- GET /api/expenses
- 200 OK: `ResponseExpensesJson` (quando houver dados)
- 204 No Content: quando não houver despesas
- Exemplo:
  curl "https://localhost:5001/api/expenses"

Resposta (200):
{
"expenses": [
{
"id": 1,
"title": "Aluguel",
"date": "2025-09-01T00:00:00",
"amount": 1200.0,
"paymentType": "ElectronicTransfer"
}
]
}

3. Obter despesa por id

- GET /api/expenses/{id}
- 200 OK: `ResponseExpenseJson`
- 404 Not Found: `ResponseErrorJson` (quando não encontrado)
- Exemplo:
  curl "https://localhost:5001/api/expenses/1"

Resposta (200):
{
"id": 1,
"title": "Aluguel",
"description": "Aluguel do apartamento",
"date": "2025-09-01T00:00:00",
"amount": 1200.0,
"paymentType": "ElectronicTransfer"
}

4. Atualizar despesa

- PUT /api/expenses/{id}
- Body: `RequestExpenseJson`
- 204 No Content: atualização bem-sucedida
- 400 Bad Request: validação falhou
- 404 Not Found: despesa não encontrada
- Exemplo:
  curl -X PUT "https://localhost:5001/api/expenses/1" -H "Content-Type: application/json" -d '{
  "title": "Aluguel - Setembro",
  "description": "Aluguel atualizado",
  "date": "2025-09-01T00:00:00Z",
  "amount": 1250.00,
  "paymentType": "ElectronicTransfer"
  }'

5. Deletar despesa

- DELETE /api/expenses/{id}
- 204 No Content: exclusão concluída
- 404 Not Found: despesa não encontrada
- Exemplo:
  curl -X DELETE "https://localhost:5001/api/expenses/1"

---

## DTOs / Modelos (principais)

- RequestExpenseJson

  - title: string
  - description: string (opcional)
  - date: DateTime
  - amount: decimal
  - paymentType: enum (Cash, CreditCard, DebitCard, ElectronicTransfer)

- ResponseRegisterExpenseJson

  - title: string

- ResponseExpenseJson

  - id, title, description, date, amount, paymentType

- ResponseShortExpenseJson

  - versão reduzida usada em listagem

- ResponseExpensesJson

  - expenses: array de ResponseShortExpenseJson

- ResponseErrorJson
  - errorMessages: string[]

---

## Validações (FluentValidation)

Implementadas em `ExpenseValidator`:

- Title: obrigatório
- Amount: > 0
- Date: não pode ser no futuro (<= DateTime.UtcNow)
- PaymentType: deve ser um valor válido do enum

Em caso de falha, a aplicação lança `ErrorOnValidationException` com a lista de mensagens. O filtro global converte isso em `ResponseErrorJson`.

---

## Tratamento de erros

- `ExceptionFilter` registra erro padronizado:
  - Se exceção é `CashFlowException` (camada `Exception`), usa StatusCode da exceção e retorna `ResponseErrorJson`.
  - Caso contrário, retorna 500 + `ResponseErrorJson` com mensagem genérica.
- Formato:
  {
  "errorMessages": ["mensagem 1", "mensagem 2"]
  }

---

## Persistência e repositórios

- EF Core com MySQL:
  - `CashFlowDbContext` com DbSet<Expense> Expenses
  - Configuração: `UseMySql(connectionString, new MySqlServerVersion(new Version(8,0,43)))`
- `ExpensesRepository` implementa:
  - `GetAll()`, `GetById(id)` (tracking e no-tracking conforme uso), `Add`, `Update`, `Delete`
- `UnityOfWork` chama `SaveChangesAsync()` para persistir alterações

---

## Injeção de dependências e AutoMapper

- Extensões:
  - `AddInfrastructure` (registra DbContext, repositórios e UnitOfWork)
  - `AddApplication` (registra use cases e AutoMapper profile)
- AutoMapper perfil (`AutoMapping`) mapeia entre Request/Response e entidade Expense

---

## Middleware e filtros

- `CultureMiddleware`: lê header `Accept-Language` e define `CultureInfo.CurrentCulture`/`CurrentUICulture` (padrão "en" se inválido)
- `ExceptionFilter`: filtro global MVC para tratamento de exceções

---

## Como rodar localmente

Requisitos:

- .NET SDK compatível
- MySQL (ou servidor compatível) rodando

Passos:

1. Configurar connection string:
   Edite `src/CashFlow.API/appsettings.json` (ou defina variável de ambiente) com:

   ```json
   "ConnectionStrings": {
     "Connection": "Server=localhost;Database=cashflow;User=root;Password=senha;Port=3306;"
   }
   ```

2. Rodar a API:
   dotnet run --project src/CashFlow.API

- Em ambiente de desenvolvimento, o Swagger UI estará disponível (Program.cs habilita quando IsDevelopment()).

Comandos úteis:

- dotnet run --project src/CashFlow.API
- dotnet test

---

## Testes

- A solução inclui testes na pasta `tests/` (ex.: validadores).
- Executar:
  dotnet test

---

## Recomendações e melhorias sugeridas

- Retornar no POST (criação) um DTO mais completo (ex.: id gerado, date created) em vez de apenas o título.
- Adicionar paginação/filtragem para GET /api/expenses.
- Adicionar autenticação/autorização (JWT, OAuth) se for expor a API.
- Expor métricas/telemetria (Prometheus / Application Insights) conforme necessidade.
- Garantir que a connection string e segredos sejam passados por variáveis de ambiente no CI/CD.
- Expandir cobertura de testes (use cases, repositórios, controllers).

---

## Observações finais

O projeto segue uma separação clara de responsabilidades com padrões comuns (Repository + UnitOfWork + Use Cases). A documentação acima cobre as principais características que foram implementadas no código presente neste repositório.

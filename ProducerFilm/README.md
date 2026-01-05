# ProducerFilm API

API RESTful para análise de filmes vencedores do Golden Raspberry Awards utilizando arquitetura Domain-Driven Design (DDD).

---

## 📋 Pré-requisitos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

---

## 🚀 Como Rodar o Projeto

### 1. Clonar o repositório

```bash
git clone https://github.com/danillson/ProducerFilm.git
cd ProducerFilm
```

### 2. Restaurar as dependências

```bash
dotnet restore
```

### 3. Executar a aplicação

```bash
dotnet run --project ProducerFilm/ProducerFilm. csproj
```

Ao iniciar, a aplicação irá:
- Aplicar as migrações do banco de dados SQLite automaticamente
- Processar arquivos CSV da pasta `fileToRead` e importar os dados
- Mover os arquivos processados para `fileProcessed`
- Iniciar o servidor web

### 4. Acessar a aplicação

- **URL padrão**: `http://localhost:5231`
- **URL alternativa**: `https://localhost:7231/index.html`

---

## 📖 Acessar o Swagger

O Swagger UI está disponível no ambiente de desenvolvimento para documentação e testes interativos da API.

### Acessar

1. Execute a aplicação conforme descrito acima
2. Abra o navegador e acesse: 

```
http://localhost:5231/
https://localhost:7231/index.html
```

O Swagger UI será carregado automaticamente na raiz da aplicação. 

### Endpoints disponíveis no Swagger

| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/movielisthistory` | Lista todos os filmes |
| GET | `/api/movielisthistory/winners` | Apenas vencedores |
| GET | `/api/movies/winner-interval` | Intervalo de vitórias dos produtores |

---

## 🧪 Executar os Testes de Integração

O projeto possui testes de integração utilizando xUnit, FluentAssertions e banco de dados em memória. 

### Executar todos os testes

```bash
dotnet test ProducerFilm. IntegrationTests
```

### Executar com detalhes

```bash
dotnet test ProducerFilm.IntegrationTests --verbosity detailed
```

### Executar um teste específico

```bash
dotnet test ProducerFilm.IntegrationTests --filter "NomeDoTeste"
```

**Exemplo:**

```bash
dotnet test ProducerFilm.IntegrationTests --filter "GetWinnerInterval_ShouldCalculateCorrectIntervals_WithSimpleData"
```

### Executar com cobertura de código

```bash
dotnet test ProducerFilm.IntegrationTests --collect:"XPlat Code Coverage"
```

### Cenários de Teste Cobertos

- Banco de dados vazio
- Apenas um vencedor
- Cálculo correto de intervalos mínimo e máximo
- Múltiplos produtores com mesmo intervalo
- Produtor com 3+ vitórias
- Ignorar não-vencedores
- Múltiplos produtores no mesmo filme
- Formato correto da resposta JSON
- Cenário real do Golden Raspberry Awards

---

## 🛠 Tecnologias Utilizadas

- **. NET 8** - Framework principal
- **Entity Framework Core 8** - ORM
- **SQLite** - Banco de dados
- **Swashbuckle. AspNetCore** - Documentação Swagger
- **CsvHelper** - Leitura de arquivos CSV
- **xUnit** - Framework de testes
- **FluentAssertions** - Assertions legíveis
- **Microsoft.AspNetCore.Mvc. Testing** - Testes de integração

---

## 📁 Estrutura do Projeto

```
ProducerFilm/
├── Application/           # Camada de aplicação (serviços, DTOs)
├── Domain/                # Camada de domínio (entidades, interfaces)
├── Infrastructure/        # Camada de infraestrutura (repositórios, DbContext)
├── Presentation/          # Camada de apresentação (controllers)
├── Migrations/            # Migrações do Entity Framework
├── fileToRead/            # Pasta para arquivos CSV a serem processados
├── fileProcessed/         # Pasta para arquivos CSV já processados
├── Program.cs             # Ponto de entrada da aplicação
└── appsettings.json       # Configurações da aplicação

ProducerFilm. IntegrationTests/
├── Common/                # Classes base para testes
├── Endpoints/             # Testes dos endpoints
└── Factories/             # Factory para WebApplicationFactory
```

---

## 📝 Licença

Este projeto está sob a licença MIT. 

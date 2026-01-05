# Testes de Integração - ProducerFilm API

## ?? Visão Geral

Testes de integração completos para o endpoint `/api/movies/winner-interval` usando xUnit, WebApplicationFactory e banco de dados em memória.

## ??? Estrutura dos Testes

```
ProducerFilm.IntegrationTests/
??? Common/
?   ??? IntegrationTestBase.cs          # Classe base para testes
??? Factories/
?   ??? CustomWebApplicationFactory.cs   # Factory personalizada para testes
??? Endpoints/
    ??? WinnerIntervalEndpointTests.cs  # Testes do endpoint winner-interval
```

## ?? Cenários de Teste Implementados

### ? 1. GetWinnerInterval_ShouldReturnOk_WhenNoWinnersExist
**Objetivo**: Verificar comportamento com banco vazio  
**Expectativa**: Retorna HTTP 200 com listas vazias

### ? 2. GetWinnerInterval_ShouldReturnOk_WhenOnlyOneWinnerExists
**Objetivo**: Verificar comportamento com apenas 1 vencedor  
**Expectativa**: Retorna HTTP 200 com listas vazias (necessita 2+ vitórias)

### ? 3. GetWinnerInterval_ShouldCalculateCorrectIntervals_WithSimpleData
**Objetivo**: Verificar cálculo correto de intervalos  
**Dados**:
- Joel Silver: 1990 ? 1991 (intervalo de 1 ano)
- Matthew Vaughn: 2002 ? 2015 (intervalo de 13 anos)

**Expectativa**: 
- Min: Joel Silver (1 ano)
- Max: Matthew Vaughn (13 anos)

### ? 4. GetWinnerInterval_ShouldHandleMultipleProducersWithSameInterval
**Objetivo**: Verificar comportamento com múltiplos produtores com mesmo intervalo  
**Expectativa**: Ambos aparecem no resultado

### ? 5. GetWinnerInterval_ShouldHandleMultipleWinsForSameProducer
**Objetivo**: Verificar produtor com 3+ vitórias  
**Dados**: Producer X - 1980, 1985, 1990 (dois intervalos de 5 anos)  
**Expectativa**: Retorna ambos os intervalos

### ? 6. GetWinnerInterval_ShouldIgnoreNonWinners
**Objetivo**: Verificar que não-vencedores são ignorados  
**Dados**: Vencedores + filmes com winner = null, "", "no"  
**Expectativa**: Calcula apenas baseado nos vencedores (winner = "yes")

### ? 7. GetWinnerInterval_ShouldHandleMultipleProducersInSameMovie
**Objetivo**: Verificar separação de múltiplos produtores  
**Dados**: "Producer A, Producer B"  
**Expectativa**: Cada produtor é contabilizado separadamente

### ? 8. GetWinnerInterval_ShouldReturnCorrectFormat
**Objetivo**: Verificar formato correto da resposta  
**Expectativa**:
- Content-Type: application/json
- Estrutura correta dos objetos
- Validações de dados

### ? 9. GetWinnerInterval_ShouldHandleRealWorldScenario
**Objetivo**: Teste com cenário real baseado no Golden Raspberry Awards  
**Dados**: Múltiplos produtores com vitórias reais  
**Expectativa**: Cálculos corretos conforme dados históricos

## ?? Como Executar os Testes

### Executar todos os testes
```bash
dotnet test ProducerFilm.IntegrationTests
```

### Executar com detalhes
```bash
dotnet test ProducerFilm.IntegrationTests --verbosity detailed
```

### Executar teste específico
```bash
dotnet test ProducerFilm.IntegrationTests --filter "GetWinnerInterval_ShouldCalculateCorrectIntervals_WithSimpleData"
```

### Executar com cobertura de código
```bash
dotnet test ProducerFilm.IntegrationTests --collect:"XPlat Code Coverage"
```

## ?? Resultado dos Testes

```
Test summary: total: 9; failed: 0; succeeded: 9; skipped: 0
? Todos os testes passaram!
```

## ?? Tecnologias Utilizadas

- **xUnit** - Framework de testes
- **FluentAssertions** - Assertions mais legíveis e descritivas
- **WebApplicationFactory** - Criação de servidor de teste in-memory
- **Entity Framework Core InMemory** - Banco de dados em memória para testes
- **Microsoft.AspNetCore.Mvc.Testing** - Testes de integração para ASP.NET Core

## ?? Padrões Utilizados

### Arrange-Act-Assert (AAA)
Todos os testes seguem o padrão AAA:
```csharp
[Fact]
public async Task Test_Example()
{
    // Arrange - Preparar dados
    await SeedDatabaseAsync(context => { ... });
    
    // Act - Executar ação
    var response = await _client.GetAsync("/api/movies/winner-interval");
    
    // Assert - Verificar resultado
    response.StatusCode.Should().Be(HttpStatusCode.OK);
}
```

### Isolamento de Testes
- Cada teste usa uma instância única da factory
- Banco de dados em memória com nome único por teste
- Limpeza automática após cada teste

### Fluent Assertions
```csharp
result!.Min.Should().HaveCount(1);
result.Min[0].Producer.Should().Be("Joel Silver");
result.Min[0].Interval.Should().Be(1);
```

## ??? Custom Web Application Factory

```csharp
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Remove DbContext real
        // Adiciona DbContext InMemory
        // Configura ambiente de teste
    }
}
```

**Características**:
- ? Substitui SQLite por InMemory Database
- ? Nome único de banco para cada instância
- ? Ambiente configurado como "Testing"
- ? Sem dependências externas

## ?? Exemplo de Teste Completo

```csharp
[Fact]
public async Task GetWinnerInterval_ShouldCalculateCorrectIntervals_WithSimpleData()
{
    // Arrange
    await SeedDatabaseAsync(context =>
    {
        context.MovieListHistories.Add(new MovieListHistory(
            1990, "Die Hard 2", "20th Century Fox", "Joel Silver", "yes"));
        context.MovieListHistories.Add(new MovieListHistory(
            1991, "Hudson Hawk", "TriStar Pictures", "Joel Silver", "yes"));
    });

    // Act
    var response = await _client.GetAsync("/api/movies/winner-interval");

    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.OK);
    
    var result = await response.Content.ReadFromJsonAsync<WinnerIntervalResponseDto>();
    result.Should().NotBeNull();
    result!.Min.Should().HaveCount(1);
    result.Min[0].Producer.Should().Be("Joel Silver");
    result.Min[0].Interval.Should().Be(1);
}
```

## ?? Benefícios dos Testes de Integração

1. **? Confiança no Sistema**
   - Testa toda a stack: API ? Application ? Domain ? Infrastructure

2. **? Detecção Precoce de Bugs**
   - Problemas são identificados antes de produção

3. **? Documentação Viva**
   - Os testes documentam o comportamento esperado

4. **? Refatoração Segura**
   - Permite mudanças com confiança

5. **? Feedback Rápido**
   - Execução rápida com banco em memória

## ?? Métricas de Cobertura

- **Endpoints testados**: 100% do endpoint winner-interval
- **Cenários cobertos**: 9 cenários diferentes
- **Edge cases**: Incluídos (banco vazio, 1 vencedor, múltiplos produtores)
- **Validações**: HTTP Status, formato JSON, dados corretos

## ?? CI/CD Integration

Para integrar com pipeline de CI/CD:

```yaml
# Azure DevOps / GitHub Actions
- name: Run Integration Tests
  run: dotnet test ProducerFilm.IntegrationTests --logger "trx;LogFileName=test-results.trx"
  
- name: Publish Test Results
  uses: dorny/test-reporter@v1
  with:
    name: Integration Tests
    path: test-results.trx
    reporter: dotnet-trx
```

## ?? Troubleshooting

### Erro: "Cannot access disposed object"
**Solução**: Cada teste deve ter sua própria instância da factory

### Erro: "Database already exists"
**Solução**: Use nome único de banco (Guid) na factory

### Erro: "Services cannot be resolved"
**Solução**: Adicione `using Microsoft.Extensions.DependencyInjection;`

## ?? Próximos Passos

- [ ] Adicionar testes de performance
- [ ] Implementar testes de carga
- [ ] Adicionar testes de segurança
- [ ] Medir cobertura de código
- [ ] Testes E2E com banco real

---

**Testes de Integração completos e funcionais!** ???


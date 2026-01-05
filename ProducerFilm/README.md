# ProducerFilm - Sistema de Importação de Filmes

## ?? Descrição

Sistema de importação automática de dados de filmes a partir de arquivos CSV para banco de dados SQLite.

## ?? Funcionalidades Implementadas

### ? Importação de CSV
- Leitura automática de arquivos CSV da pasta `fileToRead`
- Processamento ao iniciar a aplicação
- Ignorar primeira linha (cabeçalho)
- Gravação dos dados na tabela `MovieListHistory`
- Movimentação dos arquivos processados para `fileProcessed`

### ? Estrutura do CSV
```csv
year;title;studios;producers;winner
1980;Can't Stop the Music;Associated Film Distribution;Allan Carr;yes
1980;Cruising;Lorimar Productions, United Artists;Jerry Weintraub;
```

**Campos:**
- `year` (int) - Ano do filme
- `title` (string) - Título do filme
- `studios` (string) - Estúdios
- `producers` (string) - Produtores
- `winner` (string) - "yes" para vencedores, vazio caso contrário

### ? API REST - Endpoints Disponíveis

#### MovieListHistory
- `GET /api/movielisthistory` - Lista todos os filmes
- `GET /api/movielisthistory/{id}` - Obtém filme por ID
- `GET /api/movielisthistory/year/{year}` - Filmes por ano
- `GET /api/movielisthistory/winners` - Apenas vencedores
- `GET /api/movielisthistory/statistics` - Estatísticas
- `POST /api/movielisthistory` - Criar novo filme
- `PUT /api/movielisthistory/{id}` - Atualizar filme
- `DELETE /api/movielisthistory/{id}` - Deletar filme

#### Outros
- `GET /api/films` - Lista de filmes (exemplo)
- `GET /api/hello` - Endpoint de teste
- `GET /todos` - Lista de TODOs (exemplo)

## ??? Como Usar

### 1. Preparar arquivo CSV
Coloque seu arquivo CSV na pasta `ProducerFilm/fileToRead/` com o formato:
```csv
year;title;studios;producers;winner
1980;Can't Stop the Music;Associated Film Distribution;Allan Carr;yes
```

### 2. Executar a aplicação
```bash
dotnet run --project ProducerFilm/ProducerFilm.csproj
```

### 3. O que acontece
? Banco de dados é criado/migrado automaticamente
? Arquivos CSV são lidos da pasta `fileToRead`
? Dados são importados para a tabela `MovieListHistory`
? Arquivos são movidos para `fileProcessed`
? Swagger UI abre automaticamente

### 4. Acessar Swagger
- URL: `http://localhost:5231/`
- Documentação interativa de todos os endpoints

## ?? Exemplo de Logs

```
info: ProducerFilm.Services.FileProcessorService[0]
      Verificando arquivos CSV na pasta fileToRead...
info: ProducerFilm.Services.FileProcessorService[0]
      1 arquivo(s) CSV encontrado(s).
info: ProducerFilm.Services.FileProcessorService[0]
      Processando arquivo: movies.csv
info: ProducerFilm.Services.FileProcessorService[0]
      Dados importados com sucesso do arquivo: movies.csv
info: ProducerFilm.Services.FileProcessorService[0]
      10 registro(s) importado(s) do arquivo movies.csv
info: ProducerFilm.Services.FileProcessorService[0]
      Arquivo movido: movies.csv -> fileProcessed/
info: ProducerFilm.Services.FileProcessorService[0]
      Processamento de arquivos CSV concluído.
```

## ??? Estrutura do Banco de Dados

### Tabela: MovieListHistories
| Coluna | Tipo | Descrição |
|--------|------|-----------|
| Id | INTEGER | Chave primária (auto-increment) |
| Year | INTEGER | Ano do filme |
| Title | TEXT(300) | Título do filme |
| Studios | TEXT(200) | Estúdios produtores |
| Producers | TEXT(300) | Produtores |
| Winner | TEXT(10) | "yes" ou vazio |
| CreatedAt | TEXT | Data de criação do registro |

**Índices:**
- `IX_MovieListHistories_Year` - Índice no campo Year para consultas otimizadas

## ?? Pacotes NuGet Utilizados

- `Microsoft.EntityFrameworkCore.Sqlite` (8.0.11)
- `Microsoft.EntityFrameworkCore.Design` (8.0.11)
- `Swashbuckle.AspNetCore` (10.1.0)
- `CsvHelper` (33.1.0)

## ?? Estrutura de Pastas

```
ProducerFilm/
??? Controllers/
?   ??? HelloController.cs
?   ??? FilmsController.cs
?   ??? MovieListHistoryController.cs
??? Data/
?   ??? AppDbContext.cs
??? Models/
?   ??? Film.cs
?   ??? MovieListHistory.cs
??? Services/
?   ??? FileProcessorService.cs
??? Migrations/
??? fileToRead/          # Coloque arquivos CSV aqui
??? fileProcessed/       # Arquivos processados vão aqui
??? producerfilm.db      # Banco de dados SQLite
```

## ?? Configuração

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=producerfilm.db"
  }
}
```

### Separador CSV
O sistema está configurado para usar **ponto e vírgula (;)** como separador.
Para alterar, edite `FileProcessorService.cs`:
```csharp
Delimiter = ";",  // Altere aqui
```

## ?? Observações Importantes

1. **Primeira linha do CSV é ignorada** (deve conter os nomes das colunas)
2. **Arquivos já processados** recebem timestamp se houver duplicação
3. **Campo winner** é opcional (pode estar vazio)
4. **Migração automática** ao iniciar a aplicação
5. **Logs detalhados** de todas as operações

## ?? Testando

### Via Swagger UI
1. Execute a aplicação
2. Acesse `http://localhost:5231/`
3. Teste os endpoints interativamente

### Via curl
```bash
# Listar todos os filmes
curl http://localhost:5231/api/movielisthistory

# Filmes vencedores
curl http://localhost:5231/api/movielisthistory/winners

# Estatísticas
curl http://localhost:5231/api/movielisthistory/statistics
```

## ?? To-Do Future

- [ ] Validação de dados do CSV
- [ ] Suporte a múltiplos separadores
- [ ] Interface web para upload de CSV
- [ ] Exportação de dados para CSV
- [ ] Relatórios personalizados

---

**Desenvolvido com .NET 8 + Entity Framework Core + SQLite**

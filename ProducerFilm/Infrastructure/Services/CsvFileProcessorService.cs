using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ProducerFilm.Domain.Entities;
using ProducerFilm.Domain.Interfaces;

namespace ProducerFilm.Infrastructure.Services;

public class CsvFileProcessorService
{
    private readonly ILogger<CsvFileProcessorService> _logger;
    private readonly IMovieListHistoryRepository _repository;
    private readonly string _fileToReadPath;
    private readonly string _fileProcessedPath;

    public CsvFileProcessorService(
        ILogger<CsvFileProcessorService> logger,
        IMovieListHistoryRepository repository,
        IWebHostEnvironment environment)
    {
        _logger = logger;
        _repository = repository;
        _fileToReadPath = Path.Combine(environment.ContentRootPath, "fileToRead");
        _fileProcessedPath = Path.Combine(environment.ContentRootPath, "fileProcessed");

        // Garantir que as pastas existam
        Directory.CreateDirectory(_fileToReadPath);
        Directory.CreateDirectory(_fileProcessedPath);
    }

    public async Task ProcessCsvFilesAsync()
    {
        try
        {
            _logger.LogInformation("Verificando arquivos CSV na pasta fileToRead...");

            var csvFiles = Directory.GetFiles(_fileToReadPath, "*.csv");

            if (csvFiles.Length == 0)
            {
                _logger.LogInformation("Nenhum arquivo CSV encontrado na pasta fileToRead.");
                return;
            }

            _logger.LogInformation($"{csvFiles.Length} arquivo(s) CSV encontrado(s).");

            foreach (var file in csvFiles)
            {
                var fileName = Path.GetFileName(file);
                _logger.LogInformation($"Processando arquivo: {fileName}");

                var recordsImported = await ReadAndSaveCsvDataAsync(file);
                _logger.LogInformation($"{recordsImported} registro(s) importado(s) do arquivo {fileName}");

                MoveToProcessedFolder(file, fileName);
            }

            _logger.LogInformation("Processamento de arquivos CSV concluído.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao processar arquivos CSV.");
        }
    }

    private async Task<int> ReadAndSaveCsvDataAsync(string filePath)
    {
        try
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = ";",
                MissingFieldFound = null,
                BadDataFound = null
            };

            using var reader = new StreamReader(filePath);
            using var csv = new CsvReader(reader, config);

            csv.Context.RegisterClassMap<MovieListHistoryCsvMap>();
            var records = csv.GetRecords<MovieListHistoryCsv>().ToList();

            if (records.Count == 0)
            {
                _logger.LogWarning($"Nenhum registro encontrado no arquivo: {Path.GetFileName(filePath)}");
                return 0;
            }

            foreach (var record in records)
            {
                try
                {
                    var movie = new MovieListHistory(
                        record.Year,
                        record.Title,
                        record.Studios,
                        record.Producers,
                        record.Winner
                    );

                    await _repository.AddAsync(movie);
                }
                catch (ArgumentException ex)
                {
                    _logger.LogWarning(ex, "Registro inválido ignorado: {Title}", record.Title);
                }
            }

            await _repository.SaveChangesAsync();

            _logger.LogInformation($"Dados importados com sucesso do arquivo: {Path.GetFileName(filePath)}");
            return records.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao ler e gravar dados do arquivo CSV: {Path.GetFileName(filePath)}");
            return 0;
        }
    }

    private void MoveToProcessedFolder(string file, string fileName)
    {
        var destinationPath = Path.Combine(_fileProcessedPath, fileName);

        if (File.Exists(destinationPath))
        {
            var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            var extension = Path.GetExtension(fileName);
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            fileName = $"{fileNameWithoutExtension}_{timestamp}{extension}";
            destinationPath = Path.Combine(_fileProcessedPath, fileName);
        }

        File.Move(file, destinationPath);
        _logger.LogInformation($"Arquivo movido: {fileName} -> fileProcessed/");
    }
}

// Classe auxiliar para mapear o CSV
internal class MovieListHistoryCsv
{
    public int Year { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Studios { get; set; }
    public string? Producers { get; set; }
    public string? Winner { get; set; }
}

// Mapeamento das colunas do CSV
internal sealed class MovieListHistoryCsvMap : ClassMap<MovieListHistoryCsv>
{
    public MovieListHistoryCsvMap()
    {
        Map(m => m.Year).Name("year");
        Map(m => m.Title).Name("title");
        Map(m => m.Studios).Name("studios");
        Map(m => m.Producers).Name("producers");
        Map(m => m.Winner).Name("winner").Optional();
    }
}

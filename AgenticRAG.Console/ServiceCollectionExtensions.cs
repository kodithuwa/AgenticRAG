using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Embeddings;
using Npgsql;
using RagApp.Data;
using RagApp.Services;

namespace RagApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRagServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Database ──────────────────────────────────────────────────────────
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");

        services.AddDbContext<RagDbContext>(options => 
                                            options.UseNpgsql(connectionString,
                                            npgsqlOptions => npgsqlOptions.UseVector()));

        services.AddScoped<DatabaseInitializer>();

        //    var connectionString = configuration.GetConnectionString("DefaultConnection")
        //?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.");

        //    // Register NpgsqlDataSource with Vector type mapping — this is what fixes
        //    // "Writing values of 'Pgvector.Vector' is not supported" InvalidCastException
        //    var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        //    dataSourceBuilder.UseVector();
        //    var dataSource = dataSourceBuilder.Build();

        //    services.AddSingleton(dataSource);

        //    services.AddDbContext<RagDbContext>(options =>
        //        options.UseNpgsql(dataSource)
        //               .EnableSensitiveDataLogging(false)
        //               .EnableDetailedErrors(false));

        //    services.AddScoped<DatabaseInitializer>();

        // ── Semantic Kernel ───────────────────────────────────────────────────
        var ollamaSection = configuration.GetSection("Ollama");
        var baseUrl = ollamaSection["BaseUrl"]
            ?? throw new InvalidOperationException("Ollama:BaseUrl is required.");
        var embeddingModel = ollamaSection["EmbeddingModel"] ?? "text-embedding-3-small";
        var chatModel = ollamaSection["ChatModel"] ?? "gpt-4o-mini";

        var kernelBuilder = Kernel.CreateBuilder();
        kernelBuilder.AddOllamaTextEmbeddingGeneration(
            modelId: "nomic-embed-text",        // or "mxbai-embed-large", etc.
            endpoint: new Uri("http://localhost:11434")
        );

        kernelBuilder.AddOllamaChatCompletion(
            modelId: "llama3.2",   // ← swap to any model you pulled
            endpoint: new Uri("http://localhost:11434")
        );

        var kernel = kernelBuilder.Build();
        services.AddSingleton(kernel);
        services.AddSingleton(kernel.GetRequiredService<ITextEmbeddingGenerationService>());
        services.AddSingleton(kernel.GetRequiredService<IChatCompletionService>());

        // ── Application services ──────────────────────────────────────────────
        services.AddScoped<EmbeddingService>();
        services.AddScoped<DocumentService>();
        services.AddScoped<VectorSearchService>();
        services.AddScoped<RagService>();

        // ── Logging ───────────────────────────────────────────────────────────
        services.AddLogging(b => b
            .AddConsole()
            .SetMinimumLevel(LogLevel.Information));

        return services;
    }
}

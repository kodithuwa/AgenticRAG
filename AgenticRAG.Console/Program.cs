using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RagApp;
using RagApp.Data;
using RagApp.Services;

// ── Configuration ─────────────────────────────────────────────────────────────
var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false)
    .AddEnvironmentVariables()
    .Build();

// ── DI container ──────────────────────────────────────────────────────────────
var services = new ServiceCollection();
services.AddRagServices(configuration);
await using var provider = services.BuildServiceProvider();

// ── Initialise database ───────────────────────────────────────────────────────
using (var scope = provider.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await initializer.InitializeAsync();
}

// ── REPL ──────────────────────────────────────────────────────────────────────
PrintBanner();

while (true)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.Write("\n> ");
    Console.ResetColor();

    var input = Console.ReadLine()?.Trim();
    if (string.IsNullOrWhiteSpace(input)) continue;

    var parts = input.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
    var command = parts[0].ToLowerInvariant();

    using var scope = provider.CreateScope();

    switch (command)
    {
        // ── ingest <source> <text...> ─────────────────────────────────────
        case "ingest":
        {
            if (parts.Length < 2)
            {
                PrintUsage();
                break;
            }
            var argParts = parts[1].Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
            if (argParts.Length < 2)
            {
                Console.WriteLine("Usage: ingest <source-name> <content text...>");
                break;
            }

            var source  = argParts[0];
            var content = argParts[1];

            var docService = scope.ServiceProvider.GetRequiredService<DocumentService>();
            Console.WriteLine($"Ingesting '{source}'...");
            await docService.IngestDocumentAsync(source, content);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ Document ingested successfully.");
            Console.ResetColor();
            break;
        }

        // ── ingest-file <path> ────────────────────────────────────────────
        case "ingest-file":
        {
            if (parts.Length < 2 || !File.Exists(parts[1]))
            {
                Console.WriteLine("File not found. Usage: ingest-file <path>");
                break;
            }

            var path    = parts[1];
            var content = await File.ReadAllTextAsync(path);
            var source  = Path.GetFileName(path);

            var docService = scope.ServiceProvider.GetRequiredService<DocumentService>();
            Console.WriteLine($"Ingesting file '{source}' ({content.Length} chars)...");
            await docService.IngestDocumentAsync(source, content);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("✓ File ingested successfully.");
            Console.ResetColor();
            break;
        }

        // ── ask <question> ────────────────────────────────────────────────
        case "ask":
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Usage: ask <your question>");
                break;
            }

            var ragConfig    = configuration.GetSection("Rag");
            var topK         = ragConfig.GetValue("TopK", 3);
            var minScore     = ragConfig.GetValue("MinRelevanceScore", 0.0);

            var ragService   = scope.ServiceProvider.GetRequiredService<RagService>();

            Console.WriteLine("\nSearching knowledge base...");
            var answer = await ragService.AskAsync(parts[1], topK, minScore);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n── Answer ───────────────────────────────────────────────────────");
            Console.ResetColor();
            Console.WriteLine(answer);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("─────────────────────────────────────────────────────────────────");
            Console.ResetColor();
            break;
        }

        // ── search <query> ────────────────────────────────────────────────
        case "search":
        {
            if (parts.Length < 2)
            {
                Console.WriteLine("Usage: search <query>");
                break;
            }

            var searchService = scope.ServiceProvider.GetRequiredService<VectorSearchService>();
            var results       = await searchService.SearchAsync(parts[1], topK: 5);

            if (results.Count == 0)
            {
                Console.WriteLine("No results found.");
                break;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"\nTop {results.Count} results:");
            Console.ResetColor();

            foreach (var (result, idx) in results.Select((r, i) => (r, i + 1)))
            {
                Console.WriteLine($"\n[{idx}] {result.Chunk.Source} — score: {result.Score:P1}");
                Console.WriteLine($"    {result.Chunk.Content[..Math.Min(200, result.Chunk.Content.Length)]}...");
            }
            break;
        }

        case "help":
            PrintUsage();
            break;

        case "exit":
        case "quit":
            Console.WriteLine("Goodbye!");
            return;

        default:
            Console.WriteLine($"Unknown command '{command}'. Type 'help' for available commands.");
            break;
    }
}

static void PrintBanner()
{
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("""
        ╔══════════════════════════════════════════════════════╗
        ║          .NET 10 RAG Console — pgvector + SK         ║
        ╚══════════════════════════════════════════════════════╝
        """);
    Console.ResetColor();
    PrintUsage();
}

static void PrintUsage()
{
    Console.WriteLine("""

    Commands:
      ingest <source> <text>     Ingest a text snippet into the knowledge base
      ingest-file <path>         Ingest a .txt file into the knowledge base
      search <query>             Semantic search (returns raw chunks)
      ask <question>             Ask a question (full RAG pipeline)
      help                       Show this help
      exit / quit                Exit the application
    """);
}

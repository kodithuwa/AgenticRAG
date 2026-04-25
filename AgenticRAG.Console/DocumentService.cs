using Microsoft.Extensions.Logging;
using Pgvector;
using RagApp.Data;
using RagApp.Models;

namespace RagApp.Services;

/// <summary>
/// Handles document ingestion: splits text into chunks and stores them with embeddings.
/// </summary>
public class DocumentService(
    RagDbContext dbContext,
    EmbeddingService embeddingService,
    ILogger<DocumentService> logger)
{
    // nomic-embed-text REAL context limit in Ollama = 2048 tokens
    // ~4 chars/token → 2048 tokens ≈ 8000 chars ≈ ~400 words
    // Using 200 words to stay well within the safe limit
    private const int DefaultChunkSize = 200;
    private const int DefaultChunkOverlap = 20;
    private const int MaxCharsPerChunk = 6000; // hard character safety cap

    public async Task IngestDocumentAsync(
        string source,
        string content,
        string? metadata = null,
        int chunkSize = DefaultChunkSize,
        int overlap = DefaultChunkOverlap,
        CancellationToken ct = default)
    {
        logger.LogInformation("Ingesting document '{Source}' ({Length} chars)", source, content.Length);

        var chunks = SplitIntoChunks(content, chunkSize, overlap);
        logger.LogInformation("Split into {Count} chunks", chunks.Count);

        var entities = new List<DocumentChunk>();

        // Embed one chunk at a time — Ollama does not support batch embedding
        for (int i = 0; i < chunks.Count; i++)
        {
            logger.LogInformation("Embedding chunk {Current}/{Total}", i + 1, chunks.Count);
            var embedding = await embeddingService.GetEmbeddingAsync(chunks[i], ct);

            entities.Add(new DocumentChunk
            {
                Source = source,
                Content = chunks[i],
                Metadata = metadata is not null ? $"{metadata} | chunk:{i + 1}" : $"chunk:{i + 1}",
                Embedding = new Vector(embedding)
            });
        }

        await dbContext.DocumentChunks.AddRangeAsync(entities, ct);
        await dbContext.SaveChangesAsync(ct);

        logger.LogInformation("Saved {Count} chunks for '{Source}'", entities.Count, source);
    }

    private static List<string> SplitIntoChunks(string text, int chunkSize, int overlap)
    {
        var chunks = new List<string>();
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int start = 0;
        while (start < words.Length)
        {
            int end = Math.Min(start + chunkSize, words.Length);
            var chunk = string.Join(' ', words[start..end]);

            // Hard cap: truncate if chunk exceeds max char limit
            if (chunk.Length > MaxCharsPerChunk)
                chunk = chunk[..MaxCharsPerChunk];

            chunks.Add(chunk);
            if (end == words.Length) break;
            start += chunkSize - overlap;
        }

        return chunks;
    }
}

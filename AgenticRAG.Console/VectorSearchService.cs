using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using RagApp.Data;
using RagApp.Models;

namespace RagApp.Services;

public record SearchResult(DocumentChunk Chunk, double Score);

/// <summary>
/// Performs approximate nearest-neighbour search using pgvector cosine distance.
/// </summary>
public class VectorSearchService(
    RagDbContext dbContext,
    EmbeddingService embeddingService,
    ILogger<VectorSearchService> logger)
{
    public async Task<IReadOnlyList<SearchResult>> SearchAsync(
        string query,
        int topK = 3,
        double minScore = 0.0,
        CancellationToken ct = default)
    {
        logger.LogInformation("Searching for top {TopK} results for query: {Query}", topK, query);

        var queryEmbedding = await embeddingService.GetEmbeddingAsync(query, ct);
        var queryVector = new Vector(queryEmbedding);

        // Cosine distance: 0 = identical, 2 = opposite. Score = 1 - distance.
        var results = await dbContext.DocumentChunks
            .Where(c => c.Embedding != null)
            .OrderBy(c => c.Embedding!.CosineDistance(queryVector))
            .Take(topK)
            .Select(c => new
            {
                Chunk    = c,
                Distance = c.Embedding!.CosineDistance(queryVector)
            })
            .ToListAsync(ct);

        return results
            .Select(r => new SearchResult(r.Chunk, 1.0 - (double)r.Distance))
            .Where(r => r.Score >= minScore)
            .ToList();
    }
}

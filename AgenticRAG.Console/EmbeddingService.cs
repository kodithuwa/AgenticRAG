using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.Embeddings;

namespace RagApp.Services;

/// <summary>
/// Wraps the Semantic Kernel ITextEmbeddingGenerationService to produce float[] vectors.
/// </summary>
public class EmbeddingService(
    ITextEmbeddingGenerationService embeddingGenerator,
    ILogger<EmbeddingService> logger)
{
    // nomic-embed-text real limit in Ollama = 2048 tokens ≈ 6000 chars safe limit
    private const int MaxCharLength = 6_000;

    private static string Truncate(string text) =>
        text.Length > MaxCharLength ? text[..MaxCharLength] : text;

    public async Task<float[]> GetEmbeddingAsync(string text, CancellationToken ct = default)
    {
        var truncated = Truncate(text);
        if (truncated.Length < text.Length)
            logger.LogWarning("Text truncated from {Original} to {Truncated} chars for embedding", text.Length, truncated.Length);

        logger.LogDebug("Generating embedding for text of length {Length}", truncated.Length);
        var result = await embeddingGenerator.GenerateEmbeddingAsync(truncated, cancellationToken: ct);
        return result.ToArray();
    }

    public async Task<IReadOnlyList<float[]>> GetEmbeddingsAsync(
        IEnumerable<string> texts,
        CancellationToken ct = default)
    {
        var textList = texts.ToList();
        logger.LogDebug("Generating embeddings for {Count} texts one by one", textList.Count);

        // Ollama does not support batch embedding — must call one at a time
        var results = new List<float[]>();
        foreach (var text in textList)
        {
            var embedding = await GetEmbeddingAsync(text, ct);
            results.Add(embedding);
        }
        return results;
    }
}

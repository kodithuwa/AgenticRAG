using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace RagApp.Services;

/// <summary>
/// Orchestrates the RAG pipeline: retrieve relevant chunks → inject into prompt → generate answer.
/// </summary>
public class RagService(
    VectorSearchService vectorSearch,
    IChatCompletionService chatCompletion,
    ILogger<RagService> logger)
{
    private const string SystemPrompt = """
        You are a helpful assistant that answers questions based solely on the provided context.
        If the answer cannot be found in the context, say "I don't have enough information to answer that."
        Be concise and accurate. Always cite the source document when relevant.
        """;

    public async Task<string> AskAsync(
        string question,
        int topK = 3,
        double minRelevanceScore = 0.0,
        CancellationToken ct = default)
    {
        // 1. Retrieve relevant context
        var searchResults = await vectorSearch.SearchAsync(question, topK, minRelevanceScore, ct);

        if (searchResults.Count == 0)
        {
            logger.LogWarning("No relevant documents found for question: {Question}", question);
            return "I don't have enough information to answer that question.";
        }

        // 2. Build context block
        var contextBuilder = new System.Text.StringBuilder();
        contextBuilder.AppendLine("=== CONTEXT ===");
        foreach (var (result, index) in searchResults.Select((r, i) => (r, i + 1)))
        {
            contextBuilder.AppendLine($"[{index}] Source: {result.Chunk.Source} (relevance: {result.Score:P1})");
            contextBuilder.AppendLine(result.Chunk.Content);
            contextBuilder.AppendLine();
        }
        contextBuilder.AppendLine("=== END CONTEXT ===");

        logger.LogInformation("Found {Count} relevant chunks. Building prompt...", searchResults.Count);

        // 3. Call chat model
        var history = new ChatHistory(SystemPrompt);
        history.AddUserMessage($"{contextBuilder}\n\nQuestion: {question}");

        var response = await chatCompletion.GetChatMessageContentAsync(history, cancellationToken: ct);
        return response.Content ?? "No response generated.";
    }
}

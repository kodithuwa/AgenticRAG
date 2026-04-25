using Pgvector;

namespace RagApp.Models;

/// <summary>
/// Represents a document chunk stored in the vector database.
/// </summary>
public class DocumentChunk
{
    public int Id { get; set; }

    /// <summary>The source document title or filename.</summary>
    public string Source { get; set; } = string.Empty;

    /// <summary>The actual text content of this chunk.</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>Optional metadata (e.g. page number, section).</summary>
    public string? Metadata { get; set; }

    /// <summary>Vector embedding produced by the embedding model (1536 dims for text-embedding-3-small).</summary>
    public Vector? Embedding { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

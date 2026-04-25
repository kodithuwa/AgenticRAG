using Microsoft.EntityFrameworkCore;
using Npgsql;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using RagApp.Models;

namespace RagApp.Data;

public class RagDbContext(DbContextOptions<RagDbContext> options) : DbContext(options)
{
    public DbSet<DocumentChunk> DocumentChunks => Set<DocumentChunk>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasPostgresExtension("vector");

        modelBuilder.Entity<DocumentChunk>(entity =>
        {
            entity.ToTable("document_chunks");

            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                  .HasColumnName("id")
                  .UseIdentityAlwaysColumn();

            entity.Property(e => e.Source)
                  .HasColumnName("source")
                  .HasMaxLength(500)
                  .IsRequired();

            entity.Property(e => e.Content)
                  .HasColumnName("content")
                  .IsRequired();

            entity.Property(e => e.Metadata)
                  .HasColumnName("metadata");

            entity.Property(e => e.Embedding)
                  .HasColumnName("embedding")
                  .HasColumnType("vector(768)")  // 768 dims for nomic-embed-text
                  .HasConversion(
                      v => v,
                      v => v);

            entity.Property(e => e.CreatedAt)
                  .HasColumnName("created_at")
                  .HasDefaultValueSql("NOW()");

            // IVFFlat index for approximate nearest-neighbour search
            entity.HasIndex(e => e.Embedding)
                  .HasMethod("ivfflat")
                  .HasOperators("vector_cosine_ops")
                  .HasStorageParameter("lists", 100)
                  .HasDatabaseName("ix_document_chunks_embedding");
        });
    }
}

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RagApp.Data;

public class DatabaseInitializer(RagDbContext dbContext, ILogger<DatabaseInitializer> logger)
{
    public async Task InitializeAsync(CancellationToken ct = default)
    {
        logger.LogInformation("Ensuring database is created and migrations are applied...");
        await dbContext.Database.MigrateAsync(ct);
        logger.LogInformation("Database ready.");
    }
}

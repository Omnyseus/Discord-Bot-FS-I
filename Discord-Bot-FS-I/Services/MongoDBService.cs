using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Discord_Bot_FS_I.Services;

// ReSharper disable once InconsistentNaming
public class MongoDBService
{
    #region Constants

    private const string DatabaseName = "Discord_Bot_FSI_DB";

    #endregion

    #region Fields

    private readonly IMongoDatabase _db;
    private readonly ILogger<MongoDBService> _logger;

    #endregion

    #region Constructors

    public MongoDBService(ILogger<MongoDBService> logger)
    {
        _logger = logger;
        var connectionString = "TODO inject";

        _logger.LogInformation("Connecting to MongoDB");
        var client = new MongoClient(connectionString);
        _db = client.GetDatabase(DatabaseName);
    }

    #endregion

    #region Pulbic Methods

    public IMongoCollection<T> GetCollection<T>(string collectionName)
        => _db.GetCollection<T>(collectionName);

    #endregion
}

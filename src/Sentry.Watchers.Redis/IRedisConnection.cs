using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Sentry.Watchers.Redis
{
    public interface IRedisConnection
    {

        /// <summary>
        /// Connection string of the Redis server.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Timeout for connection to the Redis server.
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// Opens a connection to the Redis database.
        /// </summary>
        /// <returns>Instance of IRedis.</returns>
        Task<IRedis> GetDatabaseAsync(int database);
    }

    /// <summary>
    /// Default implementation of the IRedisConnection based on the StackExchange.Redis.
    /// </summary>
    public class RedisConnection : IRedisConnection
    {
        private IConnectionMultiplexer _connectionMultiplexer;

        public string ConnectionString { get; }
        public TimeSpan Timeout { get; }

        public RedisConnection(string connectionString, TimeSpan timeout)
        {
            ConnectionString = connectionString;
            Timeout = timeout;
        }

        public async Task<IRedis> GetDatabaseAsync(int databaseId)
        {
            _connectionMultiplexer = await ConnectionMultiplexer.ConnectAsync(ConnectionString);
            var database = _connectionMultiplexer.GetDatabase(databaseId);
            var ping = await database.PingAsync();

            return new Redis(database);
        }
    }
}
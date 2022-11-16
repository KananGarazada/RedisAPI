using RedisAPI.Model;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisAPI.Data
{
    public class RedisPlatformRepo : IPlatformRepo
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisPlatformRepo(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }

        public void CreatePlatform(Platform plat)
        {
            if(plat == null)
            {
                throw new ArgumentNullException(nameof(plat));
            }

            var db = _redis.GetDatabase();

            var serialPlat = JsonSerializer.Serialize(plat);

            db.StringSet(plat.Id, serialPlat);
        }

        public IEnumerable<Platform> GetAllPlatforms()
        {
            throw new NotImplementedException();
        }

        public Platform? GetPlatformById(string platformId)
        {
            var db = _redis.GetDatabase();

            var plat = db.StringGet(platformId);

            if(!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }

            return null;
        }
    }
}

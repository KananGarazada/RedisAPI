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

            //db.StringSet(plat.Id, serialPlat);
            //db.SetAdd("PlatformSet", serialPlat);
            db.HashSet("hashplatform", new HashEntry[]
                {new HashEntry(plat.Id, serialPlat)});
        }

        public IEnumerable<Platform?>? GetAllPlatforms()
        {
            var db = _redis.GetDatabase();

            //var completeSet = db.SetMembers("PlatformSet");

            var completeSet = db.HashGetAll("hashplatform");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
                return obj;
            }

            return null;
        }

        public Platform? GetPlatformById(string platformId)
        {
            var db = _redis.GetDatabase();

            //var plat = db.StringGet(platformId);

            var plat = db.HashGet("hashplatform", platformId);

            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<Platform>(plat);
            }

            return null;
        }
    }
}

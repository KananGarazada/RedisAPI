using RedisAPI.Model;

namespace RedisAPI.Data
{
    public interface IPlatformRepo
    {
        void CreatePlatform(Platform plat);
        Platform? GetPlatformById(string platformId);
        IEnumerable<Platform> GetAllPlatforms();
    }
}

using System.Net;
using System.Text.Json;
using StackExchange.Redis;

namespace MGH.Core.Infrastructure.Cache.Redis.Services;

public class CachingService<T>(IConnectionMultiplexer redis) : ICachingService<T>
    where T : class
{
    public T Get(string key)
    {
        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        return JsonSerializer.Deserialize<T>(redis1.StringGet(key));
    }

    public async Task<T> GetAsync(string key)
    {
        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        var result = await redis1.StringGetAsync(key);
        return JsonSerializer.Deserialize<T>(result);
    }

    public IEnumerable<T> GetList(string key)
    {
        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        var result = redis1.StringGet(key);
        return JsonSerializer.Deserialize<IEnumerable<T>>(result);
    }

    public async Task<IEnumerable<T>> GetListAsync(string key)
    {
        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        var result = await redis1.StringGetAsync(key);
        return JsonSerializer.Deserialize<IEnumerable<T>>(result);
    }

    public void Remove(string key)
    {
        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        redis1.KeyDelete(key);
    }



    public void RemoveByDb(int dbNumber)
    {
        var endPoints = redis.GetEndPoints();
        foreach (var item in endPoints)
        {
            var server = redis.GetServer(item);
            RedisKey[] enumerable = server.Keys(dbNumber, pattern: "*").ToArray();
            foreach (var current in enumerable)
                Remove(current, dbNumber);
        }
    }

    public void FlushAll()
    {
        var endpoints = GetRedisEndPoint();
        foreach (var endpoint in endpoints)
        {
            var server = redis.GetServer(endpoint);
            server.FlushAllDatabases();
        }
    }

    public void RemoveByPattern(string pattern)
    {
        var endpoints = GetRedisEndPoint();
        foreach (var endpoint in endpoints)
        {
            var servers = redis.GetServer(endpoint);
            var dbNumber = SelectDbByKeyName(pattern);
            var keys = servers.Keys(dbNumber, pattern + "*").ToArray();
            foreach (var key in keys)
            {
                var redis1 = redis.GetDatabase(dbNumber);
                redis1.KeyDelete(key);
            }
        }
    }

    public void Set(string key, object obj, int time = 3)
    {
        var cacheObj = JsonSerializer.Serialize(obj);
        var ts = new TimeSpan(0, time, 0);
        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        redis1.StringSet(key, cacheObj, ts);

    }

    public async Task SetAsync(string key, object obj, int time = 3)
    {
        var cacheObj = JsonSerializer.Serialize(obj);
        var ts = new TimeSpan(0, time, 0);

        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        await redis1.StringSetAsync(key, cacheObj, ts);
    }

    private int SelectDbByKeyName(string key)
    {
        key = key.Trim().ToLower();
        if (key.StartsWith("a") || key.StartsWith("b") || key.StartsWith("c") || key.StartsWith("d"))
            return 0;
        if (key.StartsWith("e") || key.StartsWith("f") || key.StartsWith("g") || key.StartsWith("h"))
            return 1;
        if (key.StartsWith("i") || key.StartsWith("j") || key.StartsWith("k") || key.StartsWith("l"))
            return 2;
        if (key.StartsWith("m") || key.StartsWith("n") || key.StartsWith("o") || key.StartsWith("p"))
            return 3;
        if (key.StartsWith("q") || key.StartsWith("r") || key.StartsWith("s") || key.StartsWith("t"))
            return 4;
        if (key.StartsWith("u") || key.StartsWith("v") || key.StartsWith("w") || key.StartsWith("x"))
            return 5;
        if (key.StartsWith("y") || key.StartsWith("z"))
            return 6;
        return 0;
    }

    public bool Exists(string key)
    {
        var redis1 = redis.GetDatabase(SelectDbByKeyName(key));
        return redis1.KeyExists(key);
    }

    private EndPoint[] GetRedisEndPoint()
    {
        var endpoints = redis.GetEndPoints(true);
        return endpoints;
    }

    private void Remove(string key, int dbNumber)
    {
        var redis1 = redis.GetDatabase(dbNumber);
        redis1.KeyDelete(key);
    }
}
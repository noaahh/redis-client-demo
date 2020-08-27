using System;
using System.Collections.Generic;
using ServiceStack.Redis;

namespace RedisClientDemo
{
    internal class Program
    {
        private static IRedisClient _redisClient;

        public static void Main(string[] args)
        {
            // redis://clientid:password@localhost:6380?ssl=true&db=1
            var clientsManager = new BasicRedisClientManager("localhost:6379");
            try
            {
                _redisClient = clientsManager.GetClient();
                PrintCredentials();

                var valueMap = new Dictionary<string, string>();
                for (var i = 0; i < 100; i++)
                {
                    valueMap.Add($"key_{i}", $"value_{i}");
                }
                _redisClient.SetAll(valueMap);
                // _redisClient.RemoveAll(valueMap.Keys);

                Console.WriteLine("Getting all stored values");
                var values = _redisClient.GetAll<string>(valueMap.Keys);
                PrintDictionary(values);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void PrintCredentials()
        {
            Console.WriteLine($"Host: {_redisClient.Host} | Password: {_redisClient.Password}");
        }

        private static void PrintInfo()
        {
            var info = _redisClient.Info;
            foreach (var (key, value) in info)
            {
                Console.WriteLine($"{key}: {value}");
            }
        }

        private static void PrintDictionary(IDictionary<string, string> dictionary)
        {
            foreach (var (key, value) in dictionary)
            {
                Console.WriteLine($"Key: {key} | Value: {value}");
            }
        }
    }
}

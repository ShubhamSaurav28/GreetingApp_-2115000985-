using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using StackExchange.Redis;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingAppContext _dbContext;
        private readonly StackExchange.Redis.IDatabase _redisDb;

        public GreetingRL(GreetingAppContext context, IConnectionMultiplexer redis)
        {
            _dbContext = context;
            _redisDb = redis.GetDatabase();
        }

        public string GetHelloRL()
        {
            return "Hello World";
        }

        public string PostHelloRL(PostRequestModel postRequestModel)
        {
            if (postRequestModel == null)
            {
                throw new ArgumentNullException(nameof(postRequestModel), "PostRequestModel cannot be null.");
            }

            bool hasFirstName = !string.IsNullOrWhiteSpace(postRequestModel.FirstName);
            bool hasLastName = !string.IsNullOrWhiteSpace(postRequestModel.LastName);

            if (hasFirstName && hasLastName)
            {
                return $"Hello {postRequestModel.FirstName} {postRequestModel.LastName}";
            }
            else if (hasFirstName)
            {
                return $"Hello {postRequestModel.FirstName}";
            }
            else if (hasLastName)
            {
                return $"Hello Mr.{postRequestModel.LastName}";
            }
            else
            {
                return "Hello World";
            }
        }

        public async Task<string> UserGreetingRL(GreetingRequestModel greetingRequestModel, int userId)
        {
            if (greetingRequestModel == null)
            {
                throw new ArgumentNullException(nameof(greetingRequestModel), "GreetingRequestModel cannot be null.");
            }

            var greeting = new MessageEntity
            {
                Message = greetingRequestModel.Greeting,
                UserId = userId,
            };

            _dbContext.GreetingMessage.Add(greeting);
            await _dbContext.SaveChangesAsync();

            // Invalidate cache
            await _redisDb.KeyDeleteAsync("GreetingMessages");

            return greeting.Message;
        }

        public async Task<string> GreetingFindRL(GreetingIdFind greetingIdFind)
        {
            if (greetingIdFind == null)
            {
                throw new ArgumentNullException(nameof(greetingIdFind), "GreetingIdFind cannot be null.");
            }

            string cacheKey = $"Greeting:{greetingIdFind.Id}";
            var cachedData = await _redisDb.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
            {
                return cachedData.ToString();
            }

            var result = await _dbContext.GreetingMessage.FirstOrDefaultAsync(g => g.Id == greetingIdFind.Id);

            if (result == null)
            {
                return null;
            }

            await _redisDb.StringSetAsync(cacheKey, result.Message, TimeSpan.FromMinutes(10));

            return result.Message;
        }

        public async Task<List<MessageEntity>> GetAllMessagesRL()
        {
            string cacheKey = "GreetingMessages";
            var cachedData = await _redisDb.StringGetAsync(cacheKey);

            if (!cachedData.IsNullOrEmpty)
            {
                Console.WriteLine("✅ Fetching data from Redis Cache...");
                return JsonSerializer.Deserialize<List<MessageEntity>>(cachedData);
            }

            Console.WriteLine("⚠️ Cache miss! Fetching data from Database...");
            var messages = await _dbContext.GreetingMessage.ToListAsync();

            // Store the data in Redis for future requests
            await _redisDb.StringSetAsync(cacheKey, JsonSerializer.Serialize(messages), TimeSpan.FromMinutes(10));

            return messages;
        }


        public async Task<string> EditGreetingRL(int id, string updatedMessage)
        {
            try
            {
                var greeting = await _dbContext.GreetingMessage.FirstOrDefaultAsync(g => g.Id == id);
                if (greeting == null)
                {
                    return null;
                }

                greeting.Message = updatedMessage;
                await _dbContext.SaveChangesAsync();

                // Invalidate cache
                await _redisDb.KeyDeleteAsync($"Greeting:{id}");
                await _redisDb.KeyDeleteAsync("GreetingMessages");

                return greeting.Message;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> DeleteGreetingRL(int id)
        {
            try
            {
                var greeting = await _dbContext.GreetingMessage.FirstOrDefaultAsync(g => g.Id == id);
                if (greeting == null)
                {
                    return false;
                }

                _dbContext.GreetingMessage.Remove(greeting);
                await _dbContext.SaveChangesAsync();

                // Invalidate cache
                await _redisDb.KeyDeleteAsync($"Greeting:{id}");
                await _redisDb.KeyDeleteAsync("GreetingMessages");

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

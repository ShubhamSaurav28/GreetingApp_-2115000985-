using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
        private readonly GreetingAppContext _dbContext;
        public GreetingRL(GreetingAppContext context)
        {
            _dbContext = context;
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
        public string UserGreetingRL(GreetingRequestModel greetingRequestModel)
        {
            if (greetingRequestModel == null)
            {
                throw new ArgumentNullException(nameof(greetingRequestModel), "GreetingRequestModel cannot be null.");
            }

            var greeting = new MessageEntity
            {
                Message = greetingRequestModel.Greeting,
            };

            _dbContext.GreetingMessage.Add(greeting);
            _dbContext.SaveChanges();

            return greeting.Message;
        }
        public string GreetingFindRL(GreetingIdFind greetingIdFind) 
        {
            if (greetingIdFind == null)
            {
                throw new ArgumentNullException(nameof(greetingIdFind), "GreetingIdFind cannot be null.");
            }

            var result = _dbContext.GreetingMessage.FirstOrDefault(g => g.Id == greetingIdFind.Id);

            if (result == null)
            {
                return null;
            }

            return result.Message;
        }
        public List<MessageEntity> GetAllMessagesRL()
        {
            try
            {
                return _dbContext.GreetingMessage.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<MessageEntity>();
            }
        }
        public string EditGreetingRL(int id, string updatedMessage)
        {
            try
            {
                var greeting = _dbContext.GreetingMessage.FirstOrDefault(g => g.Id == id);
                if (greeting == null)
                {
                    return null;
                }

                greeting.Message = updatedMessage;
                _dbContext.SaveChanges();
                return greeting.Message;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public bool DeleteGreetingRL(int id)
        {
            try
            {
                var greeting = _dbContext.GreetingMessage.FirstOrDefault(g => g.Id == id);
                if (greeting == null)
                {
                    return false;
                }
                _dbContext.GreetingMessage.Remove(greeting);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}

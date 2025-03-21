using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        public string GetHelloRL();
        public string PostHelloRL(PostRequestModel postRequestModel);
        public Task<string> UserGreetingRL(GreetingRequestModel greetingRequestModel, int userId);
        public Task<string> GreetingFindRL(GreetingIdFind greetingIdFind);
        public Task<List<MessageEntity>> GetAllMessagesRL();
        public Task<string> EditGreetingRL(int id, string updatedMessage);
        public Task<bool> DeleteGreetingRL(int id);


    }
}

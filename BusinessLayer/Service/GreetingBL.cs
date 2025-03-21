using System.Collections.Generic;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;

namespace BusinessLayer.Service
{
    public class GreetingBL : IGreetingBL
    {
        private readonly IGreetingRL _greetingRL;

        public GreetingBL(IGreetingRL greetingRL)
        {
            _greetingRL = greetingRL;
        }

        public string GetHelloBL()
        {
            return _greetingRL.GetHelloRL();
        }

        public string PostHelloBL(PostRequestModel postRequestModel)
        {
            return _greetingRL.PostHelloRL(postRequestModel);
        }

        public async Task<string> UserGreetingBL(GreetingRequestModel greetingRequestModel, int userId)
        {
            return await _greetingRL.UserGreetingRL(greetingRequestModel, userId);
        }

        public async Task<string> GreetingFindBL(GreetingIdFind greetingIdFind)
        {
            return await _greetingRL.GreetingFindRL(greetingIdFind);
        }

        public async Task<List<MessageEntity>> GetAllMessagesBL()
        {
            return await _greetingRL.GetAllMessagesRL();
        }

        public async Task<string> EditGreetingBL(int id, string updatedMessage)
        {
            return await _greetingRL.EditGreetingRL(id, updatedMessage);
        }

        public async Task<bool> DeleteGreetingBL(int id)
        {
            return await _greetingRL.DeleteGreetingRL(id);
        }
    }
}

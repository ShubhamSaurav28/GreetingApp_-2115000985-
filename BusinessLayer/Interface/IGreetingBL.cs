using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace BusinessLayer.Interface
{
    public interface IGreetingBL
    {
        public string GetHelloBL();
        public string PostHelloBL(PostRequestModel postRequestModel);
        public Task<string> UserGreetingBL(GreetingRequestModel greetingRequestModel, int userId);
        public Task<string> GreetingFindBL(GreetingIdFind greetingIdFind);
        public Task<List<MessageEntity>> GetAllMessagesBL();
        public Task<string> EditGreetingBL(int id, string updatedMessage);
        public Task<bool> DeleteGreetingBL(int id);


    }
}

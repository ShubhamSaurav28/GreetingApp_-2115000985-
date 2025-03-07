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
        public string UserGreetingRL(GreetingRequestModel greetingRequestModel);
        public string GreetingFindRL(GreetingIdFind greetingIdFind);
        public List<MessageEntity> GetAllMessagesRL();
        public string EditGreetingRL(int id, string updatedMessage);
        public bool DeleteGreetingRL(int id);


    }
}

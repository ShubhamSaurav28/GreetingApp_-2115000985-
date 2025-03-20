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
        public string UserGreetingBL(GreetingRequestModel greetingRequestModel, int userId);
        public string GreetingFindBL(GreetingIdFind greetingIdFind);
        public List<MessageEntity> GetAllMessagesBL();
        public string EditGreetingBL(int id, string updatedMessage);
        public bool DeleteGreetingBL(int id);


    }
}

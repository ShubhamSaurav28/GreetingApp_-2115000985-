using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interface;
using ModelLayer.Model;
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
            var result = _greetingRL.PostHelloRL(postRequestModel);
            return result;
        }
        public string UserGreetingBL(GreetingRequestModel greetingRequestModel)
        {
            var result = _greetingRL.UserGreetingRL(greetingRequestModel);
            return result;
        }

    }
}

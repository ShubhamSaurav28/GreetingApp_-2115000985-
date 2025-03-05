using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;

namespace RepositoryLayer.Interface
{
    public interface IGreetingRL
    {
        public string GetHelloRL();
        public string PostHelloRL(PostRequestModel postRequestModel);
        public string UserGreetingRL(GreetingRequestModel greetingRequestModel);

    }
}

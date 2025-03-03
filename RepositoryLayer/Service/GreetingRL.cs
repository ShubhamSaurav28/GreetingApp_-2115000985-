using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelLayer.Model;
using RepositoryLayer.Interface;

namespace RepositoryLayer.Service
{
    public class GreetingRL : IGreetingRL
    {
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

    }
}

using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// Class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class HelloGreetingController : ControllerBase
    {
        private static Dictionary<string, string> dict = new Dictionary<string, string>();

        /// <summary>
        /// Get method to get the greeting message
        /// </summary>
        /// <returns>"Hello, World!"</returns>
        [HttpGet]
        public IActionResult Get()
        {
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Hello to Greeting App API Endpoint",
                Data = "Hello, World!"
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Post method to store a key-value pair
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>Stored key and value</returns>
        [HttpPost]
        public IActionResult Post(RequestModel requestModel)
        {
            dict[requestModel.key] = requestModel.value;

            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Data stored successfully",
                Data = $"Key: {requestModel.key}, Value: {requestModel.value}"
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Put method to update a key-value pair
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>Updated key and value</returns>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            if (!dict.ContainsKey(requestModel.key))
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Key not found",
                    Data = null
                });
            }

            dict[requestModel.key] = requestModel.value;

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Data updated successfully",
                Data = $"Key: {requestModel.key}, Value: {requestModel.value}"
            });
        }

        /// <summary>
        /// Patch method to modify a key's value
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>Modified key and value</returns>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            if (!dict.ContainsKey(requestModel.key))
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Key not found",
                    Data = null
                });
            }

            dict[requestModel.key] += requestModel.value;

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Data modified successfully",
                Data = $"Key: {requestModel.key}, New Value: {dict[requestModel.key]}"
            });
        }

        /// <summary>
        /// Delete method to remove a key-value pair
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Deletion status</returns>
        [HttpDelete]
        public IActionResult Delete(DeleteRequestModel deleteRequestModel)
        {
            if (!dict.ContainsKey(deleteRequestModel.key))
            {
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Key not found",
                    Data = null
                });
            }

            dict.Remove(deleteRequestModel.key);

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Data deleted successfully",
                Data = $"Deleted Key: {deleteRequestModel.key}"
            });
        }
    }
}

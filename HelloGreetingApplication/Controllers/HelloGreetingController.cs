using BusinessLayer.Interface;
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
        private readonly IGreetingBL _greetingBL;
        private ILogger<HelloGreetingController> _logger;

        public HelloGreetingController(IGreetingBL greetingBL, ILogger<HelloGreetingController> logger)
        {
            _greetingBL = greetingBL;
            _logger = logger;
        }

        /// <summary>
        /// Get method to get the greeting message
        /// </summary>
        /// <returns>"Hello, World!"</returns>
        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("GET request received.");
            var result = _greetingBL.GetHelloBL();
            _logger.LogInformation("GET response: {Result}", result);
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Hello to Greeting App API Endpoint",
                Data = result
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// Post method to store a key-value pair
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns>Stored key and value</returns>
        [HttpPost]
        public IActionResult Post(PostRequestModel postRequestModel)
        {

            _logger.LogInformation("POST request received with FirstName: {FirstName}, LastName: {LastName}", postRequestModel.FirstName, postRequestModel.LastName);
            var result = _greetingBL.PostHelloBL(postRequestModel);
            _logger.LogInformation("POST response: {Result}", result);
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Data stored successfully",
                Data = result
            };
            return Ok(responseModel);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Greeting")]
        public IActionResult GreetingPost(GreetingRequestModel greetingRequestModel) 
        {
            _logger.LogInformation("POST request received with Greeting Message");
            var result = _greetingBL.UserGreetingBL(greetingRequestModel);
            _logger.LogInformation("POST response: {Result}", result);
            ResponseModel<string> responseModel = new ResponseModel<string>
            {
                Success = true,
                Message = "Data stored successfully",
                Data = result
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
            _logger.LogInformation("PUT request received to update Key: {Key}", requestModel.key);
            if (!dict.ContainsKey(requestModel.key))
            {
                _logger.LogWarning("PUT failed: Key {Key} not found", requestModel.key);
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Key not found",
                    Data = null
                });
            }

            dict[requestModel.key] = requestModel.value;
            _logger.LogInformation("PUT successful: Updated Key {Key} with Value {Value}", requestModel.key, requestModel.value);

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
            _logger.LogInformation("PATCH request received for Key: {Key}", requestModel.key);
            if (!dict.ContainsKey(requestModel.key))
            {
                _logger.LogWarning("PATCH failed: Key {Key} not found", requestModel.key);
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Key not found",
                    Data = null
                });
            }

            dict[requestModel.key] += requestModel.value;
            _logger.LogInformation("PATCH successful: Modified Key {Key}, New Value: {Value}", requestModel.key, dict[requestModel.key]);

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
            _logger.LogInformation("DELETE request received for Key: {Key}", deleteRequestModel.key);
            if (!dict.ContainsKey(deleteRequestModel.key))
            {
                _logger.LogWarning("DELETE failed: Key {Key} not found", deleteRequestModel.key);
                return NotFound(new ResponseModel<string>
                {
                    Success = false,
                    Message = "Key not found",
                    Data = null
                });
            }

            dict.Remove(deleteRequestModel.key);
            _logger.LogInformation("DELETE successful: Removed Key {Key}", deleteRequestModel.key);

            return Ok(new ResponseModel<string>
            {
                Success = true,
                Message = "Data deleted successfully",
                Data = $"Deleted Key: {deleteRequestModel.key}"
            });
        }
    }
}

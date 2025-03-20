using System.Linq;
using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModelLayer.Model;
using RepositoryLayer.Entity;

namespace HelloGreetingApplication.Controllers
{
    /// <summary>
    /// Class providing API for HelloGreeting
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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

            try
            {
                var result = _greetingBL.GetHelloBL();
                _logger.LogInformation("GET response: {Result}", result);

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Hello to Greeting App API Endpoint",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GET request.");

                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Internal server error. Please try again later.",
                    Data = null
                });
            }
        }


        /// <summary>
        /// Retrieves all stored greeting messages.
        /// </summary>
        /// <returns>List of greeting messages.</returns>
        [HttpGet]
        [Route("GetAllMessages")]
        public IActionResult GetMessages()
        {
            _logger.LogInformation("GET request received for GetAllMessages.");

            try
            {
                var result = _greetingBL.GetAllMessagesBL();

                if (result == null || result.Count == 0)
                {
                    _logger.LogWarning("GET request successful but no messages found.");
                    return NotFound(new ResponseModel<List<MessageEntity>>
                    {
                        Success = false,
                        Message = "No messages found.",
                        Data = null
                    });
                }

                _logger.LogInformation("GET request successful. Returning {Count} messages.", result.Count);

                return Ok(new ResponseModel<List<MessageEntity>>
                {
                    Success = true,
                    Message = "Messages retrieved successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetAllMessages request.");

                return StatusCode(500, new ResponseModel<List<MessageEntity>>
                {
                    Success = false,
                    Message = "Internal server error. Please try again later.",
                    Data = null
                });
            }
        }


        /// <summary>
        /// Post method to store a firstname and lastname
        /// </summary>
        /// <param name="postRequestModel"></param>
        /// <returns>Stored key and value</returns>
        [HttpPost]
        public IActionResult Post(PostRequestModel postRequestModel)
        {
            _logger.LogInformation("POST request received with FirstName: {FirstName}, LastName: {LastName}",
                postRequestModel.FirstName, postRequestModel.LastName);

            try
            {
                if (postRequestModel == null)
                {
                    _logger.LogWarning("POST request failed: Request model is null.");
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid request. Request data cannot be null.",
                        Data = null
                    });
                }

                var result = _greetingBL.PostHelloBL(postRequestModel);

                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogWarning("POST request successful but no data returned.");
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to store data.",
                        Data = null
                    });
                }

                _logger.LogInformation("POST request successful. Stored data: {Result}", result);

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Data stored successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing POST request.");

                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Internal server error. Please try again later.",
                    Data = null
                });
            }
        }


        /// <summary>
        /// Handles the POST request to store a greeting message.
        /// </summary>
        /// <param name="greetingRequestModel">The request model containing greeting details.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPost]
        [Route("Greeting")]
        public IActionResult GreetingPost(GreetingRequestModel greetingRequestModel)
        {
            _logger.LogInformation("POST request received for storing a greeting message.");

            try
            {
                var userIdClaim = User.FindFirst("userId")?.Value;
                if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
                {
                    return Unauthorized(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Unauthorized: User ID not found in token"
                    });
                }

                if (greetingRequestModel == null)
                {
                    _logger.LogWarning("POST request failed: Request model is null.");
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Invalid request. Greeting data cannot be null.",
                        Data = null
                    });
                }

                var result = _greetingBL.UserGreetingBL(greetingRequestModel,userId);

                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogWarning("POST request successful, but no data was stored.");
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Failed to store the greeting message.",
                        Data = null
                    });
                }

                _logger.LogInformation("POST request successful. Stored greeting message: {Result}", result);

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting message stored successfully.",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the POST request.");

                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "Internal server error. Please try again later.",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Finds a greeting message by its ID.
        /// </summary>
        /// <param name="greetingIdFind">The request model containing the greeting ID.</param>
        /// <returns>The greeting message if found.</returns>
        [HttpPost]
        [Route("FindGreeting")]
        public IActionResult GreetingFind(GreetingIdFind greetingIdFind)
        {
            try
            {
                _logger.LogInformation("POST request received with Greeting ID: {GreetingId}", greetingIdFind.Id);

                var result = _greetingBL.GreetingFindBL(greetingIdFind);

                if (result == null)
                {
                    _logger.LogWarning("No data found for Greeting ID: {GreetingId}", greetingIdFind.Id);
                    return BadRequest(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Data not found",
                        Data = null
                    });
                }

                _logger.LogInformation("POST response successful for Greeting ID: {GreetingId}", greetingIdFind.Id);
                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Data found",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request for Greeting ID: {GreetingId}", greetingIdFind.Id);
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal server error occurred",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Updates an existing key-value pair.
        /// </summary>
        /// <param name="requestModel">The request containing the key and new value.</param>
        /// <returns>Updated key-value pair.</returns>
        [HttpPut]
        public IActionResult Put(RequestModel requestModel)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Key: {Key}", requestModel.key);
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal server error occurred",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Updates an existing greeting message.
        /// </summary>
        /// <param name="id">The ID of the greeting to update.</param>
        /// <param name="editGreetingModel">The updated greeting message.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPut]
        [Route("EditGreeting/{id}")]
        public IActionResult Edit(int id, EditGreetingModel editGreetingModel)
        {
            try
            {
                _logger.LogInformation("PUT request received to update Greeting ID: {Id}", id);

                var result = _greetingBL.EditGreetingBL(id, editGreetingModel.Greeting);

                if (result == null)
                {
                    _logger.LogWarning("PUT failed: Greeting ID {Id} not found", id);
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Greeting ID not found",
                        Data = null
                    });
                }

                _logger.LogInformation("PUT successful: Updated Greeting ID {Id} with New Message: {Message}", id, editGreetingModel.Greeting);

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting updated successfully",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating Greeting ID: {Id}", id);
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal server error occurred",
                    Data = null
                });
            }
        }


        /// <summary>
        /// Updates an existing key-value pair in the dictionary.
        /// </summary>
        /// <param name="requestModel">The request model containing the key and value to update.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpPatch]
        public IActionResult Patch(RequestModel requestModel)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while modifying Key: {Key}", requestModel.key);
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal server error occurred",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Deletes a key-value pair from storage.
        /// </summary>
        /// <param name="deleteRequestModel">The request containing the key to delete.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpDelete]
        public IActionResult Delete(DeleteRequestModel deleteRequestModel)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Key: {Key}", deleteRequestModel.key);
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal server error occurred",
                    Data = null
                });
            }
        }

        /// <summary>
        /// Deletes a greeting by ID.
        /// </summary>
        /// <param name="id">The ID of the greeting to delete.</param>
        /// <returns>A response indicating success or failure.</returns>
        [HttpDelete]
        [Route("DeleteGreeting/{id}")]
        public IActionResult DeleteGreeting(int id)
        {
            try
            {
                _logger.LogInformation("DELETE request received for ID: {ID}", id);

                bool isDeleted = _greetingBL.DeleteGreetingBL(id);

                if (!isDeleted)
                {
                    _logger.LogWarning("DELETE failed: Greeting with ID {ID} not found", id);
                    return NotFound(new ResponseModel<string>
                    {
                        Success = false,
                        Message = "Greeting not found",
                        Data = null
                    });
                }

                _logger.LogInformation("DELETE successful: Greeting with ID {ID} deleted", id);

                return Ok(new ResponseModel<string>
                {
                    Success = true,
                    Message = "Greeting deleted successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting Greeting ID: {ID}", id);
                return StatusCode(500, new ResponseModel<string>
                {
                    Success = false,
                    Message = "An internal server error occurred",
                    Data = null
                });
            }
        }


    }
}

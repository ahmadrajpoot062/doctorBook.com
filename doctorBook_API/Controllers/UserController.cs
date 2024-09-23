using Core.Interfaces;
using doctorBook_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace doctorBook_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepository<ApiUsers> _userRepository;

        public UserController(IRepository<ApiUsers> userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet("GetAllUsers")]
        [Authorize]
        public async Task<ActionResult<List<ApiUsers>>> GetAllUsers()
        {
            var users = await _userRepository.GetAll();
            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }
            return Ok(users);
        }

        
        [HttpGet("GetUserById/{id}")]
        [Authorize]
        public async Task<ActionResult<ApiUsers>> GetUserById(int id)
        {
            var user = await _userRepository.GetById(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }

        
        [HttpPost("RegisterUser")]
        public async Task<ActionResult> RegisterUser([FromBody] ApiUsers user)
        {
            if (user == null)
            {
                return BadRequest("Invalid user data.");
            }

            await _userRepository.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
        }

        
        [HttpPut("UpdateUser/{id}")]
        [Authorize]
        public async Task<ActionResult> UpdateUser(int id, [FromBody] ApiUsers updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest("User ID mismatch.");
            }

            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            await _userRepository.Update(updatedUser);
            return NoContent();
        }

        
        [HttpDelete("DeleteUser/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var existingUser = await _userRepository.GetById(id);
            if (existingUser == null)
            {
                return NotFound($"User with ID {id} not found.");
            }

            await _userRepository.Delete(id);
            return NoContent();
        }

    }

}

using Microsoft.AspNetCore.Mvc;
using UserService.Models;
using UserService.Repositories;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace UserService.Controllers
{
    [ApiController]
    [Route("v1/api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        [HttpGet]
        [SwaggerOperation(Summary = "Get all users")]
        public async Task<ActionResult<List<User>>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        [SwaggerOperation(Summary = "Get user by ID")]
        public async Task<ActionResult<User>> GetUserById(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[SwaggerOperation(Summary = "Create a new user")]
        //public async Task<ActionResult<User>> CreateAsync([FromBody] User user)
        //{
        //    user.Id = new Guid();
        //    var createdUser = await _userRepository.CreateAsync(user);
        //    return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        //}

        [HttpPost]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create a new user")]
        public async Task<ActionResult<User>> CreateAsync([FromBody] User user)
        {
            if (user.Id == Guid.Empty)
                user.Id = Guid.NewGuid();

            var createdUser = await _userRepository.CreateAsync(user);
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
        }


        [HttpDelete("{id:guid}")]
        [SwaggerOperation(Summary = "Soft delete a user")]
        public async Task<ActionResult> SoftDelete(Guid id)
        {
            var success = await _userRepository.SoftDeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok();
        }

        [HttpDelete("permanentdelete/{id:guid}")]
        [SwaggerOperation(Summary = "Permanently delete a user")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var success = await _userRepository.DeleteAsync(id);
            if (!success)
                return NotFound();

            return Ok();
        }
    }
}

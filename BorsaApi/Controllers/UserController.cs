using BorsaApi.Concrete;
using BorsaApi.DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Collections.Generic;

namespace BorsaApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet("GetAll")]
		public ActionResult<List<User>> Get()
        {
            return _userService.Get();
		}

		[HttpGet("GetById")]
		public ActionResult<User> Get(string id)
		{
			var user = _userService.Get(id);
			if (user == null)
			{
				return NotFound($"User with Id = {id} not found");
			}

			return user;
		}

		[HttpPost("Insert")]
		public ActionResult<User> Post([FromBody] User user)
		{
			user.Id = ObjectId.GenerateNewId().ToString();
			_userService.Create(user);

			return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
		}

		

		[HttpDelete("Delete")]
		public ActionResult Delete(string id)
		{
			var essate = _userService.Get(id);
		
			_userService.Remove(essate.Id);
			return Ok();
		}

		
	}
}

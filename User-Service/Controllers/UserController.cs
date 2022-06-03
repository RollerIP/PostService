using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using User_Service.Contexts;
using User_Service.Messaging;
using User_Service.Models;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly DataContext _context;

        public UserController(IMessageService messageService, DataContext context)
        {
            _messageService = messageService;
            _context = context;
        }

        [HttpGet("getAll")]
        public IActionResult getAll()
        {
            IEnumerable<User> users = _context.Users;
            return Ok(users);
        }

        [HttpGet("get/{id}")]
        public  IActionResult Get(long id)
        {
            User user = _context.Users.FirstOrDefault(x=> x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("create")]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();

                BroadcastUpdate(new List<User> { user });

                return Ok(user);
            }

            return Problem("Invalid user");
        }

        private void BroadcastUpdate(List<User> updatedUsers)
        {
            _messageService.Publish("UserUpdate", updatedUsers);
        }
    }
}
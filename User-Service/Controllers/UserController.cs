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

        [HttpGet("get/{id}")]
        public  User Get(int id)
        {
            return _context.Users.Find(id);
        }

        [HttpPost("create")]
        public IActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                _context.SaveChanges();

                BroadcastUpdate(new List<User> { user });

                return Ok(user);
            }

            return Problem("Invalid user");
        }

        private void BroadcastUpdate(List<User> updatedUsers)
        {
            NatsMessage message = NatsMessage.Create("UserUpdate", JsonConvert.SerializeObject(updatedUsers));
            _messageService.Publish(message.target, message);
        }
    }
}
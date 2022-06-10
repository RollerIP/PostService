using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Microsoft.AspNetCore.Authorization;
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
        private readonly FirebaseAuth auth;

        public UserController(IMessageService messageService, DataContext context)
        {
            _messageService = messageService;
            _context = context;
            auth = FirebaseAuth.DefaultInstance;
        }

        [HttpGet("getAll"), Authorize]
        public IActionResult getAll()
        {
            IEnumerable<User> users = _context.Users;
            return Ok(users);
        }

        [HttpGet("get/{id}"), Authorize]
        public  IActionResult Get(long id)
        {
            User user = _context.Users.FirstOrDefault(x=> x.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Create(InputUser inputUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UserRecord createdUser = await auth.CreateUserAsync(new UserRecordArgs()
                    {
                        Email = inputUser.Email,
                        EmailVerified = false,
                        Password = inputUser.Password,
                        DisplayName = inputUser.DisplayName,
                        Disabled = false,
                    });
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return Problem(ex.Message);
                }

                User newUser = new User(inputUser.Email, inputUser.DisplayName);

                _context.Users.Add(newUser);
                _context.SaveChanges();

                BroadcastUpdate(new List<User> { newUser });

                return Ok(newUser);

            }

            return Problem("Invalid user");
        }

        [HttpPost("login")]
        public IActionResult Login(string email, string pasword)
        {
            
            return Ok();
        }

        private void BroadcastUpdate(List<User> updatedUsers)
        {
            _messageService.Publish("UserUpdate", updatedUsers);
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace User_Service.Models
{
    public class User
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public User() { }

        public User(string uid, string email, string displayName)
        {
            Id = uid;
            Email = email;
            DisplayName = displayName;
        }
    }
}

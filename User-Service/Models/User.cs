using System.ComponentModel.DataAnnotations;

namespace User_Service.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Email { get; set; }
        public string DisplayName { get; set; }

        public User(string email, string displayName)
        {
            Email = email;
            DisplayName = displayName;
        }
    }
}

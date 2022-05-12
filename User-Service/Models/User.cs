using System.ComponentModel.DataAnnotations;

namespace User_Service.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        
        public User() { }

        public User(string username, string avatarUrl)
        {
            Username = username;
            AvatarUrl = avatarUrl;
        }
    }
}

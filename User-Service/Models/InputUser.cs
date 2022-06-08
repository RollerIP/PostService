using System.ComponentModel;

namespace User_Service.Models
{
    public class InputUser
    {
        public long Id { get; set; }
        [DefaultValue("string@email.com")]
        public string Email { get; set; }
        public string DisplayName { get; set; }
        public string Password { get; set; }
        
        public InputUser(string email, string displayName, string password)
        {
            Email = email;
            DisplayName = displayName;
            Password = password;
        }
    }
}

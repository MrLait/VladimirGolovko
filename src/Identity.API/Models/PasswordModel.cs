namespace Identity.API.Models
{
    public class PasswordModel
    {
        public string UserId { get; set; }

        public string OldPassword { get; set; }

        public string NewPassword { get; set; }
    }
}

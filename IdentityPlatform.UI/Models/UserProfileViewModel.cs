namespace IdentityPlatform.UI.Models
{
    public class UserProfileViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string RegistrationNumber { get; set; } 
        public string Subject { get; set; } 
        public string AuthTime { get; set; }
        public string AccessToken { get; set; }
        public string IdToken { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}

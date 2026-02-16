namespace IdentityPlatform.UI.Models
{
    public class LoginViewModel
    {
        public string ClientId { get; set; }
        public string RedirectUri { get; set; }
        public string ClientName { get; set; }
        public string RegNumber { get; set; }
        public string Password { get; set; }
        public string? Error { get; set; }


        public LoginViewModel WithError(string error)
        {
            Error = error;
            return this;
        }
    }
}

namespace IdentityPlatform.UI.Models
{
    public class ConsentViewModel
    {
        public string Code { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string RedirectUri { get; set; }
        public List<string> Scopes { get; set; } 

        public ConsentViewModel()
        {
            Scopes = new List<string> { "Profile information", "Account balances", "Transaction history" };
        }
    }
}

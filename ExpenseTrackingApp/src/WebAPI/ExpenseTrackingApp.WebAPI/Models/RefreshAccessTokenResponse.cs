namespace ExpenseTrackingApp.WebAPI.Models
{
    public class RefreshAccessTokenResponse
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}

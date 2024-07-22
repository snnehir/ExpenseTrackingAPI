namespace ExpenseTrackingApp.WebAPI.Models
{
    public class LoginSuccessfulResponse
    {
        public string AccessToken { get; set; }
        public RefreshToken RefreshToken { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

    }
}

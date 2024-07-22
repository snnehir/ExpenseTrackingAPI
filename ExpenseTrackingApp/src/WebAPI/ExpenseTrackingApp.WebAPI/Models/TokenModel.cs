namespace ExpenseTrackingApp.WebAPI.Models
{
    public class TokenModel
    {
        public class TokenValidationModel
        {
            public JwtSecurityToken Token { get; set; }
            public ClaimsPrincipal Principle { get; set; }
            public bool Success { get; set; }
        }

        public class TokenCreationModel
        {
            public string AccessToken { get; set; }
            public RefreshToken RefreshToken { get; set; }
            public bool Success { get; set; }
        }
    }
}

namespace ExpenseTrackingApp.WebAPI.Models
{
    public class JwtTokenSetting
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecurityKey { get; set; }
        public int ExpireMinute { get; set; }
    }
}

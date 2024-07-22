namespace ExpenseTrackingApp.DataTransferObjects.Responses
{
    public class UserLoginResponse
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}

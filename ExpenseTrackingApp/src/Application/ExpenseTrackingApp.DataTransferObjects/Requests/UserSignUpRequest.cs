using FluentValidation;

namespace ExpenseTrackingApp.DataTransferObjects.Requests
{
    public class UserSignUpRequest
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

}

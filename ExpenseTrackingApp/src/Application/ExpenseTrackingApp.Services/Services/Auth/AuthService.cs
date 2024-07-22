using ExpenseTrackingApp.Services.Services.Schedule;

namespace ExpenseTrackingApp.Services.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        public AuthService(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest request)
        {
            // email format check 
            var emailCheck = Validation.IsValidEmail(request.Email);
            if (!emailCheck)
            {
                return BaseResponseModel<UserLoginResponse>.Fail(ConstantMessages.InvalidEmailFormat);
            }
            // email exist check
            var user = await _userService.GetUserByEmail(request.Email);
            if (user == null)
            {
                return BaseResponseModel<UserLoginResponse>.Fail(ConstantMessages.UserNotFound);
            }

            if (!SecurityHelper.VerifyHashedPassword(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BaseResponseModel<UserLoginResponse>.Fail(ConstantMessages.WrongPassword);
            }
            var response = new UserLoginResponse() { Email = request.Email, Role = "Client", UserId = user.Id };
            return BaseResponseModel<UserLoginResponse>.Success(response);
        }

        public async Task<BaseResponseModel<UserLoginResponse>> SignUp(UserSignUpRequest request)
        {
            // email format check 
            bool isValidEmail = Validation.IsValidEmail(request.Email);
            if (!isValidEmail)
            {
                return BaseResponseModel<UserLoginResponse>.Fail(ConstantMessages.InvalidEmailFormat);
            }
            // password format check
            bool isValidPassword = Validation.IsValidPassword(request.Password);
            if (!isValidPassword)
            {
                return BaseResponseModel<UserLoginResponse>.Fail(ConstantMessages.InvalidPasswordFormat);
            }
            if(!request.Password.Equals(request.ConfirmPassword))
            {
                return BaseResponseModel<UserLoginResponse>.Fail(ConstantMessages.PasswordConfirmError);
            }
            // email exist check
            var existing = await _userService.GetUserByEmail(request.Email);
            if (existing is not null)
            {
                return BaseResponseModel<UserLoginResponse>.Fail(ConstantMessages.EmailInUseError);
            }

            // save user to db
            var user = request.Adapt<User>();

            var passwordSalt = SecurityHelper.GenerateSalt(70);
            user.PasswordHash = SecurityHelper.HashPassword(request.Password, passwordSalt);
            user.PasswordSalt = passwordSalt;
            user.Role = "Client";
            await _userService.CreateAsync(user);


            ScheduleService.ScheduleSendRegisterEmailWithPassword(user.FirstName, user.LastName,
				user.Email, user.PasswordHash);

  

            return BaseResponseModel<UserLoginResponse>.Success();
        }
    }
}

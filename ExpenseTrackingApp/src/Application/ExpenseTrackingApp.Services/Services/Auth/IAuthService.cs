namespace ExpenseTrackingApp.Services.Services.Auth
{
    public interface IAuthService
    {
        Task<BaseResponseModel<UserLoginResponse>> SignUp(UserSignUpRequest userRegisterRequest);
        Task<BaseResponseModel<UserLoginResponse>> Login(UserLoginRequest userLoginRequest);
    }
}

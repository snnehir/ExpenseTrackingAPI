using Microsoft.AspNetCore.Authorization;

namespace ExpenseTrackingApp.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IDistributedCache _cache;
        public AccountController(IAuthService authService, IConfiguration configuration,IUserService userService, IDistributedCache cache)
        {
            _authService = authService;
            _configuration = configuration;
            _userService = userService;
            _cache = cache;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserSignUpRequest request)
        {
            var result = await _authService.SignUp(request);
            if(result.Succeeded)
            {
                //return Created("ok", result.Data);
                return new ObjectResult(result) { StatusCode = StatusCodes.Status201Created };
            }
            return BadRequest(result);   
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var result = await _authService.Login(request);
            if (result.Succeeded)
            {
                var user = result.Data;
                // Refresh token mechanism with redis
                JwtTokenSetting jwtTokenSettings = _configuration.GetSection("Token").Get<JwtTokenSetting>();
                var tokenResponse = TokenMethods.CreateToken(jwtTokenSettings, user);
                var refreshTokenExpireAt = DateTime.Now.AddMinutes(_configuration.GetValue<int>("RefreshTokenExpireMinute"));
                tokenResponse.RefreshToken.ExpireAt = refreshTokenExpireAt;
                await SaveTokensToCache(tokenResponse.AccessToken, tokenResponse.RefreshToken);

                var response = new LoginSuccessfulResponse()
                {
                    AccessToken = tokenResponse.AccessToken,
                    RefreshToken = tokenResponse.RefreshToken,
                    Email = user.Email,
                    Role = user.Role
                };
                return Ok(response);
            }
            return BadRequest(result);

        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshAccessTokenRequest request)
        {
            JwtTokenSetting jwtTokenSetting = _configuration.GetSection("Token").Get<JwtTokenSetting>();
            // remove "Bearer" part from header to get access token
            var accessToken = HttpContext?.Request.Headers["Authorization"].ToString().Split(" ").Last();
            if (string.IsNullOrEmpty(accessToken))
            {
                var failedResponse = BaseResponseModel<RefreshAccessTokenRequest>.Fail(ConstantMessages.AccessTokenNotFound);
                return BadRequest(failedResponse);
            }
            var validateAccessToken = TokenMethods.ValidateForRefresh(accessToken, jwtTokenSetting);
            if (!validateAccessToken.Success)
            {
                var failedResponse = BaseResponseModel<RefreshAccessTokenRequest>.Fail(ConstantMessages.InvalidTokens);
                return BadRequest(failedResponse);
            }

            var refreshTokenStr = await _cache.GetStringAsync(accessToken);
            if (string.IsNullOrEmpty(refreshTokenStr))
            {
                var failedResponse = BaseResponseModel<RefreshAccessTokenRequest>.Fail(ConstantMessages.InvalidTokens);
                return BadRequest(failedResponse);
            }
            var refreshToken = JsonSerializer.Deserialize<RefreshToken>(refreshTokenStr);
            if (!refreshToken.TokenString.Equals(request.RefreshToken))
            {
                var failedResponse = BaseResponseModel<RefreshAccessTokenRequest>.Fail(ConstantMessages.InvalidTokens);
                return BadRequest(failedResponse);
            }

            var userEmail = validateAccessToken.Principle.Claims.First(x => x.Type == "Email").Value;
            if (userEmail == null)
            {
                var failedResponse = BaseResponseModel<RefreshAccessTokenRequest>.Fail(ConstantMessages.InvalidTokens);
                return BadRequest(failedResponse);
            }
            var user = await _userService.GetUserByEmail(userEmail);
            if (user == null)
            {
                var failedResponse = BaseResponseModel<RefreshAccessTokenRequest>.Fail();
                return BadRequest(failedResponse);
            }
            var userDto = user.Adapt<UserLoginResponse>();
            // remove old tokens
            await _cache.RemoveAsync(accessToken);

            var tokenResponse = TokenMethods.CreateToken(jwtTokenSetting, userDto);
            var refreshTokenExpireAt = DateTime.Now.AddMinutes(_configuration.GetValue<int>("RefreshTokenExpireMinute"));
            tokenResponse.RefreshToken.ExpireAt = refreshTokenExpireAt;
            await SaveTokensToCache(tokenResponse.AccessToken, tokenResponse.RefreshToken);


            var response = BaseResponseModel<RefreshAccessTokenResponse>.Success(new RefreshAccessTokenResponse
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken
            });
            return Ok(response);
            
        }

        private async Task SaveTokensToCache(string accessToken, RefreshToken refreshToken)
        {
            // conversion operations for storing in redis cache
            string refreshTokenString = JsonSerializer.Serialize(refreshToken);
            var refreshTokenByteArray = Encoding.UTF8.GetBytes(refreshTokenString);
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions()
                .SetAbsoluteExpiration(refreshToken.ExpireAt)
                .SetSlidingExpiration(TimeSpan.FromMinutes(30));
            // save access token & refresh token as key value pair 
            await _cache.SetAsync(accessToken, refreshTokenByteArray, options);
        }

		[HttpGet("expenses")]
        [Authorize]
		public async Task<IActionResult> Daily([FromQuery] int userId)
		{
			var result = await _userService.GetUserExpenses(userId);
			
			return Ok(result);

		}

	}
}

namespace ExpenseTrackingApp.WebAPI.Methods
{
    public static class TokenMethods
    {
        public static TokenCreationModel CreateToken(JwtTokenSetting jwtTokenSettings, UserLoginResponse user)
        {
            DateTime expirationTime = DateTime.Now.AddMinutes(double.Parse(jwtTokenSettings.ExpireMinute.ToString()));
            var response = new TokenCreationModel();
            var key = Encoding.ASCII.GetBytes(jwtTokenSettings.SecurityKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = jwtTokenSettings.Audience,
                Issuer = jwtTokenSettings.Issuer,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = expirationTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var newToken = tokenHandler.CreateToken(tokenDescriptor);
            response.AccessToken = tokenHandler.WriteToken(newToken);
            response.Success = true;
            var refreshToken = new RefreshToken
            {
                Email = user.Email,
                TokenString = GenerateRefreshTokenString()
            };
            response.RefreshToken = refreshToken;
            return response;
        }

        private static string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];
            using var randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public static TokenValidationModel ValidateForRefresh(string authToken, JwtTokenSetting jwtSettings)
        {
            var response = new TokenValidationModel();
            var handler = new JwtSecurityTokenHandler();
            if (handler.ReadToken(authToken) is JwtSecurityToken jwtToken)
            {
                var principal = handler.ValidateToken(authToken, new TokenValidationParameters
                {
                    ValidAudience = jwtSettings.Audience,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecurityKey)),
                    ValidateLifetime = false, // access token may expire
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);
                response.Success = true;
                response.Principle = principal;
                response.Token = jwtToken;
            }
            return response;
        }
    
    }
}

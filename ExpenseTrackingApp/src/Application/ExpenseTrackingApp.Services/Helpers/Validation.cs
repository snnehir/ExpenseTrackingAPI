﻿namespace ExpenseTrackingApp.Services.Helpers
{
    public static class Validation
    {
        // Source: https://learn.microsoft.com/en-us/dotnet/standard/base-types/how-to-verify-that-strings-are-in-valid-email-format
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    if (!domainName.Equals("gmail.com"))
                    {
                        throw new ArgumentException("Invalid domain.");
                    }
                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException e)
            {
                return false;
            }
            catch (ArgumentException e)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public static bool IsValidPassword(string password)
        {
            int upperLetterCount = password.Count(char.IsUpper);
            int lowerLetterCount = password.Count(char.IsLower);
            int numberCount = password.Count(char.IsDigit);
            int symbolCount = password.Length - upperLetterCount - lowerLetterCount - numberCount;
            return password.Count() >= 8 && upperLetterCount > 0 && lowerLetterCount > 0 && numberCount > 0 && symbolCount > 0;
        }

    }
}

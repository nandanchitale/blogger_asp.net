using Blogger.EFCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
namespace Helpers.Services
{
    public class ValidationService
    {
        private readonly ILogger<ValidationService> _logger;

        public ValidationService(ILogger<ValidationService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Password is validated depending on the password verification type
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="passwordFromDb"></param>
        /// <param name="PasswordVerificationType"></param>
        /// <returns></returns>
        public bool ValidatePassword(string password, string passwordFromDb)
        {
            bool returnValue = false;
            try
            {
                if (!string.IsNullOrEmpty(password))
                {
                    string hash_pwd = ConvertToHashCode(password);
                    if (!string.IsNullOrEmpty(hash_pwd) && hash_pwd.Equals(passwordFromDb))
                        returnValue = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Validation Service > {ex.Message}");
            }
            return returnValue;
        }

        /// <summary>
        /// To convert the password to hashcode
        /// </summary>
        /// <param name="p"></param>
        /// <param name="salt"></param>
        /// <param name="hasher"></param>
        /// <returns></returns>
        private string ConvertToHashCode(string password)
        {
            string returnValue = string.Empty;
            try
            {
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    returnValue = builder.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Validation Service > {ex.Message}");
            }
            return returnValue;
        }
    }
}

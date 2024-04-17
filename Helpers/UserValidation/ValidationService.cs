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
        private readonly ApplicationDbContext _context;

        public ValidationService(ILogger<ValidationService> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            _context = dbContext;
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
                _logger.LogError($"Validation Service > ValidatePassword > {ex.Message}");
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
        public string ConvertToHashCode(string password)
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
                _logger.LogError($"Validation Service > ConvertToHashCode > {ex.Message}");
            }
            return returnValue;
        }

        public bool isEmailExists(string email)
        {
            bool returnValue = false;
            try
            {
                returnValue = _context.Users.Where(rec => rec.Email.Trim().ToLower().Equals(email.Trim().ToLower())).Any();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Validation Service > isEmailExists > {ex.Message}");
            }
            return returnValue;
        }
    }
}

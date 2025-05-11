using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services
{
    public class LoginService
    {
        private readonly ApplicaitonDbContext _dbContext;

        public LoginService(ApplicaitonDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> LoginAsync(LoginModel loginModel)
        {
            var user = await _dbContext.Users.SingleOrDefaultAsync(x => x.Email.ToLower() == loginModel.Email.ToLower()).ConfigureAwait(false);
            if (user == null)
                throw new ArgumentException("User not found");


            var hasher = new PasswordHasher<User>();

            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginModel.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new ArgumentException("Invalid password");

            return user;
        }
    }
}

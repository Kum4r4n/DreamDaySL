using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DreamDay.Services;

public class RegisterService
{
    private readonly ApplicaitonDbContext _dbContext;

    public RegisterService(ApplicaitonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User> RegisterAsync(RegisterModel register)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == register.Email).ConfigureAwait(false);
        if(user != null)
            throw new ArgumentException("User already exists");

        var hasher = new PasswordHasher<User>();
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = register.Name,
            Email = register.Email,
            Role = register.Role
        };

        newUser.PasswordHash = hasher.HashPassword(newUser, register.Password);

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return newUser;
    }
}

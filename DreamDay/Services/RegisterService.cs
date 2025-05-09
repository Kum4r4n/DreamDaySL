using DreamDay.Data;
using DreamDay.Entites;
using DreamDay.Models;
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

        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            Name = register.Name,
            Email = register.Email,
            PasswordHash = register.Password,
            Role = register.Role
        };

        _dbContext.Users.Add(newUser);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return newUser;
    }
}

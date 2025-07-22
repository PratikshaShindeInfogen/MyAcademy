using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories
{
    public class UserRepositiory : IUserRepository
    {
        private readonly UserDbContext _dbContext;

        public UserRepositiory(UserDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllAsync()
        {
            var users =  await _dbContext.Users.Where(s=> s.IsDeleted == false).ToListAsync();
            return users;
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            var userdetails = await _dbContext.Users.FirstOrDefaultAsync(s=>s.Id == id);
            if (userdetails == null)
            {
                return null;
            }
            return userdetails;

        }

        public async Task<User?> GetByNameAsync(string name)
        {
            var userdetails = await _dbContext.Users.Where(s=>s.FullName == name).FirstOrDefaultAsync();
            if(userdetails == null) return null;
            return userdetails;
        }

        public async Task<User> CreateAsync(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var userdetails = await _dbContext.Users.FindAsync(id);
            if (userdetails == null || userdetails.IsDeleted)
            {
                return false;
            }
            else
            {
                userdetails.IsDeleted = true;
                await _dbContext.SaveChangesAsync(); 
                return true;
            }
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var userdetails = await _dbContext.Users.FindAsync(id);
            _dbContext.Remove(userdetails);
            await _dbContext.SaveChangesAsync();
            return true;
        }

    }
}

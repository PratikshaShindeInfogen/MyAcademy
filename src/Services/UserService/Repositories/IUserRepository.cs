using UserService.Models;

namespace UserService.Repositories
{
    public interface IUserRepository
    {
        public  Task<List<User>> GetAllAsync();
        public Task<User?> GetByIdAsync(Guid id);
        public Task<User?> GetByNameAsync(string name);
        public Task<User> CreateAsync(User user);
        public Task<User> UpdateAsync(User user);
        public Task<bool> SoftDeleteAsync(Guid id);

        public Task<bool> DeleteAsync(Guid id);
    }
}

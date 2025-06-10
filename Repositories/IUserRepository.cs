// Repositories/IUserRepository.cs
using MBStream.Models;

namespace MBStream.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);
        Task<User> GetByEmailAsync(string email); // Add this line
    }
}

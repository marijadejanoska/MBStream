using MBStream.Data;
using MBStream.Models;
using Microsoft.EntityFrameworkCore;

namespace MBStream.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context) { _context = context; }

        public async Task<User> GetByIdAsync(int id) => await _context.Users.FindAsync(id);
        public async Task<List<User>> GetAllAsync() => await _context.Users.ToListAsync();
        public async Task<User> AddAsync(User user) { _context.Users.Add(user); await _context.SaveChangesAsync(); return user; }
        public async Task<User> UpdateAsync(User user) { _context.Users.Update(user); await _context.SaveChangesAsync(); return user; }
        public async Task<bool> DeleteAsync(int id) { var user = await _context.Users.FindAsync(id); if (user == null) return false; _context.Users.Remove(user); await _context.SaveChangesAsync(); return true; }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserEmail == email);
        }
    }
}

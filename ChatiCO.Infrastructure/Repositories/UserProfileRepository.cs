using ChatiCO.Application.Interfaces;
using ChatiCO.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatiCO.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRespository
    {
        private readonly ChatiCODbContext _context;
        public UserProfileRepository(ChatiCODbContext context)
        {
            _context = context;
        }
        public async Task<Domain.Entities.User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u=>u.UserId==userId);
        }
        public async Task UpdateUserProfileAsync(Domain.Entities.User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}

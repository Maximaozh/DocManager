using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationDB;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;

namespace Data.Repositories
{
    public class UserRepositoriy : IUserRepositoriy
    {
        private readonly AppContextDB _dbContext;

        public UserRepositoriy(AppContextDB dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> Get()
        {
            return await _dbContext.Users
                .AsNoTracking()
                .OrderBy(u => u.Id)
                .ToListAsync();
        }

        public async Task<User?> GetByLogin(string Login)
        {
            return await _dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == Login);
        }

        public async Task Add(UserRegistrate userRegistrate)
        {

            User user = new User
            {
                Login = userRegistrate.Login,
                Password = userRegistrate.Password,
                Role = userRegistrate.Role,
                Name = userRegistrate.Name,
                Surname = userRegistrate.Surname
            };

            await _dbContext.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}

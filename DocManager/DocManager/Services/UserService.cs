using ApplicationDB;
using Data.Models;
using DocManager.Data.Cryptographic;
using DocManager.Data.Jwt;
using Microsoft.EntityFrameworkCore;
using Shared.Dto;
using Shared.Dto.User;

namespace DocManager.Services
{
    public class UserService(
        AppContextDB dbContext,
        PasswordHasher passwordHasher,
        JwtProvider jwtProvider
            )
    {

        public async Task<LoginResponse> AuthenticateUser(UserLogin userLogin)
        {
            UserInfo? user = await dbContext.Users
                .AsNoTracking()
                .Select(u => new UserInfo()
                {
                    Id = u.Id.ToString(),
                    Role = u.Role,
                    Login = u.Login,
                    Password = u.Password,
                    Name = u.Name,
                    Surname = u.Surname
                })
                .FirstOrDefaultAsync(u => u.Login == userLogin.Login);

            if (user == null)
            {
                return null;
            }

            if (userLogin.Login != user.Login)
            {
                return null;
            }

            if (!passwordHasher.Verify(userLogin.Password, user.Password))
            {
                return null;
            }

            LoginResponse response = new LoginResponse() { Token = jwtProvider.GenerateJWT(user), User = user };

            return response;
        }

        public async Task<List<UserInfo>> GetByOffset(PaginateFilter pFilter)
        {
            int skipCount = (pFilter.Page - 1) * pFilter.Count;
            return await dbContext.Users
                .Skip(skipCount)
                .Take(pFilter.Count)
                .Select(u => new UserInfo()
                {
                    Id = u.Id.ToString(),
                    Login = u.Login,
                    Password = u.Password,
                    Role = u.Role,
                    Name = u.Name,
                    Surname = u.Surname,

                })
                .ToListAsync();
        }

        public async Task<int> GetCount()
        {
            return dbContext.Users.Count();
        }

        public async Task<int> Registrate(UserRegistrate user)
        {
            if (user is null)
            {
                return -1;
            }

            User? userFromDB = await dbContext.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login == user.Login);

            if (userFromDB is not null)
            {
                return -2;
            }

            User registerUser = new User
            {
                Login = user.Login,
                Password = passwordHasher.GenerateHashBCrypt(user.Password),
                Role = user.Role,
                Name = user.Name,
                Surname = user.Surname
            };

            await dbContext.AddAsync(registerUser);
            await dbContext.SaveChangesAsync();

            return 0;
        }
    }
}

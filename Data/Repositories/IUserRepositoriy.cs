using Data.Models;
using Shared.Dto;

namespace Data.Repositories
{
    public interface IUserRepositoriy
    {
        Task Add(UserRegistrate userRegistrate);
        Task<List<User>> Get();
        Task<User?> GetByLogin(string Login);
    }
}
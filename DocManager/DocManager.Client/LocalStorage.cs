using Blazored.LocalStorage;
using Shared.Dto.User;

namespace DocManager.Client
{
    public class LocalStorage(ILocalStorageService storage)
    {


        public int setData(LoginResponse loginData)
        {
            if (loginData is null)
            {
                return -1;
            }

            storage.SetItemAsync("Id", loginData.User.Id);
            storage.SetItemAsync("Login", loginData.User.Login);
            storage.SetItemAsync("Token", loginData.Token);
            storage.SetItemAsync("Password", loginData.User.Password);
            storage.SetItemAsync("Role", loginData.User.Role);
            storage.SetItemAsync("Name", loginData.User.Name);
            storage.SetItemAsync("Surname", loginData.User.Surname);

            return 0;
        }


        public async Task<string> Id() => await storage.GetItemAsync<string>("Id");
        public async Task<string> Login() => await storage.GetItemAsync<string>("Login");
        public async Task<string> Token() => await storage.GetItemAsync<string>("Token");
        public async Task<string> Password() => await storage.GetItemAsync<string>("Password");
        public async Task<string> Role() => await storage.GetItemAsync<string>("Role");
        public async Task<string> Name() => await storage.GetItemAsync<string>("Name");
        public async Task<string> Surname() => await storage.GetItemAsync<string>("Surname");
    }
}

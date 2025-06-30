using AlexSupport.Data;
using AlexSupport.ViewModels;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.Pkcs;

namespace AlexSupport.Repository.IRepository
{
    public interface IAppUserRepoistory
    {
        public Task<IEnumerable<AppUser>> GetAllAgentsAsync();
        public Task<IEnumerable<AppUser>> GetAllUsersAsync();
        public Task<AppUser> Login(string username, string password);
        public Task<AppUser> SignUp(AppUser user);

        public Task<bool> IsEmailExists(string email, int id = 0);

        public Task<bool> IsFingerprintExists(string fingerprint, int id = 0);

        public Task<bool> IsPhoneNumberExists(string phone, int id = 0);
        public int GenerateVerifyCode();
        public Task<AppUser> GetUserByEmailAsync(string Email);
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<AppUser> UpdateUserAsync(AppUser user);
        public Task<bool> InActiveUserAsync(int id);
        public Task<bool> ActiveUserAsync(int id);
        public Task<IEnumerable<AppUser>> GetAllAdminsAsync();
        public Task<bool> IsLoginNameExists(string loginname , int id = 0);

    }
}

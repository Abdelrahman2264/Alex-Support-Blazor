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

        public Task<bool> IsEmailExists(string email);

        public Task<bool> IsFingerprintExists(string fingerprint);

        public Task<bool> IsPhoneNumberExists(string phone);
        public int GenerateVerifyCode();
        public Task<AppUser> GetUserByEmailAsync(string Email);
        public Task<AppUser> GetUserByIdAsync(int id);
        public Task<AppUser> UpdateUserAsync(AppUser user);
        public Task<bool> InActiveUserAsync(int id);
        public Task<bool> ActiveUserAsync(int id);
        public Task<IEnumerable<AppUser>> GetAllAdminsAsync();

    }
}

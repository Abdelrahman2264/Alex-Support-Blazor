using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Text;
using AlexSupport.Services.Models;
using AlexSupport.Services.Extensions;

namespace AlexSupport.Repository
{
    public class AppUserRepoistory : IAppUserRepoistory
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<AppUserRepoistory> logger;
        public AppUserRepoistory(AlexSupportDB alexSupportDB, ILogger<AppUserRepoistory> logger)
        {
            this.alexSupportDB = alexSupportDB;
            this.logger = logger;
        }
        public async Task<IEnumerable<AppUser>> GetAllAgentsAsync()
        {
            try
            {
                return await alexSupportDB.AppUser
                    .Where(u => u.Role == "Agent")
                    .Include(u => u.Department)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all agents: " + ex.Message, ex);
                return new List<AppUser>();
            }
        }
        public async Task<IEnumerable<AppUser>> GetAllAdminsAsync()
        {
            try
            {
                return await alexSupportDB.AppUser
                    .Where(u => u.Role == "Admin")
                    .Include(u => u.Department)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all Admins: " + ex.Message, ex);
                return new List<AppUser>();
            }
        }

        public async Task<AppUser> Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return new AppUser();
                }

                username = username.Trim().ToLower();
                password = password.Trim(); // optional: trim password too

                var user = await alexSupportDB.AppUser
                    .FirstOrDefaultAsync(u =>
                        (u.LoginName.ToLower() == username || u.Email.ToLower() == username)
                        && u.IsActive == true && u.EmailVerified == "Yes");

                if (user != null)
                {

                    return user;
                }
                else
                {
                    logger.LogError("User not found or inactive");
                    return new AppUser();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Login: " + ex.Message, ex);
                return new AppUser();
            }
        }

        public async Task<AppUser> SignUp(AppUser user)
        {
            try
            {
                if (user != null)
                {
                    int atIndex = user.Email.IndexOf('@');
                    user.LoginName = user.Email.Substring(0, atIndex);
                    user.EmailVerified = "Yes";
                    user.Create_Date = DateTime.Now;
                    user.IsActive = true;
                    user.Role = "User";
                    await alexSupportDB.AppUser.AddAsync(user);
                    await alexSupportDB.SaveChangesAsync();
                    return user;

                }
                else
                {
                    logger.LogError("Can't regisiter this user ");
                    return new AppUser();
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error In SignUp " + ex.Message, ex);
                return new AppUser();
            }
        }
        public async Task<bool> IsEmailExists(string email, int id = 0)
        {
            try
            {
                return await alexSupportDB.AppUser.AnyAsync(u => u.Email.Trim().ToLower() == email.Trim().ToLower() && u.UID != id);
            }
            catch (Exception ex)
            {
                logger.LogError("Error checking email existence: " + ex.Message, ex);
                return false;
            }
        }
        public async Task<bool> IsLoginNameExists(string loginname, int id = 0)
        {
            try
            {
                return await alexSupportDB.AppUser.AnyAsync(u => u.LoginName.Trim().ToLower() == loginname.Trim().ToLower() && u.UID != id);
            }
            catch (Exception ex)
            {
                logger.LogError("Error checking login name existence: " + ex.Message, ex);
                return false;
            }
        }
        public async Task<bool> IsFingerprintExists(string fingerprint, int id = 0)
        {
            try
            {
                return await alexSupportDB.AppUser.AnyAsync(u => u.Fingerprint.ToString().Trim().ToLower() == fingerprint.Trim().ToLower() && u.UID != id);
            }
            catch (Exception ex)
            {
                logger.LogError("Error checking fingerprint existence: " + ex.Message, ex);
                return false;

            }
        }
        public async Task<bool> IsPhoneNumberExists(string phone , int id = 0)
        {

            try
            {
                return await alexSupportDB.AppUser.AnyAsync(u => u.MobilePhone.Trim().ToLower() == phone.Trim().ToLower() && u.UID != id);
            }

            catch (Exception ex)
            {
                logger.LogError("Error checking phone number existence: " + ex.Message, ex);
                return false;
            }
        }

        public int GenerateVerifyCode()
        {
            var random = new Random();
            var Verifyocde = random.Next(100000, 1000000);
            return Verifyocde;


        }
        public async Task<AppUser> GetUserByEmailAsync(string Email)
        {
            try
            {
                var user = await alexSupportDB.AppUser.FirstOrDefaultAsync(u => u.Email.ToLower() == Email.ToLower() && u.IsActive);
                if (user != null)
                {
                    return user;
                }
                return new AppUser();
            }
            catch (Exception ex)
            {
                logger.LogError("Encounted Exception in Get User By Email Async " + ex.Message, ex);
                return new AppUser();
            }
        }
        public async Task<AppUser> UpdateUserAsync(AppUser user)
        {
            try
            {
                if (user != null)
                {
                    var existingUser = await alexSupportDB.AppUser.FirstOrDefaultAsync(u => u.UID == user.UID);
                    if (existingUser != null)
                    {
                        existingUser.Fname = user.Fname;
                        existingUser.Lname = user.Lname;
                        existingUser.Email = user.Email;
                        existingUser.MobilePhone = user.MobilePhone;
                        existingUser.JobTitle = user.JobTitle;
                        existingUser.DID = user.DID;
                        existingUser.Password = user.Password;
                        existingUser.Phone = user.Phone;
                        existingUser.Fingerprint = user.Fingerprint;
                        int atIndex = user.Email.IndexOf('@');
                        user.LoginName = user.Email.Substring(0, atIndex);
                        existingUser.LoginName = user.LoginName;
                        existingUser.IsActive = true;
                        existingUser.ImageContentType = user.ImageContentType;
                        existingUser.ImageData = user.ImageData;
                        existingUser.EmailVerified = "Yes";
                        existingUser.Role = user.Role;
                        alexSupportDB.AppUser.Update(existingUser);
                        await alexSupportDB.SaveChangesAsync();
                        return existingUser;
                    }
                    else
                    {
                        logger.LogError("User not found for update");
                        return new AppUser();
                    }
                }
                else
                {
                    logger.LogError("Invalid user data for update");
                    return new AppUser();
                }

            }
            catch (Exception ex)
            {
                logger.LogError("Error In Update User Async " + ex.InnerException, ex);
                return new AppUser();
            }
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            try
            {
                var user = await alexSupportDB.AppUser.Include(u => u.Department).FirstOrDefaultAsync(u => u.UID == id && u.IsActive);
                if (user != null)
                {
                    return user;
                }
                return new AppUser();
            }
            catch (Exception ex)
            {
                logger.LogError("Encounted Exception in Get User By Id Async " + ex.Message, ex);
                return new AppUser();
            }
        }

        public async Task<IEnumerable<AppUser>> GetAllUsersAsync()
        {
            try
            {
                return await alexSupportDB.AppUser
                    .Include(u => u.Department)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in getting all users: " + ex.Message, ex);
                return new List<AppUser>();
            }
        }

        public async Task<bool> InActiveUserAsync(int id)
        {
            try
            {
                var user = await alexSupportDB.AppUser.FirstOrDefaultAsync(u => u.UID == id);
                if (user != null)
                {
                    user.IsActive = false;
                    user.EmailVerified = "No";
                    alexSupportDB.Update(user);
                    await alexSupportDB.SaveChangesAsync();
                    return true;
                }
                else
                {
                    logger.LogError("User not found for update");
                    return false;
                }



            }
            catch (Exception ex)
            {
                logger.LogError("Error In In Active User Async " + ex.InnerException, ex);
                return false;
            }
        }
        public async Task<bool> ActiveUserAsync(int id)
        {
            try
            {
                var user = await alexSupportDB.AppUser.FirstOrDefaultAsync(u => u.UID == id);
                if (user != null)
                {
                    user.IsActive = true;
                    user.EmailVerified = "Yes";
                    alexSupportDB.Update(user);
                    await alexSupportDB.SaveChangesAsync();

                    return true;
                }
                else
                {
                    logger.LogError("User not found for update");
                    return false;
                }



            }
            catch (Exception ex)
            {
                logger.LogError("Error In  Active User Async " + ex.InnerException, ex);
                return false;
            }
        }
    }
}



using AlexSupport.Repository.IRepository;
using AlexSupport.Services.Extensions;
using Microsoft.AspNetCore.Identity;
using BCrypt.Net;

namespace AlexSupport.Repository
{
    public class PasswordHasherRepository : IPasswordHasherRepository
    {

        private readonly HashType _hashType;
        private readonly int _workFactor;
        private readonly bool _enhancedEntropy;

        public PasswordHasherRepository(
        int workFactor = 12,
        HashType hashType = HashType.SHA256,
        bool enhancedEntropy = false)
        {
            _workFactor = workFactor;
            _hashType = hashType;
            _enhancedEntropy = enhancedEntropy;
        }

        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.EnhancedHashPassword(
           password,
           _hashType,
           _workFactor);
        }


        public bool VerifyPassword(string password, string hash)
        {
            try
            {

                return BCrypt.Net.BCrypt.EnhancedVerify(password, hash, _hashType);
            }
            catch
            {
                return false;
            }
        }
    }

}

namespace AlexSupport.Repository.IRepository
{
    public interface IPasswordHasherRepository
    {
       public string HashPassword(string password);
       public  bool VerifyPassword(string password, string hashedPassword);
    }
}

namespace AlexSupport.Services.Extensions
{
    public interface ILogService
    {
        Task CreateLogAsync(int TID, string Action);
        public Task<int> ReturnCurrentUserID();
        public Task<string> ReturnCurrentUserRole();
        public Task CreateSystemLogAsync(string Action, string type , int UID = 0);

    }

}

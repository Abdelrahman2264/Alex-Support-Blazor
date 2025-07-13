using AlexSupport.ViewModels;
using Microsoft.Extensions.Logging;

namespace AlexSupport.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        public Task<Department> CreateDepartmentAsync(Department Department);
        public Task<Department> UpdateDepartmentAsync(Department Department);
        public Task<Department> GetDepartmentAsync(int Id);
        public Task<IEnumerable<Department>> AllDepartmentsAsync();
        public Task<IEnumerable<Department>> AllSystemDepartmentsAsync();
        public Task<bool> ActivateDepartmentAsync(int Id);
        public Task<bool> InActivateDepartmentAsync(int Id);
        public Task<bool> CheckIfDepartmentExist(string name, int id = 0);

    }
}

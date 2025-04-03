using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface IDepartmentRepository
    {
        public Task<Department> CreateDepartmentAsync(Department Department);
        public Task<Department> UpdateDepartmentAsync(Department Department);
        public Task<Department> GetDepartmentAsync(int Id);
        public Task<IEnumerable<Department>> AllDepartmentsAsync();
        public Task<bool> DeleteDepartmentAsync(int Id);
    }
}

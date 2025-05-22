using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.Services.Extensions;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlexSupport.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<DepartmentRepository> logger;
        private readonly ILogService logService;
        public DepartmentRepository(AlexSupportDB alexSupportDB, ILogger<DepartmentRepository> logger, ILogService logService)
        {
            this.alexSupportDB = alexSupportDB;
            this.logger = logger;
            this.logService = logService;
        }

        public async Task<Department> CreateDepartmentAsync(Department Department)
        {
            try
            {
                if (Department != null)
                {
                    Department.IsActive = true;
                    Department.CreateDate = DateTime.Now;
                    await alexSupportDB.Department.AddAsync(Department);
                    await alexSupportDB.SaveChangesAsync();
                    await logService.CreateSystemLogAsync($"Create A New Department With Id {Department.DID} In The System", "DEPARTMENT");
                    return Department;
                }
                else
                {
                    logger.LogError("Department is null");
                    return new Department();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Createing Department Async: " + ex.Message, ex);
                return new Department();
            }
        }

        public async Task<bool> DeleteDepartmentAsync(int Id)
        {
            try
            {
                var Department = await alexSupportDB.Department.FirstOrDefaultAsync(c => c.DID == Id);
                if (Department != null)
                {
                    Department.IsActive = false;
                    await alexSupportDB.SaveChangesAsync();
                    await logService.CreateSystemLogAsync($"In Active A Department With Id {Department.DID} In The System", "DEPARTMENT");

                    return true;
                }
                logger.LogError("Department Not Found");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Deleting Department Async: " + ex.Message, ex);
                return false;
            }
        }


        public async Task<IEnumerable<Department>> AllDepartmentsAsync()
        {
            try
            {
                return await alexSupportDB.Department.OrderBy(c => c.DepartmentName).Where(u => u.IsActive).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Get ALl Departments Async: " + ex.Message, ex);
                return Enumerable.Empty<Department>();
            }
        }

        public async Task<Department> GetDepartmentAsync(int Id)
        {
            try
            {
                var Department = await alexSupportDB.Department.FirstOrDefaultAsync(c => c.DID == Id);
                if (Department != null)
                {
                    return Department;
                }
                else
                {
                    logger.LogError("Department not found");
                    return new Department();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Get Department Async: " + ex.Message, ex);
                return new Department();
            }



        }

        public async Task<Department> UpdateDepartmentAsync(Department Department)
        {
            try
            {
                var UpdatedDepartment = await alexSupportDB.Department.FirstOrDefaultAsync(c => c.DID == Department.DID);
                if (UpdatedDepartment != null)
                {
                    UpdatedDepartment.DepartmentName = Department.DepartmentName;
                    UpdatedDepartment.IsActive = true;
                    alexSupportDB.Department.Update(UpdatedDepartment);
                    await alexSupportDB.SaveChangesAsync();
                    await logService.CreateSystemLogAsync($"Update A Department With Id {UpdatedDepartment.DID} In The System", "DEPARTMENT");

                    return UpdatedDepartment;
                }
                else
                {
                    logger.LogError("Department not found");
                    return new Department();

                }
            }
            catch (Exception ex)
            {

                logger.LogError("Error in Update Department Async: " + ex.Message, ex);
                return Department;
            }
        }
        public async Task<bool> CheckIfDepartmentExist(string name, int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    var department = await alexSupportDB.Department.FirstOrDefaultAsync(u => u.DepartmentName.ToLower() == name.ToLower() && u.IsActive == true && u.DID != id);
                    if (department != null)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    var department = await alexSupportDB.Department.FirstOrDefaultAsync(u => u.DepartmentName.ToLower() == name.ToLower() && u.IsActive == true);
                    if (department != null)
                    {
                        return true;
                    }
                    return false;
                }


            }
            catch (Exception ex)
            {
                logger.LogError($"Error In Check If Department Exist: {ex.Message}", ex);
                return false;
            }
        }

    }
}

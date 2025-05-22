using AlexSupport.Data;
using AlexSupport.Repository.IRepository;
using AlexSupport.Services.Extensions;
using AlexSupport.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace AlexSupport.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AlexSupportDB alexSupportDB;
        private readonly ILogger<CategoryRepository> logger;
        private readonly INotificationService Note;
        private readonly ILogService LogService;
        public CategoryRepository(AlexSupportDB alexSupportDB, ILogger<CategoryRepository> logger, INotificationService note, ILogService logService)
        {
            this.alexSupportDB = alexSupportDB;
            this.logger = logger;
            this.Note = note;
            LogService = logService;
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            try
            {
                if (category != null)
                {
                    category.isActive = true;
                    category.CreatedDate = DateTime.Now;
                    await alexSupportDB.Category.AddAsync(category);
                    await alexSupportDB.SaveChangesAsync();
                    return category;
                }
                else
                {
                    logger.LogError("Category is null");
                    return new Category();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Createing Category Async: " + ex.Message, ex);
                return new Category();
            }
        }

        public async Task<bool> DeleteCategoryAsync(int Id)
        {
            try
            {
                var category = await alexSupportDB.Category.FirstOrDefaultAsync(c => c.CID == Id);
                if (category != null)
                {
                    category.isActive = false;
                    await alexSupportDB.SaveChangesAsync();
                    await LogService.CreateSystemLogAsync($"In Active Category  With Id: {category.CID} In the System", "CATEGORY");

                    return true;
                }
                logger.LogError("Category Not Found");
                return false;
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Deleting Category Async: " + ex.Message, ex);
                return false;
            }
        }


        public async Task<IEnumerable<Category>> AllCategoriesAsync()
        {
            try
            {

                return await alexSupportDB.Category.OrderBy(c => c.CategoryName).ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Get ALl Categories Async: " + ex.Message, ex);
                return Enumerable.Empty<Category>();
            }
        }

        public async Task<Category> GetCategoryAsync(int Id)
        {
            try
            {
                var category = await alexSupportDB.Category.FirstOrDefaultAsync(c => c.CID == Id);
                if (category != null)
                {
                    return category;
                }
                else
                {
                    logger.LogError("Category not found");
                    return new Category();
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error in Get Category Async: " + ex.Message, ex);
                return new Category();
            }



        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            try
            {
                var UpdatedCategory = await alexSupportDB.Category.FirstOrDefaultAsync(c => c.CID == category.CID);
                if (UpdatedCategory != null)
                {
                    UpdatedCategory.CategoryName = category.CategoryName;
                    UpdatedCategory.isActive = true;
                    alexSupportDB.Category.Update(UpdatedCategory);
                    await alexSupportDB.SaveChangesAsync();
                    await LogService.CreateSystemLogAsync($"Update Category With Id: {UpdatedCategory.CID} In the System", "CATEGORY");
                    return UpdatedCategory;
                }
                else
                {
                    logger.LogError("Category not found");
                    return new Category();

                }
            }
            catch (Exception ex)
            {

                logger.LogError("Error in Update Category Async: " + ex.Message, ex);
                return category;
            }
        }
        public async Task<bool> CheckIfCategoryExist(string name, int id = 0)
        {
            try
            {

                if (id != 0)
                {
                    var type = await alexSupportDB.Category.FirstOrDefaultAsync(u => u.CategoryName.ToLower() == name.ToLower() && u.isActive == true && u.CID != id);
                    if (type != null)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    var type = await alexSupportDB.Category.FirstOrDefaultAsync(u => u.CategoryName.ToLower() == name.ToLower() && u.isActive == true);
                    if (type != null)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Error in Check If Type Exist method: {ex.Message}", ex);
                return false;
            }
        }
    }
}

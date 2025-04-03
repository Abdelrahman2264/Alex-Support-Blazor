using AlexSupport.ViewModels;
using Microsoft.Extensions.Logging;

namespace AlexSupport.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<Category> CreateCategoryAsync(Category category);
        public Task<Category> UpdateCategoryAsync(Category category);
        public Task<Category> GetCategoryAsync(int Id);
        public Task<IEnumerable<Category>> AllCategoriesAsync();
        public Task<bool> DeleteCategoryAsync(int Id);
        public  Task<bool> CheckIfCategoryExist(string name, int id = 0);
        
        

    }
}

using AlexSupport.ViewModels;

namespace AlexSupport.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<Category> CreateCategoryAsync(Category category);
        public Task<Category> UpdateCategoryAsync(Category category);
        public Task<Category> GetCategoryAsync(int Id);
        public Task<IEnumerable<Category>> AllCategoriesAsync();
        public Task<bool> DeleteCategoryAsync(int Id);

    }
}

using FirstCall.Shared.ViewModels.Menus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IMenuCategoryTranslationService
    {
        Task<List<MenuCategoryTranslationViewModel>> GetTranslationByCategoryId(int categoryId);

        Task<MenuCategoryTranslationViewModel> GetTranslationById(int translationId);

        Task<MenuCategoryTranslationViewModel> AddTranslation(MenuCategoryTranslationInsertModel translationInsertModel);

        Task<MenuCategoryTranslationViewModel> UpdateTranslation(MenuCategoryTranslationUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteTranslation(int translationId);

        Task SaveAsync();
    }
}

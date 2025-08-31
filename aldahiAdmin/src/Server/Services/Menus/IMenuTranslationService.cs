using FirstCall.Shared.ViewModels.Menus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IMenuTranslationService
    {
        Task<List<MenuTranslationViewModel>> GetTranslationByMenuId(int blockId);

        Task<MenuTranslationViewModel> GetTranslationById(int translationId);

        Task<MenuTranslationViewModel> AddTranslation(MenuTranslationInsertModel translationInsertModel);

        Task<MenuTranslationViewModel> UpdateTranslation(MenuTranslationUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteTranslation(int translationId);

        Task SaveAsync();
    }
}

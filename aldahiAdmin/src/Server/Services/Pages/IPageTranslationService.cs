using FirstCall.Shared.ViewModels.Pages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IPageTranslationService
    {
        Task<List<PageTranslationViewModel>> GetTranslationByPageId(int pageId);

        Task<PageTranslationViewModel> GetTranslationById(int translationId);

        Task<PageTranslationViewModel> AddTranslation(PageTranslationInsertModel translationInsertModel);

        Task<PageTranslationViewModel> UpdateTranslation(PageTranslationUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteTranslation(int translationId);

        Task SaveAsync();
    }
}

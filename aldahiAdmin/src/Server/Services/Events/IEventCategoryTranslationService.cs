using FirstCall.Shared.ViewModels.Events;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace FirstCall.Application.Services
{
    public interface IEventCategoryTranslationService
    {
        Task<List<EventCategoryTranslationViewModel>> GetTranslationByCategoryId(int categoryId);

        Task<EventCategoryTranslationViewModel> GetTranslationById(int translationId);

        Task<EventCategoryTranslationViewModel> AddTranslation(EventCategoryTranslationInsertModel translationInsertModel);

        Task<EventCategoryTranslationViewModel> UpdateTranslation(EventCategoryTranslationUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteTranslation(int translationId);

        Task SaveAsync();
    }
}

using FirstCall.Shared.ViewModels.Events;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IEventTranslationService
    {
        Task<List<EventTranslationViewModel>> GetTranslationByEventId(int eventId);

        Task<EventTranslationViewModel> GetTranslationById(int translationId);

        Task<EventTranslationViewModel> AddTranslation(EventTranslationInsertModel translationInsertModel);

        Task<EventTranslationViewModel> UpdateTranslation(EventTranslationUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteTranslation(int translationId);

        Task SaveAsync();
    }
}

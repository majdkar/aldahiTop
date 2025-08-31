using FirstCall.Shared.ViewModels.Settings;
using FirstCall.Shared.ViewModels.Settings.Languages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface ILanguageService
    {
        Task<List<LanguageViewModel>> GetLanguages();

        Task<LanguageViewModel> GetLanguageById(int languageId);

        Task<LanguageViewModel> AddLanguage(LanguageInsertModel languageInsertModel);

        Task<LanguageViewModel> UpdateLanguage(LanguageUpdateModel languageUpdateModel);

        Task<bool> SoftDeleteLanguage(int languageId);

        Task SaveAsync();
        //Task AddLanguage1(LanguageInsertModel languageInsertModel);
    }
}

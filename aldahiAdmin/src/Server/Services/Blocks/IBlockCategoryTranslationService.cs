using FirstCall.Shared.ViewModels.Blocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IBlockCategoryTranslationService
    {
        Task<List<BlockCategoryTranslationViewModel>> GetTranslationByCategoryId(int categoryId);

        Task<BlockCategoryTranslationViewModel> GetTranslationById(int translationId);

        Task<BlockCategoryTranslationViewModel> AddTranslation(BlockCategoryTranslationInsertModel translationInsertModel);

        Task<BlockCategoryTranslationViewModel> UpdateTranslation(BlockCategoryTranslationUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteTranslation(int translationId);

        Task SaveAsync();
    }
}

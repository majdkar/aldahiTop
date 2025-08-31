using FirstCall.Shared.ViewModels.Blocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IBlockTranslationService
    {
        Task<List<BlockTranslationViewModel>> GetTranslationByBlockId(int blockId);

        Task<BlockTranslationViewModel> GetTranslationById(int translationId);

        Task<BlockTranslationViewModel> AddTranslation(BlockTranslationInsertModel translationInsertModel);

        Task<BlockTranslationViewModel> UpdateTranslation(BlockTranslationUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteTranslation(int translationId);

        Task SaveAsync();
    }
}

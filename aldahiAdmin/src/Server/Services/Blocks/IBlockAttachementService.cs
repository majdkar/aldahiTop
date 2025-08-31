using FirstCall.Shared.ViewModels.Blocks;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IBlockAttachementService
    {
        Task<List<BlockAttachementViewModel>> GetAttachementByBlockId(int blockId);

        Task<BlockAttachementViewModel> GetAttachementById(int translationId);

        Task<BlockAttachementViewModel> AddAttachement(BlockAttachementInsertModel translationInsertModel);

        Task<BlockAttachementViewModel> UpdateAttachement(BlockAttachementUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteAttachement(int translationId);

        Task SaveAsync();
    }
}

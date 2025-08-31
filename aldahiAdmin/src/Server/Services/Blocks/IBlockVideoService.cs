using System.Collections.Generic;
using System.Threading.Tasks;
using FirstCall.Shared.ViewModels.Blocks;

namespace FirstCall.Server.Services.Blocks
{
    public interface IBlockVideoService
    {
        Task<List<BlockVideoViewModel>> GetVideoByBlockId(int blockId);

        Task<BlockVideoViewModel> GetVideoById(int translationId);

        Task<BlockVideoViewModel> AddVideo(BlockVideoInsertModel translationInsertModel);

        Task<BlockVideoViewModel> UpdateVideo(BlockVideoUpdateModel translationUpdateModel);

        Task<bool> SoftDeleteVideo(int translationId);

        Task SaveAsync();
    }
}

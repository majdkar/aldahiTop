using AutoMapper;
using FirstCall.Core.Entities;
using FirstCall.Shared.ViewModels.Blocks;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstCall.Server.Services.Blocks;

namespace FirstCall.Application.Services
{
    public class BlockService : IBlockService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IBlockTranslationService translationService;
        private readonly IBlockPhotoService photoService;
        private readonly IBlockAttachementService AttachementService;
        private readonly IBlockVideoService VideoService;

        public BlockService(IUnitOfWork<int> uow, IMapper mapper,
            IBlockTranslationService translationService,
            IBlockPhotoService photoService,
            IBlockAttachementService attachementService,
            IBlockVideoService videoService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.translationService = translationService;
            this.photoService = photoService;
            this.AttachementService = attachementService;
            this.VideoService = videoService;
        }

        public async Task<List<BlockViewModel>> GetBlocks()
        {
            var blockEntities = await uow.Query<Block>().ToListAsync();
            var blocksVM = mapper.Map<List<Block>, List<BlockViewModel>>(blockEntities);
            foreach (var item in blocksVM)
            {
                item.Translations = await translationService.GetTranslationByBlockId(item.Id);

                item.BlockPhotos = await photoService.GetPhotoByBlockId(item.Id);
                item.BlockAttachements = await AttachementService.GetAttachementByBlockId(item.Id);
                item.BlockVideos = await VideoService.GetVideoByBlockId(item.Id);
            }
            return blocksVM;
        }

        public async Task<List<BlockViewModel>> GetPagedBlocks(string searchString, string orderBy)
        {
            var blockEntities = await uow.Query<Block>().ToListAsync();

            if (blockEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    blockEntities = blockEntities.Where(x => x.Description1.Contains(searchString) || x.Name.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        blockEntities = blockEntities.OrderBy(x => x.Name).ToList();
                    if (orderBy.Contains("RecordOrder"))
                        blockEntities = blockEntities.OrderBy(x => x.RecordOrder).ToList();
                    if (orderBy.Contains("Description"))
                        blockEntities = blockEntities.OrderBy(x => x.Description1).ToList();
                }
            }

            var blocksVM = mapper.Map<List<Block>, List<BlockViewModel>>(blockEntities);
            foreach (var item in blocksVM)
            {
                item.Translations = await translationService.GetTranslationByBlockId(item.Id);
                item.BlockPhotos = await photoService.GetPhotoByBlockId(item.Id);
                item.BlockAttachements = await AttachementService.GetAttachementByBlockId(item.Id);
                item.BlockVideos = await VideoService.GetVideoByBlockId(item.Id);
            }


            return blocksVM;
        }

        public async Task<List<BlockViewModel>> GetBlocksByCategoryId(int categoryId)
        {
            var blockEntities = await uow.Query<Block>().Where(x => x.CategoryId == categoryId).ToListAsync();
            var blocksVM = mapper.Map<List<Block>, List<BlockViewModel>>(blockEntities);
            foreach (var item in blocksVM)
            {
                item.Translations = await translationService.GetTranslationByBlockId(item.Id);
                item.BlockPhotos = await photoService.GetPhotoByBlockId(item.Id);
                item.BlockAttachements = await AttachementService.GetAttachementByBlockId(item.Id);
                item.BlockVideos = await VideoService.GetVideoByBlockId(item.Id);
            }
            return blocksVM;
        }

        public async Task<BlockViewModel> GetBlockById(int blockId)
        {
            var blockEntity = await uow.Query<Block>().Where(x => x.Id == blockId).FirstOrDefaultAsync();
            var blockVM = mapper.Map<Block, BlockViewModel>(blockEntity);
            blockVM.Translations = await translationService.GetTranslationByBlockId(blockVM.Id);
            blockVM.BlockPhotos = await photoService.GetPhotoByBlockId(blockVM.Id);
            blockVM.BlockAttachements = await AttachementService.GetAttachementByBlockId(blockVM.Id);
            blockVM.BlockVideos = await VideoService.GetVideoByBlockId(blockVM.Id);
            return blockVM;
        }

        public async Task<BlockViewModel> AddBlock(BlockInsertModel blockInsertModel)
        {
            try
            {
                var blockEntity = mapper.Map<BlockInsertModel, Block>(blockInsertModel);
                var result = uow.Add(blockEntity);
                await SaveAsync();
                if (result != null)
                {
                    blockEntity.Url = $"/{blockEntity.Id}";
                    uow.Update(blockEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Block, BlockViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlockViewModel> UpdateBlock(BlockUpdateModel blockUpdateModel)
        {
            try
            {
                var blockEntity = uow.Query<Block>().Where(x => x.Id == blockUpdateModel.Id).FirstOrDefault();
                if (blockEntity != null)
                {
                    blockEntity.Name = blockUpdateModel.Name ?? blockEntity.Name;
                    blockEntity.EnglishName = blockUpdateModel.EnglishName ?? blockEntity.EnglishName;
                    blockEntity.Description1 = blockUpdateModel.Description1 ?? blockEntity.Description1;
                    blockEntity.EnglishDescription1 = blockUpdateModel.EnglishDescription1 ?? blockEntity.EnglishDescription1;
                    blockEntity.Description2 = blockUpdateModel.Description2 ?? blockEntity.Description2;
                    blockEntity.EnglishDescription2 = blockUpdateModel.EnglishDescription2 ?? blockEntity.EnglishDescription2;
                    blockEntity.Description3 = blockUpdateModel.Description3 ?? blockEntity.Description3;
                    blockEntity.EnglishDescription3 = blockUpdateModel.EnglishDescription3 ?? blockEntity.EnglishDescription3;
                    blockEntity.Description4 = blockUpdateModel.Description4 ?? blockEntity.Description4;
                    blockEntity.EnglishDescription4 = blockUpdateModel.EnglishDescription4 ?? blockEntity.EnglishDescription4;
                    blockEntity.Image1 = blockUpdateModel.Image1 ?? blockEntity.Image1;
                    blockEntity.Image2 = blockUpdateModel.Image2 ?? blockEntity.Image2;
                    blockEntity.Image3 = blockUpdateModel.Image3 ?? blockEntity.Image3;
                    blockEntity.Image4 = blockUpdateModel.Image4 ?? blockEntity.Image4;

                    blockEntity.NewDate = blockUpdateModel.NewDate ?? blockEntity.NewDate;
                    blockEntity.StartDate = blockUpdateModel.StartDate ?? blockEntity.StartDate;
                    blockEntity.EndDate = blockUpdateModel.EndDate ?? blockEntity.EndDate;
                    blockEntity.StarRegistertDate = blockUpdateModel.StarRegistertDate ?? blockEntity.StarRegistertDate;
                    blockEntity.EndRegisterDate = blockUpdateModel.EndRegisterDate ?? blockEntity.EndRegisterDate;
                    blockEntity.ArticleDate = blockUpdateModel.ArticleDate ?? blockEntity.ArticleDate;
                    blockEntity.Author = blockUpdateModel.Author ?? blockEntity.Author;
                  
                    blockEntity.File = blockUpdateModel.File ?? blockEntity.File;
                    blockEntity.Url = blockUpdateModel.Url ?? blockEntity.Url;
                    blockEntity.Url1 = blockUpdateModel.Url1 ?? blockEntity.Url1;
                    blockEntity.IsVisible = blockUpdateModel.IsVisible;
                    blockEntity.IsActive = blockUpdateModel.IsActive;
                    blockEntity.RecordOrder = blockUpdateModel.RecordOrder;
                    //blockToUpdate.CategoryId = blockUpdateModel.CategoryId; // category is not changable
                    if (string.IsNullOrEmpty(blockEntity.Url))
                    {
                        blockEntity.Url = $"/{blockEntity.Id}";
                    }
                    uow.Update(blockEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Block, BlockViewModel>(blockEntity);
                    return resultVM;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<bool> SoftDeleteBlock(int blockId)
        {
            try
            {
                var blockEntity = uow.Query<Block>().Where(x => x.Id == blockId).FirstOrDefault();
                if (blockEntity != null)
                {
                    blockEntity.IsActive = !blockEntity.IsActive;
                    uow.Remove(blockEntity);
                    await SaveAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SaveAsync()
        {
            await uow.CommitAsync();
        }

        public void Dispose()
        {
            uow.Dispose();
        }

    }
}

using AutoMapper;
using FirstCall.Core.Entities;
using FirstCall.Shared.ViewModels.Blocks;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public class BlockCategoryService : IBlockCategoryService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IBlockCategoryTranslationService translationService;

        public BlockCategoryService(IUnitOfWork<int> uow, IMapper mapper, IBlockCategoryTranslationService translationService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.translationService = translationService;
        }

        public async Task<List<BlockCategoryViewModel>> GetBlockCategories()
        {
            var blockCategoriesEntities = await uow.Query<BlockCategory>().ToListAsync();
            var blockCategoriesVM = mapper.Map<List<BlockCategory>, List<BlockCategoryViewModel>>(blockCategoriesEntities);
            foreach (var item in blockCategoriesVM)
            {
                item.Translations =  await translationService.GetTranslationByCategoryId(item.Id);
            }
            return blockCategoriesVM;
        }

        public async Task<List<BlockCategoryViewModel>> GetPagedBlockCategories(string searchString, string orderBy)
        {
            var blockCategoriesEntities = await uow.Query<BlockCategory>().ToListAsync();

            if (blockCategoriesEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    blockCategoriesEntities = blockCategoriesEntities.Where(x => x.Description.Contains(searchString) || x.Name.Contains(searchString) || x.BlockType.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        blockCategoriesEntities = blockCategoriesEntities.OrderBy(x => x.Name).ToList();
                    if (orderBy.Contains("Description"))
                        blockCategoriesEntities = blockCategoriesEntities.OrderBy(x => x.Description).ToList();
                    if (orderBy.Contains("BlockType"))
                        blockCategoriesEntities = blockCategoriesEntities.OrderBy(x => x.BlockType).ToList();
                }
            }

            var blockCategoriesVM = mapper.Map<List<BlockCategory>, List<BlockCategoryViewModel>>(blockCategoriesEntities);
            foreach (var item in blockCategoriesVM)
            {
                item.Translations = await translationService.GetTranslationByCategoryId(item.Id);
            }

            return blockCategoriesVM;

        }

        public async Task<BlockCategoryViewModel> GetBlockCategoryByID(int blockCategoryId)
        {
            var blockCategoriesEntity = await uow.Query<BlockCategory>().Where(x => x.Id == blockCategoryId).FirstOrDefaultAsync();
            var blockCategoriesVM = mapper.Map<BlockCategory, BlockCategoryViewModel>(blockCategoriesEntity);
            blockCategoriesVM.Translations = await translationService.GetTranslationByCategoryId(blockCategoriesVM.Id);
            return blockCategoriesVM;
        }

        public async Task<BlockCategoryViewModel> AddBlockCategory(BlockCategoryInsertModel blockCategoryInsertModel)
        {
            try
            {
                var blockCategoryEntity = mapper.Map<BlockCategoryInsertModel, BlockCategory>(blockCategoryInsertModel);
                var result = uow.Add(blockCategoryEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockCategory, BlockCategoryViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<BlockCategoryViewModel> UpdateBlockCategory(BlockCategoryUpdateModel blockCategoryUpdateModel)
        {
            try
            {
                var blockCategoryEntity = uow.Query<BlockCategory>().Where(x => x.Id == blockCategoryUpdateModel.Id).FirstOrDefault();
                if (blockCategoryEntity != null)
                {
                    blockCategoryEntity.Name = blockCategoryUpdateModel.Name;
                    blockCategoryEntity.Description = blockCategoryUpdateModel.Description;
                    blockCategoryEntity.EnglishName = blockCategoryUpdateModel.EnglishName;
                    blockCategoryEntity.EnglishDescription = blockCategoryUpdateModel.EnglishDescription;
                    blockCategoryEntity.BlockType = blockCategoryUpdateModel.BlockType;
                    blockCategoryEntity.IsActive = blockCategoryUpdateModel.IsActive;
                    uow.Update(blockCategoryEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockCategory, BlockCategoryViewModel>(blockCategoryEntity);
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

        public async Task<bool> SoftDeleteBlockCategory(int blockCategoryId) // enable/disable
        {
            try
            {
                var blockCategoryEntity = uow.Query<BlockCategory>().Where(x => x.Id == blockCategoryId).FirstOrDefault();
                if (blockCategoryEntity != null)
                {
                    blockCategoryEntity.IsActive = !blockCategoryEntity.IsActive;
                    uow.Remove(blockCategoryEntity);
                    await SaveAsync();
                    return true;
                }
                else
                    return false;
            }
            catch (Exception )
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

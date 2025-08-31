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
    public class BlockCategoryTranslationService : IBlockCategoryTranslationService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public BlockCategoryTranslationService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<BlockCategoryTranslationViewModel>> GetTranslationByCategoryId(int categoryId)
        {
            var translationEntities = await uow.Query<BlockCategoryTranslation>().Where(x => x.CategoryId == categoryId).ToListAsync();
            var translationsVM = mapper.Map<List<BlockCategoryTranslation>, List<BlockCategoryTranslationViewModel>>(translationEntities);
            return translationsVM;
        }

        public async Task<BlockCategoryTranslationViewModel> GetTranslationById(int translationId)
        {
            var translationEntity = await uow.Query<BlockCategoryTranslation>().Where(x => x.Id == translationId).FirstOrDefaultAsync();
            var translationVM = mapper.Map<BlockCategoryTranslation, BlockCategoryTranslationViewModel>(translationEntity);
            return translationVM;
        }

        public async Task<BlockCategoryTranslationViewModel> AddTranslation(BlockCategoryTranslationInsertModel translationInsertModel)
        {
            try
            {
                var translationEntity = mapper.Map<BlockCategoryTranslationInsertModel, BlockCategoryTranslation>(translationInsertModel);
                var result = uow.Add(translationEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockCategoryTranslation, BlockCategoryTranslationViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlockCategoryTranslationViewModel> UpdateTranslation(BlockCategoryTranslationUpdateModel translationUpdateModel)
        {
            try
            {
                var translationEntity = uow.Query<BlockCategoryTranslation>().Where(x => x.Id == translationUpdateModel.Id).FirstOrDefault();
                if (translationEntity != null)
                {
                    translationEntity.Name = translationUpdateModel.Name;
                    translationEntity.Description = translationUpdateModel.Description;
                    translationEntity.Slug = translationUpdateModel.Slug;
                    translationEntity.Language = translationUpdateModel.Language;
                    translationEntity.IsActive = translationUpdateModel.IsActive;
                    translationEntity.CategoryId = translationUpdateModel.CategoryId;

                    uow.Update(translationEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockCategoryTranslation, BlockCategoryTranslationViewModel>(translationEntity);
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
        
        public async Task<bool> SoftDeleteTranslation(int translationId)
        {
            try
            {
                var translationEntity = uow.Query<BlockCategoryTranslation>().Where(x => x.Id == translationId).FirstOrDefault();
                if (translationEntity != null)
                {
                    translationEntity.IsActive = !translationEntity.IsActive;
                    uow.Update(translationEntity);
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

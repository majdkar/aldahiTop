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
    public class BlockTranslationService : IBlockTranslationService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public BlockTranslationService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<BlockTranslationViewModel>> GetTranslationByBlockId(int blockId)
        {
            var translationEntities = await uow.Query<BlockTranslation>().Where(x => x.BlockId == blockId).ToListAsync();
            var translationsVM = mapper.Map<List<BlockTranslation>, List<BlockTranslationViewModel>>(translationEntities);
            return translationsVM;
        }

        public async Task<BlockTranslationViewModel> GetTranslationById(int translationId)
        {
            var translationEntity = await uow.Query<BlockTranslation>().Where(x => x.Id == translationId).FirstOrDefaultAsync();
            var translationVM = mapper.Map<BlockTranslation, BlockTranslationViewModel>(translationEntity);
            return translationVM;
        }

        public async Task<BlockTranslationViewModel> AddTranslation(BlockTranslationInsertModel translationInsertModel)
        {
            try
            {
                var translationEntity = mapper.Map<BlockTranslationInsertModel, BlockTranslation>(translationInsertModel);
                var result = uow.Add(translationEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<BlockTranslation, BlockTranslationViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<BlockTranslationViewModel> UpdateTranslation(BlockTranslationUpdateModel translationUpdateModel)
        {
            try
            {
                var translationEntity = uow.Query<BlockTranslation>().Where(x => x.Id == translationUpdateModel.Id).FirstOrDefault();
                if (translationEntity != null)
                {
                    translationEntity.Name = translationUpdateModel.Name;
                    translationEntity.Description = translationUpdateModel.Description;
                    translationEntity.Slug = translationUpdateModel.Slug;
                    translationEntity.Language = translationUpdateModel.Language;
                    translationEntity.IsActive = translationUpdateModel.IsActive;
                    translationEntity.BlockId = translationUpdateModel.BlockId;

                    uow.Update(translationEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<BlockTranslation, BlockTranslationViewModel>(translationEntity);
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
                var translationEntity = uow.Query<BlockTranslation>().Where(x => x.Id == translationId).FirstOrDefault();
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

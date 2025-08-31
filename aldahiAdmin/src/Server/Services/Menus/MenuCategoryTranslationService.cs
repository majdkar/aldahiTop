using AutoMapper;
using FirstCall.Core.Entities;
using FirstCall.Shared.ViewModels.Menus;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public class MenuCategoryTranslationService : IMenuCategoryTranslationService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public MenuCategoryTranslationService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<MenuCategoryTranslationViewModel>> GetTranslationByCategoryId(int categoryId)
        {
            var translationEntities = await uow.Query<MenuCategoryTranslation>().Where(x => x.CategoryId == categoryId).ToListAsync();
            var translationsVM = mapper.Map<List<MenuCategoryTranslation>, List<MenuCategoryTranslationViewModel>>(translationEntities);
            return translationsVM;
        }

        public async Task<MenuCategoryTranslationViewModel> GetTranslationById(int translationId)
        {
            var translationEntity = await uow.Query<MenuCategoryTranslation>().Where(x => x.Id == translationId).FirstOrDefaultAsync();
            var translationVM = mapper.Map<MenuCategoryTranslation, MenuCategoryTranslationViewModel>(translationEntity);
            return translationVM;
        }

        public async Task<MenuCategoryTranslationViewModel> AddTranslation(MenuCategoryTranslationInsertModel translationInsertModel)
        {
            try
            {
                var translationEntity = mapper.Map<MenuCategoryTranslationInsertModel, MenuCategoryTranslation>(translationInsertModel);
                var result = uow.Add(translationEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<MenuCategoryTranslation, MenuCategoryTranslationViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MenuCategoryTranslationViewModel> UpdateTranslation(MenuCategoryTranslationUpdateModel translationUpdateModel)
        {
            try
            {
                var translationEntity = uow.Query<MenuCategoryTranslation>().Where(x => x.Id == translationUpdateModel.Id).FirstOrDefault();
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
                    var resultVM = mapper.Map<MenuCategoryTranslation, MenuCategoryTranslationViewModel>(translationEntity);
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
                var translationEntity = uow.Query<MenuCategoryTranslation>().Where(x => x.Id == translationId).FirstOrDefault();
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

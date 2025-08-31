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
    public class MenuTranslationService : IMenuTranslationService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public MenuTranslationService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<MenuTranslationViewModel>> GetTranslationByMenuId(int menuId)
        {
            var translationEntities = await uow.Query<MenueTranslate>().Where(x => x.MenueId == menuId).ToListAsync();
            var translationsVM = mapper.Map<List<MenueTranslate>, List<MenuTranslationViewModel>>(translationEntities);
            return translationsVM;
        }

        public async Task<MenuTranslationViewModel> GetTranslationById(int translationId)
        {
            var translationEntity = await uow.Query<MenueTranslate>().Where(x => x.Id == translationId).FirstOrDefaultAsync();
            var translationVM = mapper.Map<MenueTranslate, MenuTranslationViewModel>(translationEntity);
            return translationVM;
        }

        public async Task<MenuTranslationViewModel> AddTranslation(MenuTranslationInsertModel translationInsertModel)
        {
            try
            {
                var translationEntity = mapper.Map<MenuTranslationInsertModel, MenueTranslate>(translationInsertModel);
                var result = uow.Add(translationEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<MenueTranslate, MenuTranslationViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MenuTranslationViewModel> UpdateTranslation(MenuTranslationUpdateModel translationUpdateModel)
        {
            try
            {
                var translationEntity = uow.Query<MenueTranslate>().Where(x => x.Id == translationUpdateModel.Id).FirstOrDefault();
                if (translationEntity != null)
                {
                    translationEntity.Name = translationUpdateModel.Name;
                    translationEntity.HtmlText = translationUpdateModel.HtmlText;
                    translationEntity.Language = translationUpdateModel.Language;
                    translationEntity.IsActive = translationUpdateModel.IsActive;
                    translationEntity.MenueId = translationUpdateModel.MenueId;

                    uow.Update(translationEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<MenueTranslate, MenuTranslationViewModel>(translationEntity);
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
                var translationEntity = uow.Query<MenueTranslate>().Where(x => x.Id == translationId).FirstOrDefault();
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

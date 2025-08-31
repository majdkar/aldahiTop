using AutoMapper;
using FirstCall.Core.Entities;
using FirstCall.Shared.ViewModels.Pages;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public class PageTranslationService : IPageTranslationService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;

        public PageTranslationService(IUnitOfWork<int> uow, IMapper mapper)
        {
            this.uow = uow;
            this.mapper = mapper;
        }

        public async Task<List<PageTranslationViewModel>> GetTranslationByPageId(int pageId)
        {
            var translationEntities = await uow.Query<PageTranslation>().Where(x => x.PageId == pageId).ToListAsync();
            var translationsVM = mapper.Map<List<PageTranslation>, List<PageTranslationViewModel>>(translationEntities);
            return translationsVM;
        }

        public async Task<PageTranslationViewModel> GetTranslationById(int translationId)
        {
            var translationEntity = await uow.Query<PageTranslation>().Where(x => x.Id == translationId).FirstOrDefaultAsync();
            var translationVM = mapper.Map<PageTranslation, PageTranslationViewModel>(translationEntity);
            return translationVM;
        }

        public async Task<PageTranslationViewModel> AddTranslation(PageTranslationInsertModel translationInsertModel)
        {
            try
            {
                var translationEntity = mapper.Map<PageTranslationInsertModel, PageTranslation>(translationInsertModel);
                var result = uow.Add(translationEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<PageTranslation, PageTranslationViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<PageTranslationViewModel> UpdateTranslation(PageTranslationUpdateModel translationUpdateModel)
        {
            try
            {
                var translationEntity = uow.Query<PageTranslation>().Where(x => x.Id == translationUpdateModel.Id).FirstOrDefault();
                if (translationEntity != null)
                {
                    translationEntity.Name = translationUpdateModel.Name;
                    translationEntity.Description = translationUpdateModel.Description;
                    translationEntity.Link1 = translationUpdateModel.Link1;
                    translationEntity.Link2 = translationUpdateModel.Link2;
                    translationEntity.Slug = translationUpdateModel.Slug;
                    translationEntity.Language = translationUpdateModel.Language;
                    translationEntity.IsActive = translationUpdateModel.IsActive;
                    translationEntity.File = translationUpdateModel.File;
                    translationEntity.PageId = translationUpdateModel.PageId;

                    uow.Update(translationEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<PageTranslation, PageTranslationViewModel>(translationEntity);
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
                var translationEntity = uow.Query<PageTranslation>().Where(x => x.Id == translationId).FirstOrDefault();
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
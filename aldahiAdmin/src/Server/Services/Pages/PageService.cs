using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.ViewModels.Pages;
using FirstCall.Core.Entities;

namespace FirstCall.Application.Services
{
    public class PageService : IPageService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IPageTranslationService translationService;
        private readonly IPagePhotoService photoService;
        private readonly IPageAttachementService AttachementService;

        public PageService(IUnitOfWork<int> uow, IMapper mapper,
            IPageTranslationService translationService,
            IPagePhotoService photoService,
            IPageAttachementService AttachementService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.translationService = translationService;
            this.photoService = photoService;
            this.AttachementService = AttachementService;
        }

        public async Task<List<PageViewModel>> GetPages()
        {
            var pagesEntities = await uow.Query<Page>().ToListAsync();
            var PagesVM = mapper.Map<List<Page>, List<PageViewModel>>(pagesEntities);
            //foreach (var item in PagesVM)
            //{
            //    item.Translations = await translationService.GetTranslationByPageId(item.Id);
            //}
            foreach (var item in PagesVM)
            {
                item.PagePhotos = await photoService.GetPhotoByPageId(item.Id);
                item.PageAttachements = await AttachementService.GetAttachementByPageId(item.Id);
            }
            return PagesVM;
        }

        public async Task<List<PageViewModel>> GetPagedPages(string searchString, string orderBy)
        {
            var pagesEntities = await uow.Query<Page>().ToListAsync();

            if (pagesEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    pagesEntities = pagesEntities.Where(x => x.Name.Contains(searchString) || x.Type.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        pagesEntities = pagesEntities.OrderBy(x => x.Name).ToList();
                    if (orderBy.Contains("RecordOrder"))
                        pagesEntities = pagesEntities.OrderBy(x => x.RecordOrder).ToList();
                    if (orderBy.Contains("Type"))
                        pagesEntities = pagesEntities.OrderBy(x => x.Type).ToList();
                }
            }

            var pagesVM = mapper.Map<List<Page>, List<PageViewModel>>(pagesEntities);
            //foreach (var item in pagesVM)
            //{
            //    item.Translations = await translationService.GetTranslationByPageId(item.Id);
            //}
            foreach (var item in pagesVM)
            {
                item.Translations = await translationService.GetTranslationByPageId(item.Id);
                item.PagePhotos = await photoService.GetPhotoByPageId(item.Id);
                item.PageAttachements = await AttachementService.GetAttachementByPageId(item.Id);
            }

            return pagesVM;

        }

        public async Task<PageViewModel> GetPageByID(int pageId)
        {
            var pagesEntity = await uow.Query<Page>().Where(x => x.Id == pageId).FirstOrDefaultAsync();
            var pageVM = mapper.Map<Page, PageViewModel>(pagesEntity);
            pageVM.Translations = await translationService.GetTranslationByPageId(pageVM.Id);
            pageVM.PagePhotos = await photoService.GetPhotoByPageId(pageVM.Id);
            pageVM.PageAttachements = await AttachementService.GetAttachementByPageId(pageVM.Id);
            return pageVM;
        }

        public async Task<PageViewModel> AddPage(PageInsertModel pageInsertModel)
        {
            try
            {
                var pageEntity = mapper.Map<PageInsertModel, Page>(pageInsertModel);
                var result = uow.Add(pageEntity);
                await SaveAsync();
                if (result != null)
                {
                    pageEntity.Url = $"/{pageEntity.Id}";
                    uow.Update(pageEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Page, PageViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public async Task<PageViewModel> UpdatePage(PageUpdateModel pageUpdateModel)
        {
            try
            {
                var PageEntity = uow.Query<Page>().Where(x => x.Id == pageUpdateModel.Id).FirstOrDefault();
                if (PageEntity != null)
                {
                    PageEntity.Name = pageUpdateModel.Name;
                    PageEntity.Description1 = pageUpdateModel.Description1;
                    PageEntity.Description2 = pageUpdateModel.Description2;
                    PageEntity.EnglishName = pageUpdateModel.EnglishName;
                    PageEntity.EnglishDescription1 = pageUpdateModel.EnglishDescription1;
                    PageEntity.EnglishDescription2 = pageUpdateModel.EnglishDescription2;
                    PageEntity.Type = pageUpdateModel.Type;
                    PageEntity.Image = pageUpdateModel.Image;
                    PageEntity.Image1 = pageUpdateModel.Image1;
                    PageEntity.Image2 = pageUpdateModel.Image2;
                    PageEntity.Image3 = pageUpdateModel.Image3;
                    PageEntity.Image4 = pageUpdateModel.Image4;
                    PageEntity.CaptionArabic = pageUpdateModel.CaptionArabic;
                    PageEntity.CaptionEnglish = pageUpdateModel.CaptionEnglish;
                    PageEntity.MenuType = pageUpdateModel.MenuType;
                    PageEntity.GeoLocation = pageUpdateModel.GeoLocation;
                    PageEntity.RecordOrder = pageUpdateModel.RecordOrder;
                    PageEntity.IsActive = pageUpdateModel.IsActive;
                    PageEntity.IsHome = pageUpdateModel.IsHome;
                    PageEntity.IsFooter = pageUpdateModel.IsFooter;
                    PageEntity.IsHomeFooter = pageUpdateModel.IsHomeFooter;
                    PageEntity.Url = string.IsNullOrEmpty(PageEntity.Url) ? $"/{PageEntity.Id}" : pageUpdateModel.Url;

                    PageEntity.MenuId = pageUpdateModel.MenuId;

                    uow.Update(PageEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Page, PageViewModel>(PageEntity);
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

        public async Task<bool> SoftDeletePage(int pageId) // enable/disable
        {
            try
            {
                var pageEntity = uow.Query<Page>().Where(x => x.Id == pageId).FirstOrDefault();
                if (pageEntity != null)
                {
                    pageEntity.IsActive = !pageEntity.IsActive;
                    uow.Remove(pageEntity);
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
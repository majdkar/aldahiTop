using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Core.Entities;
using FirstCall.Shared.ViewModels.Menus;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;

namespace FirstCall.Application.Services
{
    public class MenuCategoryService : IMenuCategoryService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IMenuCategoryTranslationService translationService;

        public MenuCategoryService(IUnitOfWork<int> uow,IMapper mapper, IMenuCategoryTranslationService translationService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.translationService = translationService;
        }

        public async Task<List<MenuCategoryViewModel>> GetMenuCategories()
        {
            var menuCategoriesEntities = await uow.Query<MenuCategory>().ToListAsync();
            var menuCategoriesVM = mapper.Map<List<MenuCategory>, List<MenuCategoryViewModel>>(menuCategoriesEntities);
            foreach (var item in menuCategoriesVM)
            {
                item.Translations = await translationService.GetTranslationByCategoryId(item.Id);
            }
            return menuCategoriesVM;
        }

        public async Task<List<MenuCategoryViewModel>> GetPagedMenuCategories(string searchString, string orderBy)
        {
            var menuCategoriesEntities = await uow.Query<MenuCategory>().ToListAsync();

            if (menuCategoriesEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    menuCategoriesEntities = menuCategoriesEntities.Where(x => x.Name.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        menuCategoriesEntities = menuCategoriesEntities.OrderBy(x => x.Name).ToList();
                }
            }

            var menuCategoriesVM = mapper.Map<List<MenuCategory>, List<MenuCategoryViewModel>>(menuCategoriesEntities);
            foreach (var item in menuCategoriesVM)
            {
                item.Translations = await translationService.GetTranslationByCategoryId(item.Id);
            }

            return menuCategoriesVM;
        }

        public async Task<MenuCategoryViewModel> GetMenuCategoryByID(int menuCategoryId)
        {
            var menuCategoriesEntity = await uow.Query<MenuCategory>().Where(x => x.Id == menuCategoryId).FirstOrDefaultAsync();
            var menuCategoryVM = mapper.Map<MenuCategory, MenuCategoryViewModel>(menuCategoriesEntity);
            menuCategoryVM.Translations = await translationService.GetTranslationByCategoryId(menuCategoryVM.Id);
            return menuCategoryVM;
        }

        public async Task<MenuCategoryViewModel> AddMenuCategory(MenuCategoryInsertModel menuCategoryInsertModel)
        {
            try
            {
                var menuCategoriesEntity = mapper.Map<MenuCategoryInsertModel, MenuCategory>(menuCategoryInsertModel);
                var result = uow.Add(menuCategoriesEntity);
                await SaveAsync();
                if (result != null)
                {
                    var resultVM = mapper.Map<MenuCategory, MenuCategoryViewModel>(result);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MenuCategoryViewModel> UpdateMenuCategory(MenuCategoryUpdateModel menuCategoryUpdateModel)
        {
            try
            {
                var menuCategoriesEntity = uow.Query<MenuCategory>().Where(x => x.Id == menuCategoryUpdateModel.Id).FirstOrDefault(); 
                if (menuCategoriesEntity != null)
                {
                    menuCategoriesEntity.Name = menuCategoryUpdateModel.Name;
                    menuCategoriesEntity.Description = menuCategoryUpdateModel.Description;
                    menuCategoriesEntity.EnglishName = menuCategoryUpdateModel.EnglishName;
                    menuCategoriesEntity.EnglishDescription = menuCategoryUpdateModel.EnglishDescription;
                    menuCategoriesEntity.IsActive = menuCategoryUpdateModel.IsActive;
                    uow.Update(menuCategoriesEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<MenuCategory, MenuCategoryViewModel>(menuCategoriesEntity);
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

        public async Task<bool> SoftDeleteMenuCategory(int menuCategoryId) // enable/disable
        {
            try
            {
                var menuCategoriesEntity = uow.Query<MenuCategory>().Where(x => x.Id == menuCategoryId).FirstOrDefault(); 
                if (menuCategoriesEntity != null)
                {
                    menuCategoriesEntity.IsActive = !menuCategoriesEntity.IsActive;
                    uow.Remove(menuCategoriesEntity);
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

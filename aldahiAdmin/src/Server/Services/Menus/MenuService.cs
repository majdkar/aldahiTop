using AutoMapper;
using FirstCall.Core.Entities;
using FirstCall.Shared.ViewModels.Menus;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.Colors;

namespace FirstCall.Application.Services
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork<int> uow;
        private readonly IMapper mapper;
        private readonly IMenuTranslationService translationService;

        public MenuService(IUnitOfWork<int> uow, IMapper mapper, IMenuTranslationService translationService)
        {
            this.uow = uow;
            this.mapper = mapper;
            this.translationService = translationService;
        }

        public async Task<List<MenuViewModel>> GetMenus()
        {
            var menuEntities = await uow.Query<Menu>().ToListAsync();
            var menusVM = mapper.Map<List<Menu>, List<MenuViewModel>>(menuEntities);
            foreach (var item in menusVM)
            {
                item.Translations = await translationService.GetTranslationByMenuId(item.Id);
                item.ParentName = GetMenuNameById(item.ParentId);
            }
            return menusVM;
        }

        public async Task<List<MenuViewModel>> GetPagedMenus(string searchString, string orderBy)
        {
            var menuEntities = await uow.Query<Menu>().Include(x => x.Children).ToListAsync();

            if (menuEntities != null)
            {
                if (!string.IsNullOrEmpty(searchString))
                {
                    menuEntities = menuEntities.Where(x => x.Name.Contains(searchString)).ToList();
                }
                if (!string.IsNullOrEmpty(orderBy))
                {
                    if (orderBy.Contains("Name"))
                        menuEntities = menuEntities.OrderBy(x => x.Name).ToList();
                    if (orderBy.Contains("LevelOrder"))
                        menuEntities = menuEntities.OrderBy(x => x.LevelOrder).ToList();
                    if (orderBy.Contains("Type"))
                        menuEntities = menuEntities.OrderBy(x => x.Type).ToList();
                }
            }

            var menusVM = mapper.Map<List<Menu>, List<MenuViewModel>>(menuEntities);
            foreach (var item in menusVM)
            {
                item.Translations = await translationService.GetTranslationByMenuId(item.Id);
                item.ParentName = GetMenuNameById(item.ParentId);
            }

            return menusVM;
        }

        public async Task<List<MenuViewModel>> GetMenusByCategoryId(int categoryId)
        {
            var menuEntities = await uow.Query<Menu>().Where(x => x.CategoryId == categoryId).ToListAsync();
            var menusVM = mapper.Map<List<Menu>, List<MenuViewModel>>(menuEntities);
            foreach (var item in menusVM)
            {
                item.Translations = await translationService.GetTranslationByMenuId(item.Id);
                item.ParentName = GetMenuNameById(item.ParentId);
            }
            return menusVM;
        }

        public async Task<MenuViewModel> GetMenuById(int menuId)
        {
            var menuEntity = await uow.Query<Menu>().Include(x => x.Children).Where(x => x.Id == menuId).FirstOrDefaultAsync();
            var menuVM = mapper.Map<Menu, MenuViewModel>(menuEntity);
            menuVM.Translations = await translationService.GetTranslationByMenuId(menuVM.Id);
            if (menuVM.ParentId != null)
            {
                menuVM.ParentName = (await GetMenuById(menuVM.ParentId ?? 0)).Name;
                menuVM.ParentName = GetMenuNameById(menuVM.ParentId);
            }
            return menuVM;
        }

        public async Task<MenuViewModel> AddMenu(MenuInsertModel menuInsertModel)
        {
            try
            {
                var menuEntity = mapper.Map<MenuInsertModel, Menu>(menuInsertModel);
                var result = uow.Add(menuEntity);
                await SaveAsync();
                if (result != null)
                {
                    menuEntity.PageUrl = $"/{menuEntity.Id}";
                    uow.Update(menuEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Menu, MenuViewModel>(result);
                    resultVM.ParentName = GetMenuNameById(resultVM.ParentId);
                    return resultVM;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<MenuViewModel> UpdateMenu(MenuUpdateModel menuUpdateModel)
        {
            try
            {
                var menuEntity = uow.Query<Menu>().Where(x => x.Id == menuUpdateModel.Id).FirstOrDefault();
                if (menuEntity != null)
                {
                    menuEntity.Name = menuUpdateModel.Name;
                    menuEntity.EnglishName = menuUpdateModel.EnglishName;
                    menuEntity.Description1 = menuUpdateModel.Description1;
                    menuEntity.EnglishDescription1 = menuUpdateModel.EnglishDescription1;
                    menuEntity.Description2 = menuUpdateModel.Description2;
                    menuEntity.EnglishDescription2 = menuUpdateModel.EnglishDescription2;
                    menuEntity.Description3 = menuUpdateModel.Description3;
                    menuEntity.EnglishDescription3 = menuUpdateModel.EnglishDescription3;
                    menuEntity.Description4 = menuUpdateModel.Description4;
                    menuEntity.EnglishDescription4 = menuUpdateModel.EnglishDescription4;
                    menuEntity.Type = menuUpdateModel.Type;
                    menuEntity.Image1 = menuUpdateModel.Image1;
                    menuEntity.Image2 = menuUpdateModel.Image2;
                    menuEntity.Image3 = menuUpdateModel.Image3;
                    menuEntity.Image4 = menuUpdateModel.Image4;

                    menuEntity.File = menuUpdateModel.File;
                    menuEntity.FileEnglish = menuUpdateModel.FileEnglish;
                    menuEntity.PageUrl = menuUpdateModel.PageUrl;
                    menuEntity.Url = menuUpdateModel.Url;
                    menuEntity.LevelOrder = menuUpdateModel.LevelOrder;
                    menuEntity.IsActive = menuUpdateModel.IsActive;
                    menuEntity.IsHome = menuUpdateModel.IsHome;
                    menuEntity.IsFooter = menuUpdateModel.IsFooter;
                    menuEntity.IsHomeFooter = menuUpdateModel.IsHomeFooter;
                    menuEntity.CategoryId = menuUpdateModel.CategoryId;

                    menuEntity.ParentId = menuUpdateModel.ParentId;
                    menuEntity.PageUrl = string.IsNullOrEmpty(menuEntity.PageUrl) ? $"/{menuEntity.Id}" : menuUpdateModel.PageUrl;
                    //menuToUpdate.CategoryId = menuUpdateModel.CategoryId; // category is not changable
                    uow.Update(menuEntity);
                    await SaveAsync();
                    var resultVM = mapper.Map<Menu, MenuViewModel>(menuEntity);
                    if (resultVM.ParentId != null)
                    {
                        resultVM.ParentName = GetMenuNameById(resultVM.ParentId);
                    }
                    else
                    {
                        resultVM.ParentName = "";
                    }


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

        public async Task<bool> SoftDeleteMenu(int menuId)
        {
            try
            {
                var menuEntity = uow.Query<Menu>().Where(x => x.Id == menuId).FirstOrDefault();
                if (menuEntity != null)
                {
                    menuEntity.IsActive = !menuEntity.IsActive;
                    uow.Remove(menuEntity);
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

        private string GetMenuNameById(int? menuId)
        {
            if (menuId == null)
                return "";
            var menuEntity = uow.Query<Menu>().Where(x => x.Id == menuId).FirstOrDefault();
            if (menuEntity != null)
                return menuEntity.Name;
            return "";
        }

        public async Task<List<MenuViewModel>> GetMenusWithSubMenu()
        {
            var menuEntities = await uow.Query<Menu>().Include(x => x.Children).Where(x => x.ParentId == null).ToListAsync();
            var menusVM = mapper.Map<List<Menu>, List<MenuViewModel>>(menuEntities);
            foreach (var item in menusVM)
            {

                item.Translations = await translationService.GetTranslationByMenuId(item.Id);
                item.ParentName = GetMenuNameById(item.ParentId);
            }
            return menusVM;
        }
    }
}

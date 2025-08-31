using FirstCall.Shared.ViewModels.Menus;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirstCall.Application.Services
{
    public interface IMenuService
    {
        Task<List<MenuViewModel>> GetMenus();
        Task<List<MenuViewModel>> GetMenusWithSubMenu();

        Task<List<MenuViewModel>> GetPagedMenus(string searchString, string orderBy);

        Task<List<MenuViewModel>> GetMenusByCategoryId(int categoryId);

        Task<MenuViewModel> GetMenuById(int menuId);

        Task<MenuViewModel> AddMenu(MenuInsertModel menuInsertModel);

        Task<MenuViewModel> UpdateMenu(MenuUpdateModel menuUpdateModel);

        Task<bool> SoftDeleteMenu(int menuId);

        Task SaveAsync();
    }
}

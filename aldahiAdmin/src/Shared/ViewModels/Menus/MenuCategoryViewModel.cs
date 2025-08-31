using System.Collections.Generic;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuCategoryViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public bool IsActive { get; set; }
        public bool IsVisableUser { get; set; }

        public virtual List<MenuViewModel> Menus { get; set; }

        public virtual List<MenuCategoryTranslationViewModel> Translations { get; set; }

        public bool ShowTranslation { get; set; } = false;
    }
}

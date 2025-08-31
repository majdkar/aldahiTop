using System.Collections.Generic;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description1 { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription1 { set; get; }

        public string Type { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public string Image4 { get; set; }

        public string File { get; set; }
        public string FileEnglish { get; set; }

        public string PageUrl { get; set; }
        public string Url { get; set; }

        public int LevelOrder { get; set; }

        public int? ParentId { get; set; }

        public string ParentName { get; set; }

        public string Description2 { set; get; }
        public string EnglishDescription2 { set; get; }

        public string Description3 { set; get; }
        public string EnglishDescription3 { set; get; }

        public string Description4 { set; get; }
        public string EnglishDescription4 { set; get; }

        public bool IsHome { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHomeFooter { get; set; }

        public virtual List<MenuTranslationViewModel> Translations { get; set; }
        public virtual ICollection<MenuViewModel> Children { get; set; }

        public bool ShowTranslation { get; set; } = false;

        public int CategoryId { get; set; }
        public virtual MenuCategoryViewModel MenuCategory { get; set; }

        public bool IsActive { get; set; }

    }
}

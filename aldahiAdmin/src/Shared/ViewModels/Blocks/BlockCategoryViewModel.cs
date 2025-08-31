using System.Collections.Generic;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockCategoryViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public string BlockType { get; set; }

        public bool IsActive { get; set; }

        public List<BlockViewModel> Blocks { get; set; }

        public virtual List<BlockCategoryTranslationViewModel> Translations { get; set; }

        public bool ShowTranslation { get; set; } = false;
    }
}

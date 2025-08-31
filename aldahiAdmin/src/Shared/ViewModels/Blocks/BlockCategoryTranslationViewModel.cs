using System.ComponentModel.DataAnnotations;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockCategoryTranslationViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public int CategoryId { get; set; }
    }

   
}

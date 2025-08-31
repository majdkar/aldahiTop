namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuCategoryTranslationViewModel
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

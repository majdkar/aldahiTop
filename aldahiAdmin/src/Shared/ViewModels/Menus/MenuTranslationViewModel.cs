namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuTranslationViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string HtmlText { set; get; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public int MenueId { get; set; }
        public int CategoryId { get; set; }
    }
}

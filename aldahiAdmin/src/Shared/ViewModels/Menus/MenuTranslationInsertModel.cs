using FluentValidation;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuTranslationInsertModel
    {
        public string Name { set; get; }

        public string HtmlText { set; get; }

        public bool IsActive { get; set; } = true;

        public string Language { get; set; }

        public int MenueId { get; set; }
    }

    public class MenuTranslationInsertValidator : AbstractValidator<MenuTranslationInsertModel>
    {
        public MenuTranslationInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}

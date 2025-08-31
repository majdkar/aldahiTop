using FluentValidation;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuCategoryTranslationInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; } = true;

        public string Language { get; set; }

        public int CategoryId { get; set; }
    }

    public class MenuCategoryTranslationInsertValidator : AbstractValidator<MenuCategoryTranslationInsertModel>
    {
        public MenuCategoryTranslationInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}

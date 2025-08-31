using FluentValidation;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuCategoryTranslationUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public int CategoryId { get; set; }
    }

    public class MenuCategoryTranslationUpdateValidator : AbstractValidator<MenuCategoryTranslationUpdateModel>
    {
        public MenuCategoryTranslationUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}

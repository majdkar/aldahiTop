using FluentValidation;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuCategoryInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public bool IsActive { get; set; } = true;

        public bool IsVisableUser { get; set; }
    }

    public class MenuCategoryInsertValidator : AbstractValidator<MenuCategoryInsertModel>
    {
        public MenuCategoryInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");

        }
    }
}

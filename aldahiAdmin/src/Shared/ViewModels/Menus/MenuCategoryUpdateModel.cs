using FluentValidation;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuCategoryUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public bool IsActive { get; set; }
        public bool IsVisableUser { get; set; }
    }

    public class MenuCategoryUpdateValidator : AbstractValidator<MenuCategoryUpdateModel>
    {
        public MenuCategoryUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");

        }
    }
}

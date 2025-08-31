using FluentValidation;
using System.Collections.Generic;

namespace FirstCall.Shared.ViewModels.Menus
{
    public class MenuUpdateModel
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

        public int CategoryId { get; set; }

        public bool IsActive { get; set; }

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
    }

    public class MenuUpdateValidator : AbstractValidator<MenuUpdateModel>
    {
        public MenuUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("You must choose Category");
            RuleFor(p => p.LevelOrder).GreaterThanOrEqualTo(0).WithMessage("Value must be Greater or Equal to Zero");
        }
    }
}

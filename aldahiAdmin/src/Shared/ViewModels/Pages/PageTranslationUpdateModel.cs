using FluentValidation;

namespace FirstCall.Shared.ViewModels.Pages
{
    public class PageTranslationUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Link1 { get; set; }

        public string Link2 { get; set; }

        public int PageId { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public string Slug { get; set; }

        public string File { get; set; }
    }

    public class PageTranslationUpdateValidator : AbstractValidator<PageTranslationUpdateModel>
    {
        public PageTranslationUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.Description).NotEmpty().WithMessage("You must enter Description");
        }
    }
}

using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockCategoryTranslationUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public int CategoryId { get; set; }
    }

    public class BlockCategoryTranslationUpdateValidator : AbstractValidator<BlockCategoryTranslationUpdateModel>
    {
        public BlockCategoryTranslationUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}

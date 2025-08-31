using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockCategoryTranslationInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; } = true;

        public string Language { get; set; }

        public int CategoryId { get; set; }
    }

    public class BlockCategoryTranslationInsertValidator : AbstractValidator<BlockCategoryTranslationInsertModel>
    {
        public BlockCategoryTranslationInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}

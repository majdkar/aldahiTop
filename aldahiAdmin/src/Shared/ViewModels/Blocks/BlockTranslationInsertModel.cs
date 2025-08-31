using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockTranslationInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; } = true;

        public string Language { get; set; }

        public int BlockId { get; set; }
    }

    public class BlockTranslationInsertValidator : AbstractValidator<BlockTranslationInsertModel>
    {
        public BlockTranslationInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.Description).NotEmpty().WithMessage("You must enter description");
        }
    }
}

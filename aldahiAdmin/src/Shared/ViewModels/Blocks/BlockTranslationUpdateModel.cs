using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockTranslationUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public int BlockId { get; set; }
    }

    public class BlockTranslationUpdateValidator : AbstractValidator<BlockTranslationUpdateModel>
    {
        public BlockTranslationUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.Description).NotEmpty().WithMessage("You must enter description");

        }
    }
}

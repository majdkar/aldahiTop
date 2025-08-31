using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockCategoryInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public string BlockType { get; set; }

        public bool IsActive { get; set; } = true;

    }

    public class BlockCategoryInsertValidator : AbstractValidator<BlockCategoryInsertModel>
    {
        public BlockCategoryInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.BlockType).NotEmpty().WithMessage("You must enter block type");
        }
    }
}

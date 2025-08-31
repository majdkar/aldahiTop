using FluentValidation;
using System;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockCategoryUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public string BlockType { get; set; }

        public bool IsActive { get; set; }
        
    }
    public class BlockCategoryUpdateValidator : AbstractValidator<BlockCategoryUpdateModel>
    {
        public BlockCategoryUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.BlockType).NotEmpty().WithMessage("You must enter block type");
        }
    }
}

using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockPhotoInsertModel
    {
        public string Image { get; set; }

        public int BlockId { get; set; }
    }

    public class BlockPhotoInsertValidator : AbstractValidator<BlockPhotoInsertModel>
    {
        public BlockPhotoInsertValidator()
        {
            
        }
    }
}

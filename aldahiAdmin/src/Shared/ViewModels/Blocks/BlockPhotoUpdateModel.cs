using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockPhotoUpdateModel
    {
        public int Id { set; get; }

        public string Image { get; set; }

        public int BlockId { get; set; }
    }

    public class BlockPhotoUpdateValidator : AbstractValidator<BlockPhotoUpdateModel>
    {
        public BlockPhotoUpdateValidator()
        {


        }
    }
}

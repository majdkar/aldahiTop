using FluentValidation;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockAttachementUpdateModel
    {
        public int Id { set; get; }

        public string File { get; set; }
        public string Name { get; set; }

        public int BlockId { get; set; }
    }

    public class BlockAttachementUpdateValidator : AbstractValidator<BlockAttachementUpdateModel>
    {
        public BlockAttachementUpdateValidator()
        {


        }
    }
}

using FluentValidation;

namespace FirstCall.Shared.ViewModels.Pages
{
    public class PageAttachementUpdateModel
    {
        public int Id { set; get; }

        public string File { get; set; }
        public string Name { get; set; }

        public int PageId { get; set; }
    }

    public class PageAttachementUpdateValidator : AbstractValidator<PageAttachementUpdateModel>
    {
        public PageAttachementUpdateValidator()
        {


        }
    }
}

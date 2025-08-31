using FluentValidation;
using System.Collections.Generic;

namespace FirstCall.Shared.ViewModels.Pages
{
    public class PageUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description1 { set; get; }


        public string EnglishName { set; get; }

        public string EnglishDescription1 { set; get; }

        public string Image { get; set; }

        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }

        public string Description2 { set; get; }

        public string EnglishDescription2 { set; get; }

        public string CaptionArabic { set; get; }
        public string CaptionEnglish { set; get; }

        public string Type { get; set; }

        public int MenuType { get; set; }

        public string GeoLocation { get; set; }
        public bool IsActive { get; set; }

        public int RecordOrder { get; set; }
        public int MenuId { get; set; }
        public string Url { get; set; }

        public bool IsHome { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHomeFooter { get; set; }
        public virtual List<PageTranslationViewModel> Translations { get; set; }
        public virtual List<PagePhotoViewModel> PagePhotos { get; set; }
        public virtual List<PageAttachementViewModel> PageAttachements { get; set; }
    }

    public class PageUpdateValidator : AbstractValidator<PageUpdateModel>
    {
        public PageUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
        }
    }
}

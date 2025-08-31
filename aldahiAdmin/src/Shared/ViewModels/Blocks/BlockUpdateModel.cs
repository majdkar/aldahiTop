using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FirstCall.Shared.ViewModels.Blocks
{
    public class BlockUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string EnglishName { set; get; }

        public string Description1 { set; get; }
        public string EnglishDescription1 { set; get; }

        public string Description2 { set; get; }
        public string EnglishDescription2 { set; get; }

        public string Description3 { set; get; }
        public string EnglishDescription3 { set; get; }

        public string Description4 { set; get; }
        public string EnglishDescription4 { set; get; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public int CategoryId { get; set; }
        
        public bool IsVisible { get; set; }
        public string Url { get; set; }
        public string Url1 { get; set; }

        public string File { get; set; }

        public bool IsActive { get; set; }

        public int RecordOrder { get; set; }
        public DateTime? NewDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


        public DateTime? StarRegistertDate { get; set; }
        public DateTime? EndRegisterDate { get; set; }

        public DateTime? ArticleDate { get; set; }
        public string Author { set; get; }
        public virtual List<BlockTranslationViewModel> Translations { get; set; }
        public virtual List<BlockPhotoViewModel> BlockPhotos { get; set; }
        public virtual List<BlockAttachementViewModel> BlockAttachements { get; set; }
        public virtual List<BlockVideoViewModel> BlockVideos { get; set; }

    }

    public class BlockUpdateValidator : AbstractValidator<BlockUpdateModel>
    {
        public BlockUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.EnglishName).NotEmpty().WithMessage("You must enter English Name");
            RuleFor(p => p.CategoryId).NotEmpty().WithMessage("You must choose category");
            RuleFor(p => p.RecordOrder).GreaterThanOrEqualTo(0).WithMessage("Value must be Greater or Equal to Zero");
            RuleFor(p => p.StartDate)
               .LessThanOrEqualTo(p => p.EndDate).WithMessage("Start Date must be less Or Equal than End Date");
            RuleFor(p => p.EndDate)
                 .GreaterThanOrEqualTo(p => p.StartDate).WithMessage("End Date must be grater than Or Equal Start Date");
            RuleFor(p => p.StarRegistertDate)
              .LessThanOrEqualTo(p => p.EndRegisterDate).WithMessage("Start Register Date must be less than Or Equal End Register Date");
            RuleFor(p => p.EndRegisterDate)
                 .GreaterThanOrEqualTo(p => p.StarRegistertDate).WithMessage("End Register Date must be grater than Or Equal Start Register Date");
            RuleFor(p => p.StarRegistertDate)
             .GreaterThanOrEqualTo(p => p.StartDate).WithMessage("Start Register Date be grater Or Equal than than Start Register Date");
            RuleFor(p => p.EndRegisterDate)
                .LessThanOrEqualTo(p => p.EndDate).WithMessage("End Register Date must be less than Or Equal End  Date");
        }
    }
}

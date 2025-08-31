using FluentValidation;

namespace FirstCall.Shared.ViewModels.Events
{
    public class EventCategoryTranslationUpdateModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public int CategoryId { get; set; }
    }

    public class EventCategoryTranslationUpdateValidator : AbstractValidator<EventCategoryTranslationUpdateModel>
    {
        public EventCategoryTranslationUpdateValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");

        }
    }
}

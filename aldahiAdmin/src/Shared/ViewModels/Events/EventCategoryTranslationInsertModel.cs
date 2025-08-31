using FluentValidation;

namespace FirstCall.Shared.ViewModels.Events
{
    public class EventCategoryTranslationInsertModel
    {
        public string Name { set; get; }

        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public int CategoryId { get; set; }
    }

    public class EventCategoryTranslationInsertValidator : AbstractValidator<EventCategoryTranslationInsertModel>
    {
        public EventCategoryTranslationInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");

        }
    }
}

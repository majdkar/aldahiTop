using FluentValidation;

namespace FirstCall.Shared.ViewModels.Settings.Languages
{
    public class LanguageInsertModel
    {
        public string Name { get; set; }

        public string LanguageCode { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class LanguageInsertValidator : AbstractValidator<LanguageInsertModel>
    {
        public LanguageInsertValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("You must enter Name");
            RuleFor(p => p.LanguageCode).NotEmpty().WithMessage("You must enter code");
        }
    }
}

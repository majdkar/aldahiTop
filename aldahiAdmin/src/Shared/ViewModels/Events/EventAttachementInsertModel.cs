using FluentValidation;

namespace FirstCall.Shared.ViewModels.Events
{
    public class EventAttachementInsertModel
    {
        public string File { get; set; }

        public int EventId { get; set; }
        public string Name { get; set; }
    }

    public class EventAttachementInsertValidator : AbstractValidator<EventAttachementInsertModel>
    {
        public EventAttachementInsertValidator()
        {
            
        }
    }
}

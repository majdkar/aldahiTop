using FluentValidation;

namespace FirstCall.Shared.ViewModels.Events
{
    public class EventPhotoInsertModel
    {
        public string Image { get; set; }

        public int EventId { get; set; }
    }

    public class EventPhotoInsertValidator : AbstractValidator<EventPhotoInsertModel>
    {
        public EventPhotoInsertValidator()
        {
            
        }
    }
}

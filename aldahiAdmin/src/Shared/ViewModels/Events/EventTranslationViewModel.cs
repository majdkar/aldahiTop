namespace FirstCall.Shared.ViewModels.Events
{
    public class EventTranslationViewModel
    {
        public int Id { set; get; }

        public string Name { set; get; }

        public string Place { set; get; }

        public string Description { set; get; }

        public string Language { get; set; }

        public bool IsActive { get; set; }

        public string Slug { get; set; }

        public int EventId { get; set; }
    }
}

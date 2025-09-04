namespace FirstCall.Server.Models
{
    public class TelegramUpdate
    {
        public long update_id { get; set; }
        public TelegramMessage message { get; set; }
    }

    public class TelegramMessage
    {
        public long message_id { get; set; }
        public TelegramUser from { get; set; } // ← إضافة هذا
        public TelegramChat chat { get; set; }
        public string text { get; set; }
    }

    public class TelegramUser
    {
        public long id { get; set; }
        public bool is_bot { get; set; }
        public string first_name { get; set; }
        public string username { get; set; }
    }

    public class TelegramChat
    {
        public long id { get; set; }
        public string type { get; set; }
    }
}

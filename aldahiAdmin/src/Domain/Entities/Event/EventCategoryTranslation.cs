using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{

    public class EventCategoryTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(200)]
        public string Name { set; get; }

        [Column("Description"), StringLength(Int32.MaxValue)]
        public string Description { set; get; }

        public string Slug { get; set; }

        public bool IsActive { get; set; } = true;

        public string Language { get; set; }

        [ForeignKey("EventCategory")]
        public int CategoryId { get; set; }
        public virtual EventCategory EventCategory { get; set; }
    }
}

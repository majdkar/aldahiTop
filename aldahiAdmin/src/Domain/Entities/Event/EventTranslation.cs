using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{
    public class EventTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(1000)]
        public string Name { set; get; }

        [Column("Place"), StringLength(1000)]
        public string Place { set; get; }

        [Column("Description"), StringLength(Int32.MaxValue)]
        public string Description { set; get; }

        public string Language { get; set; }

        public bool IsActive { get; set; } = true;

        public string Slug { get; set; }

        [ForeignKey("Event")]
        public int EventId { get; set; }
        public virtual Event Event { get; set; }
    }
}

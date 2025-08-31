using FirstCall.Domain.Contracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{
    public class MenueTranslate 
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(200)]
        public string Name { set; get; }

        public string Language { set; get; }

        public bool IsActive { get; set; }

        [Column("HtmlText"), StringLength(Int32.MaxValue)]
        public string HtmlText { set; get; }

        [ForeignKey("Menue")]
        public int MenueId { get; set; }
        public virtual Menu Menue { get; set; }
    }
}

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{

    public class PagePhoto
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }
        public string Image { get; set; }


        [ForeignKey("Page")]
        public int PageId { get; set; }
        public virtual Page Page { get; set; }
    }
}

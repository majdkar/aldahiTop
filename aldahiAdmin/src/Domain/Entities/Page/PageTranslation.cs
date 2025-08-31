using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{
    public class PageTranslation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(200)]
        public string Name { set; get; }

        [Column("Description"), StringLength(Int32.MaxValue)]
        public string Description { set; get; }

        public string Link1 { get; set; }

        public string Link2 { get; set; }

        [ForeignKey("Page")]
        public int PageId { get; set; }
        public virtual Page Page { get; set; }

        public bool IsActive { get; set; }

        public string Language { get; set; }

        public string Slug { get; set; }

        public string File { get; set; }

        }
    }

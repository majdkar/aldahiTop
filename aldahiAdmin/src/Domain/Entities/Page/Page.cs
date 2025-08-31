using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{
    public class Page
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(200)]
        public string Name { set; get; }
        [Column("Description"), StringLength(Int32.MaxValue)]
        public string Description1 { set; get; }
        [Column("EnglishName"), StringLength(1000)]
        public string EnglishName { set; get; }

        [Column("EnglishDescription"), StringLength(Int32.MaxValue)]
        public string EnglishDescription1 { set; get; }

        public string Image { get; set; }
        public string Image1 { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public string Description2 { set; get; }
        public string EnglishDescription2 { set; get; }


        public string CaptionArabic { set; get; }
        public string CaptionEnglish { set; get; }

        public string Type { get; set; }

        public int MenuType { get; set; }

        public string GeoLocation { get; set; }

        public bool IsActive { get; set; }

        public int RecordOrder { get; set; }
        public int MenuId { get; set; }
        public string Url { get; set; }
        public bool IsHome { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHomeFooter { get; set; }
        public virtual List<PageTranslation> PageTranslations { get; set; }
        public virtual List<PagePhoto> PagePhotos { get; set; }
        public virtual List<PageAttachement> PageAttachements { get; set; }
    }
}

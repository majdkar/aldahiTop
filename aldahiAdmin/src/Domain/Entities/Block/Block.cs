using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FirstCall.Domain.Entities;

namespace FirstCall.Core.Entities
{

    public class Block
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(200)]
        public string Name { set; get; }

        [Column("EnglishName"), StringLength(200)]
        public string EnglishName { set; get; }

        public string Description1 { set; get; }
        public string EnglishDescription1 { set; get; }

        public string Description2 { set; get; }
        public string EnglishDescription2 { set; get; }

        public string Description3 { set; get; }
        public string EnglishDescription3 { set; get; }

        public string Description4 { set; get; }
        public string EnglishDescription4 { set; get; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public string Image4 { get; set; }

        [ForeignKey("BlockCategory")]
        public int CategoryId { get; set; }
        public virtual BlockCategory BlockCategory { get; set; }

        public bool IsVisible { get; set; } = true;

        public string Url { get; set; }
        public string Url1 { get; set; }

        public string File { get; set; }

        public bool IsActive { get; set; } = true;

        public int RecordOrder { get; set; }
       
        public DateTime? NewDate { get; set; }

      
        public DateTime? StartDate { get; set; }

        
        public DateTime? EndDate { get; set; }
      
        public DateTime? StarRegistertDate { get; set; }
       
       
        

      
        public DateTime? EndRegisterDate { get; set; }
       
        public DateTime? ArticleDate { get; set; }
        public string Author { get; set; }
        public virtual List<BlockTranslation> BlockTranslations { get; set; }
        public virtual List<BlockPhoto> BlockPhotos { get; set; }
        public virtual List<BlockAttachement> BlockAttachements { get; set; }
        public virtual List<BlockVideo> BlockVideos { get; set; }

    }
}

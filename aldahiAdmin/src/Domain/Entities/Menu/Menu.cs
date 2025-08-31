using FirstCall.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//using DentalMgmt.DataContext.Resources;

namespace FirstCall.Core.Entities
{

    public class Menu
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

        public bool IsHome { get; set; }

        public bool IsFooter { get; set; }

        public bool IsHomeFooter { get; set; }

        public string Type { get; set; }

        public string Image1 { get; set; }

        public string Image2 { get; set; }

        public string Image3 { get; set; }

        public string Image4 { get; set; }


        public string File { get; set; }

        public string? FileEnglish { get; set; }

        public string PageUrl { get; set; }
        public string Url { get; set; }

        public int LevelOrder { get; set; }

        [InverseProperty("Children")]
        public int? ParentId { get; set; }
        public virtual Menu Parent { get; set; }

        public virtual ICollection<Menu> Children { get; set; }

        public virtual List<MenueTranslate> MenueTranslates { get; set; }

        [ForeignKey("MenuCategory")]
        public int CategoryId { get; set; }
        public virtual MenuCategory MenuCategory { get; set; }

        public bool IsActive { get; set; }

    }
}

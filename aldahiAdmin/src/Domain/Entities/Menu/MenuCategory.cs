using FirstCall.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{
    public class MenuCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(200)]
        public string Name { set; get; }
        public string Description { set; get; }
        [Column("EnglishName"), StringLength(200)]
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public bool IsActive { get; set; }

        public bool IsVisableUser { get; set; } = false;

        public virtual List<Menu> Menus { get; set; }
    }
}

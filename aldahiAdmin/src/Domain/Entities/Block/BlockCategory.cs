using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{

    public class BlockCategory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }

        [Column("Name"), StringLength(200)]
        public string Name { set; get; }

        public string Description { set; get; }
        [Column("EnglishName"), StringLength(200)]
        public string EnglishName { set; get; }

        public string EnglishDescription { set; get; }

        public string BlockType { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual List<Block> Blocks { get; set; }
        }
    }

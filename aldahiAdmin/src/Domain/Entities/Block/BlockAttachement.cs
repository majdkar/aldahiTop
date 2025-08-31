using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Core.Entities
{

    public class BlockAttachement
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }
        public string File { get; set; }
        public string Name { get; set; }


        [ForeignKey("Block")]
        public int BlockId { get; set; }
        public virtual Block Block { get; set; }
    }
}

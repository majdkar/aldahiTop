using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirstCall.Core.Entities;

namespace FirstCall.Domain.Entities
{
    public class BlockVideo
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity), Column("Id")]
        public int Id { set; get; }
        public string Url { get; set; }


        [ForeignKey("Block")]
        public int BlockId { get; set; }
        public virtual Block Block { get; set; }
    }
}

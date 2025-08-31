using FirstCall.Core.Entities;
using FirstCall.Domain.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Domain.Entities.GeneralSettings
{
    public class Brand : AuditableEntity<int>
    {

    

        public string Name { get; set; }
        public string NameEn { get; set; }

        public decimal Tax { get; set; }

   
        public string ImageDataURL { get; set; }

    }
}
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace FirstCall.Domain.Entities.Clients
{
    public class Person : AuditableEntity<int>
    {

       

        [ForeignKey("Client")]
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [ForeignKey("Country")]
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }


        public string CityName { get; set; }

        public string PersomImageUrl { get; set; }

        public string FullName { get; set; }
        public string Phone { get; set; }

        public string Email { get; set; }
        public string Address { get; set; }


        public string AdditionalInfo { get; set; }

    }
}

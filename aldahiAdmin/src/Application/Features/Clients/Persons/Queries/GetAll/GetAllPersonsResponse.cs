
using FirstCall.Domain.Entities.Clients;
using FirstCall.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;


namespace FirstCall.Application.Features.Clients.Persons.Queries.GetAll
{
    public class GetAllPersonsResponse
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        public string UserId { get; set; }
        public virtual Client Client { get; set; }
        public int? CountryId { get; set; }
        public virtual Country Country { get; set; }

        public string CityName { get; set; }



        public string PersomImageUrl { get; set; }

        public string Phone { get; set; }

        public string FullName { get; set; }




        public string Email { get; set; }
        public string Address { get; set; }


        public string AdditionalInfo { get; set; }

    }
}

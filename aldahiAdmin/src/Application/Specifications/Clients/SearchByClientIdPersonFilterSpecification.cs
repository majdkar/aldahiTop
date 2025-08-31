using FirstCall.Application.Features.Clients.Persons.Queries.GetAllPaged;
using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Specifications.Clients
{
    internal class SearchByClientIdPersonFilterSpecification : HeroSpecification<Person>
    {
        public SearchByClientIdPersonFilterSpecification(GetAllPagedSearchPersonsQuery request)
        {
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                Criteria = p => 
                                (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                                (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) &&
                                (string.IsNullOrEmpty(request.PersonName) ? true : p.FullName.Contains(request.PersonName)) &&
                                (request.CountryId == 0 ? true : p.CountryId == request.CountryId) &&
                                (string.IsNullOrEmpty(request.CityName) ? true : p.CityName.Contains(request.CityName)) &&
                     
                 
                                (string.IsNullOrEmpty(request.Address) ? true : p.Address.Contains(request.Address)) &&
                                (string.IsNullOrEmpty(request.AdditionalInfo) ? true : p.AdditionalInfo.Contains(request.AdditionalInfo)) &&
                                (p.Email.Contains(request.SearchString) || p.Phone.Contains(request.PhoneNumber) || p.FullName.Contains(request.PersonName))
                                  &&
                                !p.IsDeleted && (p.Client.IsActive || !request.FilterActive);
            }
            else
            {
                Criteria = p =>
                                   (string.IsNullOrEmpty(request.Email) ? true : p.Email.Contains(request.Email)) &&
                                (string.IsNullOrEmpty(request.PhoneNumber) ? true : p.Phone.Contains(request.PhoneNumber)) &&
                                (string.IsNullOrEmpty(request.PersonName) ? true : p.FullName.Contains(request.PersonName)) &&
                                (request.CountryId == 0 ? true : p.CountryId == request.CountryId) &&
                                (string.IsNullOrEmpty(request.CityName) ? true : p.CityName.Contains(request.CityName)) &&


                                (string.IsNullOrEmpty(request.Address) ? true : p.Address.Contains(request.Address)) &&
                                (string.IsNullOrEmpty(request.AdditionalInfo) ? true : p.AdditionalInfo.Contains(request.AdditionalInfo)) &&
                                (p.Email.Contains(request.SearchString) || p.Phone.Contains(request.PhoneNumber) || p.FullName.Contains(request.PersonName))
                                  &&
                                !p.IsDeleted && (p.Client.IsActive || !request.FilterActive);
            }
        }
    }
}

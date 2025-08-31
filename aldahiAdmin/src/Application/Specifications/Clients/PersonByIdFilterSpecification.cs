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
    public class PersonByIdFilterSpecification : HeroSpecification<Person>
    {
        public PersonByIdFilterSpecification(int id,bool FilterActive)
        {
            Includes.Add(c => c.Client);
            Includes.Add(c => c.Country);
            Criteria = c => c.Id == id && (c.Client.IsActive || !FilterActive);
        }
    }
}

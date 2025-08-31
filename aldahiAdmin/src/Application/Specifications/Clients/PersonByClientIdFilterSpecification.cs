using FirstCall.Application.Specifications.Base;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Application.Specifications.Clients
{
    public class PersonByClientIdFilterSpecification : HeroSpecification<Person>
    {
        public PersonByClientIdFilterSpecification(int clientId)
        {
            Includes.Add(c => c.Client);
 

            Criteria = c => !c.IsDeleted&& c.ClientId == clientId;
        }
    }
}

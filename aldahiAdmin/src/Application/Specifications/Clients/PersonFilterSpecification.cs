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
    public class PersonFilterSpecification : HeroSpecification<Person>
    {
        public PersonFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => !p.IsDeleted;
            }
            else
            {
                Criteria = p => !p.IsDeleted;
            }
        }
    }
}

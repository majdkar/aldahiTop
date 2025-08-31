using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstCall.Infrastructure.Repositories
{
    public class CountryRepository :ICountryRepository
    {
        private readonly IRepositoryAsync<Country, int> _repository;

        public CountryRepository(IRepositoryAsync<Country, int> repository)
        {
            _repository = repository;
        }
    }
}
using System;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Infrastructure.Repositories
{
    public class PrincedomRepository : IPrincedomRepository
    {
        private readonly IRepositoryAsync<Princedom, int> _repository;

        public PrincedomRepository(IRepositoryAsync<Princedom, int> repository)
        {
            _repository = repository;
        }
    }
}
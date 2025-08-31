using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Infrastructure.Repositories
{
    public class NationRepository : INationRepository
    {
        private readonly IRepositoryAsync<Nation, int> _repository;

        public NationRepository(IRepositoryAsync<Nation, int> repository)
        {
            _repository = repository;
        }
    }
}
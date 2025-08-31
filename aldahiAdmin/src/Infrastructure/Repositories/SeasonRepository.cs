using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.GeneralSettings;

namespace FirstCall.Infrastructure.Repositories
{
    public class SeasonRepository : ISeasonRepository
    {
        private readonly IRepositoryAsync<Season, int> _repository;

        public SeasonRepository(IRepositoryAsync<Season, int> repository)
        {
            _repository = repository;
        }
    }
}

using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Features.Clients.Persons.Commands.AddEdit;
using FirstCall.Domain.Entities.Clients;

namespace FirstCall.Application.Features.Clients.Persons.Commands.Delete
{
    public class BlockPersonCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class BlockPersonClientCommandHandler : IRequestHandler<BlockPersonCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditPersonCommand> _localizer;

        public BlockPersonClientCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<AddEditPersonCommand> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(BlockPersonCommand command, CancellationToken cancellationToken)
        {
            var personClient = await _unitOfWork.Repository<Person>().Entities
                .Include(x => x.Client).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == command.Id);
            if (personClient != null)
            {
                personClient.Client.IsActive = false;
                personClient.Client.User.IsActive = false;
                await _unitOfWork.Repository<Person>().UpdateAsync(personClient);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(personClient.Id, _localizer["PersonClient Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["PersonClient Not Found!"]);
            }
        }
    }
}
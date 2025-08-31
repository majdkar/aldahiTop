using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FirstCall.Application.Interfaces.Repositories;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Shared.Wrapper;
using System.Threading;
using System.Threading.Tasks;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;
using FirstCall.Application.Specifications.Clients;
using System.Linq;
using FirstCall.Application.Extensions;
using FirstCall.Shared.Constants.Clients;


namespace FirstCall.Application.Features.Clients.Persons.Queries.GetById
{
    public class GetPersonByIdQuery : IRequest<Result<GetAllPersonsResponse>>
    {
        public int Id { get; set; }
        public bool FilterActive { get; set; }
    }
    internal class GetPersonByIdQueryHandler : IRequestHandler<GetPersonByIdQuery, Result<GetAllPersonsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetPersonByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetAllPersonsResponse>> Handle(GetPersonByIdQuery query, CancellationToken cancellationToken)
        {
            var personByIdFilterSpec = new PersonByIdFilterSpecification(query.Id,query.FilterActive);

            var person = await _unitOfWork.Repository<Person>().Entities.Specify(personByIdFilterSpec)
             .FirstOrDefaultAsync(x=>x.Id==query.Id && !x.IsDeleted);
           
            
            if (person == null)
            {
                return await Result<GetAllPersonsResponse>.FailAsync("Not Found");
            }
            var personClient = await _unitOfWork.Repository<Client>().Entities.FirstOrDefaultAsync(x => x.Id == person.ClientId && !x.IsDeleted);
            if (personClient == null)
            {
                return await Result<GetAllPersonsResponse>.FailAsync("Client not found");
            }
            if (!personClient.IsActive)
            {
                return await Result<GetAllPersonsResponse>.FailAsync("Client not Active");

            }
            
            var mappedPerson = _mapper.Map<GetAllPersonsResponse>(person);



            mappedPerson.UserId = mappedPerson.Client.UserId;



            return await Result<GetAllPersonsResponse>.SuccessAsync(mappedPerson);
        }
    }
}
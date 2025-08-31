using MediatR;
using FirstCall.Application.Interfaces.Repositories;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Threading;
using System;
using ClientNameSpace = FirstCall.Domain.Entities.Clients;
using FirstCall.Application.Specifications.Clients;
using FirstCall.Shared.Wrapper;
using System.Linq;
using FirstCall.Application.Extensions;
using System.Linq.Dynamic.Core;
using FirstCall.Domain.Contracts;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;
using FirstCall.Shared.Constants.Clients;

namespace FirstCall.Application.Features.Clients.Persons.Queries.GetAllPaged
{
    public class GetAllPagedPersonsQuery : IRequest<PaginatedResult<GetAllPersonsResponse>>
    {
        public string PersonName { get; }
        public string Email { get; }
        public string PhoneNumber { get; }
        public int CountryId { get; }
      


        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllPagedPersonsQuery(string personName, string email, string phoneNumber,int countryId, int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PersonName = personName;
            Email = email;
            PhoneNumber = phoneNumber;
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
         
            CountryId = countryId;
      
        }
    }
    internal class GetAllPagedPersonsQueryHandler : IRequestHandler<GetAllPagedPersonsQuery,PaginatedResult<GetAllPersonsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllPagedPersonsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPersonsResponse>> Handle(GetAllPagedPersonsQuery request, CancellationToken cancellationToken)
        {

            Expression<Func<Person, GetAllPersonsResponse>> expression = e => new GetAllPersonsResponse
            {
                Id = e.Id,
                ClientId = e.ClientId,
                FullName = e.FullName,
                Email = e.Email,
                Phone = e.Client.User.PhoneNumber,
                UserId = e.Client.UserId,
                AdditionalInfo = e.AdditionalInfo,
                Address = e.Address,
                Country = e.Country,
                CountryId = e.CountryId,
                PersomImageUrl = e.PersomImageUrl,
                 CityName = e.CityName,
                   

            };
            var ownerFilterSpec = new SearchPersonFilterSpecification(request);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Person>().Entities
                   .Specify(ownerFilterSpec)
                   .OrderByDescending(x => x.Id)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Person>().Entities
                   .Specify(ownerFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}
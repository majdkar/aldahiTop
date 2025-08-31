using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Clients.Persons.Commands.AddEdit;
using FirstCall.Application.Features.Clients.Persons.Commands.Delete;
using FirstCall.Application.Features.Clients.Persons.Queries.Export;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAll;
using FirstCall.Application.Features.Clients.Persons.Queries.GetAllPaged;
using FirstCall.Application.Features.Clients.Persons.Queries.GetById;
using FirstCall.Domain.Entities.Clients;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;

namespace FirstCall.Server.Controllers.v1.PersonsManagement
{
    public class PersonsController : BaseApiController<PersonsController>
    {

        /// <summary>
        /// Get All Persons Paged Result
        /// </summary>
        /// <param name="personName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="pageNumber"></param>
       
        /// <param name="countryId"></param>
    
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllPaged(string personName, string email, string phoneNumber, int countryId,  int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var persons = await _mediator.Send(new GetAllPagedPersonsQuery(personName, email, phoneNumber,countryId, pageNumber, pageSize, searchString, orderBy));
            return Ok(persons);
        }

        /// <summary>
        /// Get All Persons Paged Result
        /// </summary>
        /// <param name="personName"></param>
        /// <param name="email"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="pageNumber"></param>
        /// <param name="countryId"></param>
        /// <param name="cityName"></param>
   
        /// <param name="additionalInfo"></param>
     
        /// <param name="address"></param>
    
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
       /// <param name="FilterActive"></param>

        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpGet("SearcAdvanced")]
        public async Task<IActionResult> GetAllPagedSearcAdvanced(string personName, string email, string phoneNumber,  int countryId,string cityName,
             string additionalInfo,
             string address, bool FilterActive, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var persons = await _mediator.Send(new GetAllPagedSearchPersonsQuery(personName, email, phoneNumber,countryId,cityName
                ,additionalInfo, address,FilterActive, pageNumber, pageSize, searchString, orderBy));
            return Ok(persons);
        }




        /// <summary>
        /// Delete a Account Person Client
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Persons.Delete)]
        [HttpDelete("Block/{id}")]
        public async Task<IActionResult> Block(int id)
        {
            return Ok(await _mediator.Send(new BlockPersonCommand { Id = id }));
        }


        /// <summary>
        /// Get Person By ClientId
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Persons.View)]
        [HttpGet("GetByClientId/{clientId}")]
        public async Task<IActionResult> GetByClientId(int clientId)
        {
            var person = await _mediator.Send(new GetPersonByClientIdQuery { ClientId = clientId });
            return Ok(person);
        }

        /// <summary>
        /// Get All Persons
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Persons.View)]
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var persons = await _mediator.Send(new GetAllPersonsQuery());
            return Ok(persons);
        }

        /// <summary>
        /// Get Person By Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="FilterActive"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Persons.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id,bool FilterActive)
        {
            var person = await _mediator.Send(new GetPersonByIdQuery { Id = id, FilterActive = FilterActive });
            return Ok(person);
        }



   


        /// <summary>
        /// Add/Edit a Person
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPersonCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

    

        /// <summary>
        /// Delete a Person
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Persons.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePersonCommand { Id = id }));
        }

   
        /// <summary>
        /// Search Persons and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Persons.View)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportPersonsQuery(searchString)));
        }




    }
}

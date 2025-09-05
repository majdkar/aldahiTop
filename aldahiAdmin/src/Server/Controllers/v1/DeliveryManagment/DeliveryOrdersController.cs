using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FirstCall.Application.Features.Orders.Commands.AddEdit;
using FirstCall.Application.Features.Orders.Commands.Delete;

using FirstCall.Application.Features.DeliveryOrders.Queries.GetById;
using FirstCall.Shared.Constants.Permission;
using System.Threading.Tasks;
using FirstCall.Application.Features.Orders.Queries.GetAll;
using FirstCall.Application.Features.Orders.Queries.Export;
using FirstCall.Application.Features.DeliveryOrders.Queries.GetAllPaged;

namespace FirstCall.Server.Controllers.v1.DeliveryManagment
{
    public class DeliveryOrdersController : BaseApiController<DeliveryOrdersController>
    {

        /// <summary>
        /// Get All DeliveryOrders
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(string Type,int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var deliveryOrders = await _mediator.Send(new GetAllDeliveryOrdersQuery(pageNumber, pageSize, searchString, orderBy,Type));
            return Ok(deliveryOrders);
        }


        // <summary>
        // Get All Delivery Orders By order status
        // </summary>
        // <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.View)]
        [HttpGet("GetByStatus")]
        public async Task<IActionResult> GetByStatus(string Type, string status, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var deliveryOrders = await _mediator.Send(new GetDeliveryOrdersByStatusQuery(status, pageNumber, pageSize, searchString, orderBy,Type));
            return Ok(deliveryOrders);
        }


        /// <summary>
        /// Get All Delivery Orders By  order statusDelivery
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.View)]
        //[HttpGet("GetByStatusDelivery")]
        //public async Task<IActionResult> GetByStatusDelivery(string statusDelivery, int pageNumber, int pageSize, string searchString, string orderBy = null)
        //{
        //    var deliveryOrders = await _mediator.Send(new GetDeliveryOrdersByStatusDeliveryQuery(statusDelivery, pageNumber, pageSize, searchString, orderBy));
        //    return Ok(deliveryOrders);
        //}


        /// <summary>
        /// Get a DeliveryOrder By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var deliveryOrder = await _mediator.Send(new GetDeliveryOrderByIdQuery() { Id = id });
            return Ok(deliveryOrder);
        }

        /// <summary>
        /// Get a DeliveryOrders By Client Id
        /// </summary>
        /// <param name="clientId"></param>
        /// <returns>Status 200 Ok</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.View)]
        //[HttpGet("GetByClient/{clientId}")]
        //public async Task<IActionResult> GetByClient(int clientId)
        //{
        //    var deliveryOrder = await _mediator.Send(new GetAllDeliveryOrdersByClientQuery() { ClientId = clientId });
        //    return Ok(deliveryOrder);
        //}


        /// <summary>
        /// Get Paged Delivery Orders By  Client Id
        /// </summary>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.View)]
        [HttpGet("GetPagedByClient")]
        public async Task<IActionResult> GetPagedByClient(string type, int clientId, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var deliveryOrders = await _mediator.Send(new GetAllPagedDeliveryOrdersByClientQuery(clientId, pageNumber, pageSize, searchString, orderBy,type));
            return Ok(deliveryOrders);
        }




        /// <summary>
        /// Create/Update a DeliveryOrder
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DeliveryOrders.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDeliveryOrderCommand command)
        {
            return Ok(await _mediator.Send(command));
        }




        /// <summary>
        /// Delete a DeliveryOrder
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DeliveryOrders.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDeliveryOrderCommand { Id = id }));
        }

        // <summary>
        // Accept a DeliveryOrder
        // </summary>
        // <param name = "command" ></ param >
        // < returns > Status 200 OK response</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.Create)]
        //[HttpPost("accept")]
        //public async Task<IActionResult> Accept(AcceptDeliveryOrderRequestCommand command)
        //{
        //    return Ok(await _mediator.Send(command));
        //}

        /// <summary>
        /// Refuse a DeliveryOrder
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.DeliveryOrders.Edit)]
        [HttpDelete("refuse/{id}")]
        public async Task<IActionResult> Refuse(int id)
        {
            return Ok(await _mediator.Send(new RefuseDeliveryOrderRequestCommand { Id = id }));
        }

        /// <summary>
        /// Acccept Order Request For Close
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.DeliveryOrders.Edit)]
        [HttpDelete("AccceptOrderRequest/{id}")]
        public async Task<IActionResult> AccceptOrderRequest(int id)
        {
            return Ok(await _mediator.Send(new AcceptDeliveryOrderRequestCommand { Id = id }));
        }



        /// <summary>
        /// Order Close Request
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.Edit)]
        //[HttpDelete("OrderCloseRequest/{id}")]
        //public async Task<IActionResult> OrderCloseRequest(int id)
        //{
        //    return Ok(await _mediator.Send(new DeliveryOrderCloseRequestCommand { Id = id }));
        //}

        /// <summary>
        /// Search DeliveryOrders and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.DeliveryOrders.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportDeliveryOrdersQuery(searchString)));
        }
    }
}

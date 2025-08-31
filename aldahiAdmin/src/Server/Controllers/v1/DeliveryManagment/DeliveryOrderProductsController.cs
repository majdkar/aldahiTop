using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Polly;
using System.Threading.Tasks;
using FirstCall.Application.Features.OrderProducts.Commands.AddEdit;

using FirstCall.Application.Features.OrderProducts.Queries.GetAllByOrder;
using FirstCall.Application.Features.OrderProducts.Queries.GetById;
using FirstCall.Application.Features.Orders.Commands.Delete;
using FirstCall.Application.Features.Orders.Queries.Export;
using FirstCall.Shared.Constants.Permission;

namespace FirstCall.Server.Controllers.v1.DeliveryManagment
{
    public class DeliveryOrderProductsController : BaseApiController<DeliveryOrderProductsController>
    {
        /// <summary>
        /// Get All DeliveryOrderProducts
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.CompanyProfile.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int  deliveryorder, int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var DeliveryOrderProducts = await _mediator.Send(new GetAllDeliveryOrderProductsQuery(pageNumber, pageSize, searchString, orderBy, deliveryorder));
            return Ok(DeliveryOrderProducts);
        }

        /// <summary>
        /// Get a DeliveryOrder By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        //[Authorize(Policy = Permissions.DeliveryOrders.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeliveryOrderProductById(int id)
        {
            var deliveryOrder = await _mediator.Send(new GetDeliveryOrderProductByIdQuery() { Id = id });
            return Ok(deliveryOrder);
        }

        /// <summary>
        /// Add/Edit a DeliveryOrderProduct
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.CompanyProfile.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDeliveryOrderProductCommand command)
        {
            return Ok(await _mediator.Send(command));
        }


        
        /// <summary>
        /// Delete a DeliveryOrderProduct
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        //[Authorize(Policy = Permissions.CompanyProfile.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDeliveryOrderProductCommand { Id = id }));
        }

 
    }
}

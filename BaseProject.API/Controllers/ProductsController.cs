using BaseProject.Domain.Models.Dtos.Product;
using BaseProject.Service.Services.Product.Commands;
using BaseProject.Service.Services.Product.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]

    public class ProductsController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("GetProduct/{id}")]
        public async Task<CommandResult<ProductDTO>> GetProduct(Guid id) => await _mediator.Send(new GetProductQuery(id));

        [HttpGet]
        [Route("GetProducts")]
        public async Task<CommandResult<List<ProductDTO>>> GetProducts() => await _mediator.Send(new GetProductsQuery());

        [HttpPost]
        [Route("GetProductsDatasource")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult<DataSourceResult>))]
        public async Task<DataSourceResult> GetProductsDatasource([FromBody] DataTableApiRequest request)
        {
            var data = await _mediator.Send(new GetProductsDatasourceQuery(request));
            return data.Data;
        }

        [HttpPost]
        [Route("AddProduct")]
        public async Task<CommandResult<ProductDTO>> AddProduct([FromBody] AddProductDTO model) => await _mediator.Send(new AddProductCommand(model));

        [HttpPost]
        [Route("UpdateProduct")]
        public async Task<CommandResult<ProductDTO>> UpdateProduct([FromBody] UpdateProductDTO model) => await _mediator.Send(new UpdateProductCommand(model));

        [HttpDelete]
        [Route("DeleteProduct/{id}")]
        public async Task<CommandResult<ProductDTO>> DeleteProduct(Guid id) => await _mediator.Send(new DeleteProductCommand(id));
    }
}

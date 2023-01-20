using BaseProject.Domain.Models.Dtos;
using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.Product;
using BaseProject.Domain.Models.Dtos.Sales;
using BaseProject.Service.Services.Basket.Commands;
using BaseProject.Service.Services.Basket.Queries;
using BaseProject.Service.Services.Product.Commands;
using BaseProject.Service.Services.Product.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BaseProject.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
    public class BasketsController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
        [Route("GetBasket")]
        public async Task<CommandResult<BasketDTO>> GetBasket() => await _mediator.Send(new GetBasketQuery());

        [HttpPost]
        [Route("GetBasketProductsDatasource")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CommandResult<DataSourceResult>))]
        public async Task<DataSourceResult> GetBasketProductsDatasource([FromBody] DataTableApiRequest request)
        {
            var data = await _mediator.Send(new GetBasketProductsDatasourceQuery(request));
            return data.Data;
        }

        [HttpPost]
        [Route("AddProductToBasket")]
        public async Task<CommandResult<BasketProductDTO>> AddProductToBasket([FromBody] AddBasketProductDTO model) => await _mediator.Send(new AddProductToBasketCommand(model));

        [HttpPost]
        [Route("CompleteSale")]
        public async Task<CommandResult<SalesDTO>> CompleteSale([FromBody] CompleteSaleDTO model) => await _mediator.Send(new CompleteSaleCommand(model));

        [HttpDelete]
        [Route("DeleteProductFromBasket/{id}")]
        public async Task<CommandResult<BasketProductDTO>> DeleteProductFromBasket(Guid id) => await _mediator.Send(new DeleteProductFromBasketCommand(id));
    }
}

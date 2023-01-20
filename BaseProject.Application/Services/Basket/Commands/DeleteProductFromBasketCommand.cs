using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Service.Services.Basket.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Service.Services.Basket.Commands;

public class DeleteProductFromBasketCommand : CommandBase<CommandResult<BasketProductDTO>>
{
    public Guid Id { get; set; }

    public DeleteProductFromBasketCommand(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<DeleteProductFromBasketCommand, CommandResult<BasketProductDTO>>
    {
        private readonly IGenericRepository<Domain.Models.Basket> _basketRepository;
        private readonly IGenericRepository<Domain.Models.BasketProduct> _basketProductsRepository;
        private readonly IGenericRepository<Domain.Models.Product> _productRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public Handler(IGenericRepository<Domain.Models.Basket> basketRepository, IGenericRepository<BasketProduct> basketProductsRepository, IGenericRepository<Domain.Models.Product> productRepository, IMapper mapper, IMediator mediator)
        {
            _basketRepository = basketRepository;
            _basketProductsRepository = basketProductsRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<CommandResult<BasketProductDTO>> Handle(DeleteProductFromBasketCommand request, CancellationToken cancellationToken)
        {
            var currentBasket = (await _mediator.Send(new GetBasketQuery())).Data;
            var basketProduct = await _basketProductsRepository.GetFirstTracked(p=>p.BasketId == currentBasket.Id && p.ProductId == request.Id);
            if (basketProduct == null)
                return CommandResult<BasketProductDTO>.NotFound();

            var deleteResult = await _basketProductsRepository.Delete(basketProduct.Id);
            if(deleteResult.IsSucceed)
            {
                var basket = await _basketRepository.GetFirstTracked(p => p.Id == basketProduct.BasketId);
                var product = await _productRepository.GetFirstTracked(p => p.Id == basketProduct.ProductId);
                basket.TotalPrice-=product.Price;
                var updateResult = await _basketRepository.Update(basket);
                if (updateResult.IsSucceed)
                {
                    product.Stock += 1;
                    await _productRepository.Update(product);
                    return CommandResult<BasketProductDTO>.GetSucceed(_mapper.Map<BasketProductDTO>(basketProduct));
                }
            }
            return CommandResult<BasketProductDTO>.GetFailed("Ürün sepetten silinemedi");
        }
    }
}

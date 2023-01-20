using BaseProject.Domain.Models.Dtos.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseProject.Service.Services.Basket.Commands;

public class AddProductToBasketCommand : CommandBase<CommandResult<BasketProductDTO>>
{
    public AddBasketProductDTO Model { get; set; }

    public AddProductToBasketCommand(AddBasketProductDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddProductToBasketCommand, CommandResult<BasketProductDTO>>
    {
        private readonly IGenericRepository<Domain.Models.Basket> _basketRepository;
        private readonly IGenericRepository<Domain.Models.BasketProduct> _basketProductsRepository;
        private readonly IGenericRepository<Domain.Models.Product> _productRepository;
        private readonly IMapper _mapper;

        public Handler(IGenericRepository<Domain.Models.Basket> basketRepository, IGenericRepository<BasketProduct> basketProductsRepository, IGenericRepository<Domain.Models.Product> productRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _basketProductsRepository = basketProductsRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<CommandResult<BasketProductDTO>> Handle(AddProductToBasketCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetFirst(p => p.Id == request.Model.ProductId);

            if (product.Stock > 0)
            {
                var basketProduct = _mapper.Map<BasketProduct>(request.Model);
                if (basketProduct == null)
                    return CommandResult<BasketProductDTO>.NotFound();
                var addResult = await _basketProductsRepository.Add(basketProduct);
                if (addResult.IsSucceed)
                {
                    var basket = await _basketRepository.GetFirstTracked(p => p.Id == request.Model.BasketId);
                    basket.TotalPrice += product.Price;
                    var updateResult = await _basketRepository.Update(basket);
                    if (updateResult.IsSucceed)
                    {
                        product.Stock -= 1;
                        await _productRepository.Update(product);
                        return CommandResult<BasketProductDTO>.GetSucceed(_mapper.Map<BasketProductDTO>(basketProduct));
                    }
                }
            }
            return CommandResult<BasketProductDTO>.GetFailed("Ürün sepete eklenemedi");
        }
    }
}

using BaseProject.Domain.Models.Dtos.Basket;

namespace BaseProject.Service.Services.Basket.Commands;

public class AddBasketCommand : CommandBase<CommandResult<BasketDTO>>
{
    public class Handler : IRequestHandler<AddBasketCommand, CommandResult<BasketDTO>>
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IGenericRepository<Domain.Models.Basket> _repository;

        public Handler(IMapper mapper, ICurrentUserService currentUserService, IGenericRepository<Domain.Models.Basket> repository)
        {
            _mapper = mapper;
            _currentUserService = currentUserService;
            _repository = repository;
        }

        public async Task<CommandResult<BasketDTO>> Handle(AddBasketCommand request, CancellationToken cancellationToken)
        {
            var basket = new Domain.Models.Basket { AppUserId = _currentUserService.GetUserId() };
            var addResult = await _repository.Add(basket);
            return addResult.IsSucceed
                ? CommandResult<BasketDTO>.GetSucceed(_mapper.Map<BasketDTO>(addResult.Data))
                : CommandResult<BasketDTO>.GetFailed("Sepet oluşturulamadı");
        }
    }
}

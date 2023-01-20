using BaseProject.Domain.Models.Dtos.Basket;

namespace BaseProject.Service.Services.Basket.Queries;
public class GetBasketQuery :CommandBase<CommandResult<BasketDTO>>
{
    public class Handler : IRequestHandler<GetBasketQuery, CommandResult<BasketDTO>>
    {
        private readonly IGenericRepository<Domain.Models.Basket> _repository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public Handler(IGenericRepository<Domain.Models.Basket> repository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _repository = repository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<CommandResult<BasketDTO>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var basket = await _repository.GetAllFiltered(p => p.AppUserId == userId).Include(p => p.BasketProducts).ThenInclude(p => p.Product).FirstOrDefaultAsync();
            if(basket == null)
            {
                basket = (await _repository.Add(new Domain.Models.Basket { AppUserId = userId })).Data;
            }
            return CommandResult<BasketDTO>.GetSucceed(_mapper.Map<BasketDTO>(basket));
        }
    }
}

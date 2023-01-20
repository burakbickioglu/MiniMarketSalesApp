using BaseProject.Domain.Models.Dtos.Basket;
using BaseProject.Domain.Models.Dtos.Sales;
using System.Runtime.InteropServices;

namespace BaseProject.Service.Services.Basket.Commands;
public class CompleteSaleCommand : CommandBase<CommandResult<SalesDTO>>
{
    public CompleteSaleDTO Model { get; set; }

    public CompleteSaleCommand(CompleteSaleDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<CompleteSaleCommand, CommandResult<SalesDTO>>
    {
        private readonly IGenericRepository<Domain.Models.Sales> _salesRepository;
        private readonly IGenericRepository<Domain.Models.Basket> _basketRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public Handler(IGenericRepository<Sales> salesRepository, IGenericRepository<Domain.Models.Basket> basketRepository, ICurrentUserService currentUserService, IMapper mapper)
        {
            _salesRepository = salesRepository;
            _basketRepository = basketRepository;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<CommandResult<SalesDTO>> Handle(CompleteSaleCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var basket = await _basketRepository.GetAllFiltered(p => p.AppUserId == userId).FirstOrDefaultAsync();

            if (basket == null)
                return CommandResult<SalesDTO>.GetFailed("Basket not found");
            var sale = new Sales { TotalPrice = basket.TotalPrice, BasketId = basket.Id, AppUserId = basket.AppUserId, PaymentType = request.Model.PaymentType };
            var addResult = await _salesRepository.Add(sale);
            if(addResult.IsSucceed)
            {
                var deleteResult = await _basketRepository.SoftDelete(basket.Id);
                if (deleteResult.IsSucceed)
                {
                    var newBasket = new Domain.Models.Basket { AppUserId = basket.AppUserId };
                    await _basketRepository.Add(newBasket);
                    return CommandResult<SalesDTO>.GetSucceed(_mapper.Map<SalesDTO>(addResult.Data));
                }
            }
            return CommandResult<SalesDTO>.GetFailed("Satış gerçekleşmedi");
        }
    }
}

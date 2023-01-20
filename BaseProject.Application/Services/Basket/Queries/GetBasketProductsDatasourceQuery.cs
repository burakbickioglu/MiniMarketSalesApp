using BaseProject.Domain.Interfaces;
using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Service.Services.Basket.Queries;

public class GetBasketProductsDatasourceQuery :CommandBase<CommandResult<DataSourceResult>>
{
    public DataTableApiRequest Expression { get; set; }

    public GetBasketProductsDatasourceQuery(DataTableApiRequest expression)
    {
        Expression = expression;
    }

    public class Handler : IRequestHandler<GetBasketProductsDatasourceQuery, CommandResult<DataSourceResult>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Basket> _repository;
        private readonly IMediator _mediator;
        private readonly ICurrentUserService _currentUserService;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Basket> repository, IMediator mediator, ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _repository = repository;
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResult<DataSourceResult>> Handle(GetBasketProductsDatasourceQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.GetUserId();
            var products = await _repository.GetAllFiltered(p => p.AppUserId == userId).Include(p => p.BasketProducts).ThenInclude(p => p.Product).SelectMany(p=>p.BasketProducts).Select(p=>p.Product).ToListAsync();

            var dsrequest = request.Expression.Request;
            if (!string.IsNullOrEmpty(request.Expression.RequestFilters))
                dsrequest.Filters = FilterDescriptorFactory.Create(request.Expression.RequestFilters);


            var result = await products.ToDataSourceResultAsync(dsrequest, r => _mapper.Map<ProductDTO>(r));

            return CommandResult<DataSourceResult>.GetSucceed(result);
        }
    }
}

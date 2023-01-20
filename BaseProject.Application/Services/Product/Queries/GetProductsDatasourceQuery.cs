using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Service.Services.Product.Queries;
public class GetProductsDatasourceQuery : CommandBase<CommandResult<DataSourceResult>>
{
    public DataTableApiRequest Expression { get; set; }

    public GetProductsDatasourceQuery(DataTableApiRequest expression)
    {
        Expression = expression;
    }

    public class Handler : IRequestHandler<GetProductsDatasourceQuery, CommandResult<DataSourceResult>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Product> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Product> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<DataSourceResult>> Handle(GetProductsDatasourceQuery request, CancellationToken cancellationToken)
        {
            var products = _repository.GetAllFiltered().AsNoTracking();
            var dsrequest = request.Expression.Request;
            if (!string.IsNullOrEmpty(request.Expression.RequestFilters))
                dsrequest.Filters = FilterDescriptorFactory.Create(request.Expression.RequestFilters);


            var result = await products.ToDataSourceResultAsync(dsrequest, r => _mapper.Map<ProductDTO>(r));

            return CommandResult<DataSourceResult>.GetSucceed(result);

        }
    }
}

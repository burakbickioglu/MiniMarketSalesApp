using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Service.Services.Product.Queries;

public class GetProductQuery :CommandBase<CommandResult<ProductDTO>>
{
    public Guid Id { get; set; }

    public GetProductQuery(Guid id)
    {
        Id = id;
    }

    public class Handler : IRequestHandler<GetProductQuery, CommandResult<ProductDTO>>
    {
        private readonly IMapper _mapper;
        private readonly IGenericRepository<Domain.Models.Product> _repository;

        public Handler(IMapper mapper, IGenericRepository<Domain.Models.Product> repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public async Task<CommandResult<ProductDTO>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _repository.GetFirst(p => p.Id == request.Id);
            return product != null
                ? CommandResult<ProductDTO>.GetSucceed(_mapper.Map<ProductDTO>(product))
                : CommandResult<ProductDTO>.NotFound();
        }
    }
}

using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Service.Services.Product.Commands;

public class AddProductCommand :CommandBase<CommandResult<ProductDTO>>
{
    public AddProductDTO Model { get; set; }

    public AddProductCommand(AddProductDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<AddProductCommand, CommandResult<ProductDTO>>
    {
        private readonly IGenericRepository<Domain.Models.Product> _repository;
        private readonly IMapper _mapper;

        public Handler(IGenericRepository<Domain.Models.Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CommandResult<ProductDTO>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Domain.Models.Product>(request.Model);
            if (product == null)
                return CommandResult<ProductDTO>.NotFound();

            var addResult = await _repository.Add(product);
            return addResult.IsSucceed
                ? CommandResult<ProductDTO>.GetSucceed(_mapper.Map<ProductDTO>(addResult.Data))
                : CommandResult<ProductDTO>.GetFailed("Ürün eklenemedi");
        }
    }
}

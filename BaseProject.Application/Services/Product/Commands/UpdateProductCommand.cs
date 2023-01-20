using BaseProject.Domain.Models.Dtos.Product;

namespace BaseProject.Service.Services.Product.Commands;
public class UpdateProductCommand : CommandBase<CommandResult<ProductDTO>>
{
    public UpdateProductDTO Model { get; set; }

    public UpdateProductCommand(UpdateProductDTO model)
    {
        Model = model;
    }

    public class Handler : IRequestHandler<UpdateProductCommand, CommandResult<ProductDTO>>
    {
        private readonly IGenericRepository<Domain.Models.Product> _repository;
        private readonly IMapper _mapper;

        public Handler(IGenericRepository<Domain.Models.Product> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CommandResult<ProductDTO>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Domain.Models.Product>(request.Model);
            if (product == null)
                return CommandResult<ProductDTO>.GetFailed("Ürün güncellenemedi");

            var model = await _repository.GetFirst(p=>p.Id == product.Id);
            var updateResult = await _repository.Update(product);

            return updateResult.IsSucceed
                ? CommandResult<ProductDTO>.GetSucceed(_mapper.Map<ProductDTO>(updateResult.Data))
                : CommandResult<ProductDTO>.GetFailed("Ürün Güncellenemedi");
        }
    }
}
